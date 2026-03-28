namespace ProjectManagementService.Infrastructure.Repositories
{
	using Microsoft.EntityFrameworkCore;
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Entities;
	using ProjectManagementService.Infrastructure.Persistence;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Repository for company data access.
	/// </summary>
	public class CompanyRepository : ICompanyRepository
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
		public CompanyRepository(ProjectServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<Company> AddAsync(Company company, CancellationToken cancellationToken = default)
		{
			_dbContext.Companies.Add(company);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return company;
		}

		/// <inheritdoc />
		public async Task DeleteAsync(Company company, CancellationToken cancellationToken = default)
		{
			_dbContext.Companies.Remove(company);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Guid companyId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Companies.AnyAsync(x => x.Id == companyId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<Company>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _dbContext.Companies.AsNoTracking().OrderBy(x => x.Name).ToListAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Company?> GetByIdAsync(Guid companyId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == companyId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(Company company, CancellationToken cancellationToken = default)
		{
			_dbContext.Companies.Update(company);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		#endregion Public Methods
	}
}