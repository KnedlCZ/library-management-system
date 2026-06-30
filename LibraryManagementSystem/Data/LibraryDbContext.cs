using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Reader> Readers => Set<Reader>();

    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(author => author.Name).HasMaxLength(200).IsRequired();
            entity.HasIndex(author => author.Name).IsUnique();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(book => book.Title).HasMaxLength(300).IsRequired();
            entity.Property(book => book.TotalCopies).IsRequired();
            entity.ToTable(table => table.HasCheckConstraint("CK_Books_TotalCopies_Positive", "\"TotalCopies\" > 0"));

            entity
                .HasMany(book => book.Authors)
                .WithMany(author => author.Books)
                .UsingEntity("BookAuthors");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.Property(reader => reader.FullName).HasMaxLength(200).IsRequired();
            entity.Property(reader => reader.Email).HasMaxLength(256);
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(loan => loan.BorrowedAt).IsRequired();
            entity.HasIndex(loan => new { loan.BookId, loan.ReturnedAt });
            entity.HasOne(loan => loan.Book).WithMany(book => book.Loans).HasForeignKey(loan => loan.BookId);
            entity.HasOne(loan => loan.Reader).WithMany(reader => reader.Loans).HasForeignKey(loan => loan.ReaderId);
        });
    }
}
