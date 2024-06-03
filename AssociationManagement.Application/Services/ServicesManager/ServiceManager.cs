using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using AssociationManagement.Application.Services.Abstractions;
using AssociationManagement.Application.Services.Implementation;
using AssociationManagement.Core.Entities;
using AssociationManagement.DataAccess.Repositories.RepositoryManager;
using AssociationManagement.Tools.Logging;
using Softylines.Compta.Application.Services.Implementation;

namespace AssociationManagement.Application.Services.ServicesManager
{
    public class ServiceManager : IServiceManager {
        private readonly Lazy<IAccountService> accountService;
        private readonly Lazy<IUserService> userService;
        private readonly Lazy<IEmployeeService> employeeService;


        public ServiceManager(
            IRepositoryManager repositoryManager,
            ILoggerManager logger, IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings) {

            this.accountService = new Lazy<IAccountService>(() => new
                AccountService(userManager, jwtSettings));

            this.userService = new Lazy<IUserService>(() => new
                UserService(userManager, mapper, logger));
            
            this.employeeService = new Lazy<IEmployeeService>(() => new
                EmployeeService(repositoryManager, logger, mapper));
        }

        public IAccountService AccountService => this.accountService.Value;
        public IUserService UserService => this.userService.Value;
        public IEmployeeService EmployeeService => this.employeeService.Value;
    }
}
