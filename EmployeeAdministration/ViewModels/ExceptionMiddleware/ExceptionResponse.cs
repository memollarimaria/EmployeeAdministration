using System.Net;

namespace EmployeeAdministration.ViewModels.ExceptionMiddleware
{
	public record ExceptionResponse(HttpStatusCode StatusCode, string Description);

}
