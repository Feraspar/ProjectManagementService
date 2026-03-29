namespace ProjectManagementService.Core.Abstractions
{
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Contracts.Response;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface for service for project document business logic.
	/// </summary>
	public interface IProjectDocumentService
	{
		#region Public Methods

		/// <summary>
		/// Deletes a project document.
		/// </summary>
		/// <param name="documentId">Document identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task DeleteAsync(Guid documentId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets a project document by identifier.
		/// </summary>
		/// <param name="documentId">Document identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task<ProjectDocumentResponse> GetByIdAsync(Guid documentId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets all documents of the specified project.
		/// </summary>
		/// <param name="projectId">Project identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task<IReadOnlyCollection<ProjectDocumentResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Uploads a new project document.
		/// </summary>
		/// <param name="request">Document upload request.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Created project document response.</returns>
		Task<ProjectDocumentResponse> UploadAsync(UploadProjectDocumentRequest request, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}