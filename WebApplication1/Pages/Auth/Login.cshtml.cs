using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public NewUserModel User { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Aici poți adăuga logica pentru salvarea utilizatorului nou
                // De exemplu, poți salva datele într-o bază de date

                // Dacă totul este în regulă, redirectează utilizatorul
                return RedirectToPage("/Index");
            }

            // Dacă sunt erori, afișează din nou pagina cu mesajele de validare
            return Page();
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