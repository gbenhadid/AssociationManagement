using AssociationManagement.Tools.Exceptions.Base;
using System.Net;

namespace AssociationManagement.Tools.Exceptions.Abstractions
{
    public class ConflictException : ServerException{
        public ConflictException(string message)
            : base(message, HttpStatusCode.Conflict) {
        }
    }
}
