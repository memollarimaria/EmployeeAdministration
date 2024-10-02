using EmployeeAdministration.Interfaces;
using EmployeeAdministration.Services;
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
		private readonly Serilog.ILogger _logger;
		public ProjectController(IProject project, Serilog.ILogger logger)
		{
			_project = project;
			_logger = logger;
		}

		[HttpGet("GetAllProjects")]
		public async Task<IActionResult> GetAllProjects()
		{
			var projects = await _project.GetAllProject();
			if (projects == null)
			{
				_logger.Error("No projects for this user");
			}
			return Ok(projects);

		}
		[HttpGet("GetProjectTasks")]
		public async Task<IActionResult> GetProjectTasks(Guid projectId)
		{
			var tasks = await _project.GetProjectTasks(projectId);
			return Ok(tasks);
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
			await _project.CreateProject(request);
            _logger.Information("Project created successfully");
            return StatusCode(StatusCodes.Status201Created);

		}

		[HttpPut("UpdateProject")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectViewModel request)
		{
			await _project.UpdateProject(request);
			return StatusCode(StatusCodes.Status201Created);

		}

		[HttpDelete("DeleteProject")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteProject(Guid projectId)
		{
			await _project.DeleteProject(projectId);
			return StatusCode(StatusCodes.Status201Created);

		}


		[HttpPost("assignProjectTo")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AssignProjectTo([FromBody] AssignProjectViewModel model)
		{
			await _project.AssignProjectTo(model);
			return StatusCode(StatusCodes.Status201Created);
		}
	}
}
