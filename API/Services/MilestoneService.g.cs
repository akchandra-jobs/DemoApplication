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
    /// The milestoneService responsible for managing milestone related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting milestone information.
    /// </remarks>
    public interface IMilestoneService
    {
        /// <summary>Retrieves a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <returns>The milestone data</returns>
        Milestone GetById(Guid id);

        /// <summary>Retrieves a list of milestones based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of milestones</returns>
        List<Milestone> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new milestone</summary>
        /// <param name="model">The milestone data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Milestone model);

        /// <summary>Updates a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <param name="updatedEntity">The milestone data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Milestone updatedEntity);

        /// <summary>Updates a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <param name="updatedEntity">The milestone data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Milestone> updatedEntity);

        /// <summary>Deletes a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The milestoneService responsible for managing milestone related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting milestone information.
    /// </remarks>
    public class MilestoneService : IMilestoneService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Milestone class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public MilestoneService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <returns>The milestone data</returns>
        public Milestone GetById(Guid id)
        {
            var entityData = _dbContext.Milestone.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of milestones based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of milestones</returns>/// <exception cref="Exception"></exception>
        public List<Milestone> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetMilestone(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new milestone</summary>
        /// <param name="model">The milestone data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Milestone model)
        {
            model.Id = CreateMilestone(model);
            return model.Id;
        }

        /// <summary>Updates a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <param name="updatedEntity">The milestone data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Milestone updatedEntity)
        {
            UpdateMilestone(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <param name="updatedEntity">The milestone data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Milestone> updatedEntity)
        {
            PatchMilestone(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific milestone by its primary key</summary>
        /// <param name="id">The primary key of the milestone</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteMilestone(id);
            return true;
        }
        #region
        private List<Milestone> GetMilestone(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Milestone.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Milestone>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Milestone), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Milestone, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateMilestone(Milestone model)
        {
            _dbContext.Milestone.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateMilestone(Guid id, Milestone updatedEntity)
        {
            _dbContext.Milestone.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteMilestone(Guid id)
        {
            var entityData = _dbContext.Milestone.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Milestone.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchMilestone(Guid id, JsonPatchDocument<Milestone> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Milestone.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Milestone.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}