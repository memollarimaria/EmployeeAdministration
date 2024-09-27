using System.Security.Claims;

namespace EmployeeAdministration.Helpers
{
	public class StaticFunc
	{
		public static Guid ConvertGuid(Claim receiverIdClaim)
		{
			Guid receiverId = Guid.Empty;
			if (receiverIdClaim != null && Guid.TryParse(receiverIdClaim.Value, out var parsedGuid))
			{
				receiverId = parsedGuid;
			}
			return receiverId;
		}

		public static Guid GetUserId(IHttpContextAccessor httpContextAccessor)
		{
			var receiverIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (receiverIdClaim != null)
			{
				return ConvertGuid(receiverIdClaim);
			}
			else
			{
				return Guid.Empty;
			}
		}
	}
}
