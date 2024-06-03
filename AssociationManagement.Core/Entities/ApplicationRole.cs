using Microsoft.AspNetCore.Identity;
namespace AssociationManagement.Core.Entities;
public class ApplicationRole : IdentityRole {
    public DateTime RefreshTokenExpireTime { get; set; } = DateTime.Now;
}