namespace ProjectService.Core.Contracts.Response
{
	using ProjectService.Core.Enums;
	using System;

	/// <summary>
	/// DTO for projects response.
	/// </summary>
	/// <param name="Id">Project Id.</param>
	/// <param name="Name">Project name.</param>
	/// <param name="CustomerCompanyId">Customer company Id.</param>
	/// <param name="ExecutorCompanyId">Executor company Id.</param>
	/// <param name="ProjectManagerId">Project manager Id.</param>
	/// <param name="StartDate">Start date project.</param>
	/// <param name="EndDate">End date project.</param>
	/// <param name="Prioritiy">Project prioritiy.</param>
	public record ProjectListItemResponse(

		Guid Id,

		string Name,

		Guid? CustomerCompanyId,

		Guid? ExecutorCompanyId,

		Guid? ProjectManagerId,

		DateTimeOffset? StartDate,

		DateTimeOffset? EndDate,

		ProjectPriority Prioritiy
	);
}