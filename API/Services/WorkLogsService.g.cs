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
    /// The worklogsService responsible for managing worklogs related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting worklogs information.
    /// </remarks>
    public interface IWorkLogsService
    {
        /// <summary>Retrieves a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <returns>The worklogs data</returns>
        WorkLogs GetById(Guid id);

        /// <summary>Retrieves a list of worklogss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of worklogss</returns>
        List<WorkLogs> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new worklogs</summary>
        /// <param name="model">The worklogs data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(WorkLogs model);

        /// <summary>Updates a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <param name="updatedEntity">The worklogs data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, WorkLogs updatedEntity);

        /// <summary>Updates a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <param name="updatedEntity">The worklogs data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<WorkLogs> updatedEntity);

        /// <summary>Deletes a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The worklogsService responsible for managing worklogs related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting worklogs information.
    /// </remarks>
    public class WorkLogsService : IWorkLogsService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the WorkLogs class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public WorkLogsService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <returns>The worklogs data</returns>
        public WorkLogs GetById(Guid id)
        {
            var entityData = _dbContext.WorkLogs.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of worklogss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of worklogss</returns>/// <exception cref="Exception"></exception>
        public List<WorkLogs> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetWorkLogs(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new worklogs</summary>
        /// <param name="model">The worklogs data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(WorkLogs model)
        {
            model.Id = CreateWorkLogs(model);
            return model.Id;
        }

        /// <summary>Updates a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <param name="updatedEntity">The worklogs data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, WorkLogs updatedEntity)
        {
            UpdateWorkLogs(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <param name="updatedEntity">The worklogs data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<WorkLogs> updatedEntity)
        {
            PatchWorkLogs(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific worklogs by its primary key</summary>
        /// <param name="id">The primary key of the worklogs</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteWorkLogs(id);
            return true;
        }
        #region
        private List<WorkLogs> GetWorkLogs(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.WorkLogs.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<WorkLogs>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(WorkLogs), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<WorkLogs, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateWorkLogs(WorkLogs model)
        {
            _dbContext.WorkLogs.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateWorkLogs(Guid id, WorkLogs updatedEntity)
        {
            _dbContext.WorkLogs.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteWorkLogs(Guid id)
        {
            var entityData = _dbContext.WorkLogs.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.WorkLogs.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchWorkLogs(Guid id, JsonPatchDocument<WorkLogs> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.WorkLogs.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.WorkLogs.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}