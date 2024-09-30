using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Task
    {
        [Key]
        public Guid TaskId { get; set; }
        [Required]
        [StringLength(100)]
        public string TaskName { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        [DataType(DataType.Date)]
		public DateTime? DueDate { get; set; }
		[DataType(DataType.Date)]
        [Required]
		public DateTime CreatedAt { get; set; }
        [Required]
		public Guid ProjectId { get; set; }
        public Project Project { get; set; }
		public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
	}
}
