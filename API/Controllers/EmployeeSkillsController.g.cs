using Microsoft.AspNetCore.Mvc;
using DemoApplication.Models;
using DemoApplication.Services;
using DemoApplication.Entities;
using DemoApplication.Filter;
using DemoApplication.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace DemoApplication.Controllers
{
    /// <summary>
    /// Controller responsible for managing employeeskills related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting employeeskills information.
    /// </remarks>
    [Route("api/employeeskills")]
    [Authorize]
    public class EmployeeSkillsController : ControllerBase
    {
        private readonly IEmployeeSkillsService _employeeSkillsService;

        /// <summary>
        /// Initializes a new instance of the EmployeeSkillsController class with the specified context.
        /// </summary>
        /// <param name="iemployeeskillsservice">The iemployeeskillsservice to be used by the controller.</param>
        public EmployeeSkillsController(IEmployeeSkillsService iemployeeskillsservice)
        {
            _employeeSkillsService = iemployeeskillsservice;
        }

        /// <summary>Adds a new employeeskills</summary>
        /// <param name="model">The employeeskills data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("EmployeeSkills",Entitlements.Create)]
        public IActionResult Post([FromBody] EmployeeSkills model)
        {
            var id = _employeeSkillsService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of employeeskillss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of employeeskillss</returns>
        [HttpGet]
        [UserAuthorize("EmployeeSkills",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult Get([FromQuery] string filters, string searchTerm, int pageNumber = 1, int pageSize = 10, string sortField = null, string sortOrder = "asc")
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var result = _employeeSkillsService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific employeeskills by its primary key</summary>
        /// <param name="id">The primary key of the employeeskills</param>
        /// <returns>The employeeskills data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("EmployeeSkills",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _employeeSkillsService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific employeeskills by its primary key</summary>
        /// <param name="id">The primary key of the employeeskills</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("EmployeeSkills",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _employeeSkillsService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific employeeskills by its primary key</summary>
        /// <param name="id">The primary key of the employeeskills</param>
        /// <param name="updatedEntity">The employeeskills data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("EmployeeSkills",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] EmployeeSkills updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _employeeSkillsService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific employeeskills by its primary key</summary>
        /// <param name="id">The primary key of the employeeskills</param>
        /// <param name="updatedEntity">The employeeskills data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("EmployeeSkills",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<EmployeeSkills> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _employeeSkillsService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}