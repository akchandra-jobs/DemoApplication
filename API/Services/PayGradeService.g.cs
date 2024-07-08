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
    /// The paygradeService responsible for managing paygrade related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting paygrade information.
    /// </remarks>
    public interface IPayGradeService
    {
        /// <summary>Retrieves a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <returns>The paygrade data</returns>
        PayGrade GetById(Guid id);

        /// <summary>Retrieves a list of paygrades based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of paygrades</returns>
        List<PayGrade> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new paygrade</summary>
        /// <param name="model">The paygrade data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PayGrade model);

        /// <summary>Updates a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <param name="updatedEntity">The paygrade data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PayGrade updatedEntity);

        /// <summary>Updates a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <param name="updatedEntity">The paygrade data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PayGrade> updatedEntity);

        /// <summary>Deletes a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The paygradeService responsible for managing paygrade related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting paygrade information.
    /// </remarks>
    public class PayGradeService : IPayGradeService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PayGrade class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PayGradeService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <returns>The paygrade data</returns>
        public PayGrade GetById(Guid id)
        {
            var entityData = _dbContext.PayGrade.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of paygrades based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of paygrades</returns>/// <exception cref="Exception"></exception>
        public List<PayGrade> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPayGrade(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new paygrade</summary>
        /// <param name="model">The paygrade data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PayGrade model)
        {
            model.Id = CreatePayGrade(model);
            return model.Id;
        }

        /// <summary>Updates a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <param name="updatedEntity">The paygrade data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PayGrade updatedEntity)
        {
            UpdatePayGrade(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <param name="updatedEntity">The paygrade data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PayGrade> updatedEntity)
        {
            PatchPayGrade(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific paygrade by its primary key</summary>
        /// <param name="id">The primary key of the paygrade</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePayGrade(id);
            return true;
        }
        #region
        private List<PayGrade> GetPayGrade(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PayGrade.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PayGrade>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PayGrade), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PayGrade, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePayGrade(PayGrade model)
        {
            _dbContext.PayGrade.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePayGrade(Guid id, PayGrade updatedEntity)
        {
            _dbContext.PayGrade.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePayGrade(Guid id)
        {
            var entityData = _dbContext.PayGrade.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PayGrade.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPayGrade(Guid id, JsonPatchDocument<PayGrade> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PayGrade.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PayGrade.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}