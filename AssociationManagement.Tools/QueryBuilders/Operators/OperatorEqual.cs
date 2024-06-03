using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{
    public class OperatorEqual : Operator
    {
        public OperatorEqual(FilterParameter filterItem) : base(filterItem, true) { }
        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            var expression = Expression.Equal(memberExpression, constantExpression);
            return expression;
        }
    }
}
