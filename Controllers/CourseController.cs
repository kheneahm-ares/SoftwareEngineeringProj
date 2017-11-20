using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodingBlogDemo2.Data;
using Microsoft.AspNetCore.Authorization;
using CodingBlogDemo2.Models;
using CodingBlogDemo2.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodingBlogDemo2.Controllers
{
     //should only be accessed by Admin, need to create custom authorization
    public class CourseController : Controller
    {
        private ICourseRepository _courseRepo;
        private IAccountRepository _accountRepo;
        private ApplicationDbContext _context;




        public CourseController(ICourseRepository courseRepo, IAccountRepository accountRepo, ApplicationDbContext context)
        {
            _courseRepo = courseRepo;
            _accountRepo = accountRepo;
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {

            IEnumerable<Course> courses;


            courses = _courseRepo.Courses.Where(p => p.UserEmail == User.Identity.Name);


            return View(new CourseListViewModel
            {
                Courses = courses,
            });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {


            return View();

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Course newCourse)
        {
            if (ModelState.IsValid)
            {
                newCourse.UserEmail = User.Identity.Name;
                _courseRepo.AddCourse(newCourse);
                TempData["Success"] = "Course Successfully Created!";
                return RedirectToRoute(new
                {
                    controller = "Profile",
                    action = "Index"
                });
            }


            return View(newCourse);
        }

        public IActionResult Show(int id)
        {
            AssignmentViewModel allPosts = new AssignmentViewModel();

            //to get all posts of a current model we must first filter based on class
            IEnumerable<Post> posts = _context.Posts.Where(p => p.CourseId == id);

            //we now have all posts and its values (specifically the assignment ID and what tables they are in) based on a specific course id
            //now we go to each table and grab them 

            //we want to use list instead of Enumarable because we technically cant add to an enumerable set
            List<MultipleChoice> mcs = new List<MultipleChoice>();
            List<CodeSnippet> codeSnips = new List<CodeSnippet>();
            List<CodeSnippetNoAnswer> codeSnipsNoAnswer = new List<CodeSnippetNoAnswer>();


            foreach (Post post in posts)
            {
                //based on category type, we append the assignment to the set

                //grab from MC table
                if (post.PostCategory == 1)
                {
                    mcs.Add(_context.MultipleChoices.Where(m => m.MultipleChoiceId == post.AssignmentId).SingleOrDefault());
                }


                else if (post.PostCategory == 2)
                {
                    codeSnips.Add(_context.CodeSnippets.Where(c => c.CodeSnippetId == post.AssignmentId).SingleOrDefault());
                }


                else if (post.PostCategory == 3)
                {
                    codeSnipsNoAnswer.Add(_context.CodeSnippetNoAnswers.Where
                        (c => c.CodeSnippetNoAnswerId == post.AssignmentId).SingleOrDefault());
                }
            }

            //this view bag is used when the Create Post link is clicked
            ViewBag.CourseId = id;

            return View(new AssignmentViewModel
            {
                MultipleChoices = mcs,
                CodeSnippets = codeSnips,
                CodeSnippetNoAnswers = codeSnipsNoAnswer
            });

        }

        [Authorize(Roles = "Admin")]
        //this just gets the view and returns the course model with initialized variables87ytfdxzaa
        public IActionResult Edit(int id)
        {

            var course = _courseRepo.Courses.Where(c => c.CourseId == id).FirstOrDefault();
            return View(course);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Course course)
        {

            //tell database which course we want to edit
            var courseToUpdate = _context.Set<Course>().Where(c => c.CourseId == id).SingleOrDefault();

            courseToUpdate.CourseId = id;
            courseToUpdate.Name = course.Name;
            courseToUpdate.UserEmail = User.Identity.Name;

            _context.SaveChanges();

            //message partial, a session
            TempData["Success"] = "Course Updated!";

            return RedirectToRoute(new {
                controller = "Profile",
                action = "Index"

            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            //for now it will just be the data in the course table,
            //however, when we delete a course that has relationships with modules and assignments, we have to delete every data
            //connected to that course such as the modules, posts, and assignments in those modules!!!!!!!!!!!
            Course course = _context.Set<Course>().Where(c => c.CourseId == id).SingleOrDefault();
            _context.Entry(course).State = Microsoft.EntityFrameworkCore.EntityState.Deleted; 
            _context.SaveChanges();

            TempData["Success"] = "Course Successfully Deleted!";

            return RedirectToRoute(new
            {
                controller = "Profile",
                action = "Index"

            });

        }
    }
}
