using Abp.Events.Bus;
using BCrypt.Net;
using EmployeeAdministration.Helpers;
using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.UserViewModels;
using Entities.Enum;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeAdministration.Services
{
	public class UserService : IUser
	{
		private readonly Jwt _jwt;
		private readonly EmployeeAdministrationContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(EmployeeAdministrationContext context,Jwt jwt, IHttpContextAccessor httpContextAccessor)
        {
			_context = context;
			_jwt = jwt;
			_httpContextAccessor = httpContextAccessor;
		}
        public async System.Threading.Tasks.Task CreateUser(LogInViewModel request)
		{
			var userByEmail = _context.Users.Where(u=>u.Email == request.Email).FirstOrDefault();
			if (userByEmail != null)
			{
				throw new Exception("User already exists.");
			}
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
			var user = new User
			{
				Email = request.Email,
				Password = passwordHash,
				Role = Role.Employee,
			};
			_context.Users.Add(user);
		    _context.SaveChanges();
		}

		public async System.Threading.Tasks.Task DeleteUser(Guid userId)
		{
			var userToDelete = _context.Users.Where(u=> u.UserId == userId).FirstOrDefault();
			_context.Users.Remove(userToDelete);
			_context.SaveChanges();
		}

		public async Task<ICollection<UserViewModel>> GetAllUsers()
		{
			var users = await _context.Users.ToListAsync();
			var userViewModels = users.Select(u => new UserViewModel
			{
				Email = u.Email,
			}).ToList();

			return userViewModels;
		}

		public async Task<string> Login(LogInViewModel request)
		{
			var user = _context.Users.Where(u=>u.Email == request.Email).FirstOrDefault();
			if(user ==null || !BCrypt.Net.BCrypt.Verify(request.Password,user.Password))
			{
				throw new Exception("Wrong email or password");
			}
			var authclaims = new List<Claim>
			{
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new(ClaimTypes.Role, user.Role.ToString())
			};

			var token = _jwt.CreateToken(authclaims);
			return token;
		}

		public async System.Threading.Tasks.Task UpdateUser(UpdateUserViewModel request)
		{
			Guid userId = StaticFunc.GetUserId(_httpContextAccessor);
			var user = await _context.Users.FindAsync(userId);
			if (user == null)
			{
				throw new Exception("User not found");
			}
			user.PhotoPath = request.PhotoPath;
			user.PhotoContent = request.PhotoContent;
			_context.Users.Update(user);
			_context.SaveChanges();
		}
	}
}
