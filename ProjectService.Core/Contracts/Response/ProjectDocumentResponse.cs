namespace ProjectService.Core.Contracts.Response
{
	using System;

	/// <summary>
	/// DTO for document response.
	/// </summary>
	/// <param name="Id">Document Id.</param>
	/// <param name="FileName">File name.</param>
	/// <param name="ContentType">Content type.</param>
	/// <param name="Size">Document Size.</param>
	/// <param name="UploadedAt">Uploaded time.</param>
	public record ProjectDocumentResponse(

		Guid Id,

		string FileName,

		string ContentType,

		long Size,

		DateTimeOffset UploadedAt
	);
}