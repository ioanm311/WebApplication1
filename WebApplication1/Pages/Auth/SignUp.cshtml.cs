using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Pages.Auth
{
    public class SignUpModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SignUpModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SignUpViewModel User { get; set; } // Utilizați ViewModel pentru datele de legare

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new NewUserModel // Creați un nou obiect model care va merge în DB
                {
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email,
                    Password = HashPassword(User.Password) // Nu uitați să hash-uiți parola înainte de stocare
                };

                _context.users.Add(user); // Asigurați-vă că aveți "Users" ca DbSet în ApplicationDbContext
                await _context.SaveChangesAsync();

                // Dacă totul este în regulă, redirectează utilizatorul
                return RedirectToPage("/Auth/Login");
            }

            // Dacă sunt erori, afișează din nou pagina cu mesajele de validare
            return Page();
        }

        private string HashPassword(string password)
        {
            // Implementați o metodă de hash aici
            // Codul de hashing al parolei este omis pentru simplitate
            return password;
        }
    }

    public class SignUpViewModel // ViewModel pentru pagina de înscriere
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class NewUserModel // Modelul care va fi stocat în baza de date
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
