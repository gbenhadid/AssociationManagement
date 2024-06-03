using AssociationManagement.Core.Common;
using AssociationManagement.Core.Entities;
using AssociationManagement.DataAccess.Repositories.Abstractions;
using AssociationManagement.Tools.RequestFeatures;
using System.Linq.Expressions;

namespace AssociationManagement.DataAccess.Repositories.Implementation {
    public interface IEmployeeRepository : IBaseRepository<Employee> {

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <param name="requestParameters">Defines the filtering parameters.</param>
        /// <returns>A collection of all existing employees.</returns>
        Task<PaginatedList<Employee>> GetAllAsync(
            RequestQueryParameters requestParameters,
            Expression<Func<Employee, bool>>? searchPredicate = null);

        /// <summary>
        /// Retrieves an employee by Id.
        /// </summary>
        /// <param name="employeeId">Defines the Id of the employee.</param>
        ///<returns>An employee if exists.</returns>
        Task<Employee?> GetByIdAsync(int employeeId);
    }
}