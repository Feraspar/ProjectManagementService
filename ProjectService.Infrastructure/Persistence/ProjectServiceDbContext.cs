namespace ProjectService.Infrastructure.Persistence
{
	using Microsoft.EntityFrameworkCore;
	using ProjectService.Core.Entities;

	/// <summary>
	/// Database context.
	/// </summary>
	public class ProjectServiceDbContext : DbContext
	{
		#region Public Properties

		/// <summary>
		/// Table for companies.
		/// </summary>
		public DbSet<Company> Companies => Set<Company>();

		/// <summary>
		/// Table for employees.
		/// </summary>
		public DbSet<Employee> Employees => Set<Employee>();

		/// <summary>
		/// Table for documents.
		/// </summary>
		public DbSet<ProjectDocument> ProjectDocuments => Set<ProjectDocument>();

		/// <summary>
		/// Link table for projects and employees.
		/// </summary>
		public DbSet<ProjectEmployee> ProjectEmployees => Set<ProjectEmployee>();

		/// <summary>
		/// Table for projects.
		/// </summary>
		public DbSet<Project> Projects => Set<Project>();

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="options">Database context configuration parameters.</param>
		public ProjectServiceDbContext(DbContextOptions<ProjectServiceDbContext> options) : base(options)
		{
		}

		#endregion Public Constructors

		#region Protected Methods

		/// <summary>
		/// Configures the data model.
		/// </summary>
		/// <param name="modelBuilder">Database schema constructor.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Company>(entity =>
			{
				entity.ToTable("companies");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
			});

			modelBuilder.Entity<Employee>(entity =>
			{
				entity.ToTable("employees");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.FirstName).IsRequired().HasMaxLength(100);

				entity.Property(x => x.LastName).IsRequired().HasMaxLength(100);

				entity.Property(x => x.MiddleName).HasMaxLength(100);

				entity.Property(x => x.Email).IsRequired().HasMaxLength(200);
				entity.HasIndex(x => x.Email).IsUnique();
			});

			modelBuilder.Entity<Project>(entity =>
			{
				entity.ToTable("projects");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.Name).IsRequired().HasMaxLength(200);

				entity.Property(x => x.StartTime).IsRequired();
				entity.HasIndex(x => x.StartTime);

				entity.Property(x => x.EndTime).IsRequired();

				entity.Property(x => x.Priority).IsRequired();
				entity.HasIndex(x => x.Priority);

				entity.HasOne(x => x.ProjectManager).WithMany(x => x.ManagedProjects).HasForeignKey(x => x.ProjectManagerId).OnDelete(DeleteBehavior.Restrict);
				entity.HasIndex(x => x.ProjectManagerId);

				entity.HasOne(x => x.CustomerCompany).WithMany(x => x.CustomerProjects).HasForeignKey(x => x.CustomerCompanyId).OnDelete(DeleteBehavior.Restrict);
				entity.HasIndex(x => x.CustomerCompanyId);

				entity.HasOne(x => x.ExecutorCompany).WithMany(x => x.ExecutorProjects).HasForeignKey(x => x.ExecutorCompanyId).OnDelete(DeleteBehavior.Restrict);
				entity.HasIndex(x => x.ExecutorCompanyId);
			});

			modelBuilder.Entity<ProjectEmployee>(entity =>
			{
				entity.ToTable("project_employees");

				entity.HasKey(x => new { x.ProjectId, x.EmployeeId });

				entity.Property(x => x.AssignedAt).IsRequired();

				entity.HasOne(x => x.Employee).WithMany(x => x.ProjectEmployees).HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(x => x.Project).WithMany(x => x.ProjectEmployees).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<ProjectDocument>(entity =>
			{
				entity.ToTable("project_documents");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.FileName).IsRequired().HasMaxLength(200);

				entity.Property(x => x.StoredFileName).IsRequired().HasMaxLength(200);

				entity.Property(x => x.ContentType).IsRequired().HasMaxLength(100);

				entity.Property(x => x.Path).IsRequired().HasMaxLength(500);

				entity.Property(x => x.Size).IsRequired();

				entity.Property(x => x.UploadedAt).IsRequired();

				entity.HasOne(x => x.Project).WithMany(x => x.Documents).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
				entity.HasIndex(x => x.ProjectId);
			});
		}

		#endregion Protected Methods
	}
}