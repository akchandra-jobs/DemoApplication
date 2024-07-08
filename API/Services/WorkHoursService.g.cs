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
    /// The workhoursService responsible for managing workhours related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting workhours information.
    /// </remarks>
    public interface IWorkHoursService
    {
        /// <summary>Retrieves a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <returns>The workhours data</returns>
        WorkHours GetById(Guid id);

        /// <summary>Retrieves a list of workhourss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of workhourss</returns>
        List<WorkHours> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new workhours</summary>
        /// <param name="model">The workhours data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(WorkHours model);

        /// <summary>Updates a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <param name="updatedEntity">The workhours data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, WorkHours updatedEntity);

        /// <summary>Updates a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <param name="updatedEntity">The workhours data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<WorkHours> updatedEntity);

        /// <summary>Deletes a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The workhoursService responsible for managing workhours related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting workhours information.
    /// </remarks>
    public class WorkHoursService : IWorkHoursService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the WorkHours class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public WorkHoursService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <returns>The workhours data</returns>
        public WorkHours GetById(Guid id)
        {
            var entityData = _dbContext.WorkHours.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of workhourss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of workhourss</returns>/// <exception cref="Exception"></exception>
        public List<WorkHours> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetWorkHours(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new workhours</summary>
        /// <param name="model">The workhours data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(WorkHours model)
        {
            model.Id = CreateWorkHours(model);
            return model.Id;
        }

        /// <summary>Updates a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <param name="updatedEntity">The workhours data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, WorkHours updatedEntity)
        {
            UpdateWorkHours(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <param name="updatedEntity">The workhours data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<WorkHours> updatedEntity)
        {
            PatchWorkHours(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific workhours by its primary key</summary>
        /// <param name="id">The primary key of the workhours</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteWorkHours(id);
            return true;
        }
        #region
        private List<WorkHours> GetWorkHours(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.WorkHours.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<WorkHours>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(WorkHours), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<WorkHours, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateWorkHours(WorkHours model)
        {
            _dbContext.WorkHours.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateWorkHours(Guid id, WorkHours updatedEntity)
        {
            _dbContext.WorkHours.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteWorkHours(Guid id)
        {
            var entityData = _dbContext.WorkHours.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.WorkHours.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchWorkHours(Guid id, JsonPatchDocument<WorkHours> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.WorkHours.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.WorkHours.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}