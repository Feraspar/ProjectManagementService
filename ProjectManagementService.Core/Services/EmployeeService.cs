namespace ProjectManagementService.Core.Services
{
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Contracts.Response;
	using ProjectManagementService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	public class EmployeeService : IEmployeeService
	{
		#region Private Fields

		/// <summary>
		/// Employee repository.
		/// </summary>
		private readonly IEmployeeRepository _employeeRepository;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="employeeRepository">Employee repository.</param>
		public EmployeeService(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<EmployeeListResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(request);

			await ValidateEmployeeRequestAsync(request.FirstName, request.LastName, request.Email, null, cancellationToken);

			Employee employee = new()
			{
				Id = Guid.NewGuid(),
				FirstName = request.FirstName.Trim(),
				LastName = request.LastName.Trim(),
				MiddleName = NormalizeOptionalValue(request.MiddleName),
				Email = request.Email.Trim()
			};

			Employee createdEmployee = await _employeeRepository.AddAsync(employee, cancellationToken);

			return MapToResponse(createdEmployee);
		}

		/// <inheritdoc />
		public async Task DeleteAsync(Guid employeeId, CancellationToken cancellationToken = default)
		{
			Employee? employee = await _employeeRepository.GetByIdAsync(employeeId, cancellationToken);

			if (employee is null)
			{
				throw new KeyNotFoundException($"Employee with id '{employeeId}' was not found.");
			}

			await _employeeRepository.DeleteAsync(employee, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<EmployeeListResponse>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			IReadOnlyCollection<Employee> employees = await _employeeRepository.GetAllAsync(cancellationToken);

			return employees.Select(MapToResponse).ToList();
		}

		/// <inheritdoc />
		public async Task<EmployeeListResponse> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
		{
			Employee? employee = await _employeeRepository.GetByIdAsync(employeeId, cancellationToken);

			if (employee == null)
			{
				throw new KeyNotFoundException($"Employee with id '{employeeId}' was not found.");
			}

			return MapToResponse(employee);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<EmployeeListResponse>> SearchAsync(string? searchTerm, int take = 10, CancellationToken cancellationToken = default)
		{
			IReadOnlyCollection<Employee> employees = await _employeeRepository.SearchAsync(searchTerm, take, cancellationToken);

			return employees.Select(MapToResponse).ToList();
		}

		public async Task<EmployeeListResponse> UpdateAsync(Guid employeeId, UpdateEmployeeRequest request, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(request);

			Employee? employee = await _employeeRepository.GetByIdAsync(employeeId, cancellationToken);

			if (employee is null)
			{
				throw new KeyNotFoundException($"Employee with id '{employeeId}' was not found.");
			}

			await ValidateEmployeeRequestAsync(request.FirstName, request.LastName, request.Email, employeeId, cancellationToken);

			employee.FirstName = request.FirstName.Trim();
			employee.LastName = request.LastName.Trim();
			employee.MiddleName = NormalizeOptionalValue(request.MiddleName);
			employee.Email = request.Email.Trim();

			await _employeeRepository.UpdateAsync(employee, cancellationToken);

			return MapToResponse(employee);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Builds a full name string.
		/// </summary>
		/// <param name="lastName">Last name.</param>
		/// <param name="firstName">First name.</param>
		/// <param name="middleName">Middle name.</param>
		private static string BuildFullName(string lastName, string firstName, string? middleName)
		{
			return string.Join(
				' ',
				new[] { lastName, firstName, middleName }
					.Where(x => !string.IsNullOrWhiteSpace(x)));
		}

		/// <summary>
		/// Maps an employee entity to response DTO.
		/// </summary>
		/// <param name="employee">Employee entity.</param>
		private static EmployeeListResponse MapToResponse(Employee employee)
		{
			return new EmployeeListResponse(
				employee.Id,
				employee.FirstName,
				employee.LastName,
				employee.MiddleName,
				employee.Email,
				BuildFullName(employee.LastName, employee.FirstName, employee.MiddleName));
		}

		/// <summary>
		/// Normalizes an optional string value.
		/// </summary>
		/// <param name="value">Source value.</param>
		private static string? NormalizeOptionalValue(string? value)
		{
			return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
		}

		/// <summary>
		/// Validates employee request data.
		/// </summary>
		/// <param name="firstName">Employee first name.</param>
		/// <param name="lastName">Employee last name.</param>
		/// <param name="email">Employee email.</param>
		/// <param name="excludedEmployeeId">Employee identifier to exclude from email uniqueness check.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		private async Task ValidateEmployeeRequestAsync(string firstName, string lastName, string email, Guid? excludedEmployeeId, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(firstName))
			{
				throw new ArgumentException("Employee first name is required.");
			}

			if (string.IsNullOrWhiteSpace(lastName))
			{
				throw new ArgumentException("Employee last name is required.");
			}

			if (string.IsNullOrWhiteSpace(email))
			{
				throw new ArgumentException("Employee email is required.");
			}

			bool isEmailUnique = await _employeeRepository.IsEmailUniqueAsync(email, excludedEmployeeId, cancellationToken);

			if (!isEmailUnique)
			{
				throw new InvalidOperationException("Employee email must be unique.");
			}
		}

		#endregion Private Methods
	}
}