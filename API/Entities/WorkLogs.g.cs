using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a worklogs entity with essential details
    /// </summary>
    public class WorkLogs
    {
        /// <summary>
        /// TenantId of the WorkLogs 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the WorkLogs 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the WorkLogs 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Foreign key referencing the Employee to which the WorkLogs belongs 
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Employee
        /// </summary>
        [ForeignKey("EmployeeId")]
        public Employee? EmployeeId_Employee { get; set; }
        /// <summary>
        /// Foreign key referencing the Activity to which the WorkLogs belongs 
        /// </summary>
        public Guid? ActivityId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Activity
        /// </summary>
        [ForeignKey("ActivityId")]
        public Activity? ActivityId_Activity { get; set; }

        /// <summary>
        /// Date of the WorkLogs 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        /// <summary>
        /// Hours of the WorkLogs 
        /// </summary>
        public int? Hours { get; set; }
        /// <summary>
        /// Description of the WorkLogs 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// CreatedOn of the WorkLogs 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the WorkLogs 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the WorkLogs 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the WorkLogs 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}