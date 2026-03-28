namespace ProjectManagementService.Core.Abstractions
{
	using ProjectManagementService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface for repository for company data access.
	/// </summary>
	public interface ICompanyRepository
	{
		#region Public Methods

		/// <summary>
		/// Add company to database.
		/// </summary>
		/// <param name="company">Company.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<Company> AddAsync(Company company, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete company from database.
		/// </summary>
		/// <param name="company">Company.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task DeleteAsync(Company company, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks whether an company exists.
		/// </summary>
		/// <param name="companyId">Employee identifier.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<bool> ExistsAsync(Guid companyId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get all companies from database.
		/// </summary>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<IReadOnlyCollection<Company>> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Get company by id from database.
		/// </summary>
		/// <param name="companyId">Employee Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<Company?> GetByIdAsync(Guid companyId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update company data.
		/// </summary>
		/// <param name="company">Employee.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task UpdateAsync(Company company, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}