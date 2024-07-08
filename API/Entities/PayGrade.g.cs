using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a paygrade entity with essential details
    /// </summary>
    public class PayGrade
    {
        /// <summary>
        /// TenantId of the PayGrade 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the PayGrade 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the PayGrade 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the PayGrade 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Level of the PayGrade 
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// SalaryRangeLow of the PayGrade 
        /// </summary>
        public int? SalaryRangeLow { get; set; }
        /// <summary>
        /// SalaryRangeHigh of the PayGrade 
        /// </summary>
        public int? SalaryRangeHigh { get; set; }

        /// <summary>
        /// CreatedOn of the PayGrade 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the PayGrade 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the PayGrade 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the PayGrade 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}