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
    /// Controller responsible for managing timeentry related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting timeentry information.
    /// </remarks>
    [Route("api/timeentry")]
    [Authorize]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryService _timeEntryService;

        /// <summary>
        /// Initializes a new instance of the TimeEntryController class with the specified context.
        /// </summary>
        /// <param name="itimeentryservice">The itimeentryservice to be used by the controller.</param>
        public TimeEntryController(ITimeEntryService itimeentryservice)
        {
            _timeEntryService = itimeentryservice;
        }

        /// <summary>Adds a new timeentry</summary>
        /// <param name="model">The timeentry data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("TimeEntry",Entitlements.Create)]
        public IActionResult Post([FromBody] TimeEntry model)
        {
            var id = _timeEntryService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of timeentrys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeentrys</returns>
        [HttpGet]
        [UserAuthorize("TimeEntry",Entitlements.Read)]
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

            var result = _timeEntryService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <returns>The timeentry data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("TimeEntry",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _timeEntryService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("TimeEntry",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _timeEntryService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <param name="updatedEntity">The timeentry data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("TimeEntry",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] TimeEntry updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _timeEntryService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <param name="updatedEntity">The timeentry data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("TimeEntry",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<TimeEntry> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _timeEntryService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}