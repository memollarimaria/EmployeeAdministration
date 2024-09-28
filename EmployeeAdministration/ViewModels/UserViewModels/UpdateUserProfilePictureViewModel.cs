using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeAdministration.ViewModels.UserViewModels
{
	public class UpdateUserProfilePictureViewModel
	{
		[JsonIgnore]
		[DataType(DataType.Upload)]
		[Display(Name = "Upload Image")]
		public IFormFile? ImageFile { get; set; }
	}
}
