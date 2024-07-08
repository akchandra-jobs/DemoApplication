using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a timeallocation entity with essential details
    /// </summary>
    public class TimeAllocation
    {
        /// <summary>
        /// TenantId of the TimeAllocation 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the TimeAllocation 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the TimeAllocation 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Foreign key referencing the Employee to which the TimeAllocation belongs 
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Employee
        /// </summary>
        [ForeignKey("EmployeeId")]
        public Employee? EmployeeId_Employee { get; set; }
        /// <summary>
        /// Foreign key referencing the Activity to which the TimeAllocation belongs 
        /// </summary>
        public Guid? ActivityId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Activity
        /// </summary>
        [ForeignKey("ActivityId")]
        public Activity? ActivityId_Activity { get; set; }
        /// <summary>
        /// TimeAllocated of the TimeAllocation 
        /// </summary>
        public int? TimeAllocated { get; set; }
        /// <summary>
        /// Description of the TimeAllocation 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// CreatedOn of the TimeAllocation 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the TimeAllocation 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the TimeAllocation 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the TimeAllocation 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}