using CodingBlogDemo2.Data;
using CodingBlogDemo2.Models;
using CodingBlogDemo2.Models.ProfileViewModels;
using CodingBlogDemo2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sakura.AspNetCore;


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

        public IActionResult Index(int? page, int? pageSize)
        {


            ViewBag.FName = _accountRepository.getUserFName(User.Identity.Name);

            bool isAdmin = User.IsInRole("Admin");
            ViewBag.isAdmin = isAdmin;

            IEnumerable<Course> courses;
            IEnumerable<Post> posts;
            ApplicationUser currentUser;

            List<CourseInfo> courseInfos = new List<CourseInfo>();

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



            courses = _courseRepository.Courses.Where(p => p.UserEmail == User.Identity.Name);

            foreach (Course cs in courses)
            {
                CourseInfo newCourseInfo = new CourseInfo();
                newCourseInfo.Course = cs;


                ApplicationUser user = _context.Users.Where(c => c.Email == cs.UserEmail).First();
                newCourseInfo.InstructorLName = user.LastName;

                courseInfos.Add(newCourseInfo);
            }

            //get posts based on who you follow and/or created
            posts = _context.Posts;
            currentUser = _accountRepository.getUserByEmail(currentUserEmail);


            List<Activity> activities = new List<Activity>();


            if (isAdmin) //only admins can have activities of addition and editing of courses/posts
            {
                //lets check if user has created or editted any courses and add to activity
                var courseActivities = _context.Courses.Where(c => c.UserEmail == currentUserEmail);

                List<Post> allPosts = new List<Post>();

                foreach (Course c in courseActivities)
                {
                    Activity newActivity = new Activity
                    {
                        CourseName = c.Name,
                    };

                    //we check if its a creation or an edit, so no duplicate activities
                    if (c.WhenEdited > c.WhenCreated) //if the edit is "newer" than the creation then it is an edit
                    {
                        newActivity.Time = c.WhenEdited;
                        newActivity.Type = "CourseEdit";
                    }
                    else
                    {
                        newActivity.Time = c.WhenCreated;
                        newActivity.Type = "CourseCreation";
                    }
                    activities.Add(newActivity);


                    int i = 1;
                    if(i == 1)
                    {
                        var coursePosts = _context.Posts.Where(p => p.CourseId == c.CourseId);
                        allPosts.AddRange(coursePosts);
                    }
                    i++;
                    
                }

                //for each post belonging to the admin's course
                foreach(Post p in allPosts)
                {
                    Activity newActivity = new Activity();

                    var coursePost = _context.Courses.Where(c => c.CourseId == p.CourseId).First();

                    //grab post name
                    if(p.PostCategory == 1)
                    {
                        var mc = _context.MultipleChoices.Where(m => m.MultipleChoiceId == p.AssignmentId).First();
                        newActivity.PostName = mc.Name;
                        newActivity.CourseName = coursePost.Name;
                        

                        //we check if its a creation or an edit, so no duplicate activities
                        if (mc.WhenEdited > mc.WhenCreated) //if the edit is "newer" than the creation then it is an edit
                        {
                            newActivity.Time = mc.WhenEdited;
                            newActivity.Type = "PostEdit";
                        }
                        else
                        {
                            newActivity.Time = mc.WhenCreated;
                            newActivity.Type = "PostCreation";
                        }

                    }
                    else if(p.PostCategory == 2)
                    {

                        var codeSnip = _context.CodeSnippets.Where(cs => cs.CodeSnippetId == p.AssignmentId).First();
                        newActivity.PostName = codeSnip.Name;
                        newActivity.CourseName = coursePost.Name;

                        //we check if its a creation or an edit, so no duplicate activities
                        if (codeSnip.WhenEdited > codeSnip.WhenCreated) //if the edit is "newer" than the creation then it is an edit
                        {
                            newActivity.Time = codeSnip.WhenEdited;
                            newActivity.Type = "PostEdit";
                        }
                        else
                        {
                            newActivity.Time = codeSnip.WhenCreated;
                            newActivity.Type = "PostCreation";
                        }

                    }
                    else if(p.PostCategory == 3)
                    {

                        var codeSnipNoAnswer = _context.CodeSnippetNoAnswers.Where(cs => cs.CodeSnippetNoAnswerId == p.AssignmentId).First();
                        newActivity.PostName = codeSnipNoAnswer.Name;
                        newActivity.CourseName = coursePost.Name;

                        //we check if its a creation or an edit, so no duplicate activities
                        if (codeSnipNoAnswer.WhenEdited > codeSnipNoAnswer.WhenCreated) //if the edit is "newer" than the creation then it is an edit
                        {
                            newActivity.Time = codeSnipNoAnswer.WhenEdited;
                            newActivity.Type = "PostEdit";
                        }
                        else
                        {
                            newActivity.Time = codeSnipNoAnswer.WhenCreated;
                            newActivity.Type = "PostCreation";
                        }

                    }

                    activities.Add(newActivity);
                }

            }

 
            var userSubmissions = _context.Submissions.Where(s => s.UserEmail == currentUserEmail);

            //every user submission should be an activity
            foreach (Submission s in userSubmissions)
            {
                Activity newActivity = new Activity();

                //we grab the course name sthis submission it belongs to
                var coursePostName = _context.Courses.Where(c => c.CourseId == s.CourseId).First().Name;
                newActivity.CourseName = coursePostName;

                if (s.CategoryId == 1)
                {
                    var mc = _context.MultipleChoices.Where(m => m.MultipleChoiceId == s.AssignmentId).First();
                    newActivity.PostName = mc.Name;

                }
                else if (s.CategoryId == 2)
                {
                    var codeSnip = _context.CodeSnippets.Where(cs => cs.CodeSnippetId == s.AssignmentId).First();
                    newActivity.PostName = codeSnip.Name;
                }
                else if (s.CategoryId == 3)
                {
                    var codeSnipNoAnswer = _context.CodeSnippetNoAnswers.Where(cs => cs.CodeSnippetNoAnswerId == s.AssignmentId).First();
                    newActivity.PostName = codeSnipNoAnswer.Name;
                }

                newActivity.Time = s.DateCreated;
                newActivity.Type = "PostSubmission";

                activities.Add(newActivity);
            }

            CourseListViewModel courseList = new CourseListViewModel
            {
                CourseInfos = courseInfos,
                CoursesRegistered = courseRegistrationInfos,          

            };


            //pagination 
            int no = page ?? 1;
            int size = pageSize ?? 10;

            return View(new HomePageViewModel
            {
                CourseList = courseList,
                Activities = activities.OrderByDescending(a => a.Time).ToPagedList(size, no) //for pagination
            } 
            );

        }

    }
}
