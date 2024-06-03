using AssociationManagement.DataAccess.Repositories.Implementation;

namespace AssociationManagement.DataAccess.Repositories.RepositoryManager {
    public interface IRepositoryManager {

        /// <summary>
        /// Defines a lazy loaded <see cref="EmployeeRepository"/> instance.
        /// </summary>
        IEmployeeRepository Employee { get; }


        /// <summary>
        /// Saves changes to all tracked entities.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}