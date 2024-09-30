using Abp.Events.Bus;
using BCrypt.Net;
using EmployeeAdministration.EventsBus.UserEventHandler;
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

			EventBus.Default.Trigger(new UserCreatedEvent { UserId = user.UserId, Email = user.Email });

		}

		public async System.Threading.Tasks.Task DeleteUser(Guid userId)
		{
			var userToDelete = _context.Users.Where(u=> u.UserId == userId).FirstOrDefault();

			if (userToDelete == null)
			{
				throw new Exception("User not found");
			}

			_context.Users.Remove(userToDelete);
			_context.SaveChanges();

			EventBus.Default.Trigger(new UserDeletedEvent { UserId = userToDelete.UserId, Email = userToDelete.Email });

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

			EventBus.Default.TriggerAsync(new UserLoggedInEvent { UserId = user.UserId, Email = user.Email });
	
			return token;
		}

		public async System.Threading.Tasks.Task UpdateUserProfilePicture(UpdateUserProfilePictureViewModel request)
		{
			Guid userId = StaticFunc.GetUserId(_httpContextAccessor);

			var user = await _context.Users.FindAsync(userId);
			if (user == null)
			{
				throw new Exception("User not found");
			}

			if (request.ImageFile != null)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);

				using (var stream = new MemoryStream())
				{
					await request.ImageFile.CopyToAsync(stream);
					var imageData = stream.ToArray();

					user.PhotoContent = imageData; 
					user.PhotoPath = fileName;

				}
			}

			_context.Users.Update(user);
			await _context.SaveChangesAsync();

			EventBus.Default.TriggerAsync(new UserProfilePictureUpdatedEvent { UserId = user.UserId, PhotoContent = user.PhotoContent });

		}
	}
}
