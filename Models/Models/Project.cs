using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<Task> Tasks { get; set; }

    }
}
