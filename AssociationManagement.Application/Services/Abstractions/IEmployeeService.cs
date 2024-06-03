using AssociationManagement.Application.Dtos.Employees;
using AssociationManagement.Core.Common;
using AssociationManagement.Tools.RequestFeatures;

namespace AssociationManagement.Application.Services.Abstractions
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Creates an employee.
        /// </summary>
        /// <param name="employeeCreationRequest">Employee creation request model</param>
        /// <returns>a <see cref="EmployeeCreationResponse> instance."/></returns>
        Task<EmployeeCreationResponse> AddAsync(EmployeeCreationRequest employeeCreationRequest);

        /// <summary>
        /// Deletes an employee.
        /// </summary>
        /// <param name="employeeId">The id of the employee</param>
        Task DeleteAsync(int employeeId);
        
        /// <summary>
        /// Retrieves all employees by pagination.
        /// </summary>
        /// <param name="RequestQueryParameters">Request parameters (filtering, sorting, pagination ...)</param>
        /// <returns></returns>
        Task<PaginatedList<EmployeeResponse>> GetAllAsync(RequestQueryParameters RequestQueryParameters);

        /// <summary>
        /// Retrieves an employee by Id.
        /// </summary>
        /// <param name="employeeId">The id of the employee</param>
        /// <returns>A <see cref="EmployeeResponse"/> if an employee exists.</returns>
        Task<EmployeeResponse?> GetByIdAsync(int employeeId);

        /// <summary>
        /// Updates an employee by Id.
        /// </summary>
        /// <param name="employeeUpdateRequest">The new version of the employee.</param>
        /// <param name="employeeId">The id of the employee</param>
        Task UpdateAsync(EmployeeUpdateRequest employeeUpdateRequest, int employeeId);
    }
}