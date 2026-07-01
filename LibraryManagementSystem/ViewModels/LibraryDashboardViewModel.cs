using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels;

public class LibraryDashboardViewModel
{
    public IReadOnlyList<BookAvailabilityViewModel> Books { get; init; } = [];

    public IReadOnlyList<Reader> Readers { get; init; } = [];

    public IReadOnlyList<LoanViewModel> ActiveLoans { get; init; } = [];

    public string? Message { get; init; }

    public string? ErrorMessage { get; init; }
}

public class BookAvailabilityViewModel
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Isbn { get; init; }

    public int TotalCopies { get; init; }

    public int BorrowedCopies { get; init; }

    public int AvailableCopies => TotalCopies - BorrowedCopies;

    public string Authors { get; init; } = string.Empty;
}

public class LoanViewModel
{
    public int Id { get; init; }

    public string BookTitle { get; init; } = string.Empty;

    public string ReaderName { get; init; } = string.Empty;

    public DateTime BorrowedAt { get; init; }

    public int DaysBorrowed { get; init; }
}
