using System.Linq.Expressions;

namespace GGEdu.Infrastructure.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
                return memberExpression.Member.Name;

            throw new ArgumentException("Invalid expression format");
        }
    }
}
