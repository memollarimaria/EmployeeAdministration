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
        [Key]
        public Guid UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 6, ErrorMessage = "Password must have at least 6 characters")]
        public string Password { get; set; }
        [Required]  
        public Role Role { get; set; }
		public string? PhotoPath { get; set; } = null!;
		public byte[]? PhotoContent { get; set; }
		public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
		public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
	}
}
