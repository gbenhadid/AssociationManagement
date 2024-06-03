namespace Softylines.Compta.Application.Dtos.User;

public record ApplicationUserReadDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}