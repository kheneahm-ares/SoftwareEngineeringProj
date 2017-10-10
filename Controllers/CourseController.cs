using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodingBlogDemo2.Data;
using Microsoft.AspNetCore.Authorization;
using CodingBlogDemo2.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodingBlogDemo2.Controllers
{
    [Authorize] //should only be accessed by Admin, need to create custom authorization
    public class CourseController : Controller
    {
        private ICourseRepository _courseRepo;
        private IAccountRepository _accountRepo;


       

        public CourseController(ICourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        

        public IActionResult Create()
        {
            

            return View();

        }

        [HttpPost]
        public IActionResult Create(Course newCourse)
        {
            if (ModelState.IsValid)
            {
                newCourse.UserEmail = User.Identity.Name;
                _courseRepo.AddCourse(newCourse);
                return RedirectToRoute(new
                {
                    controller="Profile",
                    action="Index"
                });
            }

            return View(newCourse);
        }
        public IActionResult Edit()
        {
            return View();

        }

        public IActionResult Delete()
        {
            return View();

        }
    }
}
