using BCrypt.Net;


namespace LoginRegisterRoles_TelerikDapper.Utilities;
public class PasswordHelper
{
	// Hash the password
	public static string HashPassword(string password)
	{
		return BCrypt.Net.BCrypt.HashPassword(password);  // Hash the plain password
	}

	public static bool VerifyPassword(string inputPassword, string storedHash)
	{
		return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
	}
}
