using System;
using System.Linq;
using CSharpProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace CSharpProject.Controllers
{
    public class UserController : Controller
    {
        private MyContext _context;

        public UserController(MyContext context){
            _context=context;
        }

        // [HttpGet("user")]
        // public IActionResult Index(){
        //     if(HttpContext.Session.GetInt32("UserId")!=null){
        //         return View();
        //     } else {
        //         return RedirectToAction("LoginForm");
        //     }
        // }
        [HttpGet("user/reg")]
        public IActionResult Register(){
            if(HttpContext.Session.GetInt32("OrgId") != null){
                return Redirect("/");
            }
            else if(HttpContext.Session.GetInt32("UserId")==null){
                return View();
            } else {
                return RedirectToAction( "UserDashBoard");
            }
        }

        [HttpPost("create")]
        public IActionResult RegisterForm(User NewUser){
            Console.WriteLine("hhhhhh");
            if(ModelState.IsValid){
                if(_context.Users.ToList().Count==0){
                    PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    NewUser.Password = passwordHasher.HashPassword(NewUser,NewUser.Password);
                    NewUser.IsAdmin=true;
                    _context.Users.Add(NewUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId",NewUser.UserId);
                    // Admin Routes
                    return RedirectToAction( "UserDashBoard");
                }
                if(_context.Users.Any(u=> u.Email==NewUser.Email)){
                    ModelState.AddModelError("Email","This Email is already registered");
                    return View("Register");
                } else {
                    PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    NewUser.Password = passwordHasher.HashPassword(NewUser,NewUser.Password);
                    _context.Users.Add(NewUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId",NewUser.UserId);
                    return RedirectToAction( "UserDashBoard");
                }
            } else {
                return View("Register");
            }
        }

        [HttpGet("user/login")]
        public IActionResult Login(){
            if(HttpContext.Session.GetInt32("OrgId") != null){
                return Redirect("/");
            }
            else if(HttpContext.Session.GetInt32("UserId")==null){
                return View();
            } else {
                return RedirectToAction( "UserDashBoard");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginForm(Login LoggedUser){
            if (ModelState.IsValid){
                User DbUser = _context.Users.FirstOrDefault(u => u.Email==LoggedUser.Email);
                if (DbUser == null){
                    ModelState.AddModelError("Email","Email or Password is Invalid");
                    return View("Login");
                } else {
                    PasswordHasher<Login> passwordHasher = new PasswordHasher<Login>();
                    var result = passwordHasher.VerifyHashedPassword(LoggedUser,DbUser.Password,LoggedUser.Password);
                    if (result==0){
                        ModelState.AddModelError("Email","Email or Password is Invalid");
                        return View("Login");
                    } else {
                        HttpContext.Session.SetInt32("UserId",DbUser.UserId);
                        return RedirectToAction("UserDashBoard");
                    }
                }
            } else {
                return View("Login");
            }
        }
        
        [HttpGet("user/dashboard")]
        public IActionResult UserDashBoard(){
            if (HttpContext.Session.GetInt32("OrgId") != null){
                Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
            }else{
                Organization org = new Organization();
                ViewBag.OrgObj = org;
            }
            
            ViewBag.works=_context.Works
            .OrderByDescending(x => x.CreatedAt)
            .Include(o=>o.CreatedBy).ToList();
            ViewBag.LoggedOrg = HttpContext.Session.GetInt32("OrgId");
            if(HttpContext.Session.GetInt32("UserId")!=null){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
            }
            
            ViewBag.LoggedUser=HttpContext.Session.GetInt32("UserId");
           
            return View("UserDashBoard");
        }

        [HttpGet("user/logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
         [HttpGet("opportunity/{id}")]
         public IActionResult UserOpportunity(int id){
            
             ViewBag.LoggedOrg = HttpContext.Session.GetInt32("OrgId");
             Work work= _context.Works
             .Include(o=>o.CreatedBy)
             .Include(w=>w.Workers)
             .ThenInclude(u=>u.User)
             .FirstOrDefault(w=> w.WorkId == id);
             ViewBag.LoggedUser=HttpContext.Session.GetInt32("UserId");
             if(HttpContext.Session.GetInt32("UserId")!=null){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
            }
            if (HttpContext.Session.GetInt32("OrgId") != null){
                Organization orgNav = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = orgNav;
            }else{
                Organization orgNav = new Organization();
                ViewBag.OrgObj = orgNav;
            }


             return View(work);
         }
         [HttpGet("add/opportunity/{id}")]
         public IActionResult AddOpportunity(int id){
             Association association=new Association();
             User user = _context.Users.FirstOrDefault(u =>u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
             Work work = _context.Works.FirstOrDefault(u => u.WorkId == id);
             user.NumberOfHours += work.NumberOfHours;
             association.UserId=(int)HttpContext.Session.GetInt32("UserId");
             association.WorkId=id;
             _context.Associations.Add(association);
            _context.SaveChanges();
             return RedirectToAction("UserOpportunity",id);
         }

         [HttpGet("remove/opportunity/{id}")]
         public IActionResult RemovexwxOpportunity(int id){
             int? loginUser=HttpContext.Session.GetInt32("UserId");
             Association association=_context.Associations.FirstOrDefault(a=>a.UserId == (int)loginUser && a.WorkId == id);
            User user = _context.Users.FirstOrDefault(u =>u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
             Work work = _context.Works.FirstOrDefault(u => u.WorkId == id);
             user.NumberOfHours -= work.NumberOfHours;
             _context.Remove(association);
             _context.SaveChanges();
             return RedirectToAction("UserOpportunity",id);
         }

         [HttpGet("user/profile")]
         public IActionResult UserProfile(){
             if(HttpContext.Session.GetInt32("UserId") == null){
                 return RedirectToAction("Login");
             }
             else{
                 User user = _context.Users.Include(u=>u.Works).FirstOrDefault(u=> u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
                return View("UserProfile",user);
             }
             
         }
         [HttpPost("user/profile/update")]
         public IActionResult UpdateForm(User UpdatedUser)
         {
             User OldProfile = _context.Users.FirstOrDefault(u=> u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
             if(ModelState.IsValid){
                 Console.WriteLine("hello"); 
                 OldProfile.FirstName = UpdatedUser.FirstName;
                 OldProfile.LastName = UpdatedUser.LastName;
                 OldProfile.Email = UpdatedUser.Email;
                 OldProfile.Gender = UpdatedUser.Gender;
                 OldProfile.City = UpdatedUser.City;
                 OldProfile.Country = UpdatedUser.Country;
                 OldProfile.DoB = UpdatedUser.DoB;
                 OldProfile.Phone = UpdatedUser.Phone;
                 _context.SaveChanges();
                 return RedirectToAction("UserProfile");
             }
             else{
                 Console.WriteLine("???????????");
                 return View("UserProfile",OldProfile);
             } 
         }

        [HttpGet("user/myopportunity")]
        public IActionResult MyOpportunity()
        {
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Login");
            }else{
                User MyOpportunity = _context.Users
                .Include(w => w.Works)
                .ThenInclude(c => c.Work)
                .ThenInclude(q=> q.CreatedBy)
                .FirstOrDefault(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
                return View("MyOpportunity", MyOpportunity);

            }
        }
        [HttpGet("user/orgnization")]
        public IActionResult Orgnizations(){
            if(HttpContext.Session.GetInt32("UserId")!=null){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
            }
            List<Organization> organizations= _context.Organizations.ToList();
            return View("Orgnizations",organizations);
        }
        [HttpGet("{category}/opportunities")]
        public IActionResult Categories(string category){
             if (HttpContext.Session.GetInt32("OrgId") != null){
                Organization org = _context.Organizations.FirstOrDefault(u => u.OrganizationId==(int)HttpContext.Session.GetInt32("OrgId")); 
                ViewBag.OrgObj = org;
            }else{
                Organization org = new Organization();
                ViewBag.OrgObj = org;
            }
            if(HttpContext.Session.GetInt32("UserId")!=null){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
            }
            ViewBag.CatName=category;
            List<Work> works = _context.Works.Include(w=>w.CreatedBy).Where(w => w.Category==category).ToList();
            return View("Categories",works);
        }
        [HttpPost("contact")]
        public IActionResult Contact(ContactUs contact){
            if(ModelState.IsValid){
                _context.Add(contact);
                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            else{
                return View("Index");
            }
        }
       
    }
}