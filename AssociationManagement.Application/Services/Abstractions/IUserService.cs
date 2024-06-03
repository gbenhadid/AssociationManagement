using AssociationManagement.Core.Entities;
using Softylines.Compta.Application.Dtos;
namespace AssociationManagement.Application.Services.Abstractions {
    public interface IUserService {

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="userCreationRequest">Defines the user instance.</param>
        /// <returns>A user if exists</returns>
        /// <exception cref="AlreadyExistsException"></exception>
        /// <exception cref="BadRequestException"></exception>
        Task<ApplicationUser> CreateUser(ApplicationUserDto user);
    }
}
