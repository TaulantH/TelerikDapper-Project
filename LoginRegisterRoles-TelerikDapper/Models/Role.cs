﻿using System.ComponentModel.DataAnnotations;

namespace LoginRegisterRoles_TelerikDapper.Models
{
	public class Role
	{
		[Key]

		public int RoleId { get; set; }
		public string RoleName { get; set; }
	}
}
