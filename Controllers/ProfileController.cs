using CodingBlogDemo2.Data;
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
        private ApplicationDbContext _context;

        public ProfileController(IAccountRepository accountRepo, ICourseRepository courseRepo, ApplicationDbContext context)
        {
            _accountRepository = accountRepo;
            _courseRepository = courseRepo;
            _context = context;
        }

        public IActionResult Index()
        {
            
            ViewBag.FName = _accountRepository.getUserFName(User.Identity.Name);
            ViewBag.isAdmin = _accountRepository.IsAdmin(User.Identity.Name);

            IEnumerable<Course> courses;
            IEnumerable<Post> posts;
            ApplicationUser currentUser;


            courses = _courseRepository.Courses.Where(p => p.UserEmail == User.Identity.Name);
            posts = _context.Posts;
            currentUser = _accountRepository.getUserByEmail(User.Identity.Name);


            return View(new CourseListViewModel
            {
                Courses = courses,
                Posts = posts,
                ProfessorName = currentUser.LastName
            });

        }

    }
}
