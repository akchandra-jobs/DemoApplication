using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a activity entity with essential details
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// TenantId of the Activity 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Activity 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the Activity 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Activity 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Description of the Activity 
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Foreign key referencing the TimePeriod to which the Activity belongs 
        /// </summary>
        public Guid? TimePeriodId { get; set; }

        /// <summary>
        /// Navigation property representing the associated TimePeriod
        /// </summary>
        [ForeignKey("TimePeriodId")]
        public TimePeriod? TimePeriodId_TimePeriod { get; set; }
        /// <summary>
        /// EstimatedTime of the Activity 
        /// </summary>
        public int? EstimatedTime { get; set; }

        /// <summary>
        /// CreatedOn of the Activity 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Activity 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Activity 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Activity 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}