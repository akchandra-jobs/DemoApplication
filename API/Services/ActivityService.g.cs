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
    /// The activityService responsible for managing activity related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting activity information.
    /// </remarks>
    public interface IActivityService
    {
        /// <summary>Retrieves a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <returns>The activity data</returns>
        Activity GetById(Guid id);

        /// <summary>Retrieves a list of activitys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of activitys</returns>
        List<Activity> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new activity</summary>
        /// <param name="model">The activity data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Activity model);

        /// <summary>Updates a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <param name="updatedEntity">The activity data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Activity updatedEntity);

        /// <summary>Updates a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <param name="updatedEntity">The activity data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Activity> updatedEntity);

        /// <summary>Deletes a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The activityService responsible for managing activity related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting activity information.
    /// </remarks>
    public class ActivityService : IActivityService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Activity class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ActivityService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <returns>The activity data</returns>
        public Activity GetById(Guid id)
        {
            var entityData = _dbContext.Activity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of activitys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of activitys</returns>/// <exception cref="Exception"></exception>
        public List<Activity> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetActivity(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new activity</summary>
        /// <param name="model">The activity data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Activity model)
        {
            model.Id = CreateActivity(model);
            return model.Id;
        }

        /// <summary>Updates a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <param name="updatedEntity">The activity data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Activity updatedEntity)
        {
            UpdateActivity(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <param name="updatedEntity">The activity data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Activity> updatedEntity)
        {
            PatchActivity(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific activity by its primary key</summary>
        /// <param name="id">The primary key of the activity</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteActivity(id);
            return true;
        }
        #region
        private List<Activity> GetActivity(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Activity.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Activity>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Activity), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Activity, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateActivity(Activity model)
        {
            _dbContext.Activity.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateActivity(Guid id, Activity updatedEntity)
        {
            _dbContext.Activity.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteActivity(Guid id)
        {
            var entityData = _dbContext.Activity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Activity.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchActivity(Guid id, JsonPatchDocument<Activity> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Activity.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Activity.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}