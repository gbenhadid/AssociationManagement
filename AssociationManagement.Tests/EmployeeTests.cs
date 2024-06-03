using Microsoft.AspNetCore.Mvc;
using Moq;
using AssociationManagement.Application.Dtos.Employees;
using AssociationManagement.Application.Services.Abstractions;
using AssociationManagement.Application.Services.ServicesManager;
using AssociationManagement.Core.Common;
using AssociationManagement.Tools.RequestFeatures;
using AssociationManagement.WebAPI.Controllers;

namespace AssociationManagement.Tests;

public class EmployeeTests {
    [Fact]
    public async Task Index_ReturnsOkResult_WithPaginatedList() {
        // Arrange
        var serviceManagerMock = new Mock<IServiceManager>();
        var controller = new EmployeesController(serviceManagerMock.Object);
        var employeeParameters = new RequestQueryParameters();

        var paginatedList = new PaginatedList<EmployeeResponse>(new List<EmployeeResponse>(), 0, 1, 10);
        serviceManagerMock.Setup(s => s.EmployeeService.GetAllAsync(employeeParameters))
            .ReturnsAsync(paginatedList);

        // Act
        var result = await controller.Index(employeeParameters);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsType<PaginatedList<EmployeeResponse>>(okResult.Value);
        Assert.Equal(paginatedList, model);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithEmployeeResponse() {
        // Arrange
        var serviceManagerMock = new Mock<IServiceManager>();
        var controller = new EmployeesController(serviceManagerMock.Object);
        var employeeId = 1;
        var employeeResponse = new EmployeeResponse();

        serviceManagerMock.Setup(s => s.EmployeeService.GetByIdAsync(employeeId))
            .ReturnsAsync(employeeResponse);

        // Act
        var result = await controller.GetById(employeeId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsType<EmployeeResponse>(okResult.Value);
        Assert.Equal(employeeResponse, model);
    }

    [Fact]
    public async Task Create_ReturnsCreatedResult() {
        // Arrange
        var employeeServiceMock = new Mock<IEmployeeService>();
        var serviceManagerMock = new Mock<IServiceManager>();
        serviceManagerMock.Setup(s => s.EmployeeService).Returns(employeeServiceMock.Object);
        var controller = new EmployeesController(serviceManagerMock.Object);
        var employeeCreationRequest = new EmployeeCreationRequest();

        // Act
        var result = await controller.Create(employeeCreationRequest);

        // Assert
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WhenEmployeeExists() {
        // Arrange
        var serviceManagerMock = new Mock<IServiceManager>();
        var controller = new EmployeesController(serviceManagerMock.Object);
        var employeeId = 1;
        var existingEmployee = new EmployeeResponse();

        serviceManagerMock.Setup(s => s.EmployeeService.GetByIdAsync(employeeId))
            .ReturnsAsync(existingEmployee);

        // Act
        var result = await controller.Delete(employeeId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFoundResult_WhenEmployeeDoesNotExist() {
        // Arrange
        var serviceManagerMock = new Mock<IServiceManager>();
        var controller = new EmployeesController(serviceManagerMock.Object);
        var employeeId = 1;

        serviceManagerMock.Setup(s => s.EmployeeService.GetByIdAsync(employeeId))
            .ReturnsAsync((EmployeeResponse)null);

        // Act
        var result = await controller.Delete(employeeId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOkResult_WhenModelStateIsValid() {
        // Arrange
        var employeeServiceMock = new Mock<IEmployeeService>();
        var serviceManagerMock = new Mock<IServiceManager>();
        serviceManagerMock.Setup(s => s.EmployeeService).Returns(employeeServiceMock.Object);
        var controller = new EmployeesController(serviceManagerMock.Object);
        var employeeUpdateRequest = new EmployeeUpdateRequest();
        var employeeId = 1;

        // Act
        var result = await controller.Update(employeeUpdateRequest, employeeId);

        // Assert
        Assert.IsType<OkResult>(result);
        serviceManagerMock.Verify(s => s.EmployeeService.UpdateAsync(It.IsAny<EmployeeUpdateRequest>(), employeeId), Times.Once);
    }

}