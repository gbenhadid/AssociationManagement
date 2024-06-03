using Microsoft.AspNetCore.Mvc.Filters;
using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder.Operators
{
    public class OperatorGreaterThanOrEqual : Operator
    {
        public OperatorGreaterThanOrEqual(FilterParameter filterItem) : base(filterItem, true) { }

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
        }
    }
}
