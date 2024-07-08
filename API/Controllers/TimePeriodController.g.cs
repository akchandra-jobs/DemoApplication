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
    /// Controller responsible for managing timeperiod related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting timeperiod information.
    /// </remarks>
    [Route("api/timeperiod")]
    [Authorize]
    public class TimePeriodController : ControllerBase
    {
        private readonly ITimePeriodService _timePeriodService;

        /// <summary>
        /// Initializes a new instance of the TimePeriodController class with the specified context.
        /// </summary>
        /// <param name="itimeperiodservice">The itimeperiodservice to be used by the controller.</param>
        public TimePeriodController(ITimePeriodService itimeperiodservice)
        {
            _timePeriodService = itimeperiodservice;
        }

        /// <summary>Adds a new timeperiod</summary>
        /// <param name="model">The timeperiod data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("TimePeriod",Entitlements.Create)]
        public IActionResult Post([FromBody] TimePeriod model)
        {
            var id = _timePeriodService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of timeperiods based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeperiods</returns>
        [HttpGet]
        [UserAuthorize("TimePeriod",Entitlements.Read)]
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

            var result = _timePeriodService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <returns>The timeperiod data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("TimePeriod",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _timePeriodService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("TimePeriod",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _timePeriodService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <param name="updatedEntity">The timeperiod data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("TimePeriod",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] TimePeriod updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _timePeriodService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <param name="updatedEntity">The timeperiod data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("TimePeriod",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<TimePeriod> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _timePeriodService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}