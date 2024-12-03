using System.ComponentModel.DataAnnotations;

namespace LoginRegisterRoles_TelerikDapper.Models
{
	public class User
	{
		[Key]
		public int UserId { get; set; }
		[Required]
		[Display(Name = "Username")]
		[StringLength(20, MinimumLength = 5, ErrorMessage = "Username should be between 5 and 20 characters.")]
		public string UserName
		{
			get;
			set;
		}

		[Required(ErrorMessage = "First Name is Required")]
		[Display(Name = "First Name:")]
		public string FirstName
		{
			get; set;
		}

		[Required(ErrorMessage = "Last Name is Required")]
		[Display(Name = "Last Name:")]
		public string LastName
		{
			get;
			set;
		}


		[RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
		[Display(Name = "Account Email:")]
		public string Email
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "Choose Password:")]
		[DataType(DataType.Password)]
		public string Password
		{
			get;
			set;
		}
		[Required]
		[Display(Name = "Confirm Password:")]
		[Compare("Password")]
		[DataType(DataType.Password)]
		public string? ConfirmPassword
		{
			get;
			set;
		}

		[Required]
		[Display(Name = "Agree to Terms and Conditions:")]
		public bool Agree
		{
			get;
			set;
		}

		public DateTime? DateOfBirth { get; set; }
		public int RoleId { get; set; }

	}
}