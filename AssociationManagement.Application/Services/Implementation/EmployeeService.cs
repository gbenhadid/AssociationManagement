using AutoMapper;
using AssociationManagement.Application.Dtos.Employees;
using AssociationManagement.Application.Services.Abstractions;
using AssociationManagement.Core.Common;
using AssociationManagement.Core.Entities;
using AssociationManagement.DataAccess.Repositories.RepositoryManager;
using AssociationManagement.Tools.Exceptions.Abstractions;
using AssociationManagement.Tools.Logging;
using AssociationManagement.Tools.RequestFeatures;
using System.Linq.Expressions;

namespace AssociationManagement.Application.Services.Implementation
{
    public class EmployeeService(
        IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper) : IEmployeeService {

        private readonly IRepositoryManager repository = repository;
        private readonly ILoggerManager logger = logger;
        private readonly IMapper mapper = mapper;

        /// <inheritdoc />
        public async Task<PaginatedList<EmployeeResponse>> GetAllAsync(RequestQueryParameters RequestQueryParameters) {

            Expression<Func<Employee, bool>>? searchTerm =
                !string.IsNullOrEmpty(RequestQueryParameters.searchTerm) 

                ? x => string.Concat(x.FirstName, x.LastName)
                    .Trim()
                    .Contains(RequestQueryParameters.searchTerm.ToLower()
                        .Trim())
                : null!;

            PaginatedList<Employee> paginatedEmployees = await repository.Employee.GetAllAsync(RequestQueryParameters, searchTerm);

            return mapper.Map<PaginatedList<Employee>, PaginatedList<EmployeeResponse>>(paginatedEmployees);
        }

        /// <inheritdoc />
        public async Task<EmployeeResponse?> GetByIdAsync(int employeeId) {
            Employee? maybeEmployee = await repository.Employee.GetByIdAsync(employeeId)
                ?? throw new NotFoundException("Employee not found, please contact support.");

            return mapper.Map<EmployeeResponse>(maybeEmployee);
        }

        /// <inheritdoc />
        public async Task<EmployeeCreationResponse> AddAsync(EmployeeCreationRequest employeeCreationRequest) {

            Employee? maybeEmployee = mapper.Map<Employee>(employeeCreationRequest);
            repository.Employee.Create(maybeEmployee);

            bool success = await repository.SaveAsync();
            if(success) {
                logger.LogInfo($"Employee created successfully with id : {maybeEmployee.Id}");
            }
            return new EmployeeCreationResponse() { Id = maybeEmployee.Id };
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int employeeId) {
            Employee? maybeEmployee = await repository.Employee.GetByIdAsync(employeeId);

            if(maybeEmployee is null) {
                logger.LogError($"An error occured : cannot delete employee with id {employeeId}, the latter doesn't exist");
                throw new NotFoundException($"Cannot delete employee with id {employeeId}, the latter doesn't exist");
            }

            repository.Employee.Delete(maybeEmployee);

            bool success = await repository.SaveAsync();

            if(!success) {
                logger.LogError($"An error occured while deleting employee with id : {maybeEmployee.Id}");
                throw new InternalServerException($"An error occured while deleting employee with id: {maybeEmployee.Id}");
            }
            logger.LogInfo($"Employee with id : {maybeEmployee.Id} is deleted successfully.");
        }

        /// <inheritdoc />
        public async Task UpdateAsync(EmployeeUpdateRequest employeeUpdateRequest, int employeeId) {
            Employee? maybeEmployee = await repository.Employee.GetByIdAsync(employeeId);

            if(maybeEmployee is null) {
                logger.LogError($"An error occured : cannot update employee with id : {employeeId}, the latter doesn't exist");
                throw new BadRequestException($"An error occured: cannot update employee with id: {employeeId} the latter doesn't exst.");
            }

            mapper.Map(employeeUpdateRequest, maybeEmployee);

            repository.Employee.Update(maybeEmployee);
            await repository.SaveAsync();
        }

    }
}
