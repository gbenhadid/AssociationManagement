using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using AssociationManagement.Core.Common;
using AssociationManagement.Core.Entities;
using AssociationManagement.DataAccess.Extensions;
using AssociationManagement.DataAccess.Persistance.Configurations;

namespace AssociationManagement.DataAccess.Persistance;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> {

    private readonly IConfiguration configuration;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, 
        IConfiguration configuration) : base(options) {

        this.configuration = configuration;
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        SetupConfiguration(modelBuilder);
        modelBuilder.AddSoftDeleteFilter();
        modelBuilder.SetDecimalPrecision();
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        var dbConnectionString = configuration.GetConnectionString("DataBaseConnection");

        optionsBuilder.UseSqlServer(
            connectionString: dbConnectionString,

            sqlServerOptionsAction: builder => {
                builder.EnableRetryOnFailure(
                    maxRetryCount: 5, 
                    maxRetryDelay: TimeSpan.FromSeconds(10), 
                    errorNumbersToAdd: null);
            });
    }

    /// <summary>
    /// Sets up the entity configurations for various entities in the database model.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> instance to which the configurations are applied.</param>
    private void SetupConfiguration(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        ApplyAuditInfo();
        ApplySoftDelete();

        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Applies auditing information to entities being modified.
    /// </summary>
    private void ApplyAuditInfo() {
        foreach(var entry in ChangeTracker.Entries()) {
            ApplyAuditInfo(entry); 
        }
    }


    /// <summary>
    /// Applies auditing information to a specific entity.
    /// </summary>
    /// <param name="entry">The entry to apply auditing for.</param>
    private void ApplyAuditInfo(EntityEntry entry) {
        if(entry.State == EntityState.Modified && entry.Entity is BaseAuditableEntity ModifiedAuditableEntity) {
            ModifiedAuditableEntity.UpdatedAt = DateTime.UtcNow;
        } else if(entry.State == EntityState.Added && entry.Entity is BaseAuditableEntity AddedAuditableEntity) {
            AddedAuditableEntity.CreatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Applies soft delete logic to entities being deleted.
    /// </summary>
    private void ApplySoftDelete() {
        foreach(var entry in ChangeTracker.Entries()) {
            if(entry.State == EntityState.Deleted && entry.Entity is ISoftDelete softDelete) {
                entry.State = EntityState.Modified;
                softDelete.IsDeleted = true;
                softDelete.DeletedAt = DateTime.UtcNow;
            }
        }
    }

}
