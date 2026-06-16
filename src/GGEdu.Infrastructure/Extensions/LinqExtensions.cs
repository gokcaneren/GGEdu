using System.Linq.Expressions;

namespace GGEdu.Infrastructure.Extensions
{
    public static class LinqExtensions
    {

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool contidion,
            Func<TSource, bool> predicate)
        {
            if (contidion)
            {
                source.Where(predicate);
                return source;
            }

            return source;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition,
            Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if(source == null || !source.Any())
            {
                return true;
            }

            return false;
        }

        public static bool IsNullOrEmpty<TSource>(this IQueryable<TSource> source)
        {
            if (source == null || !source.Any())
            {
                return true;
            }

            return false;
        }
    }
}
