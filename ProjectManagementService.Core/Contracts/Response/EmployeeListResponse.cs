namespace ProjectManagementService.Core.Contracts.Response
{
	using System;

	/// <summary>
	/// DTO for employee list response.
	/// </summary>
	/// <param name="Id">Employee Id.</param>
	/// <param name="FirstName">First name.</param>
	/// <param name="LastName">Last name.</param>
	/// <param name="MiddleName">Middle name.</param>
	/// <param name="Email">Email.</param>
	/// <param name="FullName">Full name.</param>
	public record EmployeeListResponse(

		Guid Id,

		string FirstName,

		string LastName,

		string? MiddleName,

		string Email,

		string FullName
	);
}