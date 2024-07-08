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
    /// The timeperiodService responsible for managing timeperiod related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timeperiod information.
    /// </remarks>
    public interface ITimePeriodService
    {
        /// <summary>Retrieves a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <returns>The timeperiod data</returns>
        TimePeriod GetById(Guid id);

        /// <summary>Retrieves a list of timeperiods based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeperiods</returns>
        List<TimePeriod> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new timeperiod</summary>
        /// <param name="model">The timeperiod data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(TimePeriod model);

        /// <summary>Updates a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <param name="updatedEntity">The timeperiod data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, TimePeriod updatedEntity);

        /// <summary>Updates a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <param name="updatedEntity">The timeperiod data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<TimePeriod> updatedEntity);

        /// <summary>Deletes a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The timeperiodService responsible for managing timeperiod related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timeperiod information.
    /// </remarks>
    public class TimePeriodService : ITimePeriodService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the TimePeriod class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public TimePeriodService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <returns>The timeperiod data</returns>
        public TimePeriod GetById(Guid id)
        {
            var entityData = _dbContext.TimePeriod.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of timeperiods based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeperiods</returns>/// <exception cref="Exception"></exception>
        public List<TimePeriod> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetTimePeriod(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new timeperiod</summary>
        /// <param name="model">The timeperiod data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(TimePeriod model)
        {
            model.Id = CreateTimePeriod(model);
            return model.Id;
        }

        /// <summary>Updates a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <param name="updatedEntity">The timeperiod data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, TimePeriod updatedEntity)
        {
            UpdateTimePeriod(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <param name="updatedEntity">The timeperiod data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<TimePeriod> updatedEntity)
        {
            PatchTimePeriod(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific timeperiod by its primary key</summary>
        /// <param name="id">The primary key of the timeperiod</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteTimePeriod(id);
            return true;
        }
        #region
        private List<TimePeriod> GetTimePeriod(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.TimePeriod.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<TimePeriod>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(TimePeriod), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<TimePeriod, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateTimePeriod(TimePeriod model)
        {
            _dbContext.TimePeriod.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateTimePeriod(Guid id, TimePeriod updatedEntity)
        {
            _dbContext.TimePeriod.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteTimePeriod(Guid id)
        {
            var entityData = _dbContext.TimePeriod.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.TimePeriod.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchTimePeriod(Guid id, JsonPatchDocument<TimePeriod> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.TimePeriod.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.TimePeriod.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}