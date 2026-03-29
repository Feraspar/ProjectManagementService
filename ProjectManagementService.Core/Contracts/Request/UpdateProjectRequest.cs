namespace ProjectManagementService.Core.Contracts.Request
{
	using ProjectManagementService.Core.Enums;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// DTO for update project reqquest.
	/// </summary>
	/// <param name="Name">Project name.</param>
	/// <param name="CustomerCompanyId">Customer company Id.</param>
	/// <param name="ExecutorCompanyId">Executor company Id.</param>
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

		DateTimeOffset StartDate,

		DateTimeOffset EndDate,

		ProjectPriority Priority,

		IReadOnlyCollection<Guid> EmployeeIds
	);
}