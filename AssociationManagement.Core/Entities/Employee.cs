using AssociationManagement.Core.Common;

namespace AssociationManagement.Core.Entities {
    public class Employee : BaseEntity {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string FullName { get; private set; } = string.Empty;
    }
}
