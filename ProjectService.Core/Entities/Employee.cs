namespace ProjectService.Core.Entities
{
	using System;

	/// <summary>
	/// Employee entity.
	/// </summary>
	public class Employee
	{
		#region Public Properties

		/// <summary>
		/// Employee Id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Employee email.
		/// </summary>
		public string Email { get; set; } = null!;

		/// <summary>
		/// Employee first name.
		/// </summary>
		public string FirstName { get; set; } = null!;

		/// <summary>
		/// Employee last name.
		/// </summary>
		public string LastName { get; set; } = null!;

		/// <summary>
		/// Employee middle name.
		/// </summary>
		public string? MiddleName { get; set; }

		/// <summary>
		/// Projects where emploee acts as a manager.
		/// </summary>
		public ICollection<Project> ManagedProjects = new List<Project>();

		public ICollection<ProjectEmployee> ProjectEmployees = new List<ProjectEmployee>();

		#endregion Public Properties
	}
}