namespace ProjectService.Core.Entities
{
	using System;

	/// <summary>
	/// Project document entity;
	/// </summary>
	public class ProjectDocument
	{
		#region Public Properties

		/// <summary>
		/// Project document Id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Project Id.
		/// </summary>
		public Guid ProjectId { get; set; }

		/// <summary>
		/// Project document name.
		/// </summary>
		public string FileName { get; set; } = null!;

		/// <summary>
		/// Project document name in storage.
		/// </summary>
		public string StoragedFileName { get; set; } = null!;

		/// <summary>
		/// Project document type.
		/// </summary>
		public string ContentType { get; set; } = null!;

		/// <summary>
		/// Project document size.
		/// </summary>
		public long Size { get; set; }

		/// <summary>
		/// Project document uploaded time.
		/// </summary>
		public DateTimeOffset UploadedAt { get; set; }

		/// <summary>
		/// Project to which the document relates.
		/// </summary>
		public Project Project { get; set; } = null!;

		#endregion Public Properties
	}
}