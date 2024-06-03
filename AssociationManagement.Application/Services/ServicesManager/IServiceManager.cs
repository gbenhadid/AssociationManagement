using AssociationManagement.Application.Services.Abstractions;

namespace AssociationManagement.Application.Services.ServicesManager {
    public interface IServiceManager {
        IEmployeeService EmployeeService { get; }
        IAccountService AccountService { get; }
        IUserService UserService { get; }
    }
}