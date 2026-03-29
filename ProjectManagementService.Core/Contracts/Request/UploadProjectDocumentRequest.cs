namespace ProjectManagementService.Core.Contracts.Request
{
	using System;

	/// <summary>
	/// Request for uploading a project document.
	/// </summary>
	/// <param name="ProjectId">Project identifier.</param>
	/// <param name="FileName">Original file name.</param>
	/// <param name="ContentType">File content type.</param>
	/// <param name="Content">File content stream.</param>
	public record UploadProjectDocumentRequest(

		Guid ProjectId,

		string FileName,

		string ContentType,

		Stream Content
	);
}