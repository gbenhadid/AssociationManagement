using AssociationManagement.Tools.Exceptions.Base;
using System.Net;

namespace AssociationManagement.Tools.Exceptions.Abstractions
{
    public class UnauthorizedException : ServerException {
        public UnauthorizedException(string message, HttpStatusCode statusCode = HttpStatusCode.Unauthorized) : base(message, statusCode) {
        }
    }
}
