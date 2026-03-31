namespace ProjectManagementService.Core.Contracts.Request
{
	using ProjectManagementService.Core.Enums;

	/// <summary>
	/// Request for getting projects with filtering and sorting.
	/// </summary>
	/// <param name="StartDateFrom">Lower bound of project start date.</param>
	/// <param name="StartDateTo">Upper bound of project start date.</param>
	/// <param name="Priorities">Project priorities for filtering.</param>
	/// /// <param name="CustomerCompanyName">Customer company name for filtering.</param>
	/// <param name="ExecutorCompanyName">Executor company name for filtering.</param>
	/// <param name="ProjectManagerId">Project manager Id.</param>
	/// <param name="SortBy">Sort field.</param>
	/// <param name="SortDescending">Indicates whether sorting should be descending.</param>
	public record FiltredProjectsRequest(

		DateTime? StartDateFrom = null,

		DateTime? StartDateTo = null,

		IReadOnlyCollection<ProjectPriority>? Priorities = null,

		string? CustomerCompanyName = null,

		string? ExecutorCompanyName = null,

		Guid? ProjectManagerId = null,

		string? SortBy = null,

		bool SortDescending = false
	);
}