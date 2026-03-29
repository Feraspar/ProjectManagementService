namespace ProjectManagementService.Core.Services
{
	using Abstractions;
	using Contracts.Request;
	using Contracts.Response;
	using Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Service for project business logic.
	/// </summary>
	public class ProjectService : IProjectService
	{
		#region Private Fields

		/// <summary>
		/// Employee repository.
		/// </summary>
		private readonly IEmployeeRepository _employeeRepository;

		/// <summary>
		/// Project repository.
		/// </summary>
		private readonly IProjectRepository _projectRepository;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="projectRepository">Project repository.</param>
		/// <param name="employeeRepository">Employee repository.</param>
		public ProjectService(IProjectRepository projectRepository, IEmployeeRepository employeeRepository)
		{
			_projectRepository = projectRepository;
			_employeeRepository = employeeRepository;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task AddEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default)
		{
			bool projectExists = await _projectRepository.ExistsAsync(projectId, cancellationToken);

			if (!projectExists)
			{
				throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
			}

			bool employeeExists = await _employeeRepository.ExistsAsync(employeeId, cancellationToken);

			if (!employeeExists)
			{
				throw new KeyNotFoundException($"Employee with id '{employeeId}' was not found.");
			}

			bool employeeAlreadyAssigned = await _projectRepository.HasEmployeeAsync(projectId, employeeId, cancellationToken);

			if (employeeAlreadyAssigned)
			{
				throw new InvalidOperationException("The employee is already assigned to the project.");
			}

			ProjectEmployee projectEmployee = new()
			{
				ProjectId = projectId,
				EmployeeId = employeeId,
				AssignedAt = DateTimeOffset.UtcNow
			};

			await _projectRepository.AddEmployeeAsync(projectEmployee, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<ProjectDetailsResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(request);

			await ValidateProjectRequestAsync(
				request.Name, 
				request.CustomerCompanyName, 
				request.ExecutorCompanyName, 
				request.ProjectManagerId, 
				request.StartDate, 
				request.EndDate, 
				request.EmployeeIds, 
				cancellationToken);

			List<Guid> employeeIds = NormalizeEmployeeIds(request.EmployeeIds, request.ProjectManagerId);

			Project project = new()
			{
				Id = Guid.NewGuid(),
				Name = request.Name.Trim(),
				CustomerCompanyName = request.CustomerCompanyName,
				ExecutorCompanyName = request.ExecutorCompanyName,
				ProjectManagerId = request.ProjectManagerId,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				Priority = request.Priority
			};

			await _projectRepository.AddAsync(project, cancellationToken);

			foreach (Guid employeeId in employeeIds)
			{
				ProjectEmployee projectEmployee = new()
				{
					ProjectId = project.Id,
					EmployeeId = employeeId,
					AssignedAt = DateTimeOffset.UtcNow,
				};

				await _projectRepository.AddEmployeeAsync(projectEmployee, cancellationToken);
			}

			Project? createdProject = await _projectRepository.GetByIdWithDetailsAsync(project.Id, cancellationToken);

			if (createdProject == null)
			{
				throw new InvalidOperationException("The created project could not be loaded.");
			}

			return MapToDetailsResponse(createdProject);
		}

		/// <inheritdoc />
		public async Task DeleteAsync(Guid projectId, CancellationToken cancellationToken = default)
		{
			Project? project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);

			if (project == null)
			{
				throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
			}

			await _projectRepository.DeleteAsync(project, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<ProjectListItemResponse>> GetAllAsync(FiltredProjectsRequest request, CancellationToken cancellationToken = default)
		{
			IReadOnlyCollection<Project> projects = await _projectRepository.GetAllAsync(request, cancellationToken);

			return projects.Select(MapToListItemResponse).ToList();
		}

		/// <inheritdoc />
		public async Task<ProjectDetailsResponse> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
		{
			Project? project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);

			if (project == null)
			{
				throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
			}

			return MapToDetailsResponse(project);
		}

		/// <inheritdoc />
		public async Task RemoveEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default)
		{
			Project? project = await _projectRepository.GetByIdWithDetailsAsync(projectId, cancellationToken);

			if (project == null)
			{
				throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
			}

			bool employeeAssigned = project.ProjectEmployees.Any(x => x.EmployeeId == employeeId);

			if (!employeeAssigned)
			{
				throw new InvalidOperationException("The employee is not assigned to the project.");
			}

			if (project.ProjectManagerId == employeeId)
			{
				throw new InvalidOperationException("The project manager cannot be removed from the project team.");
			}

			await _projectRepository.RemoveEmployeeAsync(projectId, employeeId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<ProjectDetailsResponse> UpdateAsync(Guid projectId, UpdateProjectRequest request, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(request);

			Project? existingProject = await _projectRepository.GetByIdWithDetailsAsync(projectId, cancellationToken);

			if (existingProject == null)
			{
				throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
			}

			await ValidateProjectRequestAsync(
				request.Name,
				request.CustomerCompanyName,
				request.ExecutorCompanyName,
				request.ProjectManagerId,
				request.StartDate,
				request.EndDate,
				request.EmployeeIds,
				cancellationToken);

			List<Guid> requestedEmployeeIds = NormalizeEmployeeIds(request.EmployeeIds, request.ProjectManagerId);

			existingProject.Name = request.Name.Trim();
			existingProject.CustomerCompanyName = request.CustomerCompanyName;
			existingProject.ExecutorCompanyName = request.ExecutorCompanyName;
			existingProject.ProjectManagerId = request.ProjectManagerId;
			existingProject.StartDate = request.StartDate;
			existingProject.EndDate = request.EndDate;
			existingProject.Priority = request.Priority;

			await _projectRepository.UpdateAsync(existingProject, cancellationToken);

			HashSet<Guid> currentEmployeeIds = existingProject.ProjectEmployees.Select(x => x.EmployeeId).ToHashSet();

			HashSet<Guid> newEmployeeIds = requestedEmployeeIds.ToHashSet();

			List<Guid> employeeIdsToAdd = newEmployeeIds.Except(currentEmployeeIds).ToList();

			List<Guid> employeeIdsToRemove = currentEmployeeIds.Except(newEmployeeIds).ToList();

			foreach (Guid employeeId in employeeIdsToAdd)
			{
				ProjectEmployee projectEmployee = new()
				{
					ProjectId = projectId,
					EmployeeId = employeeId,
					AssignedAt = DateTimeOffset.UtcNow
				};

				await _projectRepository.AddEmployeeAsync(projectEmployee, cancellationToken);
			}

			foreach (Guid employeeId in employeeIdsToRemove)
			{
				await _projectRepository.RemoveEmployeeAsync(projectId, employeeId, cancellationToken);
			}

			Project? updatedProject = await _projectRepository.GetByIdWithDetailsAsync(projectId, cancellationToken);

			if (updatedProject == null)
			{
				throw new InvalidOperationException("The updated project could not be loaded.");
			}

			return MapToDetailsResponse(updatedProject);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Maps a project entity to a details response.
		/// </summary>
		/// <param name="project">Project entity.</param>
		/// <returns>Project details response.</returns>
		private static ProjectDetailsResponse MapToDetailsResponse(Project project)
		{
			IReadOnlyCollection<ProjectEmployeeResponse> employees = project.ProjectEmployees
				.OrderBy(x => x.Employee.LastName)
				.Select(x => new ProjectEmployeeResponse(
					x.EmployeeId,
					$"{x.Employee.LastName} {x.Employee.FirstName} {x.Employee.MiddleName}".Trim(),
					x.Employee.Email,
					x.AssignedAt))
				.ToList();

			IReadOnlyCollection<ProjectDocumentResponse> documents = project.Documents
				.OrderByDescending(x => x.UploadedAt)
				.Select(x => new ProjectDocumentResponse(
					x.Id,
					x.FileName,
					x.ContentType,
					x.Size,
					x.UploadedAt))
				.ToList();

			return new ProjectDetailsResponse(
				project.Id,
				project.Name,
				project.CustomerCompanyName,
				project.ExecutorCompanyName,
				project.ProjectManagerId,
				$"{project.ProjectManager.LastName} {project.ProjectManager.FirstName} {project.ProjectManager.MiddleName}".Trim(),
				project.StartDate,
				project.EndDate,
				project.Priority,
				employees,
				documents);
		}

		/// <summary>
		/// Maps a project entity to a list item response.
		/// </summary>
		/// <param name="project">Project entity.</param>
		/// <returns>Project list item response.</returns>
		private static ProjectListItemResponse MapToListItemResponse(Project project)
		{
			return new ProjectListItemResponse(
				project.Id,
				project.Name,
				project.CustomerCompanyName,
				project.ExecutorCompanyName,
				$"{project.ProjectManager.LastName} {project.ProjectManager.FirstName} {project.ProjectManager.MiddleName}".Trim(),
				project.StartDate,
				project.EndDate,
				project.Priority);
		}

		/// <summary>
		/// Adds the project manager to the employee collection if needed and removes duplicates.
		/// </summary>
		/// <param name="employeeIds">Employee identifiers.</param>
		/// <param name="projectManagerId">Project manager identifier.</param>
		/// <returns>Normalized employee identifier list.</returns>
		private static List<Guid> NormalizeEmployeeIds(IReadOnlyCollection<Guid> employeeIds, Guid projectManagerId)
		{
			HashSet<Guid> normalizedEmployeeIds = employeeIds.ToHashSet();
			normalizedEmployeeIds.Add(projectManagerId);

			return normalizedEmployeeIds.ToList();
		}

		/// <summary>
		/// Validates common project request data.
		/// </summary>
		/// <param name="name">Project name.</param>
		/// <param name="customerCompanyId">Customer company identifier.</param>
		/// <param name="executorCompanyId">Executor company identifier.</param>
		/// <param name="projectManagerId">Project manager identifier.</param>
		/// <param name="startTime">Project start time.</param>
		/// <param name="endTime">Project end time.</param>
		/// <param name="employeeIds">Employee identifiers.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		private async Task ValidateProjectRequestAsync(string name, string customerCompanyName, string executorCompanyName, Guid projectManagerId, DateTimeOffset startTime, DateTimeOffset endTime, IReadOnlyCollection<Guid> employeeIds, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Project name is required.");
			}

			if (startTime > endTime)
			{
				throw new ArgumentException("Project start time cannot be greater than end time.");
			}

			if (string.IsNullOrWhiteSpace(customerCompanyName))
			{
				throw new ArgumentException("Customer company name is required.");
			}

			if (string.IsNullOrWhiteSpace(executorCompanyName))
			{
				throw new ArgumentException("Executor company name is required.");
			}

			bool projectManagerExists = await _employeeRepository.ExistsAsync(projectManagerId, cancellationToken);
			if (!projectManagerExists)
			{
				throw new KeyNotFoundException($"Project manager with id '{projectManagerId}' was not found.");
			}

			if (employeeIds.Count != employeeIds.Distinct().Count())
			{
				throw new ArgumentException("The employee list contains duplicate identifiers.");
			}

			foreach (Guid employeeId in employeeIds)
			{
				bool employeeExists = await _employeeRepository.ExistsAsync(employeeId, cancellationToken);

				if (!employeeExists)
				{
					throw new KeyNotFoundException($"Employee with id '{employeeId}' was not found.");
				}
			}
		}

		#endregion Private Methods
	}
}