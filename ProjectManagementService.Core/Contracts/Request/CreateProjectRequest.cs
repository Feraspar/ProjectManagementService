namespace ProjectManagementService.Core.Contracts.Request
{
	using ProjectManagementService.Core.Enums;

	/// <summary>
	/// DTO for create project request.
	/// </summary>
	/// <param name="Name">Project name.</param>
	/// <param name="CustomerCompanyId">Customer company Id.</param>
	/// <param name="ExecutorCompanyId">Executor company Id.</param>
	/// <param name="ProjectManagerId">Project manager Id.</param>
	/// <param name="StartDate">Start date project.</param>
	/// <param name="EndDate">End date project.</param>
	/// <param name="Priority">Project prioritiy.</param>
	/// <param name="EmployeeIds">Ids of employees.</param>
	public record CreateProjectRequest(

		string Name,

		Guid CustomerCompanyId,

		Guid ExecutorCompanyId,

		Guid ProjectManagerId,

		DateTimeOffset StartDate,

		DateTimeOffset EndDate,

		ProjectPriority Priority,

		IReadOnlyCollection<Guid> EmployeeIds
	);
}