using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Logbook1.Pages
{
    public class Question2Model : PageModel
    {
        public float toConvert { get; set; }
        public string result { get; set; }
        public void OnGet()
        {
        }


        public void OnPost()
        {
            var exchangeRate = 1.17;
            this.result = (float.Parse(Request.Form["toConvert"]) * exchangeRate).ToString();
        }
    }
}



