namespace ProjectManagementService.Infrastructure.Repositories
{
	using Microsoft.EntityFrameworkCore;
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Entities;
	using ProjectManagementService.Infrastructure.Persistence;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Repository for employee data access.
	/// </summary>
	public class EmployeeRepository : IEmployeeRepository
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
		public EmployeeRepository(ProjectServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default)
		{
			_dbContext.Employees.Add(employee);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return employee;
		}

		/// <inheritdoc />
		public async Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default)
		{
			_dbContext.Employees.Remove(employee);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Guid employeeId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Employees.AnyAsync(x => x.Id == employeeId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _dbContext.Employees.AsNoTracking().OrderBy(x => x.LastName).ToListAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Employee?> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludedEmployeeId = null, CancellationToken cancellationToken = default)
		{
			string normilizedEmail = email.Trim().ToLowerInvariant();

			IQueryable<Employee> query = _dbContext.Employees.AsNoTracking().Where(x => x.Email.ToLower() == normilizedEmail);

			if (excludedEmployeeId.HasValue)
			{
				query = query.Where(x => x.Id != excludedEmployeeId.Value);
			}

			return !await query.AnyAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<Employee>> SearchAsync(string? searchTerm, int take = 10, CancellationToken cancellationToken = default)
		{
			IQueryable<Employee> query = _dbContext.Employees.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				string normilizedSearchTerm = searchTerm.Trim().ToLower();

				query = query.Where(x => x.FirstName.ToLower().Contains(normilizedSearchTerm) ||
				x.LastName.ToLower().Contains(normilizedSearchTerm) ||
				(x.MiddleName != null && x.MiddleName.ToLower().Contains(normilizedSearchTerm)) ||
				x.Email.ToLower().Contains(normilizedSearchTerm));
			}

			return await query.OrderBy(x => x.LastName).Take(take).ToListAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
		{
			_dbContext.Employees.Update(employee);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		#endregion Public Methods
	}
}