namespace ProjectManagementService.Core.Contracts.Response
{
	/// <summary>
	/// Error response.
	/// </summary>
	/// <param name="StatusCode">HTTP status code.</param>
	/// <param name="Message">Error message.</param>
	public record ErrorResponse(

		int StatusCode,

		string Message
	);
}