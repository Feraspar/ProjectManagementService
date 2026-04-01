namespace ProjectManagementService.Tests
{
	using Moq;
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Entities;
	using ProjectManagementService.Core.Services;
	using System;
	using System.Threading.Tasks;

	public class EmployeeServiceTests
	{
		#region Private Fields

		private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
		private readonly Mock<IProjectRepository> _projectRepositoryMock;
		private readonly EmployeeService _sut;

		#endregion Private Fields

		#region Public Constructors

		public EmployeeServiceTests()
		{
			_employeeRepositoryMock = new Mock<IEmployeeRepository>();
			_projectRepositoryMock = new Mock<IProjectRepository>();
			_sut = new EmployeeService(_employeeRepositoryMock.Object, _projectRepositoryMock.Object);
		}

		#endregion Public Constructors

		#region Public Methods

		[Fact]
		public async Task CreateAsync_ValidRequest_ReturnsCorrectResponse()
		{
			/// Arrange
			var request = new CreateEmployeeRequest
			(
				FirstName: "John",
				LastName: "Smith",
				MiddleName: null,
				Email: "John@company.mail"
			);

			var employee = new Employee
			{
				Id = Guid.NewGuid(),
				FirstName = request.FirstName,
				LastName = request.LastName,
				MiddleName = request.MiddleName,
				Email = request.Email
			};

			_employeeRepositoryMock.Setup(x => x.IsEmailUniqueAsync(request.Email, null, It.IsAny<CancellationToken>())).ReturnsAsync(true);
			_employeeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>())).ReturnsAsync(employee);

			/// Act
			var result = await _sut.CreateAsync(request);

			/// Assert
			Assert.NotNull(result);
			Assert.Equal("John", result.FirstName);
			Assert.Equal("Smith", result.LastName);
			Assert.Equal("John@company.mail", result.Email);
			Assert.Equal("Smith John", result.FullName);

			_employeeRepositoryMock.Verify(x => x.AddAsync(It.Is<Employee>(e => e.FirstName == "John" && e.LastName == "Smith" && e.Email == "John@company.mail"), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task CreateAsync_DublicateEmail_ThrowsInvalidOperationException()
		{
			/// Arrange
			var request = new CreateEmployeeRequest
			(
				FirstName: "John",
				LastName: "Smith",
				MiddleName: "Luter",
				Email: "dublicate@company.mail"
			);

			_employeeRepositoryMock.Setup(x => x.IsEmailUniqueAsync(request.Email, null, It.IsAny<CancellationToken>())).ReturnsAsync(false);

			/// Act
			var action = async () => await _sut.CreateAsync(request);

			/// Assert
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
			Assert.Equal("Employee email must be unique.", exception.Message);

			_employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task DeleteAsync_EmployeeIsProjectManager_ThrowsInvalidOperationException()
		{
			/// Arrange
			var employee = new Employee
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				MiddleName = "Luter",
				Email = "John@company.mail"
			};

			_employeeRepositoryMock.Setup(x => x.GetByIdAsync(employee.Id, It.IsAny<CancellationToken>())).ReturnsAsync(employee);
			_projectRepositoryMock.Setup(x => x.GetAllAsync(It.Is<FiltredProjectsRequest>(r => r.ProjectManagerId == employee.Id), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Project>
			{
				new()
				{
					Id = Guid.NewGuid(),
					Name = "TestProject",
					CustomerCompanyName = "Customer",
					ExecutorCompanyName = "Executor",
					ProjectManagerId = employee.Id,
					StartDate = DateTime.UtcNow.Date,
					EndDate = DateTime.UtcNow.Date.AddDays(1)
				}
			});

			/// Act
			var action = async () => await _sut.DeleteAsync(employee.Id);

			/// Assert
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
			Assert.Equal("Employee can't be deleted because they are assigned as a project manager.", exception.Message);

			_employeeRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		#endregion Public Methods
	}
}