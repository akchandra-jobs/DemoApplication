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
    /// The timezoneService responsible for managing timezone related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timezone information.
    /// </remarks>
    public interface ITimezoneService
    {
        /// <summary>Retrieves a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <returns>The timezone data</returns>
        Timezone GetById(Guid id);

        /// <summary>Retrieves a list of timezones based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timezones</returns>
        List<Timezone> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new timezone</summary>
        /// <param name="model">The timezone data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Timezone model);

        /// <summary>Updates a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <param name="updatedEntity">The timezone data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Timezone updatedEntity);

        /// <summary>Updates a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <param name="updatedEntity">The timezone data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Timezone> updatedEntity);

        /// <summary>Deletes a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The timezoneService responsible for managing timezone related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting timezone information.
    /// </remarks>
    public class TimezoneService : ITimezoneService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Timezone class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public TimezoneService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <returns>The timezone data</returns>
        public Timezone GetById(Guid id)
        {
            var entityData = _dbContext.Timezone.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of timezones based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of timezones</returns>/// <exception cref="Exception"></exception>
        public List<Timezone> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetTimezone(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new timezone</summary>
        /// <param name="model">The timezone data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Timezone model)
        {
            model.Id = CreateTimezone(model);
            return model.Id;
        }

        /// <summary>Updates a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <param name="updatedEntity">The timezone data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Timezone updatedEntity)
        {
            UpdateTimezone(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <param name="updatedEntity">The timezone data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Timezone> updatedEntity)
        {
            PatchTimezone(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific timezone by its primary key</summary>
        /// <param name="id">The primary key of the timezone</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteTimezone(id);
            return true;
        }
        #region
        private List<Timezone> GetTimezone(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Timezone.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Timezone>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Timezone), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Timezone, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateTimezone(Timezone model)
        {
            _dbContext.Timezone.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateTimezone(Guid id, Timezone updatedEntity)
        {
            _dbContext.Timezone.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteTimezone(Guid id)
        {
            var entityData = _dbContext.Timezone.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Timezone.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchTimezone(Guid id, JsonPatchDocument<Timezone> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Timezone.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Timezone.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}