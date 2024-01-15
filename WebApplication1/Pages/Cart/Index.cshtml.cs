
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using static ShopModel;

namespace WebApplication1.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger; // Logger
        public decimal TotalPrice { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }


        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                ShoppingCartItems = _context.ShoppingCart
                            .Where(cart => cart.UserId == userId.Value)
                            .Select(cartItem => new ShoppingCartItem
                            {
                                ShoppingCartId = cartItem.ShoppingCartId,
                                ProductName = cartItem.ProductName,
                                Quantity = cartItem.Quantity,
                                Price = cartItem.Price
                            }).ToList();
                TotalPrice = ShoppingCartItems.Sum(item => item.Price * item.Quantity);
                return Page();
            }
            else
            {
                // Setează TotalPrice la 0 dacă nu există un userId valid sau dacă nu sunt articole în coș
                TotalPrice = 0;
                return RedirectToPage("/Auth/Login");
            }
        }

        public async Task<IActionResult> OnPostStergereItemAsync(int itemId)
        {
           

            var item = await _context.ShoppingCart.FindAsync(itemId);
            if (item != null)
            {
                _context.ShoppingCart.Remove(item);
                await _context.SaveChangesAsync();
                
            }

            await OnGetAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApplyDiscountAsync(string discountCode)
        {
            if (string.IsNullOrWhiteSpace(discountCode))
            {
                // Dacă discountCode este null, gol sau spații albe, doar reîncarcă pagina.
                await OnGetAsync();
                return Page();
            }

            await OnGetAsync(); // Reîncarcă elementele din coș

            if (ShoppingCartItems.Any())
            {
                switch (discountCode.ToUpper())
                {
                    case "B5":
                        TotalPrice *= 0.95m; // Reducere de 5%
                        break;
                    case "B10":
                        TotalPrice *= 0.90m; // Reducere de 10%
                        break;
                    case "B15":
                        TotalPrice *= 0.85m; // Reducere de 15%
                        break;
                    case "B25":
                        TotalPrice *= 0.80m; // Reducere de 15%
                        break;
                    case "B35":
                        TotalPrice *= 0.75m; // Reducere de 15%
                        break;
                    default:
                        ErrorMessage = "Cod invalid";
                        break;
                }
            }

            return Page();
        }
    }
}
