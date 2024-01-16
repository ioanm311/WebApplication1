using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserLoginModel User { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _context.users.FirstOrDefault(u => u.Email == User.Email);
            if (user == null)
            {
                // Utilizatorul nu există
                ModelState.AddModelError("User.Email", "User does not exist. Please sign up.");
                return Page();
            }
            else if (!VerifyPasswordHash(User.Password, user.Password, user.Salt))
            {
                // Parola este greșită
                ModelState.AddModelError("User.Password", "Invalid credentials.");
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            return RedirectToPage("/Shop");
        }

        private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA256(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == storedHash;
            }
        }
    }

    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
