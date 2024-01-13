using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;

public class ShopModel : PageModel
{
    public class Product
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ProductType { get; set; }
        // Alte proprietăți necesare
    }

    [BindProperty(SupportsGet = true)]
    public decimal? MinPrice { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal? MaxPrice { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SearchName { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ProductType { get; set; }

    private readonly ApplicationDbContext _context;

    public List<Product> Products { get; set; }
    public List<string> ProductTypes { get; set; }

    public ShopModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        IQueryable<Product> query = _context.products;

        ProductTypes = await GetProductTypesAsync();

        if (MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= MinPrice.Value);
        }

        if (MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= MaxPrice.Value);
        }

        if (!string.IsNullOrEmpty(SearchName))
        {
            query = query.Where(p => p.Name.Contains(SearchName));
        }

        if (!string.IsNullOrEmpty(ProductType))
        {
            query = query.Where(p => p.ProductType == ProductType);
        }

        Products = await query.ToListAsync();
    }

    public async Task<List<string>> GetProductTypesAsync()
    {
        return await _context.products
                             .Select(p => p.ProductType)
                             .Distinct()
                             .ToListAsync();
    }
}
