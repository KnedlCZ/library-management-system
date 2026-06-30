namespace LibraryManagementSystem.Services;

public interface ILibraryService
{ 
    Task AddBookAsync(string title, string? isbn, int totalCopies, string authors); 
    Task AddReaderAsync(string fullName, string? email); 
    Task<(bool Success, string Message)> BorrowBookAsync(int bookId, int readerId); 
    Task ReturnLoanAsync(int loanId); 
    Task SeedAsync();
}
