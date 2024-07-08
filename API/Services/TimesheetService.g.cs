using DemoApplication.Models;
using DemoApplication.Data;
using DemoApplication.Filter;
using DemoApplication.Entities;
using DemoApplication.Logger;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace DemoApplication.Services
{
    /// <summary>
    /// The timesheetService responsible for managing timesheet related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timesheet information.
    /// </remarks>
    public interface ITimesheetService
    {
        /// <summary>Retrieves a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <returns>The timesheet data</returns>
        Timesheet GetById(Guid id);

        /// <summary>Retrieves a list of timesheets based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timesheets</returns>
        List<Timesheet> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new timesheet</summary>
        /// <param name="model">The timesheet data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Timesheet model);

        /// <summary>Updates a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <param name="updatedEntity">The timesheet data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Timesheet updatedEntity);

        /// <summary>Updates a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <param name="updatedEntity">The timesheet data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Timesheet> updatedEntity);

        /// <summary>Deletes a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The timesheetService responsible for managing timesheet related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timesheet information.
    /// </remarks>
    public class TimesheetService : ITimesheetService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Timesheet class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public TimesheetService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <returns>The timesheet data</returns>
        public Timesheet GetById(Guid id)
        {
            var entityData = _dbContext.Timesheet.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of timesheets based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timesheets</returns>/// <exception cref="Exception"></exception>
        public List<Timesheet> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetTimesheet(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new timesheet</summary>
        /// <param name="model">The timesheet data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Timesheet model)
        {
            model.Id = CreateTimesheet(model);
            return model.Id;
        }

        /// <summary>Updates a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <param name="updatedEntity">The timesheet data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Timesheet updatedEntity)
        {
            UpdateTimesheet(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <param name="updatedEntity">The timesheet data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Timesheet> updatedEntity)
        {
            PatchTimesheet(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific timesheet by its primary key</summary>
        /// <param name="id">The primary key of the timesheet</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteTimesheet(id);
            return true;
        }
        #region
        private List<Timesheet> GetTimesheet(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Timesheet.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Timesheet>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Timesheet), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Timesheet, object>>(Expression.Convert(property, typeof(object)), parameter);
                if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(lambda);
                }
                else if (sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(lambda);
                }
                else
                {
                    throw new ApplicationException("Invalid sort order. Use 'asc' or 'desc'");
                }
            }

            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return paginatedResult;
        }

        private Guid CreateTimesheet(Timesheet model)
        {
            _dbContext.Timesheet.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateTimesheet(Guid id, Timesheet updatedEntity)
        {
            _dbContext.Timesheet.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteTimesheet(Guid id)
        {
            var entityData = _dbContext.Timesheet.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Timesheet.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchTimesheet(Guid id, JsonPatchDocument<Timesheet> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Timesheet.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Timesheet.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}