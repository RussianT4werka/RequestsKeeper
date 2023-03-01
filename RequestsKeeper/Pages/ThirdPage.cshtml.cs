using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RequestsKeeper.DB;
using RequestsKeeper.Model;
using RequestsKeeper.Tools;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RequestsKeeper.Pages
{
    public class ThirdPageModel : PageModel
    {
        public string Message { get; set; }
        public string Exist { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        private ILogger<ThirdPageModel> _logger;
        private readonly User502Context user502Context;

        public ThirdPageModel(ILogger<ThirdPageModel> logger, User502Context user502Context)
        {
            _logger = logger;
            this.user502Context = user502Context;
        }

        bool IsValid(string email)
        {
            try
            {
                new MailAddress(email);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public IActionResult OnPost(string email, string password)
        {
            
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Message = "Необходимо заполнить поля для регистрации";
                return null;
            }
            var user = user502Context.Users.FirstOrDefault(s => s.Email == email);
            if(user == null && IsValid(email))
            {
                user502Context.Users.Add(new User {Email = email, Login = email.Split('@')[0], Password = Hash.HashPass(password) });
                user502Context.SaveChanges();
                return RedirectToPage("VisitForm", Session.CreateSession(user));
            }
            else
            {
                Exist = "Пользователь с такой электронной почтой уже существует";
                return null;
            }
        }

        
    }
}
