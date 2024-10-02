using EmployeeAdministration.ViewModels.UserViewModels;
using Entities.Models;
using Task = System.Threading.Tasks.Task;

namespace EmployeeAdministration.Interfaces
{
	public interface IUser
	{
		Task<ICollection<UserViewModel>> GetAllUsers();
		Task<string> Login(LogInViewModel request);
		Task CreateUser(LogInViewModel request);
		Task DeleteUser(Guid userId);
		Task UpdateUserProfilePicture(UpdateUserProfilePictureViewModel request);
		Task UpdateUser(UpdateUserViewModel request);

    }
}
