using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a billing entity with essential details
    /// </summary>
    public class Billing
    {
        /// <summary>
        /// TenantId of the Billing 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Billing 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key referencing the Client to which the Billing belongs 
        /// </summary>
        public Guid? ClientId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Client
        /// </summary>
        [ForeignKey("ClientId")]
        public Client? ClientId_Client { get; set; }
        /// <summary>
        /// Foreign key referencing the CustomTask to which the Billing belongs 
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Navigation property representing the associated CustomTask
        /// </summary>
        [ForeignKey("TaskId")]
        public CustomTask? TaskId_CustomTask { get; set; }

        /// <summary>
        /// Required field RatePerHour of the Billing 
        /// </summary>
        [Required]
        public int RatePerHour { get; set; }

        /// <summary>
        /// Required field TotalPrice of the Billing 
        /// </summary>
        [Required]
        public int TotalPrice { get; set; }

        /// <summary>
        /// Required field InvoiceDate of the Billing 
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// CreatedOn of the Billing 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Billing 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Billing 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Billing 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}