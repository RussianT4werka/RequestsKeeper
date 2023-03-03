using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RequestsKeeper.DB;
using RequestsKeeper.Tools;
using System.Net;

namespace RequestsKeeper.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly User502Context user502Context;

        public IndexModel(ILogger<IndexModel> logger, User502Context user502Context)
        {
            _logger = logger;
            this.user502Context = user502Context;
        }

        public IActionResult OnPost(string login, string password)
        {
            if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                Message = "Необходимо заполнить поля для авторизации";
                return null;
            }
            var user = user502Context.Users.Include(x => x.Visitors).FirstOrDefault(x => x.Login == login && x.Password == Hash.HashPass(password));
            if(user == null)
            {
                Message = "Неверный логин или пароль";
                return null;
            }
            else
            {
                return RedirectToPage("SecondPage", Session.CreateSession(user));
            }
        }



    }
}