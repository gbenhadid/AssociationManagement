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
    public class OperatorContains : Operator
    {
        public OperatorContains(FilterParameter filterItem) : base(filterItem) { }
        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            if (memberExpression.Type != StringPresets.StringType)
                throw new InvalidCastException("Type of property must be string");

            var expression = Expression.Call(memberExpression, StringPresets.ContainsMethod, constantExpression);

            return expression;
        }
    }
}
