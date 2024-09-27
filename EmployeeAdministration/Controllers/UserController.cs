using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.UserViewModels;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdministration.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUser _user;

        public UserController(IUser user)
        {
			_user = user;
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("CreateUser")]
		public async Task<IActionResult> CreateUser(LogInViewModel request)
		{

			try
			{
				await _user.CreateUser(request);
				return Ok(new { message = "User creation successful" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
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

			return BadRequest(new { message = "User login unsuccessful" });
		}


		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _user.GetAllUsers();
			if (users == null)
			{
				return NotFound();
			}
			return Ok(users);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateUser(UpdateUserViewModel request)
		{
			try
			{
				await _user.UpdateUser(request);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex);
			}
		}
		[HttpDelete]
		public async Task<IActionResult> DeleteUser(Guid userId)
		{
			try
			{
				await _user.DeleteUser(userId);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex);
			}
		}
	}
}
