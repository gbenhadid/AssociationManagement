using AssociationManagement.Tools.Exceptions.Base;
using System.Net;

namespace AssociationManagement.Tools.Exceptions.Abstractions
{
    public class ForbiddenException : ServerException {
        public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden) {
        }
    }
}
