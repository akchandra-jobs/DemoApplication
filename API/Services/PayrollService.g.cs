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
    /// The payrollService responsible for managing payroll related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting payroll information.
    /// </remarks>
    public interface IPayrollService
    {
        /// <summary>Retrieves a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <returns>The payroll data</returns>
        Payroll GetById(Guid id);

        /// <summary>Retrieves a list of payrolls based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of payrolls</returns>
        List<Payroll> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new payroll</summary>
        /// <param name="model">The payroll data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Payroll model);

        /// <summary>Updates a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <param name="updatedEntity">The payroll data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Payroll updatedEntity);

        /// <summary>Updates a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <param name="updatedEntity">The payroll data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Payroll> updatedEntity);

        /// <summary>Deletes a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The payrollService responsible for managing payroll related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting payroll information.
    /// </remarks>
    public class PayrollService : IPayrollService
    {
        private DemoApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Payroll class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PayrollService(DemoApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <returns>The payroll data</returns>
        public Payroll GetById(Guid id)
        {
            var entityData = _dbContext.Payroll.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of payrolls based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of payrolls</returns>/// <exception cref="Exception"></exception>
        public List<Payroll> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPayroll(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new payroll</summary>
        /// <param name="model">The payroll data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Payroll model)
        {
            model.Id = CreatePayroll(model);
            return model.Id;
        }

        /// <summary>Updates a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <param name="updatedEntity">The payroll data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Payroll updatedEntity)
        {
            UpdatePayroll(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <param name="updatedEntity">The payroll data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Payroll> updatedEntity)
        {
            PatchPayroll(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific payroll by its primary key</summary>
        /// <param name="id">The primary key of the payroll</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePayroll(id);
            return true;
        }
        #region
        private List<Payroll> GetPayroll(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Payroll.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Payroll>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Payroll), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Payroll, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePayroll(Payroll model)
        {
            _dbContext.Payroll.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePayroll(Guid id, Payroll updatedEntity)
        {
            _dbContext.Payroll.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePayroll(Guid id)
        {
            var entityData = _dbContext.Payroll.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Payroll.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPayroll(Guid id, JsonPatchDocument<Payroll> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Payroll.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Payroll.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}