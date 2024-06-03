using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{
    public class OperatorStartsWith : Operator
    {
        public OperatorStartsWith(FilterParameter filterItem) : base(filterItem) { }

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
          
            // Check if the property starts with the specified value
            var expression = Expression.Call(memberExpression, "StartsWith", null, constantExpression);
            return expression;
        }
    }
}
