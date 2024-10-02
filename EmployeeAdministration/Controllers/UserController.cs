using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.UserViewModels;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EmployeeAdministration.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUser _user;
		private readonly ILogger<UserController> _logger;

        public UserController(IUser user, ILogger<UserController> logger)
        {
			_user = user;
			_logger = logger;
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("CreateUser")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateUser(LogInViewModel request)
		{
			await _user.CreateUser(request);
			return StatusCode(StatusCodes.Status201Created);
		}



		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(LogInViewModel request)
		{
			var response = await _user.Login(request);

			if (response != null)
			{
				return Ok(response);
			}

			return StatusCode(StatusCodes.Status400BadRequest);
		}



		[HttpGet]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _user.GetAllUsers();
			if (users == null)
			{
				return StatusCode(StatusCodes.Status404NotFound);
			}
			return Ok(users);
		}

		[HttpPut("UpdateUserProfilePicture")]
		public async Task<IActionResult> UpdateUserProfilePicture([FromForm] UpdateUserProfilePictureViewModel request)
		{
		     await _user.UpdateUserProfilePicture(request);
			 return StatusCode(StatusCodes.Status201Created);
		}

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserViewModel request)
        {
            await _user.UpdateUser(request);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete]
		public async Task<IActionResult> DeleteUser(Guid userId)
		{
			await _user.DeleteUser(userId);
			return StatusCode(StatusCodes.Status201Created);

		}
	}
}
