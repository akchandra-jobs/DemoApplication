using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a employeetraining entity with essential details
    /// </summary>
    public class EmployeeTraining
    {
        /// <summary>
        /// TenantId of the EmployeeTraining 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the EmployeeTraining 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the EmployeeTraining 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the EmployeeTraining 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// EmployeeId of the EmployeeTraining 
        /// </summary>
        public Guid? EmployeeId { get; set; }
        /// <summary>
        /// TrainingCourse of the EmployeeTraining 
        /// </summary>
        public string? TrainingCourse { get; set; }

        /// <summary>
        /// TrainingDate of the EmployeeTraining 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? TrainingDate { get; set; }
        /// <summary>
        /// Duration of the EmployeeTraining 
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// CreatedOn of the EmployeeTraining 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the EmployeeTraining 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the EmployeeTraining 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the EmployeeTraining 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}