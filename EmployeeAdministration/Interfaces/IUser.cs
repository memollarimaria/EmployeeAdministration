using Entities.Models;
using Task = System.Threading.Tasks.Task;

namespace EmployeeAdministration.Interfaces
{
	public interface IUser
	{
		Task<string> Login(User request);
		Task CreateUser(User request);
		Task DeleteUser(Guid userId);
		Task UpdateUser(User request);
	}
}
