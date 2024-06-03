using System.ComponentModel.DataAnnotations;

namespace AssociationManagement.Application.Dtos.User {
    public record ApplicationUserChangePasswordDto {
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be between 6 and 100 characters long.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Old password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Old password must be between 6 and 100 characters long.")]
        public string OldPassword { get; set; }
    }
}
