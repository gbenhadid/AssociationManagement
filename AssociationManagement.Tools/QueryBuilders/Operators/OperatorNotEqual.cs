using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{
    public class OperatorNotEqual : Operator
    {
        public OperatorNotEqual(FilterParameter filterItem) : base(filterItem) { }

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            // Check if the property is not equal to the specified value
            var expression = Expression.NotEqual(memberExpression, constantExpression);
            return expression;
        }
    }
}
