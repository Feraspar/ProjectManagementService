namespace ProjectManagementService.Core.Entities
{
	using System;

	/// <summary>
	/// Link table for employees and projects.
	/// </summary>
	public class ProjectEmployee
	{
		#region Public Properties

		/// <summary>
		/// Project Id.
		/// </summary>
		public Guid ProjectId { get; set; }

		/// <summary>
		/// Employee Id.
		/// </summary>
		public Guid EmployeeId { get; set; }

		/// <summary>
		/// Assigned time.
		/// </summary>
		public DateTimeOffset AssignedAt { get; set; }

		/// <summary>
		/// Assigned employee.
		/// </summary>
		public Employee Employee { get; set; } = null!;

		/// <summary>
		/// Project.
		/// </summary>
		public Project Project { get; set; } = null!;

		#endregion Public Properties
	}
}