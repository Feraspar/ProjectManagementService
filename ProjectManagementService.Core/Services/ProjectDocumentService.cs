namespace ProjectManagementService.Core.Services
{
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Contracts.Response;
	using ProjectManagementService.Core.Entities;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Service for project document business logic.
	/// </summary>
	public class ProjectDocumentService : IProjectDocumentService
	{
		#region Private Fields

		/// <summary>
		/// Project document repository.
		/// </summary>
		private readonly IProjectDocumentRepository _projectDocumentRepository;

		/// <summary>
		/// Project repository.
		/// </summary>
		private readonly IProjectRepository _projectRepository;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="projectDocumentRepository">Project document repository.</param>
		/// <param name="projectRepository">Project repository.</param>
		public ProjectDocumentService(IProjectDocumentRepository projectDocumentRepository, IProjectRepository projectRepository)
		{
			_projectDocumentRepository = projectDocumentRepository;
			_projectRepository = projectRepository;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<ProjectDocumentResponse> AddAsync(UploadProjectDocumentRequest request, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(request);

			bool projectExists = await _projectRepository.ExistsAsync(request.ProjectId, cancellationToken);

			if (!projectExists)
			{
				throw new KeyNotFoundException($"Project with id '{request.ProjectId}' was not found.");
			}

			ValidateUploadRequest(request);

			ProjectDocument document = new()
			{
				Id = Guid.NewGuid(),
				ProjectId = request.ProjectId,
				FileName = request.FileName.Trim(),
				StoredFileName = request.StoredFileName.Trim(),
				ContentType = request.ContentType.Trim(),
				Size = request.Size,
				Path = request.Path.Trim(),
				UploadedAt = DateTimeOffset.UtcNow
			};

			ProjectDocument createdDocument = await _projectDocumentRepository.AddAsync(document, cancellationToken);

			return MapToResponse(createdDocument);
		}

		/// <inheritdoc />
		public async Task DeleteAsync(Guid documentId, CancellationToken cancellationToken = default)
		{
			ProjectDocument? document = await _projectDocumentRepository.GetByIdAsync(documentId, cancellationToken);

			if (document is null)
			{
				throw new KeyNotFoundException($"Project document with id '{documentId}' was not found.");
			}

			await _projectDocumentRepository.DeleteAsync(document, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<ProjectDocumentResponse> GetByIdAsync(Guid documentId, CancellationToken cancellationToken = default)
		{
			ProjectDocument? document = await _projectDocumentRepository.GetByIdAsync(documentId, cancellationToken);

			if (document is null)
			{
				throw new KeyNotFoundException($"Project document with id '{documentId}' was not found.");
			}

			return MapToResponse(document);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<ProjectDocumentResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
		{
			bool projectExists = await _projectRepository.ExistsAsync(projectId, cancellationToken);

			if (!projectExists)
			{
				throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
			}

			IReadOnlyCollection<ProjectDocument> documents = await _projectDocumentRepository.GetAllAsync(projectId, cancellationToken);

			return documents.Select(MapToResponse).ToList();
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Maps a project document entity to response DTO.
		/// </summary>
		/// <param name="document">Project document entity.</param>
		private static ProjectDocumentResponse MapToResponse(ProjectDocument document)
		{
			return new ProjectDocumentResponse(
				document.Id,
				document.FileName,
				document.ContentType,
				document.Size,
				document.UploadedAt);
		}

		/// <summary>
		/// Validates upload request data.
		/// </summary>
		/// <param name="request">Upload request.</param>
		private static void ValidateUploadRequest(UploadProjectDocumentRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.FileName))
			{
				throw new ArgumentException("File name is required.");
			}

			if (string.IsNullOrWhiteSpace(request.StoredFileName))
			{
				throw new ArgumentException("Stored file name is required.");
			}

			if (string.IsNullOrWhiteSpace(request.ContentType))
			{
				throw new ArgumentException("Content type is required.");
			}

			if (string.IsNullOrWhiteSpace(request.Path))
			{
				throw new ArgumentException("File path is required.");
			}

			if (request.Size <= 0)
			{
				throw new ArgumentException("File size must be greater than zero.");
			}
		}

		#endregion Private Methods
	}
}