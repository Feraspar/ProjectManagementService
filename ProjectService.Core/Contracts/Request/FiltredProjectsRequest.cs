using ProjectService.Core.Enums;

namespace ProjectService.Core.Contracts.Request
{
	/// <summary>
	/// Request for getting projects with filtering and sorting.
	/// </summary>
	/// <param name="StartDateFrom">Lower bound of project start date.</param>
	/// <param name="StartDateTo">Upper bound of project start date.</param>
	/// <param name="Priorities">Project priorities for filtering.</param>
	/// <param name="CustomerCompanyId">Customer company Id.</param>
	/// <param name="ExecutorCompanyId">Executor company Id.</param>
	/// <param name="ProjectManagerId">Project manager Id.</param>
	/// <param name="SortBy">Sort field.</param>
	/// <param name="SortDescending">Indicates whether sorting should be descending.</param>
	public record FiltredProjectsRequest(

		DateTimeOffset? StartDateFrom,

		DateTimeOffset? StartDateTo,

		IReadOnlyCollection<ProjectPriority>? Priorities,

		Guid? CustomerCompanyId,

		Guid? ExecutorCompanyId,

		Guid? ProjectManagerId,

		string? SortBy,

		bool SortDescending
	);
}