using AssociationManagement.Tools.QueryBuilder;
using AssociationManagement.Tools.RequestFeatures;
using System.Linq.Expressions;
using System.Reflection;

namespace AssociationManagement.DataAccess.Extensions {

    /// <summary>
    /// Provides extension methods for building and modifying queries using the <see cref="IQueryable{T}"/> interface.
    /// </summary>
    public static class QueryBuilder {
        /// <summary>
        /// Builds and applies filters to the specified query based on the provided <see cref="RequestQueryParameters"/>.
        /// </summary>
        /// <typeparam name="T">The type of entities in the query.</typeparam>
        /// <param name="query">The original query.</param>
        /// <param name="parameters">The request query parameters containing filter information.</param>
        /// <returns>The modified query with applied filters.</returns>
        public static IQueryable<T> BuildQuery<T>(IQueryable<T> query, RequestQueryParameters parameters) {
            if(parameters.FilterParameters is null) {
                return query;
            }
            var entityType = typeof(T);

            foreach(var filter in parameters.FilterParameters) {
                if(filter.ColumnName is null) {
                    continue; 
                }

                var property = entityType.GetProperty(
                    name: filter.ColumnName, 
                    bindingAttr: BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) 
                        ?? throw new InvalidOperationException($"Property with name {filter.ColumnName} doesn't exist");

                if(!string.IsNullOrEmpty(filter.ColumnName) && filter.Value is not null) {
                    query = BuildFilter(query, filter);
                }
            }

            return query;
        }

        /// <summary>
        /// Builds and applies sorting to the specified query based on the provided <see cref="RequestQueryParameters"/>.
        /// </summary>
        /// <typeparam name="T">The type of entities in the query.</typeparam>
        /// <param name="query">The original query.</param>
        /// <param name="parameters">The request query parameters containing sort information.</param>
        /// <returns>The modified query with applied sorting.</returns>
        public static IQueryable<T> BuildSort<T>(IQueryable<T> query, RequestQueryParameters parameters) {
            if(parameters.SortParameters is null) {
                return query;
            }

            var entityType = typeof(T);

            foreach(var sort in parameters.SortParameters) {
                var property = entityType.GetProperty(
                    name:sort.SortBy, 
                    bindingAttr: BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) 
                        ?? throw new InvalidOperationException($"Property with name {sort.SortBy} doesn't exist");

                if(!string.IsNullOrEmpty(sort.SortBy)) {
                    query = BuildSort(query, sort);
                }
            }

            return query;
        }

        /// <summary>
        /// Builds and applies sorting to the specified query based on the provided <see cref="SortParameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of entities in the query.</typeparam>
        /// <param name="query">The original query.</param>
        /// <param name="sortParameter">The sort parameter containing information about the sorting.</param>
        /// <returns>The modified query with applied sorting.</returns>
        public static IQueryable<T> BuildSort<T>(IQueryable<T> query, SortParameter sortParameter) {
            var property = typeof(T).GetProperty(
                name: sortParameter.SortBy,
                bindingAttr:  BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) 
                    ?? throw new InvalidOperationException($"Property with name {sortParameter.SortBy} doesn't exist");

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda<Func<T, object>>(propertyAccess, parameter);

            if(sortParameter.Descending) {
                query = query.OrderByDescending(orderByExp);
            } else {
                query = query.OrderBy(orderByExp);
            }

            return query;
        }

        /// <summary>
        /// Builds and applies a filter to the specified query based on the provided <see cref="FilterParameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of entities in the query.</typeparam>
        /// <param name="query">The original query.</param>
        /// <param name="FilterItem">The filter parameter containing information about the filter.</param>
        /// <returns>The modified query with applied filter.</returns>
        private static IQueryable<T> BuildFilter<T>(IQueryable<T> query, FilterParameter FilterItem) {
            Expression? body = null;
            var _parameter = Expression.Parameter(typeof(T), "x");
            var @operator = Operator.GetOperator(FilterItem);
            var itemExpression = @operator.GetExpression(_parameter);
            body = body != null ? Expression.AndAlso(body, itemExpression) : itemExpression;

            return query.Where(Expression.Lambda<Func<T, bool>>(body, _parameter));
        }

        /// <summary>
        /// Builds and applies grouping to the specified query based on the provided group-by property.
        /// </summary>
        /// <typeparam name="T">The type of entities in the query.</typeparam>
        /// <param name="query">The original query.</param>
        /// <param name="groupBy">The name of the property to group by.</param>
        /// <returns>The modified query with applied grouping.</returns>
        public static IQueryable<IGrouping<int, T>> BuildGroupBy<T>(IQueryable<T> query, string groupBy) {
            var entityType = typeof(T);

            var property = entityType.GetProperty(
                name : groupBy,
                bindingAttr: BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) 
                    ?? throw new InvalidOperationException($"Property with name {groupBy} doesn't exist");

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var groupByExp = Expression.Lambda<Func<T, int>>(propertyAccess, parameter);

            return query.GroupBy(groupByExp);
        }
    }
}
