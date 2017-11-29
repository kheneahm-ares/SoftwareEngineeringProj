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
     [Authorize]
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

            List<CourseInfo> courseRegistrationInfos = new List<CourseInfo>();

            string currentUserEmail = User.Identity.Name;

            //get all registrations of current user
            IEnumerable<Register> registrations = _context.Registers.Where(r => r.UserEmail == currentUserEmail);

            foreach (Register r in registrations)
            {
                CourseInfo newCourseInfo = new CourseInfo();
                Course cs = _context.Courses.Where(c => c.CourseId == r.CourseId).First();
                newCourseInfo.Course = cs;

                ApplicationUser user = _context.Users.Where(c => c.Email == cs.UserEmail).First();
                newCourseInfo.InstructorLName = user.LastName;

                courseRegistrationInfos.Add(newCourseInfo);
            }


            courses = _courseRepo.Courses.Where(p => p.UserEmail == User.Identity.Name);


            return View(new CourseListViewModel
            {
                Courses = courses,
                CoursesRegistered = courseRegistrationInfos
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

        [Authorize(Roles = "Admin")]
        public IActionResult Report(int id)
        {
            //an admin can only do "CUD" functionalities if he/she is the creator of the course
            if (!IsOwner(id))
            {
                return RedirectToRoute(new
                {
                    controller = "Account",
                    action = "AccessDenied"
                });
            }

            List<Report> reports = new List<Report>();

            //we want to grab all the users that are registered this course
            IEnumerable<Register> registrations = _context.Registers.Where(r => r.CourseId == id);

            int coursePostsTotal = _context.Posts.Where(p => p.CourseId == id).Count();


            //we want to show the users, how many submissions they made and how many 
            foreach(Register r in registrations)
            {
                Report newReport = new Report();
                ApplicationUser user = _context.Users.Where(u => u.Email == r.UserEmail).First();
                newReport.FirstName = user.FirstName;
                newReport.LastName = user.LastName;

                //since submissions have a user email, we can just check how many times the user submitted to a course
                newReport.Submissions = _context.Submissions.Where(s => s.CourseId == id && s.UserEmail == user.Email).Count();


                if(newReport.Submissions > 0)
                {
                //we want to get their most recent submission
                Submission sub = _context.Submissions.Where(s => s.CourseId == id && s.UserEmail == user.Email).OrderByDescending(s => s.DateCreated).FirstOrDefault();

                newReport.SubmissionTime = sub.DateCreated;

                }
                newReport.CoursePostTotal = coursePostsTotal;

                newReport.SubmissionActivityPercentage = ((double)newReport.Submissions / coursePostsTotal) * 100;

                newReport.UserEmail = user.Email;

                reports.Add(newReport);
            }






            return View(new ReportViewModel
            {
                Reports = reports
            });
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

            ViewBag.CourseName = _context.Courses.Where(c => c.CourseId == id).First().Name;

            //used to show follow or unfollow
            string userEmail = User.Identity.Name;

            ViewBag.isFollowing = _context.Registers.Any(r => r.UserEmail == userEmail);


            string courseCreatorEmail = _context.Courses.Where(c => c.CourseId == id).First().UserEmail;
            ViewBag.CourseCreator = courseCreatorEmail;
            ViewBag.CourseCreatorLName = _context.Users.Where(c => c.Email == courseCreatorEmail).First().LastName;



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

            //an admin can only do "CUD" functionalities if he/she is the creator of the course
            if (!IsOwner(id))
            {
                return RedirectToRoute(new
                {
                    controller = "Account",
                    action = "AccessDenied"
                });
            }

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

        public IActionResult Search(String searchQuery)
        {
            List<CourseInfo> courseInfos = new List<CourseInfo>();


            //get courses where a name is like what was searched

            var courses = from c in _context.Courses select c;

            var coursesCount = courses.Count();

            var specifiedCourses = courses.Where(c => c.Name.Contains(searchQuery));


            //this code has occured at least twice, needs to be put in a repo so that it it can be reused

            if (specifiedCourses != null)
            {


                foreach (Course course in specifiedCourses)
                {

                    CourseInfo newCourseInfo = new CourseInfo();
                    newCourseInfo.Course = course;

                    ApplicationUser user = _context.Users.Where(u => u.Email == course.UserEmail).First();
                    newCourseInfo.InstructorLName = user.LastName;

                    //add new course info
                    courseInfos.Add(newCourseInfo);
                }
            }


            ViewBag.Search = searchQuery;

            return View(new CourseListViewModel
            {
                CourseInfos = courseInfos
            });
        }

        [HttpPost]
        public IActionResult Follow(int courseId)
        {
            string userEmail = User.Identity.Name;


            //we have to check whether or not the courseId is even existen
            //if somehow they are able to get into this controller with a non-existing courseId 
            bool isACourse = _context.Courses.Any(c => c.CourseId == courseId);
            if (!isACourse)
            {
                return NotFound(); 
            }


            //in registers table, add user with the corresponding courseId
            //create new registration
            Register newRegistration = new Register
            {
                CourseId = courseId,
                UserEmail = userEmail
            };


            //add and save to database
            _context.Add(newRegistration);
            _context.SaveChanges();

            string courseName = _context.Courses.Where(c => c.CourseId == courseId).First().Name;
            TempData["Success"] = $"You are now following {courseName}!";


            return RedirectToRoute(new
            {
                controller = "Course",
                action = "Show",
                id = courseId
            });
        }


        [HttpPost]
        public IActionResult Unfollow(int courseId)
        {
            //we want the current user to be unregistered
            string currentUserEmail = User.Identity.Name;
            Register reg = _context.Registers.Where(r => r.CourseId == courseId && r.UserEmail == currentUserEmail).SingleOrDefault(); //grab specific registration
            _context.Entry(reg).State = Microsoft.EntityFrameworkCore.EntityState.Deleted; //delete
            _context.SaveChanges();



            string courseName = _context.Courses.Where(c => c.CourseId == courseId).First().Name;
            TempData["Success"] = $"You have now unfollowed {courseName}!";

            return RedirectToRoute(new
            {
                controller = "Course",
                action = "Show",
                id = courseId
            });
        }

        private bool IsOwner(int courseId)
        {
            //get specific post,
            bool isOwner = false;
            string currentUserEmail = User.Identity.Name;


            //we can grab the user of the post by checking who the owner of the course it belongs to
            var course = _context.Courses.Where(c => c.CourseId == courseId).First();


            return isOwner = currentUserEmail == course.UserEmail; ;
        }
    }
}
