using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class UserTask
	{
		public Guid UserId { get; set; }
		public User User { get; set; }
		public Guid TaskId { get; set; }
		public Task Task { get; set; }
	}
}
