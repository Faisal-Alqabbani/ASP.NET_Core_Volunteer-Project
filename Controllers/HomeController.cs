using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharpProject.Models;
using Microsoft.AspNetCore.Http;

namespace CSharpProject.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context){
            _context=context;
        }

        [HttpGet("")]
        public IActionResult Index(){
            // ViewBag.UserLog=HttpContext.Session.GetInt32("UserId");
            // ViewBag.OrgLog=HttpContext.Session.GetInt32("OrgId");
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                return Redirect("/user/dashboard");
            }
            else if (HttpContext.Session.GetInt32("OrgId") != null)
            {
                return Redirect($"/orgnaization/{HttpContext.Session.GetInt32("OrgId")}");
            }else
            {
              return View();  
            }
            
        }

    }
}
