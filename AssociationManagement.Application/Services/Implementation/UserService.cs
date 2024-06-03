using AutoMapper;
using Microsoft.AspNetCore.Identity;
using AssociationManagement.Application.Services.Abstractions;
using AssociationManagement.Core.Entities;
using AssociationManagement.Tools.Exceptions.Abstractions;
using AssociationManagement.Tools.Logging;
using Softylines.Compta.Application.Dtos;

namespace Softylines.Compta.Application.Services.Implementation;
public class UserService(
    UserManager<ApplicationUser> _userManager,
    IMapper _mapper,
    ILoggerManager _logger) : IUserService {

    ///<inheritdoc/>
    public async Task<ApplicationUser> CreateUser(ApplicationUserDto userCreationRequest) {
        await ValidateAsync(userCreationRequest);

        var userModel = _mapper.Map<ApplicationUser>(userCreationRequest);

        await ValidateCreateAction(userModel, userCreationRequest.Password);

        return userModel;
    }

    /// <summary>
    /// Validates user attributes.
    /// </summary>
    /// <param name="userCreationRequest"></param>
    private async Task ValidateAsync(ApplicationUserDto userCreationRequest) {
        await ValidateByNameAsync(userCreationRequest);

        await ValidateByEmailAsync(userCreationRequest);
    }

    /// <summary>
    /// validates user by name.
    /// </summary>
    /// <param name="userCreationRequest">the user to be created.</param>
    /// <exception cref="AlreadyExistsException">throw already exists exception if user exists.</exception>
    private async Task ValidateByNameAsync(ApplicationUserDto userCreationRequest) {
        ApplicationUser? maybeUser = await _userManager.FindByNameAsync(userCreationRequest.UserName);

        if(maybeUser is not null) {
            _logger.LogError("user name already exists");

            throw new AlreadyExistsException("user name already exists");
        }
    }

    /// <summary>
    /// Validates user by email.
    /// </summary>
    /// <param name="userCreationRequest"></param>
    /// <returns></returns>
    /// <exception cref="AlreadyExistsException">throw already exist exception if this user exist</exception>
    private async Task ValidateByEmailAsync(ApplicationUserDto userCreationRequest) {
        ApplicationUser? maybeUser = await _userManager.FindByEmailAsync(userCreationRequest.Email);

        if(maybeUser is not null) {
            _logger.LogError("email is already exist");

            throw new AlreadyExistsException("email is already exist");
        }
    }

    /// <summary>
    /// validate the add user action
    /// </summary>
    /// <param name="user">the user model</param>
    /// <param name="password">the password of the user</param>
    /// <exception cref="BadRequestException">throw bad request if any error occures</exception>
    private async Task ValidateCreateAction(ApplicationUser user, string password) {
        IdentityResult createResult = await _userManager.CreateAsync(user, password);

        if(!createResult.Succeeded) {
            string errors = string.Join(", ", createResult.Errors.Select(x => x.Description));
            _logger.LogError(errors);

            throw new BadRequestException(errors);
        }
    }
}
