using AutoMapper;
using EmployeeAdministration.DTOs.ProjectDTOs;
using EmployeeAdministration.DTOs.TaskDTOs;
using EmployeeAdministration.DTOs.UserDTOs;
using EmployeeAdministration.DTOs.UserProjectDTOs;
using EmployeeAdministration.DTOs.UserTaskDTOs;
using Entities.Models;

namespace EmployeeAdministration.Mappings
{
	public class GeneralProfile: Profile
	{
        public GeneralProfile()
        {
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<Project, ProjectDTO>().ReverseMap();
			CreateMap<UserProject, UserProjectDTO>().ReverseMap();
			CreateMap<UserTask, UserTaskDTO>().ReverseMap();
			CreateMap<Entities.Models.Task, TaskDTO>().ReverseMap();
		}
	}
}
