namespace AssociationManagement.Core.Entities;
public class JwtSettings {
    public string Secret { get; set; } = "17f2507a9757e7a342da035f1f863126e0cfa9f57c0ac75917052a09efefc280";
    public string Issuer { get; set; } = "back";
    public int JWTExpirationTime { get; set; } = 3600;
    public int RefreshExpirationTime { get; set; } = 3600 * 24 * 7;
}
