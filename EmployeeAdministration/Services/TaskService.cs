using EmployeeAdministration.Helpers;
using EmployeeAdministration.Interfaces;
using EmployeeAdministration.ViewModels.ProjectsViewModels;
using EmployeeAdministration.ViewModels.TasksViewModels;
using Entities.Enum;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

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
				CreatedAt  =  DateTime.Now,
				DueDate = request.DueDate,
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
			_context.SaveChanges();
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
		}
	}
}
