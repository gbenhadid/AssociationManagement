using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using AssociationManagement.Core.Common;
using AssociationManagement.DataAccess.Extensions;
using AssociationManagement.DataAccess.Persistance;
using AssociationManagement.DataAccess.Repositories.Abstractions;
using AssociationManagement.Tools.RequestFeatures;
using System.Linq.Expressions;

namespace AssociationManagement.DataAccess.Repositories.Implementation {
    public class BaseRepository<T> : IBaseRepository<T> where T : class {

        protected ApplicationDbContext _DbContext;
        public BaseRepository(ApplicationDbContext dbContext)
              => _DbContext = dbContext;

        /// <inheritdoc />
        public void Create(T entity) => _DbContext.Set<T>().Add(entity);

        /// <inheritdoc />
        public void Delete(T entity) => _DbContext.Set<T>().Remove(entity);

        /// <inheritdoc />
        public void DeleteMany(IEnumerable<T> entities) => _DbContext.Set<T>().RemoveRange(entities);

        /// <inheritdoc />
        public IQueryable<T> FindAll(bool trackChanges = false) {
            return !trackChanges ?
                 _DbContext.Set<T>()
                 .AsNoTracking() :
                 _DbContext.Set<T>();
        }

        /// <inheritdoc />
        public async Task<PaginatedList<T>> FindAllAsync(
            RequestQueryParameters requestParams,
            bool trackChanges,
            Expression<Func<T, bool>>? searchPredicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? additionalFilter = null) {

            var query = _DbContext.Set<T>().AsQueryable();
            if(trackChanges) {
                query = query.AsNoTracking();
            }

            if(include is not null) {
                query = include(query);
            }
            if(requestParams.FilterParameters is not null && requestParams.FilterParameters.Count() > 0)
                query = QueryBuilder.BuildQuery(query, requestParams);

            if(requestParams.SortParameters is not null && requestParams.SortParameters.Count() > 0)
                query = QueryBuilder.BuildSort(query, requestParams);

            if(!string.IsNullOrEmpty(requestParams.searchTerm)) {
                query = query.Where(searchPredicate);
            }


            if(additionalFilter != null) {
                query = query.Where(additionalFilter);
            }

            var totalRecords = query.Count();

            var items = await query.Skip((requestParams.PageNumber - 1) * requestParams.PageSize)
                                  .Take(requestParams.PageSize)
                                  .ToListAsync();

            return new PaginatedList<T>(items, totalRecords, requestParams.PageNumber, requestParams.PageSize);
        }



        /// <inheritdoc />
        public async Task<PaginatedList<IGrouping<int, T>>> FindAllGroupByAsync(
            RequestQueryParameters requestParams, 
            bool trackChanges, Expression<Func<T, bool>>? searchPredicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? additionalFilter = null) {

            var query = _DbContext.Set<T>().AsQueryable();
            if(trackChanges) {
                query = query.AsNoTracking();
            }

            if(include != null) {
                query = include(query);
            }
            if(requestParams.FilterParameters is not null && requestParams.FilterParameters.Count() > 0)
                query = QueryBuilder.BuildQuery(query, requestParams);

            if(requestParams.SortParameters is not null && requestParams.SortParameters.Count() > 0)
                query = QueryBuilder.BuildSort(query, requestParams);

            if(!string.IsNullOrEmpty(requestParams.searchTerm)) {
                query = query.Where(searchPredicate);
            }


            if(additionalFilter != null) {
                query = query.Where(additionalFilter);
            }
            var groupedQuery = QueryBuilder.BuildGroupBy(query, requestParams.GroupBy!);

            var groupedData = await groupedQuery.ToListAsync();
            var pagedData = groupedData
                        .Skip((requestParams.PageNumber - 1) * requestParams.PageSize)
                        .Take(requestParams.PageSize)
                        .ToList();

            var totalRecords = groupedData.Count();

            return new PaginatedList<IGrouping<int, T>>(pagedData, totalRecords, requestParams.PageNumber, requestParams.PageSize);
        }

        /// <inheritdoc />
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) {
            return !trackChanges ?
                   _DbContext.Set<T>()
                   .Where(expression)
                   .AsNoTracking() :
                   _DbContext.Set<T>()
                   .Where(expression);
        }

        /// <inheritdoc />
        public IQueryable<T> FindByCondition(IQueryable<T> query, Expression<Func<T, bool>> expression, bool trackChanges) {
            return trackChanges
                ? query.Where(expression)
                : query.Where(expression)
                      .AsNoTracking();
        }

        /// <inheritdoc />
        public void Update(T entity) => _DbContext.Set<T>().Update(entity);

        /// <inheritdoc />
        public int Count() => _DbContext.Set<T>().Count();
       
    }
}
