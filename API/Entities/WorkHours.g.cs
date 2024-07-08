using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a workhours entity with essential details
    /// </summary>
    public class WorkHours
    {
        /// <summary>
        /// TenantId of the WorkHours 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the WorkHours 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key referencing the CustomTask to which the WorkHours belongs 
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Navigation property representing the associated CustomTask
        /// </summary>
        [ForeignKey("TaskId")]
        public CustomTask? TaskId_CustomTask { get; set; }

        /// <summary>
        /// Required field StartDate of the WorkHours 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Required field EndDate of the WorkHours 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Required field Hours of the WorkHours 
        /// </summary>
        [Required]
        public int Hours { get; set; }

        /// <summary>
        /// CreatedOn of the WorkHours 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the WorkHours 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the WorkHours 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the WorkHours 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}