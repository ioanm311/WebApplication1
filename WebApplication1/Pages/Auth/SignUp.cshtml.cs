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
        public NewUserModel User { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Hash the password - aici ar trebui să utilizați o funcție de hashing mai sigură
                using var hmac = new HMACSHA512();
                var user = new NewUserModel
                {
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email,
                    Password = User.Password
                };

                _context.users.Add(user);
                await _context.SaveChangesAsync();

                // Dacă totul este în regulă, redirectează utilizatorul
                return RedirectToPage("/Index");
            }

            // Dacă sunt erori, afișează din nou pagina cu mesajele de validare
            return Page();
        }
    }

    public class NewUserModel
    {
        public int Id { get; set; } // Asigurați-vă că adăugați o coloană corespunzătoare în baza de date
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
}