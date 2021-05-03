using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OK.ReadingIsGood.Shared.Core.Constants;

namespace OK.ReadingIsGood.Shared.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageNumber, int pageSize, out int pageCount, out int totalCount)
        {
            totalCount = source.Count();
            pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var skip = pageSize * (pageNumber - 1);
            var take = pageSize;

            return source.Skip(skip).Take(take);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sort, string order = OrderConstants.Ascending)
        {
            if (string.IsNullOrEmpty(sort))
            {
                return source;
            }

            if (string.IsNullOrEmpty(order))
            {
                order = OrderConstants.Ascending;
            }

            var paramExp = Expression.Parameter(typeof(T), "x");
            var propExp = GetPropertyExpression<T>(paramExp, sort, out Type returnType);
            var lambdaExp = Expression.Lambda(propExp, paramExp);

            var queryExpression = Expression.Call(
                typeof(Queryable),
                order.ToLowerInvariant() == OrderConstants.Descending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy),
                new Type[] { source.ElementType, returnType },
                source.Expression,
                lambdaExp);

            return source.Provider.CreateQuery<T>(queryExpression);
        }

        private static Expression GetPropertyExpression<T>(ParameterExpression parameter, string propertyName, out Type returnType)
        {
            returnType = typeof(T);

            var body = parameter as Expression;

            foreach (var member in propertyName.Split('.'))
            {
                var nestedProperty = returnType.GetProperty(member, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                returnType = nestedProperty.PropertyType;
                body = Expression.PropertyOrField(body, nestedProperty.Name);
            }

            return body;
        }
    }
}