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
    /// Controller responsible for managing timeallocation related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting timeallocation information.
    /// </remarks>
    [Route("api/timeallocation")]
    [Authorize]
    public class TimeAllocationController : ControllerBase
    {
        private readonly ITimeAllocationService _timeAllocationService;

        /// <summary>
        /// Initializes a new instance of the TimeAllocationController class with the specified context.
        /// </summary>
        /// <param name="itimeallocationservice">The itimeallocationservice to be used by the controller.</param>
        public TimeAllocationController(ITimeAllocationService itimeallocationservice)
        {
            _timeAllocationService = itimeallocationservice;
        }

        /// <summary>Adds a new timeallocation</summary>
        /// <param name="model">The timeallocation data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("TimeAllocation",Entitlements.Create)]
        public IActionResult Post([FromBody] TimeAllocation model)
        {
            var id = _timeAllocationService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of timeallocations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeallocations</returns>
        [HttpGet]
        [UserAuthorize("TimeAllocation",Entitlements.Read)]
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

            var result = _timeAllocationService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <returns>The timeallocation data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("TimeAllocation",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _timeAllocationService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("TimeAllocation",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _timeAllocationService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <param name="updatedEntity">The timeallocation data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("TimeAllocation",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] TimeAllocation updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _timeAllocationService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <param name="updatedEntity">The timeallocation data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("TimeAllocation",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<TimeAllocation> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _timeAllocationService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}