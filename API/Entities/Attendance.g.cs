using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a attendance entity with essential details
    /// </summary>
    public class Attendance
    {
        /// <summary>
        /// TenantId of the Attendance 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Attendance 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key referencing the Employee to which the Attendance belongs 
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Employee
        /// </summary>
        [ForeignKey("EmployeeId")]
        public Employee? EmployeeId_Employee { get; set; }

        /// <summary>
        /// Date of the Attendance 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// InTime of the Attendance 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? InTime { get; set; }

        /// <summary>
        /// OutTime of the Attendance 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? OutTime { get; set; }

        /// <summary>
        /// CreatedOn of the Attendance 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Attendance 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Attendance 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Attendance 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}