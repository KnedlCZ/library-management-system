namespace LibraryManagementSystem.Models;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    
    public int TotalCopies { get; set; }

    public ICollection<Author> Authors { get; set; } = new List<Author>();

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
