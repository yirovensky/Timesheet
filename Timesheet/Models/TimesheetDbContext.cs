using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Timesheet.Models
{
    public class TimesheetDbContext : DbContext
    {
        public TimesheetDbContext(DbContextOptions<TimesheetDbContext> options) : base(options) { }

        // DbSet для работы с таблицей Timesheets в базе данных
        public DbSet<Timesheet> Timesheets { get; set; }

        // DbSet для работы с таблицей Employees в базе данных
        public DbSet<Employee> Employees { get; set; }

        // DbSet для работы с таблицей Reasons в базе данных
        public DbSet<Reason> Reasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи между таблицей Timesheets и Employees
            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.Employee)         // Указываем, что у Timesheet есть один Employee
                .WithMany()                      // Указываем, что у Employee может быть много связанных Timesheet
                .HasForeignKey(t => t.Employee_id); // Указываем внешний ключ, связывающий две таблицы

            // Настройка связи между таблицей Timesheets и Reasons
            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.Reason)           // Указываем, что у Timesheet есть одна Reason
                .WithMany()                      // Указываем, что у Reason может быть много связанных Timesheet
                .HasForeignKey(t => t.Reason_id);  // Указываем внешний ключ, связывающий две таблицы
        }
    }

}
