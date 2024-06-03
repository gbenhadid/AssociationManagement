using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using AssociationManagement.DataAccess.Persistance;
using AssociationManagement.DataAccess.Repositories.RepositoryManager;

namespace AssociationManagement.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection AddDataAccess(this IServiceCollection services) {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            return services;
        }
    }
}
