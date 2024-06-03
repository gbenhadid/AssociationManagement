using Microsoft.EntityFrameworkCore.Query;
using AssociationManagement.Core.Common;
using AssociationManagement.Tools.RequestFeatures;
using System.Linq.Expressions;

namespace AssociationManagement.DataAccess.Repositories.Abstractions {
    public interface IBaseRepository<T> {

        /// <summary>
        /// Finds all entities of type T based on the provided query parameters, with optional search predicates, include expressions, and additional filters.
        /// </summary>
        /// <param name="requestParams">Query parameters for pagination and sorting.</param>
        /// <param name="trackChanges">Indicates whether to track changes for the entities.</param>
        /// <param name="searchPredicate">Optional search predicate to filter entities based on a specific condition.</param>
        /// <param name="include">Optional include expression for eager loading related entities.</param>
        /// <param name="additionalFilter">Additional filter expression to further refine the query.</param>
        /// <returns>A task that represents the asynchronous operation and returns a paginated list of entities of type T.</returns>
        Task<PaginatedList<T>> FindAllAsync(
            RequestQueryParameters requestParams,
            bool trackChanges,
            Expression<Func<T, bool>>? searchPredicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? additionalFilter = null);

        /// <summary>
        /// Finds all entities of type T based on the provided query parameters, grouped by the specified key.
        /// </summary>
        /// <param name="requestParams">Query parameters for pagination and sorting.</param>
        /// <param name="trackChanges">Indicates whether to track changes for the entities.</param>
        /// <param name="searchPredicate">Optional search predicate to filter entities based on a specific condition.</param>
        /// <param name="include">Optional include expression for eager loading related entities.</param>
        /// <param name="additionalFilter">Additional filter expression to further refine the query.</param>
        /// <returns>A task that represents the asynchronous operation and returns a paginated list of grouped entities of type T.</returns>
        Task<PaginatedList<IGrouping<int, T>>> FindAllGroupByAsync(
            RequestQueryParameters requestParams,
            bool trackChanges,
            Expression<Func<T, bool>>? searchPredicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? additionalFilter = null);

        /// <summary>
        /// Finds all entities of type T without applying any additional filters or conditions.
        /// </summary>
        /// <param name="trackChanges">Indicates whether to track changes for the entities.</param>
        /// <returns>An IQueryable representing all entities of type T.</returns>
        IQueryable<T> FindAll(bool trackChanges);

        /// <summary>
        /// Finds entities of type T based on the provided query, condition expression, and tracking option.
        /// </summary>
        /// <param name="query">The base query to apply the condition.</param>
        /// <param name="expression">The condition expression to filter entities.</param>
        /// <param name="trackChanges">Indicates whether to track changes for the entities.</param>
        /// <returns>An IQueryable representing entities of type T based on the specified condition.</returns>
        IQueryable<T> FindByCondition(
            IQueryable<T> query,
            Expression<Func<T, bool>> expression,
            bool trackChanges);

        /// <summary>
        /// Finds entities of type T based on the provided condition expression and tracking option.
        /// </summary>
        /// <param name="expression">The condition expression to filter entities.</param>
        /// <param name="trackChanges">Indicates whether to track changes for the entities.</param>
        /// <returns>An IQueryable representing entities of type T based on the specified condition.</returns>
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        /// <summary>
        /// Creates a new entity of type T.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        void Create(T entity);

        /// <summary>
        /// Updates an existing entity of type T.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an entity of type T.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes multiple entities of type T.
        /// </summary>
        /// <param name="entities">The collection of entities to be deleted.</param>
        void DeleteMany(IEnumerable<T> entities);
    }
}
