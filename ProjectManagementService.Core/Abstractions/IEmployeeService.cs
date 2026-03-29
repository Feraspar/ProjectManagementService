namespace ProjectManagementService.Core.Abstractions
{
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Contracts.Response;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IEmployeeService
	{
		#region Public Methods

		/// <summary>
		/// Creates a new employee.
		/// </summary>
		/// <param name="request">Employee creation request.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<EmployeeListResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes an employee.
		/// </summary>
		/// <param name="employeeId">Employee Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task DeleteAsync(Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets all employees.
		/// </summary>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<IReadOnlyCollection<EmployeeListResponse>> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets an employee by identifier.
		/// </summary>
		/// <param name="employeeId">Employee Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<EmployeeListResponse> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Searches employees.
		/// </summary>
		/// <param name="searchTerm">Search term.</param>
		/// <param name="take">Maximum number of records to return.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<IReadOnlyCollection<EmployeeListResponse>> SearchAsync(string? searchTerm, int take = 10, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing employee.
		/// </summary>
		/// <param name="employeeId">Employee Id.</param>
		/// <param name="request">Employee update request.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<EmployeeListResponse> UpdateAsync(Guid employeeId, UpdateEmployeeRequest request, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}