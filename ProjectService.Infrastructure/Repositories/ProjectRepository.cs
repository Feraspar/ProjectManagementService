namespace ProjectService.Infrastructure.Repositories
{
	using Microsoft.EntityFrameworkCore;
	using ProjectService.Core.Abstractions;
	using ProjectService.Core.Contracts.Request;
	using ProjectService.Core.Entities;
	using ProjectService.Infrastructure.Persistence;
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
			await _dbContext.Projects.AddAsync(project, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return project;
		}

		/// <inheritdoc />
		public async Task AddEmployeeAsync(ProjectEmployee projectEmployee, CancellationToken cancellationToken = default)
		{
			await _dbContext.ProjectEmployees.AddAsync(projectEmployee, cancellationToken);
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
			IQueryable<Project> query = _dbContext.Projects.AsNoTracking().Include(x => x.CustomerCompany).Include(x => x.ExecutorCompany).Include(x => x.ProjectManager);

			if (request.StartDateFrom.HasValue)
			{
				query = query.Where(x => x.StartDate >= request.StartDateFrom.Value);
			}

			if (request.StartDateTo.HasValue)
			{
				query = query.Where(x => x.StartDate <= request.StartDateTo.Value);
			}

			if (request.Priorities is not null && request.Priorities.Any())
			{
				query = query.Where(x => request.Priorities.Contains(x.Priority));
			}

			if (request.CustomerCompanyId.HasValue)
			{
				query = query.Where(x => x.CustomerCompanyId == request.CustomerCompanyId.Value);
			}

			if (request.ExecutorCompanyId.HasValue)
			{
				query = query.Where(x => x.ExecutorCompanyId == request.ExecutorCompanyId.Value);
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
				.Include(x => x.CustomerCompany)
				.Include(x => x.ExecutorCompany)
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

				"customercompany" => sortDescending
					? query.OrderByDescending(x => x.CustomerCompany.Name)
					: query.OrderBy(x => x.CustomerCompany.Name),

				"executorcompany" => sortDescending
					? query.OrderByDescending(x => x.ExecutorCompany.Name)
					: query.OrderBy(x => x.ExecutorCompany.Name),

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