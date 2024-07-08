using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a payroll entity with essential details
    /// </summary>
    public class Payroll
    {
        /// <summary>
        /// TenantId of the Payroll 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the Payroll 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the Payroll 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the Payroll 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// EmployeeId of the Payroll 
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// PayPeriodStartDate of the Payroll 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? PayPeriodStartDate { get; set; }

        /// <summary>
        /// PayPeriodEndDate of the Payroll 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? PayPeriodEndDate { get; set; }
        /// <summary>
        /// GrossPay of the Payroll 
        /// </summary>
        public int? GrossPay { get; set; }
        /// <summary>
        /// Deductions of the Payroll 
        /// </summary>
        public int? Deductions { get; set; }
        /// <summary>
        /// NetPay of the Payroll 
        /// </summary>
        public int? NetPay { get; set; }

        /// <summary>
        /// CreatedOn of the Payroll 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Payroll 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the Payroll 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Payroll 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}