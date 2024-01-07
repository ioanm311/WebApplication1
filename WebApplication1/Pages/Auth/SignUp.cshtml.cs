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

        private string HashPassword(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(salt);
            }
            return salt;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verifică dacă emailul există deja
            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.Email == User.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("User.Email", "Email already in use.");
                return Page();
            }

            byte[] salt = GenerateSalt();
            var user = new NewUserModel
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Password = HashPassword(User.Password, salt),
                Salt = Convert.ToBase64String(salt)
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Auth/Login");
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
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).*", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
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

        [Required]
        public string Salt { get; set; } // Noua coloană pentru salt
    }
}
