using System.ComponentModel.DataAnnotations;

namespace Softylines.Compta.Application.Dtos.User;

public record UpdateApplicationUserDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 100 characters.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(20, ErrorMessage = "Username cannot be longer than 100 characters.")]
    public string UserName { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format.")]
    [StringLength(13, ErrorMessage = "Phone number cannot be longer than 20 digits.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Enterprise ID is required.")]
    [EmailAddress]
    public string Email { get; set; }
}