
using Microsoft.EntityFrameworkCore;
using AssociationManagement.Core.Common;
using System.Linq.Expressions;

namespace AssociationManagement.DataAccess.Extensions;
/// <summary>
/// Provides extension methods for configuring Entity Framework Core models using <see cref="ModelBuilder"/>.
/// </summary>
public static class ModelBuilderExtensions {
    /// <summary>
    /// Sets the decimal precision for all decimal properties in the model.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> instance.</param>
    /// <param name="precision">The precision of the decimal properties. Default is 18.</param>
    /// <param name="scale">The scale of the decimal properties. Default is 3.</param>
    public static void SetDecimalPrecision(this ModelBuilder modelBuilder, int precision = 18, int scale = 3) {
        var entities = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

        foreach(var property in entities) {
            property.SetColumnType($"decimal({precision},{scale})");
        }
    }

    /// <summary>
    /// Adds a soft delete filter to entities implementing the specified soft delete interface.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> instance.</param>
    public static void AddSoftDeleteFilter(this ModelBuilder modelBuilder) {
        foreach(var entityType in modelBuilder.Model.GetEntityTypes()) {
            if(typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType)) {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(BuildSoftDeleteFilter(entityType.ClrType));
            }
        }
    }

    /// <summary>
    /// Builds a soft delete filter expression for entities implementing soft delete.
    /// </summary>
    /// <param name="type">The type of the entity implementing soft delete.</param>
    /// <returns>A <see cref="LambdaExpression"/> representing the soft delete filter.</returns>
    public static LambdaExpression BuildSoftDeleteFilter(this Type type) {
        var parameter = Expression.Parameter(type, "e");
        var isDeletedProperty = Expression.Property(parameter, "IsDeleted");
        var condition = Expression.Equal(isDeletedProperty, Expression.Constant(false, typeof(bool)));

        var deletedAtProperty = Expression.Property(parameter, "DeletedAt");
        var deletedAtIsNull = Expression.Equal(deletedAtProperty, Expression.Constant(null, typeof(DateTime?)));
        condition = Expression.AndAlso(condition, deletedAtIsNull);

        var lambda = Expression.Lambda(condition, parameter);
        return lambda;
    }
}