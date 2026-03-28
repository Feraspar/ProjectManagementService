namespace ProjectService.Core.Abstractions
{
	using ProjectService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface for repository for documents.
	/// </summary>
	public interface IProjectDocumentRepository
	{
		#region Public Methods

		/// <summary>
		/// Add document to database.
		/// </summary>
		/// <param name="projectDocument">Company.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<ProjectDocument> AddAsync(ProjectDocument projectDocument, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete document from database.
		/// </summary>
		/// <param name="projectDocument">Company.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task DeleteAsync(ProjectDocument projectDocument, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks whether an document exists.
		/// </summary>
		/// <param name="projectDocumentId">Employee identifier.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<bool> ExistsAsync(Guid projectDocumentId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get all documents by project id from database.
		/// </summary>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<IReadOnlyCollection<ProjectDocument>> GetAllAsync(Guid projectId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get document by id from database.
		/// </summary>
		/// <param name="projectDocumentId">Employee Id.</param>
		/// <param name="cancellationToken">Token to cancel the operation in progress.</param>
		Task<ProjectDocument?> GetByIdAsync(Guid projectDocumentId, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}