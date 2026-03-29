namespace ProjectManagementService.Infrastructure.Files
{
	using ProjectManagementService.Core.Abstractions;
	using ProjectManagementService.Core.Contracts.Request;
	using ProjectManagementService.Core.Contracts.Response;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class LocalFileStorageService : IFileStorageService
	{
		#region Private Fields

		/// <summary>
		/// Root storage path.
		/// </summary>
		private readonly string _rootPath;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="rootPath">Root storage path.</param>
		public LocalFileStorageService(string rootPath)
		{
			_rootPath = rootPath;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public Task DeleteFileAsync(string path, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				throw new ArgumentException("File path is required.");
			}

			if (File.Exists(path))
			{
				File.Delete(path);
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return Task.FromResult(false);
			}

			return Task.FromResult(File.Exists(path));
		}

		/// <inheritdoc />
		public async Task<SavedFileResponse> SaveFileAsync(SaveFileRequest request, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(request);

			if (string.IsNullOrWhiteSpace(request.FileName))
			{
				throw new ArgumentException("File name is required.");
			}

			if (string.IsNullOrWhiteSpace(request.ContentType))
			{
				throw new ArgumentException("Content type is required.");
			}

			string projectDirectory = Path.Combine(_rootPath, "projects", request.ProjectId.ToString());

			Directory.CreateDirectory(projectDirectory);

			string safeFileName = Path.GetFileName(request.FileName);
			string storedFileName = $"{Guid.NewGuid():N}_{safeFileName}";
			string fullPath = Path.Combine(projectDirectory, storedFileName);

			await using FileStream fileStream = new(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 81920, useAsync: true);

			await request.Content.CopyToAsync(fileStream, cancellationToken);
			await fileStream.FlushAsync(cancellationToken);

			long size = fileStream.Length;

			return new SavedFileResponse(safeFileName, storedFileName, fullPath, request.ContentType, size);
		}

		#endregion Public Methods
	}
}