using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a timeentry entity with essential details
    /// </summary>
    public class TimeEntry
    {
        /// <summary>
        /// TenantId of the TimeEntry 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the TimeEntry 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key referencing the CustomTask to which the TimeEntry belongs 
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Navigation property representing the associated CustomTask
        /// </summary>
        [ForeignKey("TaskId")]
        public CustomTask? TaskId_CustomTask { get; set; }

        /// <summary>
        /// Required field StartTime of the TimeEntry 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Required field EndTime of the TimeEntry 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Required field Duration of the TimeEntry 
        /// </summary>
        [Required]
        public int Duration { get; set; }

        /// <summary>
        /// CreatedOn of the TimeEntry 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the TimeEntry 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the TimeEntry 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the TimeEntry 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}