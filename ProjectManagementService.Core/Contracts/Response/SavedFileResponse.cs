namespace ProjectManagementService.Core.Contracts.Response
{
	/// <summary>
	/// Response with saved file metadata.
	/// </summary>
	/// <param name="FileName">Original file name.</param>
	/// <param name="StoredFileName">Stored file name.</param>
	/// <param name="Path">Stored file path.</param>
	/// <param name="ContentType">File content type.</param>
	/// <param name="Size">File size in bytes.</param>
	public record SavedFileResponse(

		string FileName,

		string StoredFileName,

		string Path,

		string ContentType,

		long Size
	);
}