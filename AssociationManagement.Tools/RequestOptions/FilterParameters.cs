using AssociationManagement.Tools.QueryBuilder;

namespace AssociationManagement.Tools.RequestFeatures {
    public class FilterParameter {
        private string? _value { get; set; }
        public string? ColumnName { get; set; }
        public OperatorType? Operator { get; set; }
        public object? Value { 
            get => _value; 
            set => _value = value?.ToString(); 
        }

    }
}
