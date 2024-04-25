using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Logbook1.Pages
{
    public class Question1Model : PageModel
    {
        public string name { get; set; }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            this.name = Request.Form["name"];
        }
    }
}
