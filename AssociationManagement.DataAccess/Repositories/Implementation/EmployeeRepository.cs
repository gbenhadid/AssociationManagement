using Microsoft.EntityFrameworkCore;
using AssociationManagement.Core.Common;
using AssociationManagement.Core.Entities;
using AssociationManagement.DataAccess.Persistance;
using AssociationManagement.Tools.RequestFeatures;
using System.Linq.Expressions;

namespace AssociationManagement.DataAccess.Repositories.Implementation {
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository {

        public EmployeeRepository(ApplicationDbContext context) : base(context) { }

        /// <inheritdoc />
        public async Task<PaginatedList<Employee>> GetAllAsync(
            RequestQueryParameters requestParameters,
            Expression<Func<Employee, bool>>? searchPredicate = null) {

            return await FindAllAsync(
                trackChanges: false,
                requestParams: requestParameters,
                searchPredicate: searchPredicate);
        }

        /// <inheritdoc />
        public async Task<Employee?> GetByIdAsync(int employeeId) =>
            await _DbContext.Employees.SingleOrDefaultAsync(m => m.Id == employeeId);


    }
}
