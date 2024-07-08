using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a customtask entity with essential details
    /// </summary>
    public class CustomTask
    {
        /// <summary>
        /// TenantId of the CustomTask 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the CustomTask 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the CustomTask 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the CustomTask 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Description of the CustomTask 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// CreatedOn of the CustomTask 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the CustomTask 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the CustomTask 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the CustomTask 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}