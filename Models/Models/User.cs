using Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public Role Role { get; set; }
		public string? PhotoPath { get; set; } = null!;
		public byte[]? PhotoContent { get; set; }
		public ICollection<UserProject> UserProjects { get; set; }
		public ICollection<UserTask> UserTasks { get; set; }
	}
}
