namespace ProjectService.Core.Abstractions
{
	using ProjectService.Core.Contracts.Request;
	using ProjectService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface for repository for project data access.
	/// </summary>
	public interface IProjectRepository
	{
		#region Public Methods

		/// <summary>
		/// Adds a project.
		/// </summary>
		/// <param name="project">Project</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds employee to project.
		/// </summary>
		/// <param name="projectEmployee">Employee.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task AddEmployeeAsync(ProjectEmployee projectEmployee, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes the project.
		/// </summary>
		/// <param name="project">Project.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task DeleteAsync(Project project, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks if a project exists.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task<bool> ExistsAsync(Guid projectId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get all projects with optional filtration.
		/// </summary>
		/// <param name="request">Request for filtering.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task<IReadOnlyCollection<Project>> GetAllAsync(FiltredProjectsRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get project by Id.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get project by Id with details.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task<Project?> GetByIdWithDetailsAsync(Guid projectId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks if there is an employee in the project.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="employeeId">Employee Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task<bool> HasEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove employee from project.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="employeeId">Employee Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task RemoveEmployeeAsync(Guid projectId, Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update project.
		/// </summary>
		/// <param name="project">Project.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		/// <returns></returns>
		Task UpdateAsync(Project project, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}