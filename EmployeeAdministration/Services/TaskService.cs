using Abp.Events.Bus;
using EmployeeAdministration.Events;
using EmployeeAdministration.Helpers;
using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.ProjectsViewModels;
using EmployeeAdministration.ViewModels.TasksViewModels;
using Entities.Enum;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmployeeAdministration.Services
{
	public class TaskService : ITask
	{
		private readonly EmployeeAdministrationContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public TaskService(EmployeeAdministrationContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}
		public async System.Threading.Tasks.Task CreateTask(CreateTaskViewModel request)
		{
			var task = new Entities.Models.Task
			{
				TaskName = request.TaskName,
				Description = request.Description,
				IsCompleted = false,
				CreatedAt = DateTime.Now,
				DueDate = request.DueDate,
				ProjectId = request.ProjectId,
				UserTasks = new List<UserTask>()
			};

			var project = await _context.Projects
				.Include(p => p.UserProjects) 
				.FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId);

			if (project == null)
			{
				throw new Exception("Project not found.");
			}

			foreach (var userTask in request.UserTasks)
			{
				bool isUserInProject = project.UserProjects.Any(up => up.UserId == userTask.userId);

				if (isUserInProject && !task.UserTasks.Any(ut => ut.UserId == userTask.userId))
				{
					task.UserTasks.Add(new UserTask
					{
						UserId = userTask.userId,
						TaskId = task.ProjectId,
					});
				}
				else if (!isUserInProject)
				{
					throw new Exception($"User with ID {userTask.userId} is not part of the project.");
				}
			}

			_context.Tasks.Add(task);
			await _context.SaveChangesAsync();

			EventBus.Default.Trigger(new TaskCreatedEvent { TaskId = task.TaskId, TaskName = task.TaskName });
		}


		public async System.Threading.Tasks.Task CreateUserTask(CreateTaskViewModel request)
		{
			Guid userId = StaticFunc.GetUserId(_httpContextAccessor);

			var project = await _context.Projects
				.Include(p => p.UserProjects)
				.FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId
										  && p.UserProjects.Any(up => up.UserId == userId));

			if (project == null)
			{
				throw new Exception("You are not assigned to this project or the project doesn't exist.");
			}

			var task = new Entities.Models.Task
			{
				TaskName = request.TaskName,
				Description = request.Description,
				CreatedAt = DateTime.Now,
				DueDate = request.DueDate,
				IsCompleted = false,
				ProjectId = request.ProjectId,
				UserTasks = new List<UserTask>()
			};
			foreach (var userTask in request.UserTasks)
			{
				if (!task.UserTasks.Any(up => up.UserId == userTask.userId))
				{
					task.UserTasks.Add(new UserTask
					{
						UserId = userTask.userId,
						TaskId = task.ProjectId,
					});
				}
			}

			_context.Tasks.Add(task);
			await _context.SaveChangesAsync();

			EventBus.Default.Trigger(new TaskCreatedEvent { TaskId = task.TaskId, TaskName = task.TaskName });

		}


		public async System.Threading.Tasks.Task DeleteTask(Guid taskId)
		{
			var task = await _context.Tasks
				.FirstOrDefaultAsync(p => p.TaskId == taskId);

			if (task == null)
			{
				throw new Exception("Task not found.");
			}

			_context.Tasks.Remove(task);
			await _context.SaveChangesAsync();

			EventBus.Default.Trigger(new TaskDeletedEvent { TaskId = task.TaskId, TaskName = task.TaskName });

		}

		public async Task<ICollection<TaskViewModel>> GetUserTasks()
		{
			Guid userId = StaticFunc.GetUserId(_httpContextAccessor);
			var tasks = await _context.Tasks
				   .Where(p => p.UserTasks.Any(up => up.UserId == userId))
				   .ToListAsync();

			var tasksViewModel = tasks.Select(u => new TaskViewModel
			{
				TaskName = u.TaskName,
				Description = u.Description,
				CreatedAt = u.CreatedAt,
				DueDate = u.DueDate,
				IsCompleted	= u.IsCompleted,
			}).ToList();

			return tasksViewModel;
		}

		public async Task<ICollection<TaskViewModel>> GetAllTasks()
		{
			var tasks = await _context.Tasks
				   .ToListAsync();

			var tasksViewModel = tasks.Select(u => new TaskViewModel
			{
				TaskName = u.TaskName,
				Description = u.Description,
				CreatedAt = u.CreatedAt,
				DueDate = u.DueDate,
				IsCompleted = u.IsCompleted,
			}).ToList();

			return tasksViewModel;
		}

		public async System.Threading.Tasks.Task UpdateTask(UpdateTaskViewModel request)
		{
			var task = await _context.Tasks
				.Include(p => p.UserTasks)
				.FirstOrDefaultAsync(p => p.TaskId == request.TaskId);

			if (task == null)
			{
				throw new Exception("Task not found.");
			}

			task.TaskName = request.TaskName;
			task.Description = request.Description;
			task.DueDate = request.DueDate;
			task.IsCompleted = request.IsCompleted;

			_context.Tasks.Update(task);
			await _context.SaveChangesAsync();

			EventBus.Default.Trigger(new TaskUpdatedEvent { TaskId = task.TaskId, TaskName = task.TaskName });

		}

		public async System.Threading.Tasks.Task UpdateTaskStatus(Guid taskid)
		{
			var task = await _context.Tasks
				.FirstOrDefaultAsync(p => p.TaskId == taskid);
			if (task == null) {
				throw new Exception("Task not found");
				}
			task.IsCompleted = !task.IsCompleted;
			_context.Tasks.Update(task);
			await _context.SaveChangesAsync();

			EventBus.Default.Trigger(new TaskStatusUpdatedEvent
			{
				TaskId = task.TaskId,
				IsCompleted = task.IsCompleted
			});
		}

		public async System.Threading.Tasks.Task UpdateUserTaskStatus(Guid taskId)
		{
			Guid userId = StaticFunc.GetUserId(_httpContextAccessor);

			var task = await _context.Tasks
			  .Include(t => t.UserTasks) 
			  .FirstOrDefaultAsync(t => t.TaskId == taskId
										&& t.UserTasks.Any(ut => ut.UserId == userId));

			if (task == null)
			{
				throw new Exception("Task not found or you are not assigned to this task.");
			}

			task.IsCompleted = !task.IsCompleted;

			_context.Tasks.Update(task);
			await _context.SaveChangesAsync();

			EventBus.Default.Trigger(new TaskStatusUpdatedEvent
			{
				TaskId = task.TaskId,
				IsCompleted = task.IsCompleted
			});
		}
		public async System.Threading.Tasks.Task AssignTaskTo(AssignTaskViewModel request)
		{
			var task = await _context.Tasks
				.Include(t => t.Project)
				.Include(t => t.UserTasks)
				.FirstOrDefaultAsync(t => t.TaskId == request.TaskId);

			if (task == null)
			{
				throw new Exception("Task not found.");
			}

			var users = await _context.UserProjects
				.Where(up => up.ProjectId == task.Project.ProjectId && request.UserIds.Contains(up.UserId))
				.Select(up => up.User)
				.ToListAsync();

			if (!users.Any())
			{
				throw new Exception("No valid users found for assignment or they are not part of the project.");
			}

			foreach (var user in users)
			{
				if (!task.UserTasks.Any(ut => ut.UserId == user.Id))
				{
					task.UserTasks.Add(new UserTask
					{
						UserId = user.Id,
						TaskId = task.TaskId
					});
				}
			}

			await _context.SaveChangesAsync();
			EventBus.Default.Trigger(new TaskAssignedEvent
			{
				TaskId = task.TaskId,
				AssignedUserIds = request.UserIds
			});

		}

		public async System.Threading.Tasks.Task UserAssignTaskTo(AssignTaskViewModel request)
		{
			var task = await _context.Tasks
				.Include(t => t.Project)
				.Include(t => t.UserTasks)
				.FirstOrDefaultAsync(t => t.TaskId == request.TaskId);

			if (task == null)
			{
				throw new Exception("Task not found.");
			}

			var employees = await _context.UserProjects
				.Where(up => up.ProjectId == task.Project.ProjectId && request.UserIds.Contains(up.UserId))
				.Select(up => up.User)
				.ToListAsync();

			if (!employees.Any())
			{
				throw new Exception("No valid employees found for assignment or they are not part of the project.");
			}

			foreach (var employee in employees)
			{
				if (!task.UserTasks.Any(ut => ut.UserId == employee.Id)) 
				{
					task.UserTasks.Add(new UserTask
					{
						UserId = employee.Id,
						TaskId = task.TaskId
					});
				}
			}
			await _context.SaveChangesAsync();

			EventBus.Default.Trigger(new TaskAssignedEvent
			{
				TaskId = task.TaskId,
				AssignedUserIds = request.UserIds
			});
		}


	}
}
