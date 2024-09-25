using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UserRole
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<User> Users { get; set; } = null!;
    }
}
