using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RequestsKeeper.Model;
using RequestsKeeper.Tools;

namespace RequestsKeeper.Pages
{
    public class CreateRequestModel : PageModel
    {
        public string UserSession { get; set; }
        public void OnGet(string handler)
        {
            User user = Session.GetUser(handler);
            UserSession = handler;
        }

    }
}
