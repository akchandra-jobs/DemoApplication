using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a timezone entity with essential details
    /// </summary>
    public class Timezone
    {
        /// <summary>
        /// TenantId of the Timezone 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Timezone 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the Timezone 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Timezone 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Required field Offset of the Timezone 
        /// </summary>
        [Required]
        public int Offset { get; set; }

        /// <summary>
        /// CreatedOn of the Timezone 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Timezone 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Timezone 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Timezone 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}