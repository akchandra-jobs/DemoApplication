using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a performancereview entity with essential details
    /// </summary>
    public class PerformanceReview
    {
        /// <summary>
        /// TenantId of the PerformanceReview 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the PerformanceReview 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the PerformanceReview 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the PerformanceReview 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// EmployeeId of the PerformanceReview 
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// ReviewDate of the PerformanceReview 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? ReviewDate { get; set; }
        /// <summary>
        /// Feedback of the PerformanceReview 
        /// </summary>
        public string? Feedback { get; set; }
        /// <summary>
        /// Score of the PerformanceReview 
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// CreatedOn of the PerformanceReview 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the PerformanceReview 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the PerformanceReview 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the PerformanceReview 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}