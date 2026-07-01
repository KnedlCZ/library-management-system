namespace LibraryManagementSystem.ViewModels;

public class OverdueReportViewModel
{
    public int AllowedLoanDays { get; init; }

    public IReadOnlyList<LoanViewModel> Loans { get; init; } = [];
}
