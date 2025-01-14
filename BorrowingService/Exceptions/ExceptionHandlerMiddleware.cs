using System.Text.Json;

namespace BorrowingService.Exceptions
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context); // Proceed to the next middleware or endpoint
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			var response = exception switch
			{
				NotFoundException => new { StatusCode = StatusCodes.Status404NotFound, Message = exception.Message },
				BadRequestException => new { StatusCode = StatusCodes.Status400BadRequest, Message = exception.Message },
				_ => new { StatusCode = StatusCodes.Status500InternalServerError, Message = $"An unexpected error occurred: {exception.Message}" }
			};

			context.Response.StatusCode = response.StatusCode;

			_logger.LogError(exception, $"An exception occurred. {exception.Message}\n{exception.StackTrace}");

			return context.Response.WriteAsync(JsonSerializer.Serialize(response));
		}
	}

}
