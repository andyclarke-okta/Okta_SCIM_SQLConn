using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIMSQLConn.Connectors
{
    //// helper class for pagination across linq objects in .NET
    //// see http://stackoverflow.com/questions/6185159/linq-and-pagination for details.
    public static class PagingExtensions
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }
}