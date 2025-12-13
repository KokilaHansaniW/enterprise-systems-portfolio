namespace NhsPortal.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string NhsNumber { get; set; } = string.Empty;
}
