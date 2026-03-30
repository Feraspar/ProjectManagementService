using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Core.Abstractions;
using ProjectManagementService.Core.Contracts.Request;

namespace ProjectManagementService.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EmployeesController : ControllerBase
	{
		#region Private Fields

		private readonly IEmployeeService _employeeService;

		#endregion Private Fields

		#region Public Constructors

		public EmployeesController(IEmployeeService employeeService)
		{
			_employeeService = employeeService;
		}

		#endregion Public Constructors

		#region Public Methods

		[HttpPost]
		public async Task<IActionResult> Create(
		[FromBody] CreateEmployeeRequest request,
		CancellationToken cancellationToken)
		{
			var employee = await _employeeService.CreateAsync(request, cancellationToken);
			return CreatedAtAction(nameof(GetById), new { employeeId = employee.Id }, employee);
		}

		[HttpDelete("{employeeId:guid}")]
		public async Task<IActionResult> Delete(
			Guid employeeId,
			CancellationToken cancellationToken)
		{
			await _employeeService.DeleteAsync(employeeId, cancellationToken);
			return NoContent();
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var employees = await _employeeService.GetAllAsync(cancellationToken);

			return Ok(employees);
		}

		[HttpGet("{employeeId:guid}")]
		public async Task<IActionResult> GetById(Guid employeeId, CancellationToken cancellationToken)
		{
			var employee = await _employeeService.GetByIdAsync(employeeId, cancellationToken);

			return Ok(employee);
		}

		[HttpGet("search")]
		public async Task<IActionResult> Search(CancellationToken cancellationToken, [FromQuery] string? searchTerm, [FromQuery] int take = 10)
		{
			var employees = await _employeeService.SearchAsync(searchTerm, take, cancellationToken);

			return Ok(employees);
		}

		[HttpPut("{employeeId:guid}")]
		public async Task<IActionResult> Update(
			Guid employeeId,
			[FromBody] UpdateEmployeeRequest request,
			CancellationToken cancellationToken)
		{
			var employee = await _employeeService.UpdateAsync(employeeId, request, cancellationToken);
			return Ok(employee);
		}

		#endregion Public Methods
	}
}