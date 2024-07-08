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
    /// Controller responsible for managing paygrade related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting paygrade information.
    /// </remarks>
    [Route("api/paygrade")]
    [Authorize]
    public class PayGradeController : ControllerBase
    {
        private readonly IPayGradeService _payGradeService;

        /// <summary>
        /// Initializes a new instance of the PayGradeController class with the specified context.
        /// </summary>
        /// <param name="ipaygradeservice">The ipaygradeservice to be used by the controller.</param>
        public PayGradeController(IPayGradeService ipaygradeservice)
        {
            _payGradeService = ipaygradeservice;
        }

        /// <summary>Adds a new paygrade</summary>
        /// <param name="model">The paygrade data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("PayGrade",Entitlements.Create)]
        public IActionResult Post([FromBody] PayGrade model)
        {
            var id = _payGradeService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of paygrades based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of paygrades</returns>
        [HttpGet]
        [UserAuthorize("PayGrade",Entitlements.Read)]
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

            var result = _payGradeService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <returns>The paygrade data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("PayGrade",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _payGradeService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("PayGrade",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _payGradeService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <param name="updatedEntity">The paygrade data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("PayGrade",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] PayGrade updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _payGradeService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <param name="updatedEntity">The paygrade data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("PayGrade",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<PayGrade> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _payGradeService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}