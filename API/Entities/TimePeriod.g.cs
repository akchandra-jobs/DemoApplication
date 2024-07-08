using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a timeperiod entity with essential details
    /// </summary>
    public class TimePeriod
    {
        /// <summary>
        /// TenantId of the TimePeriod 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the TimePeriod 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the TimePeriod 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the TimePeriod 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// StartDate of the TimePeriod 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// EndDate of the TimePeriod 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// IsActive of the TimePeriod 
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// CreatedOn of the TimePeriod 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the TimePeriod 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the TimePeriod 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the TimePeriod 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}