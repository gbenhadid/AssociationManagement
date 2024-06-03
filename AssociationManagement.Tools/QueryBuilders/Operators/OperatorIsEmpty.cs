using AssociationManagement.Tools.RequestFeatures;
using System.Linq.Expressions;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{

    public class OperatorIsEmpty : Operator
    {
        public OperatorIsEmpty(FilterParameter filterItem) : base(filterItem, true) { }
        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            var isNullExpression = Expression.Equal(memberExpression, Expression.Constant(null));
            var isEmptyStringExpression = Expression.Equal(memberExpression, Expression.Constant(string.Empty));

            var expression = Expression.OrElse(isNullExpression, isEmptyStringExpression);
            return expression;
        }
    }
}
