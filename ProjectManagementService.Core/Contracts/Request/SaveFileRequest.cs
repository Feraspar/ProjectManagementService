namespace ProjectManagementService.Core.Contracts.Request
{
	using System;

	/// <summary>
	/// Request for saving a file to storage.
	/// </summary>
	/// <param name="ProjectId">Project identifier.</param>
	/// <param name="FileName">Original file name.</param>
	/// <param name="ContentType">File content type.</param>
	/// <param name="Content">File content stream.</param>
	public record SaveFileRequest(

		Guid ProjectId,

		string FileName,

		string ContentType,

		Stream Content
	);
}