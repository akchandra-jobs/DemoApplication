using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a employee entity with essential details
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// TenantId of the Employee 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Employee 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the Employee 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Employee 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Foreign key referencing the Department to which the Employee belongs 
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Department
        /// </summary>
        [ForeignKey("DepartmentId")]
        public Department? DepartmentId_Department { get; set; }
        /// <summary>
        /// Foreign key referencing the JobTitle to which the Employee belongs 
        /// </summary>
        public Guid? JobTitleId { get; set; }

        /// <summary>
        /// Navigation property representing the associated JobTitle
        /// </summary>
        [ForeignKey("JobTitleId")]
        public JobTitle? JobTitleId_JobTitle { get; set; }
        /// <summary>
        /// Foreign key referencing the EmploymentStatus to which the Employee belongs 
        /// </summary>
        public Guid? EmploymentStatusId { get; set; }

        /// <summary>
        /// Navigation property representing the associated EmploymentStatus
        /// </summary>
        [ForeignKey("EmploymentStatusId")]
        public EmploymentStatus? EmploymentStatusId_EmploymentStatus { get; set; }
        /// <summary>
        /// Email of the Employee 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Phone of the Employee 
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Address of the Employee 
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// HireDate of the Employee 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// CreatedOn of the Employee 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Employee 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Employee 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Employee 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}