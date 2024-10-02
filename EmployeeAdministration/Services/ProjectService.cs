using EmployeeAdministration.EventBus;
using EmployeeAdministration.Helpers;
using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.ProjectsViewModels;
using EmployeeAdministration.ViewModels.TasksViewModels;
using EmployeeAdministration.ViewModels.UserProjectsViewModels;
using EmployeeAdministration.ViewModels.UserViewModels;
using Entities.Enum;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdministration.Services
{
	public class ProjectService : IProject
	{
		private readonly EmployeeAdministrationContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
		private readonly ProjectEvent _projectEvent;
        public ProjectService(EmployeeAdministrationContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ProjectEvent projectEvent)

        {
			_context = context;
			_httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _projectEvent = projectEvent;
        }

		public async System.Threading.Tasks.Task CreateProject(CreateProjectViewModel request)
		{
			var project = new Project
			{
				ProjectName = request.ProjectName,
				Description = request.Description,
				CreatedDate = DateTime.Now,
				EndDate = request.EndDate,
				ProjectStatus = Status.Active,
				UserProjects = new List<UserProject>()
			};
			foreach (var userProjectVm in request.UserProjects)
			{
				if (!project.UserProjects.Any(up => up.UserId == userProjectVm.UserId))
				{
					project.UserProjects.Add(new UserProject
					{
						UserId = userProjectVm.UserId,
						ProjectId = project.ProjectId,
					});
				}
			}
			_context.Projects.Add(project);
            _context.SaveChanges();
            _projectEvent.LogProjectCreated(project.ProjectName);
		}

		public async System.Threading.Tasks.Task DeleteProject(Guid projectId)
		{
			var project = await _context.Projects
				.Include(p => p.Tasks)
				.FirstOrDefaultAsync(p => p.ProjectId == projectId);

			if (project == null)
			{
				throw new Exception("Project not found.");
			}

			var hasOpenTasks = project.Tasks.Any(t => !t.IsCompleted);
			if (hasOpenTasks)
			{
				throw new Exception("Cannot delete a project with open tasks.");
			}

			_context.Projects.Remove(project);
            _projectEvent.LogProjectDeleted(project.ProjectName);
            await _context.SaveChangesAsync();
		}


		public async Task<ICollection<ProjectViewModel>> GetAllProject()
		{
			var projects = await _context.Projects
				.ToListAsync();

			var projectViewModels = projects.Select(project => new ProjectViewModel
			{
				ProjectName = project.ProjectName,
				Description = project.Description,
				CreatedDate = project.CreatedDate,
				EndDate = project.EndDate,
				ProjectStatus = project.ProjectStatus,
			}).ToList();
			return projectViewModels;
		}


		public async Task<ICollection<ProjectViewModel>> GetUserProjects()
		{
			Guid userId = StaticFunc.GetUserId(_httpContextAccessor);
			var projects = await _context.Projects
				   .Include(p => p.Tasks)
				   .Where(p => p.UserProjects.Any(up => up.UserId == userId))
				   .ToListAsync(); 

			var projectViewModels = projects.Select(u => new ProjectViewModel
			{
				ProjectName = u.ProjectName,
				Description = u.Description,
				CreatedDate = u.CreatedDate,
				EndDate = u.EndDate,
				ProjectStatus = u.ProjectStatus,
			}).ToList();

			return projectViewModels;
		}

		public async System.Threading.Tasks.Task UpdateProject(UpdateProjectViewModel request)
		{
			var project = await _context.Projects
				.Include(p => p.UserProjects)
				.FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId);

			if (project == null)
			{
				throw new Exception("Project not found.");
			}

			project.ProjectName = request.ProjectName;
			project.Description = request.Description;
			project.EndDate = request.EndDate;
			project.ProjectStatus = request.ProjectStatus;

			_context.Projects.Update(project);
            await _context.SaveChangesAsync();
            _projectEvent.LogProjectUpdated(project.ProjectName);
		}

		public async Task<ICollection<TaskViewModel>> GetProjectTasks(Guid projectId)
		{
			var project = await _context.Projects
				.Include(p => p.Tasks)
				.FirstOrDefaultAsync(p => p.ProjectId == projectId);

			if (project == null)
			{
				throw new Exception("Project not found.");
			}
			var taskViewModels = project.Tasks.Select(task => new TaskViewModel
			{
				TaskName = task.TaskName,
				Description = task.Description,
				CreatedAt = task.CreatedAt,
				DueDate = task.DueDate,
				IsCompleted = task.IsCompleted
			}).ToList();

			return taskViewModels;
		}

		public async System.Threading.Tasks.Task AssignProjectTo(AssignProjectViewModel request)
		{
			var project = await _context.Projects
				.Include(p => p.UserProjects)
				.FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId);

			if (project == null)
			{
				throw new Exception("Project not found.");
			}
			var users = await _context.Users
				.Where(u => request.UserIds.Contains(u.Id))
				.ToListAsync();

			if (!users.Any())
			{
				throw new Exception("No valid users found for assignment.");
			}
            var assignedUsers = new List<string>();
            foreach (var user in users)
			{
				if (!project.UserProjects.Any(up => up.UserId == user.Id))
				{
					project.UserProjects.Add(new UserProject
					{
						UserId = user.Id,
						ProjectId = project.ProjectId
					});
                    assignedUsers.Add(user.Email);
                }
			}
			await _context.SaveChangesAsync();
            _projectEvent.LogProjectAssigned(assignedUsers);

        }


    }
}
