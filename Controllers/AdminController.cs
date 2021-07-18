using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharpProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Controllers
{
    public class AdminController : Microsoft.AspNetCore.Mvc.Controller
    {
        private MyContext _context;

        public AdminController(MyContext context){
            _context=context;
        }

        [HttpGet("admin")]
        public IActionResult Dashboard(){

           
            if(HttpContext.Session.GetInt32("UserId")!=null&&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
                 User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
                ViewBag.UserObj =  user;
                return View();
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
                return RedirectToAction("Index", "Home");
            }

 
        }

        [HttpGet("admin/users")]
        public IActionResult AllUsers(){
            if(HttpContext.Session.GetInt32("UserId")!=null&&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
                ViewBag.AllUsers=_context.Users.ToList();
                return View();
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
                return RedirectToAction("Index", "Home");
            }
            
   
        }
        [HttpGet("admin/orgs")]
        public IActionResult AllOrgs(){
            if(HttpContext.Session.GetInt32("UserId")!=null&&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
                 ViewBag.AllOrgs=_context.Organizations.ToList();
                   return View();
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
                  return RedirectToAction("Index", "Home");
            }
           
         
        }
        [HttpGet("admin/opportunities")]
        public IActionResult AllWorks(){
            if(HttpContext.Session.GetInt32("UserId")!=null&&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
               User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")); 
                ViewBag.UserObj =  user;
                ViewBag.AllWorks=_context.Works
            .Include(w => w.CreatedBy)
            .ToList();
            return View();
            }else{
                User user = new User();
                ViewBag.UserObj =  user;
                return RedirectToAction("Index", "Home");
            }            
            
           
        }

        [HttpGet("admin/delete/org/{id}")]
        public IActionResult DeleteOrg(int id){
            if(HttpContext.Session.GetInt32("UserId") != null &&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
        Organization org = _context.Organizations.SingleOrDefault(u => u.OrganizationId==id);
                    _context.Organizations.Remove(org);
                    _context.SaveChanges();
                    return RedirectToAction("AllOrgs");
            }else{
                return RedirectToAction("Index", "Home");
            }
           
        }
        [HttpGet("admin/delete/user/{id}")]
        public IActionResult DeleteUser(int id){
            if(HttpContext.Session.GetInt32("UserId") != null &&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
            User user = _context.Users.SingleOrDefault(u => u.UserId==id);
            if(user.IsAdmin == true){
                return RedirectToAction("AllUsers");
            }else{
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("AllUsers");
            }
          
              } else
              {
                  return RedirectToAction("Index", "Home");
              }
        }
        [HttpGet("admin/delete/work/{id}")]
        public IActionResult DeleteWork(int id){
            if(HttpContext.Session.GetInt32("UserId") != null &&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
                      Work work = _context.Works.SingleOrDefault(u => u.WorkId==id);
            _context.Works.Remove(work);
            _context.SaveChanges();
            return RedirectToAction("AllWorks");
            }else{
               return RedirectToAction("Index", "Home"); 
            }
  
        }
        [HttpGet("admin/user/{id}")]
        public IActionResult GetUserDetails(int id)
        {
             if(HttpContext.Session.GetInt32("UserId")!=null&&  _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId")).IsAdmin == true){
                  User user = _context.Users.FirstOrDefault(u => u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
                ViewBag.UserObj =  user;
                ViewBag.UserDetails = _context.Users
                            .Include(w =>w.Works)
                            .ThenInclude(r => r.Work)
                            .FirstOrDefault(u => u.UserId == id);
            
                return View();
             }else{
                  User user = new User();
                ViewBag.UserObj =  user;
                return RedirectToAction("Index", "Home");
             }
            
          
        }
        
    }
}