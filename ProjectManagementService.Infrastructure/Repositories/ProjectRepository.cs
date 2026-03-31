namespace ProjectManagementService.Infrastructure.Repositories
{
	using Microsoft.EntityFrameworkCore;
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Entities;
	using ProjectManagementService.Infrastructure.Persistence;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Repository for project data access.
	/// </summary>
	public class ProjectRepository : IProjectRepository
	{
		#region Private Fields

		/// <summary>
		/// Database context.
		/// </summary>
		private readonly ProjectServiceDbContext _dbContext;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="dbContext">Database context.</param>
		public ProjectRepository(ProjectServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
		{
			_dbContext.Projects.Add(project);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return project;
		}

		/// <inheritdoc />
		public async Task AddEmployeeAsync(ProjectEmployee projectEmployee, CancellationToken cancellationToken = default)
		{
			_dbContext.ProjectEmployees.Add(projectEmployee);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
		{
			_dbContext.Projects.Remove(project);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Guid projectId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Projects.AnyAsync(x => x.Id == projectId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<Project>> GetAllAsync(FiltredProjectsRequest request, CancellationToken cancellationToken = default)
		{
			IQueryable<Project> query = _dbContext.Projects.AsNoTracking().Include(x => x.ProjectManager);

			if (request.StartDateFrom.HasValue)
			{
				query = query.Where(x => x.StartDate >= request.StartDateFrom.Value);
			}

			if (request.StartDateTo.HasValue)
			{
				DateTime normilizedStartDateTo = request.StartDateTo.Value.Date.AddDays(1).AddTicks(-1);
				query = query.Where(x => x.StartDate <= normilizedStartDateTo);
			}

			if (!string.IsNullOrWhiteSpace(request.CustomerCompanyName))
			{
				string normalizedCustomerCompanyName = request.CustomerCompanyName.Trim().ToLower();

				query = query.Where(x => x.CustomerCompanyName.ToLower().Contains(normalizedCustomerCompanyName));
			}

			if (!string.IsNullOrWhiteSpace(request.ExecutorCompanyName))
			{
				string normalizedExecutorCompanyName = request.ExecutorCompanyName.Trim().ToLower();

				query = query.Where(x => x.ExecutorCompanyName.ToLower().Contains(normalizedExecutorCompanyName));
			}

			if (request.Priorities is not null && request.Priorities.Any())
			{
				query = query.Where(x => request.Priorities.Contains(x.Priority));
			}

			if (request.ProjectManagerId.HasValue)
			{
				query = query.Where(x => x.ProjectManagerId == request.ProjectManagerId.Value);
			}

			query = ApplySorting(query, request.SortBy, request.SortDescending);

			return await query.ToListAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Project?> GetByIdWithDetailsAsync(Guid projectId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Projects.AsNoTracking()
				.Include(x => x.ProjectManager)
				.Include(x => x.Documents)
				.Include(x => x.ProjectEmployees).ThenInclude(x => x.Employee)
				.FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> HasEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.ProjectEmployees.AnyAsync(x => x.ProjectId == projectId && x.EmployeeId == employeeId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default)
		{
			ProjectEmployee? projectEmployee = await _dbContext.ProjectEmployees.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.EmployeeId == employeeId, cancellationToken);

			if (projectEmployee == null)
			{
				return;
			}

			_dbContext.ProjectEmployees.Remove(projectEmployee);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
		{
			_dbContext.Projects.Update(project);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Applies sorting to the project query.
		/// </summary>
		/// <param name="query">The source query containing projects to sort.</param>
		/// <param name="sortBy">The field to sort by.</param>
		/// <param name="sortDescending">A value indicating whether the sorting should be performed in descending order.</param>
		private static IQueryable<Project> ApplySorting(IQueryable<Project> query, string? sortBy, bool sortDescending)
		{
			string normalizedSortBy = sortBy?.Trim().ToLowerInvariant() ?? "name";

			return normalizedSortBy switch
			{
				"starttime" => sortDescending
					? query.OrderByDescending(x => x.StartDate)
					: query.OrderBy(x => x.StartDate),

				"endtime" => sortDescending
					? query.OrderByDescending(x => x.EndDate)
					: query.OrderBy(x => x.EndDate),

				"priority" => sortDescending
					? query.OrderByDescending(x => x.Priority)
					: query.OrderBy(x => x.Priority),

				"customercompanyname" => sortDescending
				? query.OrderByDescending(x => x.CustomerCompanyName)
				: query.OrderBy(x => x.CustomerCompanyName),

				"executorcompanyname" => sortDescending
					? query.OrderByDescending(x => x.ExecutorCompanyName)
					: query.OrderBy(x => x.ExecutorCompanyName),

				"projectmanager" => sortDescending
					? query.OrderByDescending(x => x.ProjectManager.LastName).ThenByDescending(x => x.ProjectManager.FirstName)
					: query.OrderBy(x => x.ProjectManager.LastName).ThenBy(x => x.ProjectManager.FirstName),

				_ => sortDescending
					? query.OrderByDescending(x => x.Name)
					: query.OrderBy(x => x.Name)
			};
		}

		#endregion Private Methods
	}
}