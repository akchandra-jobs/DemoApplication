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
    /// The timeallocationService responsible for managing timeallocation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timeallocation information.
    /// </remarks>
    public interface ITimeAllocationService
    {
        /// <summary>Retrieves a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <returns>The timeallocation data</returns>
        TimeAllocation GetById(Guid id);

        /// <summary>Retrieves a list of timeallocations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeallocations</returns>
        List<TimeAllocation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new timeallocation</summary>
        /// <param name="model">The timeallocation data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(TimeAllocation model);

        /// <summary>Updates a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <param name="updatedEntity">The timeallocation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, TimeAllocation updatedEntity);

        /// <summary>Updates a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <param name="updatedEntity">The timeallocation data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<TimeAllocation> updatedEntity);

        /// <summary>Deletes a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The timeallocationService responsible for managing timeallocation related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timeallocation information.
    /// </remarks>
    public class TimeAllocationService : ITimeAllocationService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the TimeAllocation class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public TimeAllocationService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <returns>The timeallocation data</returns>
        public TimeAllocation GetById(Guid id)
        {
            var entityData = _dbContext.TimeAllocation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of timeallocations based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timeallocations</returns>/// <exception cref="Exception"></exception>
        public List<TimeAllocation> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetTimeAllocation(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new timeallocation</summary>
        /// <param name="model">The timeallocation data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(TimeAllocation model)
        {
            model.Id = CreateTimeAllocation(model);
            return model.Id;
        }

        /// <summary>Updates a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <param name="updatedEntity">The timeallocation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, TimeAllocation updatedEntity)
        {
            UpdateTimeAllocation(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <param name="updatedEntity">The timeallocation data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<TimeAllocation> updatedEntity)
        {
            PatchTimeAllocation(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific timeallocation by its primary key</summary>
        /// <param name="id">The primary key of the timeallocation</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteTimeAllocation(id);
            return true;
        }
        #region
        private List<TimeAllocation> GetTimeAllocation(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.TimeAllocation.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<TimeAllocation>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(TimeAllocation), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<TimeAllocation, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateTimeAllocation(TimeAllocation model)
        {
            _dbContext.TimeAllocation.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateTimeAllocation(Guid id, TimeAllocation updatedEntity)
        {
            _dbContext.TimeAllocation.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteTimeAllocation(Guid id)
        {
            var entityData = _dbContext.TimeAllocation.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.TimeAllocation.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchTimeAllocation(Guid id, JsonPatchDocument<TimeAllocation> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.TimeAllocation.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.TimeAllocation.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}