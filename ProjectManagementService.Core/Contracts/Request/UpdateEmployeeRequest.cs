namespace ProjectManagementService.Core.Contracts.Request
{
	/// <summary>
	/// DTO for update employee request.
	/// </summary>
	/// <param name="FirstName">First name.</param>
	/// <param name="LastName">Last name.</param>
	/// <param name="MiddleName">Middle name.</param>
	/// <param name="Email">Email.</param>
	public record UpdateEmployeeRequest(

		string FirstName,

		string LastName,

		string? MiddleName,

		string Email
	);
}