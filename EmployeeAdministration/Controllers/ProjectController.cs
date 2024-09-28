using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.ProjectsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdministration.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly IProject _project;
		public ProjectController(IProject project)
		{
			_project = project;
		}

		[HttpGet("GetAllProjects")]
		public async Task<IActionResult> GetAllProjects()
		{
			var projects = await _project.GetAllProject();
			return Ok(projects);
		}

		[HttpGet("GetAllUserProjects")]
		public async Task<IActionResult> GetAllUserProjects()
		{
			var projects = await _project.GetUserProjects();
			return Ok(projects);
		}

		[HttpPost("CreateProject")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _project.CreateProject(request);
			return Ok("Project created successfully.");
		}

		[HttpPut("UpdateProject")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			await _project.UpdateProject(request);
			return Ok("Project updated successfully");
		}

		[HttpDelete("DeleteProject")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteProject(Guid projectId)
		{
			try
			{
				await _project.DeleteProject(projectId);
				return Ok("Project deleted Successfully");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
