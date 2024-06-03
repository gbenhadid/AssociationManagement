namespace AssociationManagement.Application.Dtos.Employees
{
    public abstract record EmployeeManipulationRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}
