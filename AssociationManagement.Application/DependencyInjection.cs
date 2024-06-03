using Microsoft.Extensions.DependencyInjection;
using AssociationManagement.Application.MappingProfiles;
using AssociationManagement.Application.Services.ServicesManager;

namespace AssociationManagement.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddAutoMapper(typeof(DependencyInjection));
            services.AddAutoMapper(typeof(EmployeeProfile));
            services.AddScoped<IServiceManager, ServiceManager>();
            return services;
        }
    }
}