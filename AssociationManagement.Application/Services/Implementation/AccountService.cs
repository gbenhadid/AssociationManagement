using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AssociationManagement.Application.Dtos.Auth;
using AssociationManagement.Application.Services.Abstractions;
using AssociationManagement.Core.Entities;
using AssociationManagement.Tools.Exceptions.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace AssociationManagement.Application.Services.Implementation;
public class AccountService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtSettings> jwtSettings) : IAccountService {

    private readonly JwtSettings jwtSettings = jwtSettings.Value;

    private readonly UserManager<ApplicationUser> userManager = userManager;

    public async Task<List<Claim>?> GetUserBasicClaims(string email)
    {
        ApplicationUser? user = await this.userManager.FindByEmailAsync(email);
        if (user is null || user.LockoutEnd > DateTime.Now) {
            return null;
        }

        return new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new(ClaimTypes.Sid, user.Id)
            };
    }
    public async Task<string?> GetEmailByRefreshToken(string refreshToken)
    {
        ApplicationUser? user = await this.userManager.Users.FirstOrDefaultAsync(m =>
            m.RefreshToken == refreshToken && m.RefreshTokenExpireTime > DateTime.Now);

        return user?.Email;
    }
    public async Task<bool> SetUserRefreshToken(string username, string refreshToken, TimeSpan expirationTime)
    {
        ApplicationUser user = await this.userManager.FindByEmailAsync(username)
            ?? throw new NotFoundException("Error: user not found");

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpireTime = DateTime.Now.Add(expirationTime);

        IdentityResult result = await this.userManager.UpdateAsync(user);

        return result.Succeeded;

    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
    public string? GenerateJWTToken(List<Claim> claims)
    {
        var finalClaims = new List<Claim>();
        if (claims == null) return null;
        foreach (var claim in claims)
        {
            var type = claim.Type;
            if (claim.Type.Contains("/")) type = claim.Type.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
            finalClaims.Add(new Claim(type, claim.Value));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] secret = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = this.jwtSettings.Issuer,
            Audience = this.jwtSettings.Issuer,
            Subject = new ClaimsIdentity(finalClaims),
            Expires = DateTime.Now.AddMinutes(this.jwtSettings.JWTExpirationTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validates login request.
    /// </summary>
    /// <param name="appUser">The registered <see cref="ApplicationUser"/></param>
    /// <param name="loginRequest">The login request model.</param>
    /// <returns></returns>
    public bool IsValidLoginRequest(ApplicationUser? appUser, LoginRequest loginRequest) {
        if(appUser is null) {
            return false;
        }

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        if(string.IsNullOrEmpty(loginRequest.Password)) {
            return false;
        }

        PasswordVerificationResult hashVerificationResult = passwordHasher.VerifyHashedPassword(
            user: appUser, 
            hashedPassword: appUser.PasswordHash ?? string.Empty,
            providedPassword: loginRequest.Password);

        return hashVerificationResult is PasswordVerificationResult.Success;
    }
}
