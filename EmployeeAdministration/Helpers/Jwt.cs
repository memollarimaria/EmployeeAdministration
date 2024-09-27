using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeAdministration.Helpers
{
	public class Jwt
	{
		private readonly IConfiguration _configuration;

		public Jwt(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string CreateToken(IEnumerable<Claim> authClaims)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

			var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(authClaims),
				IssuedAt = DateTime.UtcNow,
				Issuer = _configuration["JWT:Issuer"],
				Audience = _configuration["JWT:Audience"],
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var jwt = tokenHandler.WriteToken(token);
			return jwt;
		}
	}
}
