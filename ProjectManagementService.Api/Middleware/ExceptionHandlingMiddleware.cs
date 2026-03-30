namespace ProjectManagementService.Api.Middleware
{
	using ProjectManagementService.Core.Contracts.Response;
	using System.Net;
	using System.Text.Json;

	/// <summary>
	/// Middleware for global exception handling.
	/// </summary>
	public class ExceptionHandlingMiddleware
	{
		/// <summary>
		/// Next middleware delegate.
		/// </summary>
		private readonly RequestDelegate _next;

		/// <summary>
		/// Logger instance.
		/// </summary>
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="next">Next middleware delegate.</param>
		/// <param name="logger">Logger instance.</param>
		public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		/// <summary>
		/// Handles the HTTP request.
		/// </summary>
		/// <param name="context">HTTP context.</param>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "An unhandled exception occurred.");

				await HandleExceptionAsync(context, exception);
			}
		}

		/// <summary>
		/// Writes an error response for the exception.
		/// </summary>
		/// <param name="context">HTTP context.</param>
		/// <param name="exception">Thrown exception.</param>
		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			HttpStatusCode statusCode;

			switch (exception)
			{
				case KeyNotFoundException ex:
					statusCode = HttpStatusCode.NotFound;
					break;

				case ArgumentException ex:
					statusCode = HttpStatusCode.BadRequest;
					break;

				case InvalidOperationException ex:
					statusCode = HttpStatusCode.Conflict;
					break;

				default:
					statusCode = HttpStatusCode.InternalServerError;
					break;
			}

			ErrorResponse response = new((int)statusCode, exception.Message);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;

			string json = JsonSerializer.Serialize(response);

			await context.Response.WriteAsync(json);
		}
	}
}
