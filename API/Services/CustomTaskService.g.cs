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
    /// The customtaskService responsible for managing customtask related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting customtask information.
    /// </remarks>
    public interface ICustomTaskService
    {
        /// <summary>Retrieves a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <returns>The customtask data</returns>
        CustomTask GetById(Guid id);

        /// <summary>Retrieves a list of customtasks based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of customtasks</returns>
        List<CustomTask> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new customtask</summary>
        /// <param name="model">The customtask data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(CustomTask model);

        /// <summary>Updates a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <param name="updatedEntity">The customtask data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, CustomTask updatedEntity);

        /// <summary>Updates a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <param name="updatedEntity">The customtask data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<CustomTask> updatedEntity);

        /// <summary>Deletes a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The customtaskService responsible for managing customtask related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting customtask information.
    /// </remarks>
    public class CustomTaskService : ICustomTaskService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the CustomTask class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public CustomTaskService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <returns>The customtask data</returns>
        public CustomTask GetById(Guid id)
        {
            var entityData = _dbContext.CustomTask.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of customtasks based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of customtasks</returns>/// <exception cref="Exception"></exception>
        public List<CustomTask> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetCustomTask(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new customtask</summary>
        /// <param name="model">The customtask data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(CustomTask model)
        {
            model.Id = CreateCustomTask(model);
            return model.Id;
        }

        /// <summary>Updates a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <param name="updatedEntity">The customtask data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, CustomTask updatedEntity)
        {
            UpdateCustomTask(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <param name="updatedEntity">The customtask data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<CustomTask> updatedEntity)
        {
            PatchCustomTask(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific customtask by its primary key</summary>
        /// <param name="id">The primary key of the customtask</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteCustomTask(id);
            return true;
        }
        #region
        private List<CustomTask> GetCustomTask(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.CustomTask.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<CustomTask>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(CustomTask), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<CustomTask, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateCustomTask(CustomTask model)
        {
            _dbContext.CustomTask.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateCustomTask(Guid id, CustomTask updatedEntity)
        {
            _dbContext.CustomTask.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteCustomTask(Guid id)
        {
            var entityData = _dbContext.CustomTask.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.CustomTask.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchCustomTask(Guid id, JsonPatchDocument<CustomTask> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.CustomTask.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.CustomTask.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}