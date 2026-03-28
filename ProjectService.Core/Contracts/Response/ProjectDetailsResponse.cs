namespace ProjectService.Core.Contracts.Response
{
	using ProjectService.Core.Enums;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// DTO for project details response.
	/// </summary>
	/// <param name="Id">Project Id.</param>
	/// <param name="Name">Project name.</param>
	/// <param name="CustomerCompanyId">Customer company Id.</param>
	/// <param name="CustomerCompanyName">Customer company name.</param>
	/// <param name="ExecutorCompanyId">Executor company Id.</param>
	/// <param name="ExecutorCompanyName">Executor company name.</param>
	/// <param name="ProjectManagerId">Project manager Id.</param>
	/// <param name="ProjectManagerFullName">Project manager name.</param>
	/// <param name="StartDate">Start date project.</param>
	/// <param name="EndDate">End date project.</param>
	/// <param name="Prioritiy">Project prioritiy.</param>
	/// <param name="Employees">Collection of employees on project.</param>
	/// <param name="Documents">Collection of documents in project.</param>
	public record ProjectDetailsResponse(

		Guid Id,

		string Name,

		Guid? CustomerCompanyId,
		string CustomerCompanyName,

		Guid? ExecutorCompanyId,
		string ExecutorCompanyName,

		Guid? ProjectManagerId,
		string ProjectManagerFullName,

		DateTimeOffset? StartDate,

		DateTimeOffset? EndDate,

		ProjectPriority Prioritiy,

		IReadOnlyCollection<ProjectEmployeeResponse> Employees,

		IReadOnlyCollection<ProjectDocumentResponse> Documents
	);
}