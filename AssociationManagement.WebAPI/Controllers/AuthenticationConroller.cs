using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AssociationManagement.Application.Dtos.Auth;
using AssociationManagement.Application.Services.ServicesManager;
using AssociationManagement.Core.Entities;
using AssociationManagement.Tools.Exceptions.Abstractions;
using Softylines.Compta.Application.Dtos;

namespace AssociationManagement.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]

public class AuthenticationController(
    IOptions<JwtSettings> jwtSettings,
    IServiceManager service,
    UserManager<ApplicationUser> userManager) : ControllerBase {
    private readonly IServiceManager _service = service;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly int _refreshTokenExpiration = jwtSettings.Value.RefreshExpirationTime;

    /// <summary>
    /// refresh the access token 
    /// </summary>
    /// <param name="refreshTokenInput">the old refresh token</param>
    /// <returns>A new access token</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenInput refreshTokenInput) {
        var refreshToken = refreshTokenInput.RefreshToken;
        var userEmail = await _service.AccountService.GetEmailByRefreshToken(refreshToken);

        if(string.IsNullOrWhiteSpace(userEmail))
            throw new UnauthorizedException("");

        var claims = await _service.AccountService.GetUserBasicClaims(userEmail);
        var jwtToken = _service.AccountService.GenerateJWTToken(claims);
        var newRefreshToken = _service.AccountService.GenerateRefreshToken();
        var updateSuccessfully = await _service.AccountService.SetUserRefreshToken(
            userEmail,
            newRefreshToken,
            TimeSpan.FromMinutes(_refreshTokenExpiration));

        if(!updateSuccessfully)
            throw new UnauthorizedException("");

        return Ok(new {
            JwtToken = jwtToken,
            RefreshToken = newRefreshToken
        });
    }

    /// <summary>
    /// allows anonymous user to login
    /// </summary>
    /// <param name="loginRequest"> login model </param>
    /// <returns>ok if no problem occured</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var claims = await _service.AccountService.GetUserBasicClaims(loginRequest.Email);
        if(claims is null) {
            throw new NotFoundException("Invalid E-mail and/or password");
        }

        var appUser = await _userManager.FindByEmailAsync(loginRequest.Email);

        var isValid = _service.AccountService.IsValidLoginRequest(appUser, loginRequest);
        if(!isValid) {
            throw new UnauthorizedException("Invalid E-mail and/or password");
        }

        var token = _service.AccountService.GenerateJWTToken(claims);
        var refreshToken = _service.AccountService.GenerateRefreshToken();
        var success = await _service.AccountService.SetUserRefreshToken(loginRequest.Email, refreshToken,
            TimeSpan.FromMinutes(_refreshTokenExpiration));

        if(!success) {
            throw new NotFoundException("Cannot update refresh token");
        }

        return Ok(new {
            JwtToken = token,
            RefreshToken = refreshToken
        });

    }


    /// <summary>
    /// allows anonymous user to register to the application.
    /// </summary>
    /// <param name="applicationUserRegisterRequest">user registration request model.</param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] ApplicationUserDto applicationUserRegisterRequest) {
        if(!ModelState.IsValid) return BadRequest();

        await _service.UserService.CreateUser(applicationUserRegisterRequest);

        return Ok();
    }
}
