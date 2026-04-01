namespace ProjectManagementService.Tests
{
	using Moq;
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Entities;
	using ProjectManagementService.Core.Enums;
	using ProjectManagementService.Core.Services;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class ProjectServiceTests
	{
		#region Private Fields

		private readonly Employee _employee;
		private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
		private readonly Employee _manager;
		private readonly Mock<IProjectRepository> _projectRepositoryMock;
		private readonly ProjectService _sut;

		#endregion Private Fields

		#region Public Constructors

		public ProjectServiceTests()
		{
			_projectRepositoryMock = new Mock<IProjectRepository>();
			_employeeRepositoryMock = new Mock<IEmployeeRepository>();
			_sut = new ProjectService(_projectRepositoryMock.Object, _employeeRepositoryMock.Object);

			_manager = new Employee
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Doe",
				Email = "john.doe@example.com"
			};

			_employee = new Employee
			{
				Id = Guid.NewGuid(),
				FirstName = "Jane",
				LastName = "Smith",
				Email = "jane.smith@example.com"
			};
		}

		#endregion Public Constructors

		#region Public Methods

		[Fact]
		public async Task CreateAsync_ManagerNotFound_ThrowsKeyNotFoundException()
		{
			// Arrange
			var request = new CreateProjectRequest(
				Name: "New Project",
				CustomerCompanyName: "Customer",
				ExecutorCompanyName: "Executor",
				ProjectManagerId: _manager.Id,
				StartDate: DateTime.UtcNow.Date,
				EndDate: DateTime.UtcNow.Date.AddDays(7),
				Priority: ProjectPriority.High,
				EmployeeIds: new List<Guid>()
			);

			_employeeRepositoryMock.Setup(x => x.ExistsAsync(_manager.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

			// Act
			var action = async () => await _sut.CreateAsync(request);

			// Assert
			var exception = await Assert.ThrowsAsync<KeyNotFoundException>(action);
			Assert.Equal($"Project manager with id '{request.ProjectManagerId}' was not found.", exception.Message);

			_projectRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task CreateAsync_ValidRequest_ReturnsCorrectResponse()
		{
			// Arrange
			var request = new CreateProjectRequest(
				Name: "New Project",
				CustomerCompanyName: "Customer",
				ExecutorCompanyName: "Executor",
				ProjectManagerId: _manager.Id,
				StartDate: DateTime.UtcNow.Date,
				EndDate: DateTime.UtcNow.Date.AddDays(7),
				Priority: ProjectPriority.High,
				EmployeeIds: new List<Guid> { _employee.Id }
			);

			var project = BuildProject(request);

			_employeeRepositoryMock.Setup(x => x.ExistsAsync(_manager.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

			_employeeRepositoryMock.Setup(x => x.ExistsAsync(_employee.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

			_projectRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>())).ReturnsAsync((Project p, CancellationToken _) => p);

			_projectRepositoryMock.Setup(x => x.AddEmployeeAsync(It.IsAny<ProjectEmployee>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

			_projectRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(project);

			// Act
			var result = await _sut.CreateAsync(request);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(request.Name, result.Name);
			Assert.Equal(request.CustomerCompanyName, result.CustomerCompanyName);
			Assert.Equal(request.ExecutorCompanyName, result.ExecutorCompanyName);
			Assert.Equal(_manager.Id, result.ProjectManagerId);

			_projectRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
			_projectRepositoryMock.Verify(x => x.AddEmployeeAsync(It.Is<ProjectEmployee>(pe => pe.EmployeeId == _manager.Id), It.IsAny<CancellationToken>()), Times.Once);
			_projectRepositoryMock.Verify(x => x.AddEmployeeAsync(It.Is<ProjectEmployee>(pe => pe.EmployeeId == _employee.Id), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task RemoveEmployeeFromProjectAsync_EmployeeIsProjectManager_ThrowsInvalidOperationException()
		{
			// Arrange
			var request = new CreateProjectRequest(
				Name: "New Project",
				CustomerCompanyName: "Customer",
				ExecutorCompanyName: "Executor",
				ProjectManagerId: _employee.Id,
				StartDate: DateTime.UtcNow.Date,
				EndDate: DateTime.UtcNow.Date.AddDays(7),
				Priority: ProjectPriority.High,
				EmployeeIds: new List<Guid> { _employee.Id }
			);

			var project = BuildProject(request);

			_projectRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(project.Id, It.IsAny<CancellationToken>())).ReturnsAsync(project);

			// Act
			var action = async () => await _sut.RemoveEmployeeAsync(project.Id, project.ProjectManagerId);

			// Assert
			InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
			Assert.Equal("The project manager can't be removed from the project team.", exception.Message);

			_projectRepositoryMock.Verify(x => x.RemoveEmployeeAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		#endregion Public Methods

		#region Private Methods

		private Project BuildProject(CreateProjectRequest request)
		{
			var projectId = Guid.NewGuid();
			return new Project
			{
				Id = projectId,
				Name = request.Name,
				CustomerCompanyName = request.CustomerCompanyName,
				ExecutorCompanyName = request.ExecutorCompanyName,
				ProjectManagerId = _manager.Id,
				ProjectManager = _manager,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				Priority = request.Priority,
				ProjectEmployees = new List<ProjectEmployee>
			{
				new() { ProjectId = projectId, EmployeeId = _manager.Id, Employee = _manager },
				new() { ProjectId = projectId, EmployeeId = _employee.Id, Employee = _employee }
			},
				Documents = new List<ProjectDocument>()
			};
		}

		#endregion Private Methods
	}
}