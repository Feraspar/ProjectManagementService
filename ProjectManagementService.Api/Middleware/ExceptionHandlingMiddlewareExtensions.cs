namespace ProjectManagementService.Api.Middleware
{
	/// <summary>
	/// Extensions for exception handling middleware.
	/// </summary>
	public static class ExceptionHandlingMiddlewareExtensions
	{
		/// <summary>
		/// Adds global exception handling middleware.
		/// </summary>
		/// <param name="app">Application builder.</param>
		/// <returns>Application builder.</returns>
		public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
		{
			return app.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
