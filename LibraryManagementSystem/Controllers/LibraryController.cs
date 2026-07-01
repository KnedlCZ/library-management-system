using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

public class LibraryController : Controller
{
    private readonly ILibraryService _libraryService;
    private readonly IConfiguration _configuration;
    
    public LibraryController(ILibraryService libraryService, IConfiguration configuration)
    {
        _libraryService = libraryService;
        _configuration = configuration;
    }
    
    public async Task<IActionResult> Index(string? message = null, string? errorMessage = null)
    {
        return View(await _libraryService.GetDashboardAsync(message, errorMessage));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBook(string title, string? isbn, int totalCopies, string authors)
    {
        try
        {
            await _libraryService.AddBookAsync(title, isbn, totalCopies, authors);
            return RedirectToAction(nameof(Index), new { message = "Kniha byla pridana." });
        }
        catch (ArgumentException exception)
        {
            return RedirectToAction(nameof(Index), new { errorMessage = exception.Message });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReader(string fullName, string? email)
    {
        try
        {
            await _libraryService.AddReaderAsync(fullName, email);
            return RedirectToAction(nameof(Index), new { message = "Ctenar byl pridan." });
        }
        catch (ArgumentException exception)
        {
            return RedirectToAction(nameof(Index), new { errorMessage = exception.Message });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(int bookId, int readerId)
    {
        var result = await _libraryService.BorrowBookAsync(bookId, readerId);
        return RedirectToAction(nameof(Index), new
        {
            message = result.Success ? result.Message : null,
            errorMessage = result.Success ? null : result.Message
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int loanId)
    {
        await _libraryService.ReturnLoanAsync(loanId);
        return RedirectToAction(nameof(Index), new { message = "Vypujcka byla ukoncena." });
    }

    public async Task<IActionResult> Overdue()
    {
        var allowedLoanDays = _configuration.GetValue<int>("Library:AllowedLoanDays");
        if (allowedLoanDays <= 0)
        {
            allowedLoanDays = 30;
        }

        var model = new OverdueReportViewModel
        {
            AllowedLoanDays = allowedLoanDays,
            Loans = await _libraryService.GetOverdueLoansAsync(allowedLoanDays)
        };

        return View(model);
    }
}
