using AssociationManagement.DataAccess.Persistance;
using AssociationManagement.DataAccess.Repositories.Implementation;

namespace AssociationManagement.DataAccess.Repositories.RepositoryManager {
    public sealed class RepositoryManager : IRepositoryManager {

        private readonly ApplicationDbContext _applicationDbContext;

        private readonly Lazy<IEmployeeRepository> employeeRepository;


        public RepositoryManager(
            ApplicationDbContext repositoryContext) {
            _applicationDbContext = repositoryContext;

            employeeRepository = new Lazy<IEmployeeRepository>(()
                => new EmployeeRepository(repositoryContext));

        }

        /// <inheritdoc />
        public IEmployeeRepository Employee => employeeRepository.Value;

        /// <inheritdoc />
        public async Task<bool> SaveAsync() {
            return await _applicationDbContext.SaveChangesAsync() > 0;
        }

    }
}
