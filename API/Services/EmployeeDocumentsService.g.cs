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
    /// The employeedocumentsService responsible for managing employeedocuments related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting employeedocuments information.
    /// </remarks>
    public interface IEmployeeDocumentsService
    {
        /// <summary>Retrieves a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <returns>The employeedocuments data</returns>
        EmployeeDocuments GetById(Guid id);

        /// <summary>Retrieves a list of employeedocumentss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of employeedocumentss</returns>
        List<EmployeeDocuments> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new employeedocuments</summary>
        /// <param name="model">The employeedocuments data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(EmployeeDocuments model);

        /// <summary>Updates a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <param name="updatedEntity">The employeedocuments data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, EmployeeDocuments updatedEntity);

        /// <summary>Updates a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <param name="updatedEntity">The employeedocuments data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<EmployeeDocuments> updatedEntity);

        /// <summary>Deletes a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The employeedocumentsService responsible for managing employeedocuments related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting employeedocuments information.
    /// </remarks>
    public class EmployeeDocumentsService : IEmployeeDocumentsService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the EmployeeDocuments class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public EmployeeDocumentsService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <returns>The employeedocuments data</returns>
        public EmployeeDocuments GetById(Guid id)
        {
            var entityData = _dbContext.EmployeeDocuments.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of employeedocumentss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of employeedocumentss</returns>/// <exception cref="Exception"></exception>
        public List<EmployeeDocuments> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetEmployeeDocuments(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new employeedocuments</summary>
        /// <param name="model">The employeedocuments data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(EmployeeDocuments model)
        {
            model.Id = CreateEmployeeDocuments(model);
            return model.Id;
        }

        /// <summary>Updates a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <param name="updatedEntity">The employeedocuments data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, EmployeeDocuments updatedEntity)
        {
            UpdateEmployeeDocuments(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <param name="updatedEntity">The employeedocuments data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<EmployeeDocuments> updatedEntity)
        {
            PatchEmployeeDocuments(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific employeedocuments by its primary key</summary>
        /// <param name="id">The primary key of the employeedocuments</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteEmployeeDocuments(id);
            return true;
        }
        #region
        private List<EmployeeDocuments> GetEmployeeDocuments(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.EmployeeDocuments.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<EmployeeDocuments>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(EmployeeDocuments), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<EmployeeDocuments, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateEmployeeDocuments(EmployeeDocuments model)
        {
            _dbContext.EmployeeDocuments.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateEmployeeDocuments(Guid id, EmployeeDocuments updatedEntity)
        {
            _dbContext.EmployeeDocuments.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteEmployeeDocuments(Guid id)
        {
            var entityData = _dbContext.EmployeeDocuments.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.EmployeeDocuments.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchEmployeeDocuments(Guid id, JsonPatchDocument<EmployeeDocuments> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.EmployeeDocuments.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.EmployeeDocuments.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}