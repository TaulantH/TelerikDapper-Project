using LoginRegisterRoles_TelerikDapper.Models;
using System.Numerics;

namespace LoginRegisterRoles_TelerikDapper
{ 
	public interface IAdminRepository
	{
		User Find(int id);
		IEnumerable<User> GetAllUsers();
	//User Add(User user);
	User Update(User user);
		void Remove(int id);
		Task<int> GetByIdAsync(int userId);
		Task SaveChangesAsync();
	}
}
