using EmployeeAdministration.Interfaces;
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

        public TaskController(ITask task)
		{
			_task = task;
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
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _task.CreateTask(request);
			return Ok("Task created successfully.");
		}

		[HttpPut("UpdateTask")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			await _task.UpdateTask(request);
			return Ok("Task updated successfully");
		}

		[HttpDelete("DeleteTask")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteTask(Guid taskId)
		{
			try
			{
				await _task.DeleteTask(taskId);
				return Ok("task deleted Successfully");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
