namespace ProjectManagementService.Api.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;

	[ApiController]
	[Route("api/projects/{projectId:guid}/documents")]
	public class ProjectDocumentsController : ControllerBase
	{
		#region Private Fields

		private readonly IProjectDocumentService _projectDocumentService;

		#endregion Private Fields

		#region Public Constructors

		public ProjectDocumentsController(IProjectDocumentService projectDocumentService)
		{
			_projectDocumentService = projectDocumentService;
		}

		#endregion Public Constructors

		#region Public Methods

		[HttpDelete("{documentId:guid}")]
		public async Task<IActionResult> Delete(Guid documentId, CancellationToken cancellationToken)
		{
			await _projectDocumentService.DeleteAsync(documentId, cancellationToken);
			return NoContent();
		}

		[HttpGet]
		public async Task<IActionResult> GetByProjectId(Guid projectId, CancellationToken cancellationToken)
		{
			var documents = await _projectDocumentService.GetByProjectIdAsync(projectId, cancellationToken);
			return Ok(documents);
		}

		[HttpPost]
		[RequestSizeLimit(50_000_000)]
		public async Task<IActionResult> Upload(Guid projectId, IFormFile file, CancellationToken cancellationToken)
		{
			if (file is null || file.Length == 0)
			{
				return BadRequest("File is required.");
			}

			await using Stream stream = file.OpenReadStream();

			var request = new UploadProjectDocumentRequest(projectId, file.FileName, file.ContentType, stream);

			var document = await _projectDocumentService.UploadAsync(request, cancellationToken);

			return Ok(document);
		}

		#endregion Public Methods
	}
}