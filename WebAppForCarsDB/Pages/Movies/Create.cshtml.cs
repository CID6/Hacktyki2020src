using EFCarsDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebAppForCarsDB.Pages.Movies
{
    public class CreateModel : PageModel
    {
        //all firebase code is in movieCreate.js

        private readonly EFCarsDB.Data.WebAppForCarsDBContext _context;



        public CreateModel(EFCarsDB.Data.WebAppForCarsDBContext context)
        {
            _context = context;
        }



        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FirebaseMovie Movie { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {

            }

            return RedirectToPage("./Index");
        }
    }
}
