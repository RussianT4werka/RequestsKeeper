using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RequestsKeeper.DB;
using RequestsKeeper.Model;
using RequestsKeeper.Tools;

namespace RequestsKeeper.Pages
{
    public class CreatePersonalRequestModel : PageModel
    {
        [BindProperty]
        public Request Request { get; set; } = new Request();

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
        [BindProperty]
        public string ImagePath { get; set; }
        [BindProperty]
        public string PhotoBytes { get; set; }

        public CreatePersonalRequestModel(User502Context user502Context, IWebHostEnvironment appEnvironment)
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
        }

        public IActionResult OnPost(string handler, string filldata)
        {
            if (filldata == null)
            {
                WorkerList = new SelectList(user502Context.Workers.Where(s => s.SubDivisionId == SubDivisionId).ToList(), nameof(Worker.Id), nameof(Worker.Surname));
                var uploadedFile = base.Request.Form.Files.FirstOrDefault(s => s.ContentType == "image/jpeg");
                if (uploadedFile != null)
                {
                    // путь к папке Files
                    string path = "/Images/" + uploadedFile.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        uploadedFile.CopyTo(fileStream);
                    }
                    ImagePath = Url.Content(path);
                }
                return null;
            }
            authUser = Session.GetUser(handler);
            if (string.IsNullOrEmpty(Visitor.Surname) 
                || string.IsNullOrEmpty(Visitor.Name) 
                || string.IsNullOrEmpty(Visitor.Email) 
                || !Visitor.Email.Contains('@') 
                || (DateTime.Now-Visitor.DoB).Days/365<=16 
                || string.IsNullOrEmpty(Visitor.PassportSeries)
                || Visitor.PassportSeries.Length != 4 
                || string.IsNullOrEmpty(Visitor.PassportNumber)
                || Visitor.PassportNumber.Length != 6
                || string.IsNullOrEmpty(Visitor.Note)
                || Request.StartDate.Day<=DateTime.Now.Day
                || Request.StartDate.Day>DateTime.Now.Day+15
                || Request.FinishDate.Day<Request.StartDate.Day
                || Request.FinishDate.Day>Request.StartDate.Day+15
                || string.IsNullOrEmpty(Request.TargetVisit)
                || WorkerId == 0
                || base.Request.Form.Files.FirstOrDefault(s => s.ContentType == "application/pdf") == null)
            {
                Message = "Поля заполнены неверно";

                return null; 
            }

            if (ImagePath != null)
            {
                using (var fs = new FileStream(appEnvironment.WebRootPath + ImagePath, FileMode.Open))
                {
                        Visitor.Photo = new byte[fs.Length];
                        using (MemoryStream ms = new MemoryStream(Visitor.Photo))
                        {
                            fs.CopyTo(ms);
                        }
                }
            }
            var file = base.Request.Form.Files.First(s => s.ContentType == "application/pdf");
            Visitor.ScanPassport = GetBytesFromFile(file);
            Request.UsersId = authUser.Id;
            Request.WorkerId = WorkerId;
            Request.StatusId = 1;
            Request.TypeRequestId = 1;
            authUser.Visitors.Add(Visitor);
            User user = user502Context.Users.ToList().First(s => s.Id == authUser.Id);
            user502Context.Entry(user).CurrentValues.SetValues(authUser);
            user502Context.Requests.Add(Request);
            var lastIdRequest = user502Context.Requests.ToList().Last().Id;
            var lastIdVisitor = user502Context.Visitors.ToList().Last().Id;
            var visitorsRequest = new VisitorsRequest();
            visitorsRequest.RequestId = lastIdRequest;
            visitorsRequest.VisitorsId = lastIdVisitor;
            user502Context.VisitorsRequests.Add(visitorsRequest);
            user502Context.SaveChanges();
            return RedirectToPage("SecondPage", handler);
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
