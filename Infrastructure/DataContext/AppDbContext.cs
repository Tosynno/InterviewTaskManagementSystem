using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<tbl_Onboarding> tbl_Onboardings { get; set; }
        public DbSet<tbl_Task> tbl_Tasks { get; set; }
        public DbSet<tbl_TaskAssign> tbl_TaskAssigns { get; set; }
        public DbSet<tbl_AuditLog> tbl_AuditLogs { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<tbl_TaskAssign>()
          .HasOne(p => p.tblTask)
          .WithMany(b => b.tblTaskAssign).HasForeignKey(v => v.TaskId);

            modelBuilder.Entity<tbl_TaskAssign>()
      .HasOne(p => p.tblOnboarding)
      .WithMany(b => b.tblTaskAssign).HasForeignKey(v => v.UserId);
        }
    }
}
