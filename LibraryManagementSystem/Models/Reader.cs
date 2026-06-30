namespace LibraryManagementSystem.Models;

public class Reader
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
