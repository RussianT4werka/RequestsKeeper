using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RequestsKeeper.DB;
using RequestsKeeper.Model;
using RequestsKeeper.Tools;

namespace RequestsKeeper.Pages
{
    public class SecondPageModel : PageModel
    {
        private readonly User502Context user502Context;

        public List<Request> Requests { get; set; } = new List<Request>();
        public string Message { get; set; }

        public SecondPageModel(User502Context user502Context)
        {
            this.user502Context = user502Context;
        }


        public void OnGet(string handler)
        {
            User user = Session.GetVisitor(handler);
            Requests = user502Context.Requests.Include(s => s.Worker).ThenInclude(s => s.SubDivision).Include(s => s.TypeRequest).Include(s => s.Status).Include(s => s.Users).ToList();

        }
    }
}
