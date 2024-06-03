using AssociationManagement.Tools.Exceptions.Base;

namespace AssociationManagement.Tools.Exceptions.Abstractions
{
    public class InternalServerException : ServerException {
        public InternalServerException(string message)
            : base(message) {
        }
    }
}
