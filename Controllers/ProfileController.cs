using CodingBlogDemo2.Models;
using CodingBlogDemo2.Models.ProfileViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private IAccountRepository _accountRepository;
        private ICourseRepository _courseRepository;

        public ProfileController(IAccountRepository accountRepo, ICourseRepository courseRepo)
        {
            _accountRepository = accountRepo;
            _courseRepository = courseRepo;
        }

        public IActionResult Index()
        {
            
            ViewBag.FName = _accountRepository.getUserFName(User.Identity.Name);
            ViewBag.isAdmin = _accountRepository.IsAdmin(User.Identity.Name);

            IEnumerable<Course> courses;
            ApplicationUser currentUser;


            courses = _courseRepository.Courses.Where(p => p.UserEmail == User.Identity.Name);
            currentUser = _accountRepository.getUserByEmail(User.Identity.Name);


            return View(new CourseListViewModel
            {
                Courses = courses,
                ProfessorName = currentUser.LastName
            });

        }

    }
}
