using System;
using System.IO;
using System.Linq;
using CSharpProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Controllers
{
    public class OrganizationController : Controller
    {
        private MyContext _context;
        private IWebHostEnvironment hostingEnvironment;

        public OrganizationController(MyContext context, IWebHostEnvironment hostingEnvironment){
            _context=context;
            this.hostingEnvironment = hostingEnvironment;
        }
        
        [HttpGet("orgnaization/{id}")]
        public IActionResult Index(int id){
            ViewBag.LoggedOrg= HttpContext.Session.GetInt32("OrgId");
            int? idUser= HttpContext.Session.GetInt32("UserId");

            if (HttpContext.Session.GetInt32("OrgId") != null){
                Organization orgNav = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = orgNav;
            }else{
                Organization orgNav = new Organization();
                ViewBag.OrgObj = orgNav;
            }
            if(HttpContext.Session.GetInt32("UserId")!=null){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
            }

                // Organization orgNav = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                // ViewBag.OrgObj = orgNav;
                Organization org = _context.Organizations
                .Include(o=>o.Projects)
                .FirstOrDefault(o=>o.OrganizationId==id);

                ViewBag.Active =_context.Works
                .Include(w=>w.CreatedBy)
                .OrderByDescending(w=>w.NumberOfVolunteers)
                .Where(w=>w.EndDate>=DateTime.Now && w.CreatedBy.OrganizationId==id)
                .Include(w=>w.Workers)
                .Take(3).ToList();
                return View(org);
        }

        [HttpGet("org/reg")]
        public IActionResult Register(){
            if(HttpContext.Session.GetInt32("OrgId")==null){
                return View();
            } else {
                return Redirect($"/orgnaization/{HttpContext.Session.GetInt32("OrgId")}");
            }
        }

        [HttpPost("org/create")]
        public IActionResult RegisterForm(Organization NewOrg){
            if(ModelState.IsValid){
                if(_context.Organizations.Any(u=> u.Email==NewOrg.Email)){
                    ModelState.AddModelError("Email","This Email is already registered");
                    return View("Register");
                } else {
                    PasswordHasher<Organization> passwordHasher = new PasswordHasher<Organization>();
                    NewOrg.password = passwordHasher.HashPassword(NewOrg,NewOrg.password);
                    _context.Organizations.Add(NewOrg);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("OrgId",NewOrg.OrganizationId);
                    return Redirect($"/orgnaization/{NewOrg.OrganizationId}");
                }
            } else {
                return View("Register");
            }
        }

        [HttpGet("org/login")]
        public IActionResult Login(){
            if(HttpContext.Session.GetInt32("OrgId")==null){
            return View();
            } else {
                return Redirect($"/orgnaization/{HttpContext.Session.GetInt32("OrgId")}");
            }
        }

        [HttpPost("org/login")]
        public IActionResult LoginForm(Login LoggedUser){
            if (ModelState.IsValid){
                Organization DbOrgnaizatoin = _context.Organizations.FirstOrDefault(u => u.Email==LoggedUser.Email);
                if (DbOrgnaizatoin == null){
                    ModelState.AddModelError("Email","Email or Password is Invalid");
                    return View("Login");
                } else {
                    PasswordHasher<Login> passwordHasher = new PasswordHasher<Login>();
                    var result = passwordHasher.VerifyHashedPassword(LoggedUser,DbOrgnaizatoin.password,LoggedUser.Password);
                    if (result==0){
                        ModelState.AddModelError("Email","Email or Password is Invalid");
                        return View("Login");
                    } else {

                        HttpContext.Session.SetInt32("OrgId",DbOrgnaizatoin.OrganizationId);
                        return Redirect($"/orgnaization/{DbOrgnaizatoin.OrganizationId}");
                    }
                }
            } else {
                return View("Login");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet("create/project")]
        public IActionResult NewProject(){
            if(HttpContext.Session.GetInt32("OrgId")!=null){
                Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
                return View();
            } else {
                
                return RedirectToAction("Login");
            } 
        }
        [HttpPost("create/project")]
        public IActionResult NewProjectForm(WorkViewModels WorkModel){
            int? id = HttpContext.Session.GetInt32("OrgId");
            if (ModelState.IsValid){
                string fileName = null;
                if(WorkModel.Picture != null){
                   string uploadFolder =  Path.Combine(hostingEnvironment.WebRootPath, "image");
                    fileName = Guid.NewGuid().ToString() + "_" + WorkModel.Picture.FileName;
                    string filePath = Path.Combine(uploadFolder,fileName);
                    WorkModel.Picture.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Work NewWork = new Work(); 
                    NewWork.OrganizationId=(int)id;
                    NewWork.Name = WorkModel.Name;
                    NewWork.Description = WorkModel.Description;
                    NewWork.Location = WorkModel.Location;
                    NewWork.StartDate = WorkModel.StartDate;
                    NewWork.EndDate = WorkModel.EndDate;
                    NewWork.Country = WorkModel.Country;
                    NewWork.City = WorkModel.City;
                    NewWork.NumberOfHours = WorkModel.NumberOfHours;
                    NewWork.NumberOfVolunteers = WorkModel.NumberOfVolunteers;
                    NewWork.Gender = WorkModel.Gender;
                    NewWork.Skills = WorkModel.Skills;
                    NewWork.MinAge = WorkModel.MinAge;
                    NewWork.Category = WorkModel.Category;
                    NewWork.Picture = fileName;
                _context.Works.Add(NewWork);
                _context.SaveChanges();
                return Redirect($"/orgnaization/{HttpContext.Session.GetInt32("OrgId")}");
            } else {
                 Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
                return View("NewProject");
            }
        }

        [HttpGet("edit/{id}")]
        public IActionResult EditProject(int id){
            if(HttpContext.Session.GetInt32("OrgId")!=null){
                Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
                Work work = _context.Works.FirstOrDefault(w=>w.WorkId==id);
                // WorkViewModels workModel= new WorkViewModels(){
                //     Name = work.Name,
                //     Description = work.Description,
                //     Location = work.Location,
                //     StartDate = work.StartDate,
                //     EndDate = work.EndDate,
                //     Country = work.Country,
                //     City = work.City,
                //     NumberOfHours = work.NumberOfHours,
                //     NumberOfVolunteers = work.NumberOfVolunteers,
                //     Gender = work.Gender,
                //     Skills = work.Skills,
                //     MinAge = work.MinAge,
                //     Category = work.Category,
                //     Picture = work.Picture.FileName,
                // };
                return View(work);
            } else {
                return RedirectToAction("Login");
            } 
        }
        [HttpPost("edit/project/{id}")]
        public IActionResult EditProjectForm(Work Work, int id){
            int? OrgId = HttpContext.Session.GetInt32("OrgId");
            Work oldWork = _context.Works.FirstOrDefault(w=>w.WorkId==id);
            if (ModelState.IsValid){
                // string fileName = null;
            //     if(WorkModel.Picture != null){
            //         string uploadFolder =  Path.Combine(hostingEnvironment.WebRootPath, "image");
            //         fileName = Guid.NewGuid().ToString() + "_" + WorkModel.Picture.FileName;
            //         string filePath = Path.Combine(uploadFolder,fileName);
            //         WorkModel.Picture.CopyTo(new FileStream(filePath, FileMode.Create));
            //     }
                    oldWork.OrganizationId=(int)OrgId;
                    oldWork.Name = Work.Name;
                    oldWork.Description = Work.Description;
                    oldWork.Location = Work.Location;
                    oldWork.StartDate = Work.StartDate;
                    oldWork.EndDate = Work.EndDate;
                    oldWork.Country = Work.Country;
                    oldWork.City = Work.City;
                    oldWork.NumberOfHours = Work.NumberOfHours;
                    oldWork.NumberOfVolunteers = Work.NumberOfVolunteers;
                    oldWork.Gender = Work.Gender;
                    oldWork.Skills = Work.Skills;
                    oldWork.MinAge = Work.MinAge;
                    oldWork.Category = Work.Category;
                    _context.SaveChanges();
                return RedirectToAction("UserOpportunity","User");
            } else {
                  Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
              
                return View("EditProject");
            }
        }

        [HttpGet("org/profile")]
        public IActionResult OrgProfile(){
            Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
            int? id = HttpContext.Session.GetInt32("OrgId");
            Organization Org = _context.Organizations.FirstOrDefault(o=>o.OrganizationId==id);
            if(HttpContext.Session.GetInt32("OrgId")!=null){
                return View(Org);
            } else {
                return RedirectToAction("Login");
            }
        }

        [HttpPost("org/edit")]
        public IActionResult ProfileForm(Organization NewOrg){
            int? id = HttpContext.Session.GetInt32("OrgId");
            Organization oldOrg = _context.Organizations.FirstOrDefault(o=>o.OrganizationId==id);
            if(ModelState.IsValid){
                // if(_context.Organizations.Any(u=> u.Email==NewOrg.Email)){
                //     ModelState.AddModelError("Email","This Email is already registered");
                //     return View("OrgProfile",oldOrg);
                // } else {
                    PasswordHasher<Organization> passwordHasher = new PasswordHasher<Organization>();
                    NewOrg.password = passwordHasher.HashPassword(NewOrg,NewOrg.password);
                    oldOrg.password= NewOrg.password;
                    oldOrg.Name = NewOrg.Name;
                    oldOrg.Description = NewOrg.Description;
                    oldOrg.Email = NewOrg.Email;
                    oldOrg.Phone= NewOrg.Phone;
                    oldOrg.FoundingDate = NewOrg.FoundingDate;
                    oldOrg.Address= NewOrg.Address;
                    oldOrg.UpdatedAt = DateTime.Now;
                    _context.SaveChanges();
                    return RedirectToAction("OrgProfile");
                // }
            } else {
                Console.WriteLine("??????????");
                return View("OrgProfile",oldOrg);
            }
        }
         [HttpGet("search")]
        public IActionResult Search(){
            ViewBag.LoggedOrg = HttpContext.Session.GetInt32("OrgId");
            if (HttpContext.Session.GetInt32("OrgId") != null){
                Organization orgNav = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = orgNav;
            }else{
                Organization orgNav = new Organization();
                ViewBag.OrgObj = orgNav;
            }
            if(HttpContext.Session.GetInt32("UserId")!=null){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
            }
            var works=_context.Works.Where(w=> w.WorkId == 0).ToList(); //هذا مازين
            return View("Search",works);
        }

        [HttpPost("search")]
        public IActionResult Search(string word) {
            ViewBag.LoggedOrg = HttpContext.Session.GetInt32("OrgId");
            if (HttpContext.Session.GetInt32("OrgId") != null){
                Organization orgNav = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = orgNav;
            }else{
                Organization orgNav = new Organization();
                ViewBag.OrgObj = orgNav;
            }
            if(HttpContext.Session.GetInt32("UserId")!=null){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
            }
            if(HttpContext.Session.GetInt32("OrgId") != null){
                ViewBag.Word = word;
            var Result = _context.Works
            .Include(w => w.CreatedBy)
            .Where(w => w.Name.Contains(word) && w.CreatedBy.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId"))
            .ToList();
            return View(Result);
            }else{
                var Result = _context.Works.Include(w => w.CreatedBy)
                    .Where(w => w.Name.Contains(word)).ToList();
                    return View(Result);

            }
            
        }

        [HttpGet("deleteProject/{id}")]
        public IActionResult DeleteProject(int id){
            Work work= _context.Works.SingleOrDefault(w=>w.WorkId == id);
            _context.Works.Remove(work);
            _context.SaveChanges();
            return Redirect($"/orgnaization/{HttpContext.Session.GetInt32("OrgId")}");
        }

        [HttpGet("deletePerson/{Uid}/{Wid}")]
        public IActionResult DeletePerson(int Uid, int Wid){
            Association Aso =_context.Associations.FirstOrDefault(a=>a.UserId == Uid && a.WorkId == Wid);
            _context.Associations.Remove(Aso);
            _context.SaveChanges();
            return RedirectToAction("UserOpportunity","User",new {id = Wid});
        }

        [HttpGet("category/{cat}")]
        public IActionResult Category(string cat){
            Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
            var Result = _context.Works
            .Include(w => w.CreatedBy)
            .Where(w => w.Category==cat && w.CreatedBy.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId"))
            .ToList();
            return View(Result);
        }



        // [HttpGet("detailes/{WorkId}")]
        // public IActionResult WorkDetails(int WorkId) {

        //     return View();
        // }
        
    }
    
}