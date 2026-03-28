namespace ProjectService.Core.Contracts.Response
{
	using System;

	/// <summary>
	/// DTO for employee in project response.
	/// </summary>
	/// <param name="EmployeeId">Employee Id.</param>
	/// <param name="FullName">Full name of employee.</param>
	/// <param name="email">Employee email.</param>
	/// <param name="AssignedAt">Assigned time.</param>
	public record ProjectEmployeeResponse(

		Guid EmployeeId,

		string FullName,

		string email,

		DateTimeOffset AssignedAt
	);
}