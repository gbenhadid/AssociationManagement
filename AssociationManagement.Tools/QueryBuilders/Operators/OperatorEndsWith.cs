using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{
    public class OperatorEndsWith : Operator
    {
        public OperatorEndsWith(FilterParameter filterItem) : base(filterItem) { }

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            var expression = Expression.Call(memberExpression, "EndsWith", null, constantExpression);
            return expression;
        }
    }
}
