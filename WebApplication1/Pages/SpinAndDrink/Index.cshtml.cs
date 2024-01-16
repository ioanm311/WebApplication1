using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplication1.Data; // Înlocuiește cu numele real al namespace-ului tău

public class SaveSpinResultRequest
{
    public string Result { get; set; }
}

public class SpinResultType
{
    public int Id { get; set; } // Un identificator unic pentru fiecare intrare
    public int UserId { get; set; } // ID-ul utilizatorului care a efectuat rotirea
    public string SpinResult { get; set; } // Rezultatul rotirii roții (ex: '20%', 'Free', 'Lose', etc.)
    public DateTime SpinDate { get; set; } // Data și ora la care a avut loc rotirea

    public SpinResultType()
    {
        SpinDate = DateTime.Now; // Setează implicit data și ora curentă la crearea unei noi instanțe
    }
}

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context; // Înlocuiește cu tipul real al contextului tău
    private readonly ILogger<IndexModel> _logger;
    public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> OnPostSaveSpinResult([FromBody] string result) // Tipul parametrului schimbat în string
    {
        _logger.LogInformation("Received spin result: {Result}", result); // Loghează rezultatul primit
        var userId = HttpContext.Session.GetInt32("UserId");
        
        if (userId == null)
        {
            // Tratează cazul în care ID-ul utilizatorului nu este disponibil
            return new JsonResult(new { success = false, message = "User not logged in." });
        }

        try
        {
            var spinResult = new SpinResultType
            {
                UserId = userId.Value,
                SpinResult = result
            };

            _context.SpinResults.Add(spinResult);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true }); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving spin result."); // Loghează orice excepție care apare

            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}
