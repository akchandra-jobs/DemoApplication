using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a employeedocuments entity with essential details
    /// </summary>
    public class EmployeeDocuments
    {
        /// <summary>
        /// TenantId of the EmployeeDocuments 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the EmployeeDocuments 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the EmployeeDocuments 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the EmployeeDocuments 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// EmployeeId of the EmployeeDocuments 
        /// </summary>
        public Guid? EmployeeId { get; set; }
        /// <summary>
        /// DocumentName of the EmployeeDocuments 
        /// </summary>
        public string? DocumentName { get; set; }
        /// <summary>
        /// DocumentType of the EmployeeDocuments 
        /// </summary>
        public string? DocumentType { get; set; }

        /// <summary>
        /// ExpirationDate of the EmployeeDocuments 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? ExpirationDate { get; set; }
        /// <summary>
        /// FilePath of the EmployeeDocuments 
        /// </summary>
        public string? FilePath { get; set; }

        /// <summary>
        /// CreatedOn of the EmployeeDocuments 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the EmployeeDocuments 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the EmployeeDocuments 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the EmployeeDocuments 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}