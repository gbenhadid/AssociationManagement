using AssociationManagement.Application.Dtos.Auth;
using AssociationManagement.Core.Entities;
using System.Security.Claims;

namespace AssociationManagement.Application.Services.Abstractions
{
    public interface IAccountService {
        Task<List<Claim>> GetUserBasicClaims(string email);
        Task<string?> GetEmailByRefreshToken(string refreshToken);
        Task<bool> SetUserRefreshToken(string username, string refreshToken, TimeSpan expirationTime);
        string GenerateRefreshToken();
        string? GenerateJWTToken(List<Claim> claims);
        bool IsValidLoginRequest(ApplicationUser appUser, LoginRequest loginRequest);
    }
}
