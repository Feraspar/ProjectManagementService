namespace ProjectService.Infrastructure.Repositories
{
	using Microsoft.EntityFrameworkCore;
	using ProjectService.Core.Abstractions;
	using ProjectService.Core.Entities;
	using ProjectService.Infrastructure.Persistence;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class ProjectDocumentRepository : IProjectDocumentRepository
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
		public ProjectDocumentRepository(ProjectServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<ProjectDocument> AddAsync(ProjectDocument projectDocument, CancellationToken cancellationToken = default)
		{
			_dbContext.ProjectDocuments.Add(projectDocument);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return projectDocument;
		}

		/// <inheritdoc />
		public async Task DeleteAsync(ProjectDocument projectDocument, CancellationToken cancellationToken = default)
		{
			_dbContext.ProjectDocuments.Remove(projectDocument);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Guid projectDocumentId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.ProjectDocuments.AnyAsync(x => x.Id == projectDocumentId, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<ProjectDocument>> GetAllAsync(Guid projectId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.ProjectDocuments.AsNoTracking().Where(x => x.ProjectId == projectId).OrderByDescending(x => x.UploadedAt).ToListAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<ProjectDocument?> GetByIdAsync(Guid projectDocumentId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.ProjectDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == projectDocumentId, cancellationToken);
		}

		#endregion Public Methods
	}
}