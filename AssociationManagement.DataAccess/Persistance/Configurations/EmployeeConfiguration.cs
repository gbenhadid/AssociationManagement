using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssociationManagement.Core.Entities;

namespace AssociationManagement.DataAccess.Persistance.Configurations {
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee> {
        public void Configure(EntityTypeBuilder<Employee> builder) {

        }
    }
}
