namespace ProjectManagementService.Core.Abstractions
{
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Contracts.Response;
	using System.Threading.Tasks;

	public interface IFileStorageService
	{
		#region Public Methods

		/// <summary>
		/// Deletes a file from storage.
		/// </summary>
		/// <param name="path">Stored file path.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		Task DeleteFileAsync(string path, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks whether a file exists in storage.
		/// </summary>
		/// <param name="path">Stored file path.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>True if the file exists; otherwise false.</returns>
		Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);

		/// <summary>
		/// Saves a file to storage.
		/// </summary>
		/// <param name="request">Save file request.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Saved file metadata.</returns>
		Task<SavedFileResponse> SaveFileAsync(SaveFileRequest request, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}