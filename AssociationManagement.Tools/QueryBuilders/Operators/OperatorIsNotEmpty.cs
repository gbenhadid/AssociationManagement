using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{
    public class OperatorIsNotEmpty : Operator
    {
        public OperatorIsNotEmpty(FilterParameter filterItem) : base(filterItem) { }

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            var isNotNullExpression = Expression.NotEqual(memberExpression, Expression.Constant(null));
            var isNotEmptyStringExpression = Expression.NotEqual(memberExpression, Expression.Constant(string.Empty));

            var expression = Expression.AndAlso(isNotNullExpression, isNotEmptyStringExpression);
            return expression;
        }
    }

}
