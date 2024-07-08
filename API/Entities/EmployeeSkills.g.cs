using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DemoApplication.Entities
{
#pragma warning disable
    /// <summary> 
    /// Represents a employeeskills entity with essential details
    /// </summary>
    public class EmployeeSkills
    {
        /// <summary>
        /// TenantId of the EmployeeSkills 
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Primary key for the EmployeeSkills 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of the EmployeeSkills 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Name of the EmployeeSkills 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// EmployeeId of the EmployeeSkills 
        /// </summary>
        public Guid? EmployeeId { get; set; }
        /// <summary>
        /// SkillName of the EmployeeSkills 
        /// </summary>
        public string? SkillName { get; set; }
        /// <summary>
        /// ProficiencyLevel of the EmployeeSkills 
        /// </summary>
        public int? ProficiencyLevel { get; set; }

        /// <summary>
        /// CreatedOn of the EmployeeSkills 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the EmployeeSkills 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UpdatedOn of the EmployeeSkills 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the EmployeeSkills 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}