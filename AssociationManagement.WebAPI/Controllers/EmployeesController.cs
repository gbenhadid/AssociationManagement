using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssociationManagement.Application.Dtos.Employees;
using AssociationManagement.Application.Services.ServicesManager;
using AssociationManagement.Core.Common;
using AssociationManagement.Tools.Exceptions.Abstractions;
using AssociationManagement.Tools.RequestFeatures;

namespace AssociationManagement.WebAPI.Controllers {
    [ApiController]
    [Route("api/employees")]
    [Authorize]

    /// <summary>
    /// Controller responsible for managing employee-related actions.
    /// </summary>
    public class EmployeesController : ControllerBase {

        private readonly IServiceManager _serviceManager;

        public EmployeesController(IServiceManager serviceManager) {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all students by pagination.
        /// </summary>
        /// <param name="employeeParameters">Defines the request parameters(filtering, sorting ...)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<EmployeeResponse>>> Index(
            [FromQuery] RequestQueryParameters employeeParameters) {

            PaginatedList<EmployeeResponse> employees = await _serviceManager.EmployeeService.GetAllAsync(employeeParameters);

            return Ok(employees);
        }

        /// <summary>
        /// Retrieves an employee by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id) {
            EmployeeResponse? employee = await _serviceManager.EmployeeService.GetByIdAsync(id);

            return employee is null 
                ? throw new NotFoundException($"Error: employee with id {id} cannot be found.") 
                : (ActionResult<EmployeeResponse>)Ok(employee);
        }

        /// <summary>
        /// Creates an Employee.
        /// </summary>
        /// <param name="employeeCreationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(EmployeeCreationRequest employeeCreationRequest) {
            if(!ModelState.IsValid) {
                throw new BadRequestException(string.Join(
                    separator: ", ",
                    values: ModelState.Values.SelectMany(v => v.Errors))
                );
            }

            await _serviceManager.EmployeeService.AddAsync(employeeCreationRequest);

            return Created();
        }

        /// <summary>
        /// Deletes an employee by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) {
            EmployeeResponse? employee = await _serviceManager.EmployeeService.GetByIdAsync(id);
            if(employee is null) {
                return NotFound();
            }

            await _serviceManager.EmployeeService.DeleteAsync(id);

            return Ok();
        }

        /// <summary>
        /// Updates an employee by Id.
        /// </summary>
        /// <param name="employeeUpdateRequest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] EmployeeUpdateRequest employeeUpdateRequest, int id) {
            if(!ModelState.IsValid) {
                throw new BadRequestException(string.Join(
                    separator: ", ", 
                    values : ModelState.Values.SelectMany(v => v.Errors))
                );
            }
            await _serviceManager.EmployeeService.UpdateAsync(employeeUpdateRequest, id);

            return Ok();
        }

    }
}
