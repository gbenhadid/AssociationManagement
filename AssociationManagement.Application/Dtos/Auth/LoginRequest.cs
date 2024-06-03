using System.ComponentModel.DataAnnotations;
namespace AssociationManagement.Application.Dtos.Auth
{
    public class LoginRequest
    {
        [Required] public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}
