using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RequestsKeeper.DB;
using RequestsKeeper.Model;
using RequestsKeeper.Tools;

namespace RequestsKeeper.Pages
{
    public class CreateGroupRequestModel : PageModel
    {
        [BindProperty]
        public Request Request { get; set; } = new Request();

        public List<Visitor> Visitors { get; set; } = new List<Visitor>();

        [BindProperty]
        public Visitor Visitor { get; set; } = new Visitor();

        private User authUser;
        private readonly User502Context user502Context;
        private readonly IWebHostEnvironment appEnvironment;

        public SelectList SubDivisionList { get; set; }

        public SelectList WorkerList { get; set; }

        [BindProperty]
        public int SubDivisionId { get; set; }

        [BindProperty]
        public int WorkerId { get; set; }
        public string Message { get; private set; }
        
        public CreateGroupRequestModel(User502Context user502Context, IWebHostEnvironment appEnvironment)
        {
            var subdivisions = user502Context.SubDivisions.ToList();
            SubDivisionList = new SelectList(subdivisions, nameof(SubDivision.Id), nameof(SubDivision.Name));
            WorkerList = new SelectList(user502Context.Workers.Where(s => s.SubDivisionId == subdivisions.First().Id).ToList(), nameof(Worker.Id), nameof(Worker.Surname));
            this.user502Context = user502Context;
            this.appEnvironment = appEnvironment;
        }
        public void OnGet(string handler)
        {
            authUser = Session.GetUser(handler);
            Session.RemoveVisitor(handler);
        }

        public IActionResult OnPost(string handler, string filldata, string addvisitor, string cleanform)
        {
            
            authUser = Session.GetUser(handler);
            WorkerList = new SelectList(user502Context.Workers.Where(s => s.SubDivisionId == SubDivisionId).ToList(), nameof(Worker.Id), nameof(Worker.Surname));
            if(cleanform != null)
            {
                Visitor = new Visitor();
                Request = new Request();
                Session.RemoveVisitor(handler);
            }
            Visitors = Session.ReturnListVisitor(handler);
            if (addvisitor != null)
            {
                if (string.IsNullOrEmpty(Visitor.Surname)
                || string.IsNullOrEmpty(Visitor.Name)
                || string.IsNullOrEmpty(Visitor.Email)
                || !Visitor.Email.Contains('@')
                || (DateTime.Now - Visitor.DoB).Days / 365 <= 16
                || string.IsNullOrEmpty(Visitor.PassportSeries)
                || Visitor.PassportSeries.Length != 4
                || string.IsNullOrEmpty(Visitor.PassportNumber)
                || Visitor.PassportNumber.Length != 6
                || string.IsNullOrEmpty(Visitor.Note))
                {
                    Message = "ѕол€ заполнены неверно";

                    return null;
                }
                Visitor.UsersId = authUser.Id;
                Visitor.Number = Visitors.Count+1;
                Session.CreateGroupVisitor(handler, Visitor);
                Visitor = new Visitor();
                Visitors = Session.ReturnListVisitor(handler);
                return null;
            }
            if (filldata != null)
            {
                if (Request.StartDate.Day <= DateTime.Now.Day
                || Request.StartDate.Day > DateTime.Now.Day + 15
                || Request.FinishDate.Day < Request.StartDate.Day
                || Request.FinishDate.Day > Request.StartDate.Day + 15
                || string.IsNullOrEmpty(Request.TargetVisit)
                || WorkerId == 0
                || base.Request.Form.Files.FirstOrDefault(s => s.ContentType == "application/pdf") == null
                || Visitors.Count < 5)
                {
                    Message = "ѕол€ за€вки заполнены неверно или вовсе не заполнены";
                    return null;
                }

                Request.UsersId = authUser.Id;
                Request.WorkerId = WorkerId;
                Request.StatusId = 1;
                Request.TypeRequestId = 2;
                user502Context.Requests.Add(Request);
                var lastIdRequest = user502Context.Requests.ToList().Last().Id;
                user502Context.Visitors.AddRange(Visitors);
                var visitors = user502Context.Visitors.ToList().TakeLast(Visitors.Count);
                foreach(var visitor in visitors)
                {
                    user502Context.VisitorsRequests.Add(new VisitorsRequest { RequestId = lastIdRequest, VisitorsId = visitor.Id });
                }

                User user = user502Context.Users.ToList().First(s => s.Id == authUser.Id);
                user502Context.Entry(user).CurrentValues.SetValues(authUser);
                
                user502Context.SaveChanges();

                return RedirectToPage("SecondPage", handler);
            }
            return null;

        }
        private byte[] GetBytesFromFile(IFormFile file)
        {
            byte[] data = null;
            using (var fs = file.OpenReadStream())
            {
                data = new byte[fs.Length];
                using (MemoryStream ms = new MemoryStream(data))
                {
                    fs.CopyTo(ms);
                }
            }
            return data;
        }
    }
}
