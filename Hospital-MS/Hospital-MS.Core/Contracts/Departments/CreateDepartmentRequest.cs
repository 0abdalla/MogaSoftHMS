namespace Hospital_MS.Core.Contracts.Departments
{
    public class CreateDepartmentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
