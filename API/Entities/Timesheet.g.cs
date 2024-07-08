using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a timesheet entity with essential details
    /// </summary>
    public class Timesheet
    {
        /// <summary>
        /// TenantId of the Timesheet 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Timesheet 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key referencing the Employee to which the Timesheet belongs 
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Employee
        /// </summary>
        [ForeignKey("EmployeeId")]
        public Employee? EmployeeId_Employee { get; set; }

        /// <summary>
        /// Date of the Timesheet 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        /// <summary>
        /// HoursWorked of the Timesheet 
        /// </summary>
        public int? HoursWorked { get; set; }
        /// <summary>
        /// Description of the Timesheet 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// CreatedOn of the Timesheet 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Timesheet 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Timesheet 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Timesheet 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}