namespace ProjectManagementService.Api.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;

	[ApiController]
	[Route("api/[controller]")]
	public class ProjectsController : ControllerBase
	{
		#region Private Fields

		private readonly IProjectService _projectService;

		#endregion Private Fields

		#region Public Constructors

		public ProjectsController(IProjectService projectService)
		{
			_projectService = projectService;
		}

		#endregion Public Constructors

		#region Public Methods

		[HttpPost("{projectId:guid}/employees/{employeeId:guid}")]
		public async Task<IActionResult> AddEmployee(Guid projectId, Guid employeeId, CancellationToken cancellationToken)
		{
			await _projectService.AddEmployeeAsync(projectId, employeeId, cancellationToken);
			return NoContent();
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
		{
			var project = await _projectService.CreateAsync(request, cancellationToken);

			return CreatedAtAction(nameof(GetById), new { projectId = project.Id }, project);
		}

		[HttpDelete("{projectId:guid}")]
		public async Task<IActionResult> Delete(Guid projectId, CancellationToken cancellationToken)
		{
			await _projectService.DeleteAsync(projectId, cancellationToken);
			return NoContent();
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] FiltredProjectsRequest request, CancellationToken cancellationToken)
		{
			var projects = await _projectService.GetAllAsync(request, cancellationToken);
			return Ok(projects);
		}

		[HttpGet("{projectId:guid}")]
		public async Task<IActionResult> GetById(Guid projectId, CancellationToken cancellationToken)
		{
			var project = await _projectService.GetByIdAsync(projectId, cancellationToken);
			return Ok(project);
		}

		[HttpDelete("{projectId:guid}/employees/{employeeId:guid}")]
		public async Task<IActionResult> RemoveEmployee(Guid projectId, Guid employeeId, CancellationToken cancellationToken)
		{
			await _projectService.RemoveEmployeeAsync(projectId, employeeId, cancellationToken);
			return NoContent();
		}

		[HttpPut("{projectId:guid}")]
		public async Task<IActionResult> Update(Guid projectId, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
		{
			var project = await _projectService.UpdateAsync(projectId, request, cancellationToken);
			return Ok(project);
		}

		#endregion Public Methods
	}
}