namespace ProjectManagementService.Core.Contracts.Request
{
	using System;

	/// <summary>
	/// Request for uploading a project document.
	/// </summary>
	/// <param name="ProjectId">Project identifier.</param>
	/// <param name="FileName">Original file name.</param>
	/// <param name="ContentType">File content type.</param>
	/// <param name="Size">File size in bytes.</param>
	/// <param name="Path">Stored file path.</param>
	/// <param name="StoredFileName">Stored file name.</param>
	public record UploadProjectDocumentRequest(

		Guid ProjectId,

		string FileName,

		string ContentType,

		long Size,

		string Path,

		string StoredFileName
	);
}