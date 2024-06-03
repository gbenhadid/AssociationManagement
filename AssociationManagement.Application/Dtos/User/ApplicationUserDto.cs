using System.ComponentModel.DataAnnotations;

namespace Softylines.Compta.Application.Dtos;
public record ApplicationUserDto
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

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters long.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "ConfirmPassword is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "ConfirmPassword must be between 6 and 100 characters long.")]
    [Compare("Password",ErrorMessage = "It's different to the password field")]
    public string ConfirmPassword { get; set; }


}

