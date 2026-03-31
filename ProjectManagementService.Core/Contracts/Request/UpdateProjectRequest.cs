namespace ProjectManagementService.Core.Contracts.Request
{
	using ProjectManagementService.Core.Enums;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// DTO for update project reqquest.
	/// </summary>
	/// <param name="Name">Project name.</param>
	/// <param name="CustomerCompanyName">Customer company name.</param>
	/// <param name="ExecutorCompanyName">Executor company name.</param>
	/// <param name="ProjectManagerId">Project manager Id.</param>
	/// <param name="StartDate">Start date project.</param>
	/// <param name="EndDate">End date project.</param>
	/// <param name="Priority">Project prioritiy.</param>
	/// <param name="EmployeeIds">Ids of employees.</param>
	public record UpdateProjectRequest(

		string Name,

		string CustomerCompanyName,

		string ExecutorCompanyName,

		Guid ProjectManagerId,

		DateTime StartDate,

		DateTime EndDate,

		ProjectPriority Priority,

		IReadOnlyCollection<Guid> EmployeeIds
	);
}