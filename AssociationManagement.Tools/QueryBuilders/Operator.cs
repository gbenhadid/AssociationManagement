using AssociationManagement.Tools.QueryBuilder.Operators;
using AssociationManagement.Tools.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder
{
    public abstract class Operator
    {
        public static Operator GetOperator(FilterParameter filterItem)
        {
            return filterItem.Operator switch
            {
                OperatorType.Equal => new OperatorEqual(filterItem),
                OperatorType.StartsWith => new OperatorStartsWith(filterItem),
                OperatorType.EndsWith => new OperatorEndsWith(filterItem),
                OperatorType.Contains => new OperatorContains(filterItem),
                OperatorType.NotContains => new OperatorNotContains(filterItem),
                OperatorType.NotEqual => new OperatorNotEqual(filterItem),
                OperatorType.GreaterThan => new OperatorGreaterThan(filterItem),
                OperatorType.GreaterThanOrEqual => new OperatorGreaterThanOrEqual(filterItem),
                OperatorType.LessThan => new OperatorLessThan(filterItem),
                OperatorType.LessThanOrEqual => new OperatorLessThanOrEqual(filterItem),
                _ => throw new NotImplementedException()
            };
        }


        protected readonly FilterParameter _filterItem;
        private readonly bool _withValueHolder = false;
        private protected Operator(FilterParameter filterItem)
        {
            _filterItem = filterItem;
        }
        private protected Operator(FilterParameter filterItem, bool withValueHolder)
        {
            _filterItem = filterItem;
            _withValueHolder = withValueHolder;
        }

        public Expression GetExpression(ParameterExpression parameter)
        {
            var member = GetMemberExpression(parameter);

            var constant = GetConstantExpression(member, member.Type);
            var body = GetExpressionBody(member, constant);
            return body;

        }

        private MemberExpression GetMemberExpression(ParameterExpression parameter)
        {
            var member = Expression.PropertyOrField(parameter, _filterItem.ColumnName);
            return member;
        }
        protected virtual Expression GetConstantExpression(MemberExpression memberExpression, Type memberType)
        {
            var value = ValueCastUtility.CastValueToType(_filterItem.Value, memberExpression.Type);

            if (_withValueHolder)
            {
                var valueHolder = ValueHolderUtility.GetValueHolderWithValue(value);
                return Expression.PropertyOrField(Expression.Constant(valueHolder), nameof(ValueHolder<object>.Value));
            }
            else
            {
                return Expression.Constant(value);
            }
        }
        protected virtual Expression GetConstantExpression(MemberExpression memberExpression)
        {
            var value = ValueCastUtility.CastValueToType(_filterItem.Value, memberExpression.Type);

            if (_withValueHolder)
            {
                var valueHolder = ValueHolderUtility.GetValueHolderWithValue(value);
                return Expression.PropertyOrField(Expression.Constant(valueHolder), nameof(ValueHolder<object>.Value));
            }
            else
            {
                return Expression.Constant(value);
            }
        }

        protected abstract Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression);


    }
}
