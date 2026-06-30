using AuthService.Application.DTOs.Page;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(
            this IQueryable<T> source,
            string propertyName,
            bool desc)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");

            var property = Expression.PropertyOrField(parameter, propertyName);

            var lambda = Expression.Lambda(property, parameter);

            var method = desc
                ? "OrderByDescending"
                : "OrderBy";

            var expression = Expression.Call(
                typeof(Queryable),
                method,
                new[]
                {
                typeof(T),
                property.Type
                },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(expression);
        }

        public static async Task<PageResponse<T>>
        ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PageRequest request,
        CancellationToken cancellationToken)
        {
            var total = await query.CountAsync(cancellationToken);

            var items = await query

                .Skip((request.PageNumber - 1) * request.PageSize)

                .Take(request.PageSize)

                .ToListAsync(cancellationToken);

            return new PageResponse<T>
            {
                Items = items,

                TotalCount = total,

                PageNumber = request.PageNumber,

                PageSize = request.PageSize
            };
        }
    }
}
