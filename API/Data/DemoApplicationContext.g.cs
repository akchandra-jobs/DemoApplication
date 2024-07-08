using Microsoft.EntityFrameworkCore;
using DemoApplication.Entities;

namespace DemoApplication.Data
{
    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class DemoApplicationContext : DbContext
    {
        /// <summary>
        /// Configures the database connection options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the database connection.</param>
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=codezen.database.windows.net;Initial Catalog=codezen;Persist Security Info=True;user id=Lowcodeadmin;password=NtLowCode^123*;Integrated Security=false;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=true;");
        }

        /// <summary>
        /// Configures the model relationships and entity mappings.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the database model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInRole>().HasKey(a => a.Id);
            modelBuilder.Entity<UserToken>().HasKey(a => a.Id);
            modelBuilder.Entity<RoleEntitlement>().HasKey(a => a.Id);
            modelBuilder.Entity<Entity>().HasKey(a => a.Id);
            modelBuilder.Entity<Tenant>().HasKey(a => a.Id);
            modelBuilder.Entity<User>().HasKey(a => a.Id);
            modelBuilder.Entity<Role>().HasKey(a => a.Id);
            modelBuilder.Entity<Employee>().HasKey(a => a.Id);
            modelBuilder.Entity<TimeEntry>().HasKey(a => a.Id);
            modelBuilder.Entity<TimePeriod>().HasKey(a => a.Id);
            modelBuilder.Entity<Activity>().HasKey(a => a.Id);
            modelBuilder.Entity<Milestone>().HasKey(a => a.Id);
            modelBuilder.Entity<WorkLogs>().HasKey(a => a.Id);
            modelBuilder.Entity<TimeAllocation>().HasKey(a => a.Id);
            modelBuilder.Entity<CustomTask>().HasKey(a => a.Id);
            modelBuilder.Entity<Client>().HasKey(a => a.Id);
            modelBuilder.Entity<Billing>().HasKey(a => a.Id);
            modelBuilder.Entity<WorkHours>().HasKey(a => a.Id);
            modelBuilder.Entity<Timezone>().HasKey(a => a.Id);
            modelBuilder.Entity<Breaks>().HasKey(a => a.Id);
            modelBuilder.Entity<PerformanceReview>().HasKey(a => a.Id);
            modelBuilder.Entity<Payroll>().HasKey(a => a.Id);
            modelBuilder.Entity<PayGrade>().HasKey(a => a.Id);
            modelBuilder.Entity<EmployeeSkills>().HasKey(a => a.Id);
            modelBuilder.Entity<EmployeeTraining>().HasKey(a => a.Id);
            modelBuilder.Entity<EmployeeDocuments>().HasKey(a => a.Id);
            modelBuilder.Entity<Project>().HasKey(a => a.Id);
            modelBuilder.Entity<Department>().HasKey(a => a.Id);
            modelBuilder.Entity<JobTitle>().HasKey(a => a.Id);
            modelBuilder.Entity<EmploymentStatus>().HasKey(a => a.Id);
            modelBuilder.Entity<Attendance>().HasKey(a => a.Id);
            modelBuilder.Entity<Timesheet>().HasKey(a => a.Id);
            modelBuilder.Entity<TimeOffRequest>().HasKey(a => a.Id);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.RoleId_Role).WithMany().HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.UserId_User).WithMany().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<UserToken>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<UserToken>().HasOne(a => a.UserId_User).WithMany().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.RoleId_Role).WithMany().HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.EntityId_Entity).WithMany().HasForeignKey(c => c.EntityId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<Entity>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Entity>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<Entity>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<User>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Role>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Role>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<Role>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<Employee>().HasOne(a => a.DepartmentId_Department).WithMany().HasForeignKey(c => c.DepartmentId);
            modelBuilder.Entity<Employee>().HasOne(a => a.JobTitleId_JobTitle).WithMany().HasForeignKey(c => c.JobTitleId);
            modelBuilder.Entity<Employee>().HasOne(a => a.EmploymentStatusId_EmploymentStatus).WithMany().HasForeignKey(c => c.EmploymentStatusId);
            modelBuilder.Entity<TimeEntry>().HasOne(a => a.TaskId_CustomTask).WithMany().HasForeignKey(c => c.TaskId);
            modelBuilder.Entity<Activity>().HasOne(a => a.TimePeriodId_TimePeriod).WithMany().HasForeignKey(c => c.TimePeriodId);
            modelBuilder.Entity<Milestone>().HasOne(a => a.TimePeriodId_TimePeriod).WithMany().HasForeignKey(c => c.TimePeriodId);
            modelBuilder.Entity<WorkLogs>().HasOne(a => a.EmployeeId_Employee).WithMany().HasForeignKey(c => c.EmployeeId);
            modelBuilder.Entity<WorkLogs>().HasOne(a => a.ActivityId_Activity).WithMany().HasForeignKey(c => c.ActivityId);
            modelBuilder.Entity<TimeAllocation>().HasOne(a => a.EmployeeId_Employee).WithMany().HasForeignKey(c => c.EmployeeId);
            modelBuilder.Entity<TimeAllocation>().HasOne(a => a.ActivityId_Activity).WithMany().HasForeignKey(c => c.ActivityId);
            modelBuilder.Entity<Billing>().HasOne(a => a.ClientId_Client).WithMany().HasForeignKey(c => c.ClientId);
            modelBuilder.Entity<Billing>().HasOne(a => a.TaskId_CustomTask).WithMany().HasForeignKey(c => c.TaskId);
            modelBuilder.Entity<WorkHours>().HasOne(a => a.TaskId_CustomTask).WithMany().HasForeignKey(c => c.TaskId);
            modelBuilder.Entity<Breaks>().HasOne(a => a.TimeEntryId_TimeEntry).WithMany().HasForeignKey(c => c.TimeEntryId);
            modelBuilder.Entity<Attendance>().HasOne(a => a.EmployeeId_Employee).WithMany().HasForeignKey(c => c.EmployeeId);
            modelBuilder.Entity<Timesheet>().HasOne(a => a.EmployeeId_Employee).WithMany().HasForeignKey(c => c.EmployeeId);
            modelBuilder.Entity<TimeOffRequest>().HasOne(a => a.EmployeeId_Employee).WithMany().HasForeignKey(c => c.EmployeeId);
        }

        /// <summary>
        /// Represents the database set for the UserInRole entity.
        /// </summary>
        public DbSet<UserInRole> UserInRole { get; set; }

        /// <summary>
        /// Represents the database set for the UserToken entity.
        /// </summary>
        public DbSet<UserToken> UserToken { get; set; }

        /// <summary>
        /// Represents the database set for the RoleEntitlement entity.
        /// </summary>
        public DbSet<RoleEntitlement> RoleEntitlement { get; set; }

        /// <summary>
        /// Represents the database set for the Entity entity.
        /// </summary>
        public DbSet<Entity> Entity { get; set; }

        /// <summary>
        /// Represents the database set for the Tenant entity.
        /// </summary>
        public DbSet<Tenant> Tenant { get; set; }

        /// <summary>
        /// Represents the database set for the User entity.
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Represents the database set for the Role entity.
        /// </summary>
        public DbSet<Role> Role { get; set; }

        /// <summary>
        /// Represents the database set for the Employee entity.
        /// </summary>
        public DbSet<Employee> Employee { get; set; }

        /// <summary>
        /// Represents the database set for the TimeEntry entity.
        /// </summary>
        public DbSet<TimeEntry> TimeEntry { get; set; }

        /// <summary>
        /// Represents the database set for the TimePeriod entity.
        /// </summary>
        public DbSet<TimePeriod> TimePeriod { get; set; }

        /// <summary>
        /// Represents the database set for the Activity entity.
        /// </summary>
        public DbSet<Activity> Activity { get; set; }

        /// <summary>
        /// Represents the database set for the Milestone entity.
        /// </summary>
        public DbSet<Milestone> Milestone { get; set; }

        /// <summary>
        /// Represents the database set for the WorkLogs entity.
        /// </summary>
        public DbSet<WorkLogs> WorkLogs { get; set; }

        /// <summary>
        /// Represents the database set for the TimeAllocation entity.
        /// </summary>
        public DbSet<TimeAllocation> TimeAllocation { get; set; }

        /// <summary>
        /// Represents the database set for the CustomTask entity.
        /// </summary>
        public DbSet<CustomTask> CustomTask { get; set; }

        /// <summary>
        /// Represents the database set for the Client entity.
        /// </summary>
        public DbSet<Client> Client { get; set; }

        /// <summary>
        /// Represents the database set for the Billing entity.
        /// </summary>
        public DbSet<Billing> Billing { get; set; }

        /// <summary>
        /// Represents the database set for the WorkHours entity.
        /// </summary>
        public DbSet<WorkHours> WorkHours { get; set; }

        /// <summary>
        /// Represents the database set for the Timezone entity.
        /// </summary>
        public DbSet<Timezone> Timezone { get; set; }

        /// <summary>
        /// Represents the database set for the Breaks entity.
        /// </summary>
        public DbSet<Breaks> Breaks { get; set; }

        /// <summary>
        /// Represents the database set for the PerformanceReview entity.
        /// </summary>
        public DbSet<PerformanceReview> PerformanceReview { get; set; }

        /// <summary>
        /// Represents the database set for the Payroll entity.
        /// </summary>
        public DbSet<Payroll> Payroll { get; set; }

        /// <summary>
        /// Represents the database set for the PayGrade entity.
        /// </summary>
        public DbSet<PayGrade> PayGrade { get; set; }

        /// <summary>
        /// Represents the database set for the EmployeeSkills entity.
        /// </summary>
        public DbSet<EmployeeSkills> EmployeeSkills { get; set; }

        /// <summary>
        /// Represents the database set for the EmployeeTraining entity.
        /// </summary>
        public DbSet<EmployeeTraining> EmployeeTraining { get; set; }

        /// <summary>
        /// Represents the database set for the EmployeeDocuments entity.
        /// </summary>
        public DbSet<EmployeeDocuments> EmployeeDocuments { get; set; }

        /// <summary>
        /// Represents the database set for the Project entity.
        /// </summary>
        public DbSet<Project> Project { get; set; }

        /// <summary>
        /// Represents the database set for the Department entity.
        /// </summary>
        public DbSet<Department> Department { get; set; }

        /// <summary>
        /// Represents the database set for the JobTitle entity.
        /// </summary>
        public DbSet<JobTitle> JobTitle { get; set; }

        /// <summary>
        /// Represents the database set for the EmploymentStatus entity.
        /// </summary>
        public DbSet<EmploymentStatus> EmploymentStatus { get; set; }

        /// <summary>
        /// Represents the database set for the Attendance entity.
        /// </summary>
        public DbSet<Attendance> Attendance { get; set; }

        /// <summary>
        /// Represents the database set for the Timesheet entity.
        /// </summary>
        public DbSet<Timesheet> Timesheet { get; set; }

        /// <summary>
        /// Represents the database set for the TimeOffRequest entity.
        /// </summary>
        public DbSet<TimeOffRequest> TimeOffRequest { get; set; }
    }
}