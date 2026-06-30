namespace LibraryManagementSystem.Models;

public class Loan
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public int ReaderId { get; set; }

    public Reader Reader { get; set; } = null!;

    public DateTime BorrowedAt { get; set; }

    public DateTime? ReturnedAt { get; set; }
}
