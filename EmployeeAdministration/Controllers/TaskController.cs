using EmployeeAdministration.Interfaces;
using EmployeeAdministration.Services;
using EmployeeAdministration.ViewModels.ProjectsViewModels;
using EmployeeAdministration.ViewModels.TasksViewModels;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EmployeeAdministration.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly ITask _task;
		private readonly Serilog.ILogger _logger;

		public TaskController(ITask task, Serilog.ILogger logger)
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
		public async Task<IActionResult> CreateTask([FromBody] CreateTaskViewModel request)
		{
			await _task.CreateTask(request);
            _logger.Information("Task created");
            return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPost("CreateUserTask")]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> CreateUserTask([FromBody] CreateTaskViewModel request)
		{
			await _task.CreateUserTask(request);
            _logger.Information("Task created");
            return StatusCode(StatusCodes.Status201Created);
		}


		[HttpPut("UpdateTask")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskViewModel request)
		{
			await _task.UpdateTask(request);
            _logger.Information("Task updated");
            return StatusCode(StatusCodes.Status201Created);
		}


		[HttpPut("UpdateTaskStatus")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateTaskStatus(Guid taskId)
		{
			await _task.UpdateTaskStatus(taskId);
			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpPut("UpdateUserTaskStatus")]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> UpdateUserTaskStatus(Guid taskId)
		{
			await _task.UpdateUserTaskStatus(taskId);
			return StatusCode(StatusCodes.Status201Created);
		}

		[HttpDelete("DeleteTask")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteTask(Guid taskId)
		{
				await _task.DeleteTask(taskId);
				return StatusCode(StatusCodes.Status201Created);
		}

        [HttpDelete("DeleteAssigmentTask")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAssigmentTask(RemoveAssignmentViewModel task)
        {
            await _task.RemoveAssignment(task);
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpPost("assignTaskTo")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AssignTaskTo([FromBody] AssignTaskViewModel model)
		{
			await _task.AssignTaskTo(model);
			return StatusCode(StatusCodes.Status201Created);
		}


		[HttpPost("assignUserTaskTo")]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> AssignUserTaskTo([FromBody] AssignTaskViewModel model)
		{
			await _task.UserAssignTaskTo(model);
			return StatusCode(StatusCodes.Status201Created);
		}

	}
}
