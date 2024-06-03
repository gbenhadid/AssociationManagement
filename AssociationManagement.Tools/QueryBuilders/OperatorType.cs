using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder
{
    public enum OperatorType
    {
        Equal = 0,
        Contains = 1,
        NotContains = 2,
        GreaterThan = 3,
        GreaterThanOrEqual = 4,
        IsEmpty = 5,
        LessThan = 6,
        LessThanOrEqual = 7,
        NotEqual = 8,
        EndsWith = 9,
        StartsWith = 10,
        IsNotEmpty = 11


    }
}
