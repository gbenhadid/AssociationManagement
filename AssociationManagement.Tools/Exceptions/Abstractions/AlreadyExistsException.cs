using AssociationManagement.Tools.Exceptions.Base;
using System.Net;

namespace AssociationManagement.Tools.Exceptions.Abstractions
{
    public class AlreadyExistsException : ServerException {
        public AlreadyExistsException(string message)
          : base(message, HttpStatusCode.Conflict) {
        }
    }
}
