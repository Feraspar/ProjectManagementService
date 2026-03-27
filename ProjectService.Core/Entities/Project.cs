namespace ProjectService.Core.Entities
{
	using ProjectService.Core.Enums;
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
		/// Customer company Id.
		/// </summary>
		public Guid CustomerCompanyId { get; set; }

		/// <summary>
		/// Executor company Id.
		/// </summary>
		public Guid ExecutorCompanyId { get; set; }

		/// <summary>
		/// Project manager Id.
		/// </summary>
		public Guid ProjectManagerId { get; set; }

		/// <summary>
		/// Project start time.
		/// </summary>
		public DateTimeOffset StartTime { get; set; }

		/// <summary>
		/// Project end time.
		/// </summary>
		public DateTimeOffset EndTime { get; set; }

		/// <summary>
		/// Project priority.
		/// </summary>
		public ProjectPriority Priority { get; set; }

		/// <summary>
		/// Project manager.
		/// </summary>
		public Employee ProjectManager { get; set; } = null!;

		/// <summary>
		/// Customer company.
		/// </summary>
		public Company CustomerCompany { get; set; } = null!;

		/// <summary>
		/// Executor company.
		/// </summary>
		public Company ExecutorCompany { get; set; } = null!;

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