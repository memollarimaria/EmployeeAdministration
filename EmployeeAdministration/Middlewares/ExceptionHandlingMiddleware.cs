using EmployeeAdministration.ViewModels.ExceptionMiddleware;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace EmployeeAdministration.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly Serilog.ILogger _logger;


		public ExceptionHandlingMiddleware(RequestDelegate next,Serilog.ILogger logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			_logger.Information(exception, "An unexpected error occurred.");


			ExceptionResponse response = exception switch
			{
				ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
				KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
				BadHttpRequestException _ => new ExceptionResponse(HttpStatusCode.NotFound, "Bad Request."),
				UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
				_ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
			};

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)response.StatusCode;
			await context.Response.WriteAsJsonAsync(response);
		}
	}

}
