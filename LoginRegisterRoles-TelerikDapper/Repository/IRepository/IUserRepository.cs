using LoginRegisterRoles_TelerikDapper.Models;
using System.Threading.Tasks;

namespace LoginRegisterRoles_TelerikDapper.Repository.IRepository
{
	public interface IUserRepository
	{
		Task<bool> Insert(User user);
		Task<User> Login(string email, string password);
	}
}
