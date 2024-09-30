using EmployeeAdministration.Interfaces;
using EmployeeAdministration.Services;
using EmployeeAdministration.ViewModels.ProjectsViewModels;
using EmployeeAdministration.ViewModels.TasksViewModels;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdministration.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly ITask _task;
		private readonly ILogger<UserController> _logger;

		public TaskController(ITask task, ILogger<UserController> logger)
		{
			_task = task;
			_logger = logger;
		}


		[HttpGet("GetAllUserTasks")]
		public async Task<IActionResult> GetAllUserTasks()
		{
			var tasks = await _task.GetUserTasks();
			return Ok(tasks);
		}

		[HttpGet("GetAllTasks")]
		public async Task<IActionResult> GetAllTasks()
		{
			var tasks = await _task.GetAllTasks();
			return Ok(tasks);
		}

		[HttpPost("CreateTask")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> CreateTask([FromBody] CreateTaskViewModel request)
		{
			await _task.CreateTask(request);
			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPost("CreateUserTask")]
		[Authorize(Roles = "Employee")]
		public async Task<IActionResult> CreateUserTask([FromBody] CreateTaskViewModel request)
		{
			await _task.CreateUserTask(request);
			return StatusCode(StatusCodes.Status201Created);
		}


		[HttpPut("UpdateTask")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskViewModel request)
		{
			await _task.UpdateTask(request);
			return StatusCode(StatusCodes.Status201Created);
		}


		[HttpPut("UpdateTaskStatus")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> UpdateTaskStatus(Guid taskId)
		{
			await _task.UpdateTaskStatus(taskId);
			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPut("UpdateUserTaskStatus")]
		[Authorize(Roles = "Employee")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> UpdateUserTaskStatus(Guid taskId)
		{
			await _task.UpdateUserTaskStatus(taskId);
			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpDelete("DeleteTask")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> DeleteTask(Guid taskId)
		{
				await _task.DeleteTask(taskId);
				return StatusCode(StatusCodes.Status201Created);
		}


		[HttpPost("assignTaskTo")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AssignTaskTo([FromBody] AssignTaskViewModel model)
		{
			await _task.AssignTaskTo(model);
			return StatusCode(StatusCodes.Status201Created);
		}


		[HttpPost("assignUserTaskTo")]
		[Authorize(Roles = "Employee")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AssignUserTaskTo([FromBody] AssignTaskViewModel model)
		{
			await _task.UserAssignTaskTo(model);
			return StatusCode(StatusCodes.Status201Created);
		}

	}
}
