using AssociationManagement.Tools.Exceptions.Base;
using System.Net;

namespace AssociationManagement.Tools.Exceptions.Abstractions
{
    public class NotFoundException : ServerException {
        public NotFoundException(string message)
             : base(message, HttpStatusCode.NotFound) {


        }
    }
}
