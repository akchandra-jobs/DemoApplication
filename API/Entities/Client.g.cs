using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a client entity with essential details
    /// </summary>
    public class Client
    {
        /// <summary>
        /// TenantId of the Client 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Client 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the Client 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Client 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Email of the Client 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Phone of the Client 
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Address of the Client 
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// CreatedOn of the Client 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Client 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Client 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Client 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}