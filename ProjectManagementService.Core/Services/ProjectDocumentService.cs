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

		/// <summary>
		/// Service for save file in local storage.
		/// </summary>
		private readonly IFileStorageService _fileStorageService;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="projectDocumentRepository">Project document repository.</param>
		/// <param name="projectRepository">Project repository.</param>
		public ProjectDocumentService(IProjectDocumentRepository projectDocumentRepository, IProjectRepository projectRepository, IFileStorageService fileStorageService)
		{
			_projectDocumentRepository = projectDocumentRepository;
			_projectRepository = projectRepository;
			_fileStorageService = fileStorageService;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task DeleteAsync(Guid documentId, CancellationToken cancellationToken = default)
		{
			ProjectDocument? document = await _projectDocumentRepository.GetByIdAsync(documentId, cancellationToken);

			if (document is null)
			{
				throw new KeyNotFoundException($"Project document with id '{documentId}' was not found.");
			}

			await _fileStorageService.DeleteFileAsync(document.Path, cancellationToken);
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
		public async Task<ProjectDocumentResponse> UploadAsync(UploadProjectDocumentRequest request, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(request);

			bool projectExists = await _projectRepository.ExistsAsync(request.ProjectId, cancellationToken);

			if (!projectExists)
			{
				throw new KeyNotFoundException($"Project with id '{request.ProjectId}' was not found.");
			}

			ValidateUploadRequest(request);

			SavedFileResponse savedFile = await _fileStorageService.SaveFileAsync(new SaveFileRequest(request.ProjectId, request.FileName, request.ContentType, request.Content), cancellationToken);

			ProjectDocument document = new()
			{
				Id = Guid.NewGuid(),
				ProjectId = request.ProjectId,
				FileName = savedFile.FileName,
				StoredFileName = savedFile.StoredFileName,
				ContentType = savedFile.ContentType,
				Size = savedFile.Size,
				Path = savedFile.Path,
				UploadedAt = DateTimeOffset.UtcNow
			};

			ProjectDocument createdDocument = await _projectDocumentRepository.AddAsync(document, cancellationToken);

			return MapToResponse(createdDocument);
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

			if (string.IsNullOrWhiteSpace(request.ContentType))
			{
				throw new ArgumentException("Content type is required.");
			}

			if (request.Content is null)
			{
				throw new ArgumentException("File content is required.");
			}

			if (!request.Content.CanRead)
			{
				throw new ArgumentException("File content stream must be readable.");
			}
		}

		#endregion Private Methods
	}
}