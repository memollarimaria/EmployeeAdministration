using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class EmployeeAdministrationContext : IdentityDbContext<User,UserRole,Guid>
	{
		public EmployeeAdministrationContext(DbContextOptions<EmployeeAdministrationContext> options)
	: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EmployeeAdmin;Trusted_Connection=True;");
			}
		}
		public DbSet<Task>? Tasks { get; set; }
		public DbSet<Project>? Projects { get; set; }
		public DbSet<UserProject>? UserProjects { get; set; }
		public DbSet<UserTask>? UserTasks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserProject>()
				.HasKey(up => new { up.UserId, up.ProjectId });

			modelBuilder.Entity<UserProject>()
				.HasOne(up => up.User)
				.WithMany(u => u.UserProjects)
				.HasForeignKey(up => up.UserId);

			modelBuilder.Entity<UserProject>()
				.HasOne(up => up.Project)
				.WithMany(p => p.UserProjects)
				.HasForeignKey(up => up.ProjectId);

			modelBuilder.Entity<UserTask>()
				.HasKey(ut => new { ut.UserId, ut.TaskId });

			modelBuilder.Entity<UserTask>()
				.HasOne(ut => ut.User)
				.WithMany(u => u.UserTasks)
				.HasForeignKey(ut => ut.UserId);

			modelBuilder.Entity<UserTask>()
				.HasOne(ut => ut.Task)
				.WithMany(t => t.UserTasks)
				.HasForeignKey(ut => ut.TaskId);

			modelBuilder.Entity<Task>()
			  .HasOne(t => t.Project)
			  .WithMany(p => p.Tasks)
			  .HasForeignKey(t => t.ProjectId)
			  .OnDelete(DeleteBehavior.Cascade);

			base.OnModelCreating(modelBuilder);
		}
	}

}
