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
    /// The employeetrainingService responsible for managing employeetraining related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting employeetraining information.
    /// </remarks>
    public interface IEmployeeTrainingService
    {
        /// <summary>Retrieves a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <returns>The employeetraining data</returns>
        EmployeeTraining GetById(Guid id);

        /// <summary>Retrieves a list of employeetrainings based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of employeetrainings</returns>
        List<EmployeeTraining> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new employeetraining</summary>
        /// <param name="model">The employeetraining data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(EmployeeTraining model);

        /// <summary>Updates a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <param name="updatedEntity">The employeetraining data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, EmployeeTraining updatedEntity);

        /// <summary>Updates a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <param name="updatedEntity">The employeetraining data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<EmployeeTraining> updatedEntity);

        /// <summary>Deletes a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The employeetrainingService responsible for managing employeetraining related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting employeetraining information.
    /// </remarks>
    public class EmployeeTrainingService : IEmployeeTrainingService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the EmployeeTraining class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public EmployeeTrainingService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <returns>The employeetraining data</returns>
        public EmployeeTraining GetById(Guid id)
        {
            var entityData = _dbContext.EmployeeTraining.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of employeetrainings based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of employeetrainings</returns>/// <exception cref="Exception"></exception>
        public List<EmployeeTraining> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetEmployeeTraining(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new employeetraining</summary>
        /// <param name="model">The employeetraining data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(EmployeeTraining model)
        {
            model.Id = CreateEmployeeTraining(model);
            return model.Id;
        }

        /// <summary>Updates a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <param name="updatedEntity">The employeetraining data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, EmployeeTraining updatedEntity)
        {
            UpdateEmployeeTraining(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <param name="updatedEntity">The employeetraining data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<EmployeeTraining> updatedEntity)
        {
            PatchEmployeeTraining(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific employeetraining by its primary key</summary>
        /// <param name="id">The primary key of the employeetraining</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteEmployeeTraining(id);
            return true;
        }
        #region
        private List<EmployeeTraining> GetEmployeeTraining(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.EmployeeTraining.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<EmployeeTraining>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(EmployeeTraining), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<EmployeeTraining, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateEmployeeTraining(EmployeeTraining model)
        {
            _dbContext.EmployeeTraining.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateEmployeeTraining(Guid id, EmployeeTraining updatedEntity)
        {
            _dbContext.EmployeeTraining.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteEmployeeTraining(Guid id)
        {
            var entityData = _dbContext.EmployeeTraining.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.EmployeeTraining.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchEmployeeTraining(Guid id, JsonPatchDocument<EmployeeTraining> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.EmployeeTraining.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.EmployeeTraining.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}