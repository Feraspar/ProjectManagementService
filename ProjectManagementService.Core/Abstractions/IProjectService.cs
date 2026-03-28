namespace ProjectManagementService.Core.Abstractions
{
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Contracts.Response;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface for service for project business logic.
	/// </summary>
	public interface IProjectService
	{
		#region Public Methods

		/// <summary>
		/// Adds an employee to a project.
		/// </summary>
		/// <param name="projectId">Project identifier.</param>
		/// <param name="employeeId">Employee identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task AddEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// <param name="request">Project creation request.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task<ProjectDetailsResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes a project.
		/// </summary>
		/// <param name="projectId">Project identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task DeleteAsync(Guid projectId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets a filtered and sorted project list.
		/// </summary>
		/// <param name="request">Filtering and sorting request.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task<IReadOnlyCollection<ProjectListItemResponse>> GetAllAsync(FiltredProjectsRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets a project by identifier with full details.
		/// </summary>
		/// <param name="projectId">Project identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task<ProjectDetailsResponse> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes an employee from a project.
		/// </summary>
		/// <param name="projectId">Project identifier.</param>
		/// <param name="employeeId">Employee identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task RemoveEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing project.
		/// </summary>
		/// <param name="projectId">Project identifier.</param>
		/// <param name="request">Project update request.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task<ProjectDetailsResponse> UpdateAsync(Guid projectId, UpdateProjectRequest request, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}