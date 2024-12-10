using System.ComponentModel.DataAnnotations;

namespace LoginRegisterRoles_TelerikDapper.Models
{
	public class News
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime PublishDate { get; set; } = DateTime.Now;
		public string Location { get; set; }

		public string Images { get; set; }
	}


}
