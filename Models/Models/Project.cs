    using Entities.Enum;
    using System;
    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Entities.Models
    {
        public class Project
        {
            [Key]
            public Guid ProjectId { get; set; }
            [Required]
            [StringLength(100)]
            public string ProjectName { get; set; }
            [StringLength(200)]
            public string? Description { get; set; }
            [Required]
            public DateTime CreatedDate { get; set; }
            public DateTime? EndDate { get; set; }
            public Status ProjectStatus { get; set; }
            public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
		    public ICollection<Task> Tasks { get; set; } = new List<Task>();

	    }
    }
