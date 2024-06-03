using AssociationManagement.Tools.Exceptions.Base;
using System.Net;

namespace AssociationManagement.Tools.Exceptions.Abstractions
{
    public class BadRequestException : ServerException {
        public BadRequestException(string message)
            : base(message, HttpStatusCode.BadRequest) {
        }
    }
}
