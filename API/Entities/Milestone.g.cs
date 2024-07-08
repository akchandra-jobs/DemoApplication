using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a milestone entity with essential details
    /// </summary>
    public class Milestone
    {
        /// <summary>
        /// TenantId of the Milestone 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Milestone 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the Milestone 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Milestone 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Foreign key referencing the TimePeriod to which the Milestone belongs 
        /// </summary>
        public Guid? TimePeriodId { get; set; }

        /// <summary>
        /// Navigation property representing the associated TimePeriod
        /// </summary>
        [ForeignKey("TimePeriodId")]
        public TimePeriod? TimePeriodId_TimePeriod { get; set; }
        /// <summary>
        /// Goal of the Milestone 
        /// </summary>
        public string? Goal { get; set; }

        /// <summary>
        /// Deadline of the Milestone 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// CreatedOn of the Milestone 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Milestone 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Milestone 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Milestone 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}