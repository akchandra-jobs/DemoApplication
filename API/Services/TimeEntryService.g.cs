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
    /// The timeentryService responsible for managing timeentry related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timeentry information.
    /// </remarks>
    public interface ITimeEntryService
    {
        /// <summary>Retrieves a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <returns>The timeentry data</returns>
        TimeEntry GetById(Guid id);

        /// <summary>Retrieves a list of timeentrys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeentrys</returns>
        List<TimeEntry> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new timeentry</summary>
        /// <param name="model">The timeentry data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(TimeEntry model);

        /// <summary>Updates a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <param name="updatedEntity">The timeentry data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, TimeEntry updatedEntity);

        /// <summary>Updates a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <param name="updatedEntity">The timeentry data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<TimeEntry> updatedEntity);

        /// <summary>Deletes a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The timeentryService responsible for managing timeentry related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timeentry information.
    /// </remarks>
    public class TimeEntryService : ITimeEntryService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the TimeEntry class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public TimeEntryService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <returns>The timeentry data</returns>
        public TimeEntry GetById(Guid id)
        {
            var entityData = _dbContext.TimeEntry.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of timeentrys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeentrys</returns>/// <exception cref="Exception"></exception>
        public List<TimeEntry> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetTimeEntry(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new timeentry</summary>
        /// <param name="model">The timeentry data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(TimeEntry model)
        {
            model.Id = CreateTimeEntry(model);
            return model.Id;
        }

        /// <summary>Updates a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <param name="updatedEntity">The timeentry data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, TimeEntry updatedEntity)
        {
            UpdateTimeEntry(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <param name="updatedEntity">The timeentry data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<TimeEntry> updatedEntity)
        {
            PatchTimeEntry(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific timeentry by its primary key</summary>
        /// <param name="id">The primary key of the timeentry</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteTimeEntry(id);
            return true;
        }
        #region
        private List<TimeEntry> GetTimeEntry(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.TimeEntry.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<TimeEntry>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(TimeEntry), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<TimeEntry, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateTimeEntry(TimeEntry model)
        {
            _dbContext.TimeEntry.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateTimeEntry(Guid id, TimeEntry updatedEntity)
        {
            _dbContext.TimeEntry.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteTimeEntry(Guid id)
        {
            var entityData = _dbContext.TimeEntry.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.TimeEntry.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchTimeEntry(Guid id, JsonPatchDocument<TimeEntry> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.TimeEntry.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.TimeEntry.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}