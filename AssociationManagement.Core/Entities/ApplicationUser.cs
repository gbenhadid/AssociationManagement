using Microsoft.AspNetCore.Identity;

namespace AssociationManagement.Core.Entities {
    public class ApplicationUser : IdentityUser {
        public string FullName { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpireTime { get; set; } = DateTime.Now;
    }
}
