namespace ProjectManagementService.Core.Entities
{
	using ProjectManagementService.Core.Enums;
	using System;

	/// <summary>
	/// Project entity.
	/// </summary>
	public class Project
	{
		#region Public Properties

		/// <summary>
		/// Project Id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Project name.
		/// </summary>
		public string Name { get; set; } = null!;

		/// <summary>
		/// Customer company name.
		/// </summary>
		public string CustomerCompanyName { get; set; } = null!;

		/// <summary>
		/// Executor company name.
		/// </summary>
		public string ExecutorCompanyName { get; set; } = null!;

		/// <summary>
		/// Project manager Id.
		/// </summary>
		public Guid ProjectManagerId { get; set; }

		/// <summary>
		/// Project start time.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Project end time.
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Project priority.
		/// </summary>
		public ProjectPriority Priority { get; set; }

		/// <summary>
		/// Project manager.
		/// </summary>
		public Employee ProjectManager { get; set; } = null!;

		/// <summary>
		/// Project employees.
		/// </summary>
		public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();

		/// <summary>
		/// Project documents.
		/// </summary>
		public ICollection<ProjectDocument> Documents { get; set; } = new List<ProjectDocument>();

		#endregion Public Properties
	}
}