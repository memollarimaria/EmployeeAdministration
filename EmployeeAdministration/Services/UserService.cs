using Abp.Events.Bus;
using BCrypt.Net;
using EmployeeAdministration.EventsBus.UserEventHandler;
using EmployeeAdministration.Helpers;
using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.UserViewModels;
using Entities.Enum;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeAdministration.Services
{
	public class UserService : IUser
	{
		private readonly Jwt _jwt;
		private readonly EmployeeAdministrationContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;



        public UserService(
         EmployeeAdministrationContext context,
         UserManager<User> userManager,
         SignInManager<User> signInManager,
         Jwt jwtService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwt = jwtService;
        }
        public async System.Threading.Tasks.Task CreateUser(LogInViewModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                throw new Exception("User registration failed");
            }
        }


        public async System.Threading.Tasks.Task DeleteUser(Guid userId)
		{
			var userToDelete = _context.Users.Where(u=> u.Id == userId).FirstOrDefault();

			if (userToDelete == null)
			{
				throw new Exception("User not found");
			}

			_context.Users.Remove(userToDelete);
			_context.SaveChanges();

			EventBus.Default.Trigger(new UserDeletedEvent { UserId = userToDelete.Id, Email = userToDelete.Email });

		}

        public async Task<ICollection<UserViewModel>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModels = users.Select(u => new UserViewModel
            {
                Email = u.Email,
            }).ToList();

            return userViewModels;
        }


        public async Task<string> Login(LogInViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new Exception("Wrong email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _jwt.CreateToken(authClaims);

            EventBus.Default.TriggerAsync(new UserLoggedInEvent { UserId = user.Id, Email = user.Email });

            return token;
        }

        public async System.Threading.Tasks.Task UpdateUserProfilePicture(UpdateUserProfilePictureViewModel request)
        {
            Guid userId = StaticFunc.GetUserId(_httpContextAccessor);

            var user = await _userManager.FindByIdAsync(userId.ToString());
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

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("Error updating user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                EventBus.Default.TriggerAsync(new UserProfilePictureUpdatedEvent { UserId = user.Id, PhotoContent = user.PhotoContent });
            }
        }

    }
}
