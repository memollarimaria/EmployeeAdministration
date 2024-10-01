using Entities.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User : IdentityUser<Guid>
    {
		public string? PhotoPath { get; set; } = null!;
		public byte[]? PhotoContent { get; set; }
		public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
		public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
	}
}
