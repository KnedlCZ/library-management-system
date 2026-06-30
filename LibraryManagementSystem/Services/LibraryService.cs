using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class LibraryService : ILibraryService
{
    private readonly LibraryDbContext _dbContext;

    public LibraryService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddBookAsync(string title, string? isbn, int totalCopies, string authors)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Nazev knihy je povinny.", nameof(title));
        }

        if (totalCopies < 1)
        {
            throw new ArgumentException("Pocet kusu musi byt alespon 1.", nameof(totalCopies));
        }

        var authorNames = authors
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (authorNames.Count == 0)
        {
            throw new ArgumentException("Zadejte alespon jednoho autora.", nameof(authors));
        }

        var existingAuthors = await _dbContext.Authors
            .Where(author => authorNames.Contains(author.Name))
            .ToListAsync();

        var book = new Book
        {
            Title = title.Trim(),
            TotalCopies = totalCopies
        };

        foreach (var authorName in authorNames)
        {
            var author = existingAuthors.FirstOrDefault(existing =>
                             existing.Name.Equals(authorName, StringComparison.OrdinalIgnoreCase))
                         ?? new Author { Name = authorName };

            book.Authors.Add(author);
        }

        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddReaderAsync(string fullName, string? email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Jmeno ctenare je povinne.", nameof(fullName));
        }

        _dbContext.Readers.Add(new Reader
        {
            FullName = fullName.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim()
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<(bool Success, string Message)> BorrowBookAsync(int bookId, int readerId)
    {
        var book = await _dbContext.Books
            .Include(item => item.Loans)
            .FirstOrDefaultAsync(item => item.Id == bookId);

        if (book is null)
        {
            return (false, "Kniha nebyla nalezena.");
        }

        var readerExists = await _dbContext.Readers.AnyAsync(reader => reader.Id == readerId);
        if (!readerExists)
        {
            return (false, "Ctenar nebyl nalezen.");
        }

        var borrowedCopies = book.Loans.Count(loan => loan.ReturnedAt == null);
        if (borrowedCopies >= book.TotalCopies)
        {
            return (false, "Vsechny kusy teto knihy jsou aktualne vypujcene.");
        }

        _dbContext.Loans.Add(new Loan
        {
            BookId = bookId,
            ReaderId = readerId,
            BorrowedAt = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();
        return (true, "Vypujcka byla vytvorena.");
    }

    public async Task ReturnLoanAsync(int loanId)
    {
        var loan = await _dbContext.Loans.FirstOrDefaultAsync(item => item.Id == loanId);
        if (loan is null || loan.ReturnedAt is not null)
        {
            return;
        }

        loan.ReturnedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedAsync()
    {
        if (await _dbContext.Books.AnyAsync())
        {
            return;
        }

        var capek = new Author { Name = "Karel Capek" };
        var orwell = new Author { Name = "George Orwell" };
        var gaiman = new Author { Name = "Neil Gaiman" };
        var pratchett = new Author { Name = "Terry Pratchett" };

        _dbContext.Books.AddRange(
            new Book { Title = "R.U.R.", TotalCopies = 2, Authors = [capek] },
            new Book { Title = "1984", TotalCopies = 1, Authors = [orwell] },
            new Book { Title = "Good Omens", TotalCopies = 1, Authors = [gaiman, pratchett] });

        _dbContext.Readers.AddRange(
            new Reader { FullName = "Jana Novakova", Email = "jana@example.com" },
            new Reader { FullName = "Petr Svoboda", Email = "petr@example.com" });

        await _dbContext.SaveChangesAsync();
    }
}