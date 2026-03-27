using ProjectService.Core.Enums;

namespace ProjectService.Core.Contracts.Request
{
	/// <summary>
	/// Request for getting projects with filtering and sorting.
	/// </summary>
	public record FiltredProjectsRequest(
		/// <summary>
		/// Lower bound of project start date.
		/// </summary>
		DateTimeOffset? StartDateFrom,

		/// <summary>
		/// Upper bound of project start date.
		/// </summary>
		DateTimeOffset? StartDateTo,

		/// <summary>
		/// Project priorities for filtering.
		/// </summary>
		IReadOnlyCollection<ProjectPriority>? Priorities,

		/// <summary>
		/// Customer company identifier.
		/// </summary>
		Guid? CustomerCompanyId,

		/// <summary>
		/// Executor company identifier.
		/// </summary>
		Guid? ExecutorCompanyId,

		/// <summary>
		/// Project manager identifier.
		/// </summary>
		Guid? ProjectManagerId,

		/// <summary>
		/// Sort field.
		/// </summary>
		string? SortBy,

		/// <summary>
		/// Indicates whether sorting should be descending.
		/// </summary>
		bool SortDescending
	);
}