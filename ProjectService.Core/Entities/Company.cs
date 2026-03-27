namespace ProjectService.Core.Entities
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Company entity.
	/// </summary>
	public class Company
	{
		#region Public Properties

		/// <summary>
		/// Company Id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Company name.
		/// </summary>
		public string Name { get; set; } = null!;

		/// <summary>
		/// Projects where the company acts as a customer.
		/// </summary>
		public ICollection<Project> CustomerProjects { get; set; } = new List<Project>();

		/// <summary>
		/// Projects where the company acts as a executor.
		/// </summary>
		public ICollection<Project> ExecutorProjects { get; set; } = new List<Project>();

		#endregion Public Properties
	}
}