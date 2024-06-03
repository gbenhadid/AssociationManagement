using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{
    public class OperatorNotContains : Operator
    {
        public OperatorNotContains(FilterParameter filterItem) : base(filterItem) { }

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            // Check if the property does not contain the specified value
            var containsExpression = Expression.Call(memberExpression, "Contains", null, constantExpression);
            var expression = Expression.Not(containsExpression);
            return expression;
        }
    }
}
