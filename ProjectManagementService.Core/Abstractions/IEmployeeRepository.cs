namespace ProjectManagementService.Core.Abstractions
{
	using ProjectManagementService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface for repository for employee data access.
	/// </summary>
	public interface IEmployeeRepository
	{
		#region Public Methods

		/// <summary>
		/// Add employee to database.
		/// </summary>
		/// <param name="employee">Employee.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete employee from database.
		/// </summary>
		/// <param name="employee">Employee.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks whether an employee exists.
		/// </summary>
		/// <param name="employeeId">Employee identifier.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<bool> ExistsAsync(Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get all employees from database.
		/// </summary>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Get employee by id from database.
		/// </summary>
		/// <param name="employeeId">Employee Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<Employee?> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks whether an email is unique.
		/// </summary>
		/// <param name="email">Email address.</param>
		/// <param name="excludedEmployeeId">Employee identifier to exclude from uniqueness check.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<bool> IsEmailUniqueAsync(string email, Guid? excludedEmployeeId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Searches employees by full name or email.
		/// </summary>
		/// <param name="searchTerm">Search term.</param>
		/// <param name="take">Maximum number of records to return.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<IReadOnlyCollection<Employee>> SearchAsync(string? searchTerm, int take = 20, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update employee data.
		/// </summary>
		/// <param name="employee">Employee.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}