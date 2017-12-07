using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodingBlogDemo2.Data;
using Microsoft.AspNetCore.Authorization;
using CodingBlogDemo2.Models;
using Microsoft.EntityFrameworkCore;
using CodingBlogDemo2.Models.ViewModels;
using CodingBlogDemo2.Controllers.Api;

namespace CodingBlogDemo2.Controllers
{
    [Authorize] //should only be accessed by Admin, need to create custom authorization
    public class PostController : Controller
    {
        private ApplicationDbContext _context;
        private ICategoryRepository _categoryRepo;
        private static int _courseId;

        public PostController(ApplicationDbContext context, ICategoryRepository catRepo)
        {
            _context = context;
            _categoryRepo = catRepo;
        }

        // GET: Post
        [Route("/Course/{id}/Post/", Name = "PostIndex")]
        public IActionResult Index(int? id)
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

            return View(new AssignmentViewModel
            {
                MultipleChoices = mcs,
                CodeSnippets = codeSnips,
                CodeSnippetNoAnswers = codeSnipsNoAnswer
            });
        }

        // this details page is the actual page where STUDENTS submit their ANSWERS
        [Route("/Course/{id}/Post/{assignmentId?}/{categoryId?}/{folderId?}", Name = "PostDetails")]
        public IActionResult Details(int? id, int? assignmentId, int? categoryId, int? folderId)
        {
            //NOTE THAT assignmentId != postId, postId is the id for the Post table whereas assignmentId is the actual instance of the assignment post

            //define an view model that will be used by the view
            AssignmentViewModel newModel = new AssignmentViewModel();

            //we will use this in our views, instead of creating another view model
            ViewBag.courseId = id;
            ViewBag.categoryId = categoryId;

            //we first check whether there exists an assignment based on the assignmentId, categoryId, and courseId
            var coursesPosts = _context.Posts.Where(p => p.CourseId == id && p.AssignmentId == assignmentId && p.PostCategory == p.PostCategory);
            

            //if there isnt an exiting assignment

            if(coursesPosts.Count() == 0)
            {
                return NotFound();
            }

            //we need to check whether or not the Student accessing this page has submitted an answer for this assignment
            bool hasSubmitted = false;
            ViewBag.hasSubmitted = false;
            String currentUserEmail = User.Identity.Name;

            //we check if the user is even able to submit
            //we check in registration if the user is following the course based on its courseId
            bool isFollowing = _context.Registers.Any(r => r.UserEmail == currentUserEmail && r.CourseId == id);
            ViewBag.IsFollowing = isFollowing;

            bool isCourseCreator = _context.Courses.Any(c => c.CourseId == id && c.UserEmail == currentUserEmail);

            ViewBag.IsCourseCreator = isCourseCreator;

            ViewBag.folderId = folderId;


            //if requests for a category of type Multiple Choice
            if (categoryId == 1)
            {
                hasSubmitted = _context.MultipleChoiceSubmissions.Any(s => s.AssignmentId == assignmentId && s.UserEmail == currentUserEmail);

                if (hasSubmitted == true)
                {
                    ViewBag.hasSubmitted = true;
                }
                var mc = _context.MultipleChoices.Where(m => m.MultipleChoiceId == assignmentId).SingleOrDefault();

                newModel.MC = mc;

                return View(new AssignmentViewModel
                {
                    MC = newModel.MC
                });

            }

            //if requests for category of type code snippet with an answer
            else if (categoryId == 2)
            {
                hasSubmitted = _context.CodeSnippetSubmissions.Any(mc => mc.AssignmentId == assignmentId && mc.UserEmail == currentUserEmail);

                if (hasSubmitted == true)
                {
                    ViewBag.hasSubmitted = true;
                }


                var codeSnip = _context.CodeSnippets.Where(c => c.CodeSnippetId == assignmentId).SingleOrDefault();
                newModel.CodeSnippet = codeSnip;

                return View(new AssignmentViewModel
                {
                    CodeSnippet = newModel.CodeSnippet
                });
            }

            //if requests for category of type code snippet without an answer
            else if (categoryId == 3)
            {
                hasSubmitted = _context.CodeSnippetNoAnswerSubmissions.Any(mc => mc.AssignmentId == assignmentId && mc.UserEmail == currentUserEmail);

                if (hasSubmitted == true)
                {
                    ViewBag.hasSubmitted = true;
                }

                var codeSnipNoAnswer = _context.CodeSnippetNoAnswers.Where
                    (c => c.CodeSnippetNoAnswerId == assignmentId).SingleOrDefault();
                newModel.CodeSnippetNoAnswer = codeSnipNoAnswer;

                return View(new AssignmentViewModel
                {
                    CodeSnippetNoAnswer = newModel.CodeSnippetNoAnswer
                });
            }


            

            return NotFound();
             


        }


        // GET: Post/Create
        [Authorize(Roles = "Admin")]
        [Route("/Course/{id}/Create")]
        public IActionResult Create(int id)
        {


            //we check if there even exists a course with courseId = id
            bool courseExists = _context.Courses.Any(c => c.CourseId == id);

            if (!courseExists)
            {
                return NotFound();
            }


            _courseId = id;
            //an admin can only do "CUD" functionalities if he/she is the creator of the course
            if (!IsOwner(id))
            {
                return RedirectToRoute(new
                {
                    controller = "Account",
                    action = "AccessDenied"
                });
            }


            ViewBag.Categories = _categoryRepo.Categories;
            ViewBag.Folders = _context.Folders.Where(f => f.CourseId == id);
            return View();
        }

        // POST: Post/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id}/Create/MultipleChoice", Name ="MultipleChoice")]
        public async Task<IActionResult> CreateMultipleChoice(int id, MultipleChoiceViewModel model)
        {
            var folderId = Int32.Parse(Request.Form["folder"]);

            ViewBag.Categories = _categoryRepo.Categories;
            if (ModelState.IsValid)
            {
        

                //create new multiple choice 
                MultipleChoice newMC = new MultipleChoice();


                newMC.Name = model.Name;
                newMC.Description = model.Description;

                newMC.A = model.A;
                newMC.B = model.B;
                newMC.C = model.C;
                newMC.D = model.D;

                newMC.Answer = model.Answer;

                _context.MultipleChoices.Add(newMC);
                await _context.SaveChangesAsync();

                //save changes ^^

                //create new post and pass it newly saved multiple choice's id as foreign key

                Post newPost = new Post();


                newPost.CourseId = _courseId;

                newPost.PostCategory = 1;
                newPost.AssignmentId = newMC.MultipleChoiceId;
                newPost.FolderId = folderId;

                _context.Posts.Add(newPost);

                //everytime we add a post to a folder we update the edit time of the edit col in folder
                var folderToUpdate = _context.Folders.Where(f => f.FolderId == folderId && f.CourseId == id).First();
                folderToUpdate.WhenEdited = DateTime.Now;

                await _context.SaveChangesAsync();

                TempData["Success"] = "Assignment Successfully Created!";


                //to be used for views to show activity of Curse
                UpdateCourse(id);

                return RedirectToRoute(new
                {
                    controller = "Course",
                    action = "Show",
                    id = _courseId
                });

            }
            return View(model);
        }


        // POST: Post/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id}/Create/CodeSnippet", Name = "CodeSnippet")]
        public async Task<IActionResult> CreateCodeSnippet(int id, CodeSnippetViewModel model)
        {
            var folderId = Int32.Parse(Request.Form["folder"]);

            ViewBag.Categories = _categoryRepo.Categories;
            if (ModelState.IsValid)
            {

                //create new code snippet in CodeSnippet table
                CodeSnippet newCodeSnip = new CodeSnippet();


                newCodeSnip.Name = model.Name;
                newCodeSnip.Description = model.Description;

                newCodeSnip.Code = model.Code;

                newCodeSnip.Answer = model.Answer;

                _context.CodeSnippets.Add(newCodeSnip);
                await _context.SaveChangesAsync();

                //save changes ^^^

                //create new post and pass in the newly saved code snippet id in as the foreign key

                Post newPost = new Post();


                newPost.CourseId = _courseId;

                newPost.PostCategory = 2;
                newPost.AssignmentId = newCodeSnip.CodeSnippetId;
                newPost.FolderId = folderId;

                _context.Posts.Add(newPost);

                //everytime we add a post to a folder we update the edit time of the edit col in folder
                var folderToUpdate = _context.Folders.Where(f => f.FolderId == folderId && f.CourseId == id).First();
                folderToUpdate.WhenEdited = DateTime.Now;

                await _context.SaveChangesAsync();

                //to be used for views to show activity of Curse
                UpdateCourse(id);

                TempData["Success"] = "Assignment Successfully Created!";
                return RedirectToRoute(new
                {
                    controller = "Course",
                    action = "Show",
                    id = _courseId
                });

            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id}/Create/CodeSnippetNoAnswer", Name = "CodeSnippetNoAnswer")]
        public async Task<IActionResult> CreateCodeSnippetNoAnswer(int id, CodeSnippetNoAnswerViewModel model)
        {
            var folderId = Int32.Parse(Request.Form["folder"]);

            ViewBag.Categories = _categoryRepo.Categories;
            if (ModelState.IsValid)
            {

                //create new code snippet in CodeSnippet table
                CodeSnippetNoAnswer newCodeSnip = new CodeSnippetNoAnswer();


                newCodeSnip.Name = model.Name;
                newCodeSnip.Description = model.Description;

                newCodeSnip.Code = model.Code1;
                newCodeSnip.Answer = model.Answer;

                _context.CodeSnippetNoAnswers.Add(newCodeSnip);
                await _context.SaveChangesAsync();

                //save changes ^^^

                //create new post and pass in the newly saved code snippet id in as the foreign key

                Post newPost = new Post();


                newPost.CourseId = _courseId;

                newPost.PostCategory = 3;
                newPost.AssignmentId = newCodeSnip.CodeSnippetNoAnswerId;
                newPost.FolderId = folderId;

                _context.Posts.Add(newPost);


                //everytime we add a post to a folder we update the edit time of the edit col in folder
                var folderToUpdate = _context.Folders.Where(f => f.FolderId == folderId && f.CourseId == id).First();
                folderToUpdate.WhenEdited = DateTime.Now;

                await _context.SaveChangesAsync();

                //to be used for views to show activity of Curse
                UpdateCourse(id);

                TempData["Success"] = "Assignment Successfully Created!";
                return RedirectToRoute(new
                {
                    controller = "Course",
                    action = "Show",
                    id = _courseId
                });

            }
            return View(model);
        }



        // GET:
        [Authorize(Roles = "Admin")]
        [Route("/Course/{id?}/Edit/{assignmentId?}/{categoryId?}/{folderId?}", Name ="EditPost")]
        public async Task<IActionResult> Edit(int id, int? assignmentId, int? categoryId, int? folderId)
        {

            AssignmentViewModel newModel = new AssignmentViewModel();
        
            
            //an admin can only do "CUD" functionalities if he/she is the creator of the course
            if (!IsOwner(id))
            {
                return RedirectToRoute(new
                {
                    controller = "Account",
                    action = "AccessDenied"
                });
            }

            if (categoryId == 1)
            {
              newModel.MC = _context.MultipleChoices.Where(m => m.MultipleChoiceId == assignmentId).SingleOrDefault();

            }
            else if(categoryId == 2)
            {
                newModel.CodeSnippet = _context.CodeSnippets.Where(c => c.CodeSnippetId == assignmentId).SingleOrDefault();

            }
            else if(categoryId == 3)
            {
                newModel.CodeSnippetNoAnswer = _context.CodeSnippetNoAnswers.Where
                    (c => c.CodeSnippetNoAnswerId == assignmentId).SingleOrDefault();
            }
            else
            {
                return NotFound();
            }

            // ViewBag for folders
            ViewBag.Folders = _context.Folders.Where(f => f.CourseId == id);

            ViewBag.folderId = folderId;

            return View(new AssignmentViewModel
            {
                MC = newModel.MC,
                CodeSnippet = newModel.CodeSnippet,
                CodeSnippetNoAnswer = newModel.CodeSnippetNoAnswer
            });
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id?}/Edit/{assignmentId?}/{categoryId?}/MultipleChoice", Name ="SaveMultipleChoice")]
        public async Task<IActionResult> EditMultipleChoice(int id, int? assignmentId, int? categoryId, MultipleChoice post)
        {
            var folderId = Int32.Parse(Request.Form["folder"]);

            if (ModelState.IsValid)
            {
                try
                {
                    var postToUpdate = _context.Set<MultipleChoice>().Where(m => m.MultipleChoiceId == assignmentId).SingleOrDefault();
                    var postToUpdateFolderId = _context.Posts.Where(p => p.PostCategory == categoryId && p.AssignmentId == assignmentId).SingleOrDefault();


                    postToUpdate.Name = post.Name;
                    postToUpdate.Description = post.Description;
                    postToUpdate.A = post.A;
                    postToUpdate.B = post.B;
                    postToUpdate.C = post.C;
                    postToUpdate.D = post.D;
                    postToUpdate.Answer = post.Answer;
                    postToUpdate.WhenEdited = DateTime.Now;

                    postToUpdateFolderId.FolderId = folderId;

                    //everytime we add a post to a folder we update the edit time of the edit col in folder
                    var folderToUpdate = _context.Folders.Where(f => f.FolderId == folderId && f.CourseId == id).First();
                    folderToUpdate.WhenEdited = DateTime.Now;


                    //_context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                //to be used for views to show activity of Curse
                UpdateCourse(id);

                TempData["Success"] = "Assignment Successfully Edited and Saved!";

                return RedirectToRoute(new
                {
                    controller = "Post",
                    action = "Details",
                    id = id,
                    assignmentId = assignmentId,
                    categoryId = categoryId
                });
            }
            return View(post);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id?}/Edit/{assignmentId?}/{categoryId?}/CodeSnippet", Name = "SaveCodeSnippet")]
        public async Task<IActionResult> EditCodeSnippet(int id, int? assignmentId, int? categoryId, CodeSnippet post)
        {
            var folderId = Int32.Parse(Request.Form["folder"]);

            if (ModelState.IsValid)
            {
                try
                {

                    //grab post to update
                    var postToUpdate = _context.Set<CodeSnippet>().Where(c => c.CodeSnippetId == assignmentId).SingleOrDefault();
                    var postToUpdateFolderId = _context.Posts.Where(p => p.PostCategory == categoryId && p.AssignmentId == assignmentId).SingleOrDefault();


                    postToUpdate.Name = post.Name;
                    postToUpdate.Description = post.Description;
                    postToUpdate.Answer = post.Answer;
                    postToUpdate.Code = post.Code;
                    postToUpdate.WhenEdited = DateTime.Now;

                    postToUpdateFolderId.FolderId = folderId;

                    //everytime we add a post to a folder we update the edit time of the edit col in folder
                    var folderToUpdate = _context.Folders.Where(f => f.FolderId == folderId && f.CourseId == id).First();
                    folderToUpdate.WhenEdited = DateTime.Now;


                    //_context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                //to be used for views to show activity of Curse
                UpdateCourse(id);

                TempData["Success"] = "Assignment Successfully Edited and Saved!";

                return RedirectToRoute(new
                {
                    controller = "Post",
                    action = "Details",
                    id = id,
                    assignmentId = assignmentId,
                    categoryId = categoryId
                });
            }
            return View(post);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id?}/Edit/{assignmentId?}/{categoryId?}/CodeSnippetNoAnswer", Name = "SaveCodeSnippetNoAnswer")]
        public async Task<IActionResult> EditCodeSnippetNoAnswer(int id, int? assignmentId, int? categoryId, CodeSnippetNoAnswer post)
        {
            var folderId = Int32.Parse(Request.Form["folder"]);

            if (ModelState.IsValid)
            {
                try
                {
                    var postToUpdate = _context.Set<CodeSnippetNoAnswer>().Where(c => c.CodeSnippetNoAnswerId == assignmentId).SingleOrDefault();
                    var postToUpdateFolderId = _context.Posts.Where(p => p.PostCategory == categoryId && p.AssignmentId == assignmentId).SingleOrDefault();

                    postToUpdate.Name = post.Name;
                    postToUpdate.Description = post.Description;
                    postToUpdate.Code = post.Code;
                    postToUpdate.Answer = post.Answer;
                    postToUpdate.WhenEdited = DateTime.Now;

                    postToUpdateFolderId.FolderId = folderId;

                    //everytime we add a post to a folder we update the edit time of the edit col in folder
                    var folderToUpdate = _context.Folders.Where(f => f.FolderId == folderId && f.CourseId == id).First();
                    folderToUpdate.WhenEdited = DateTime.Now;
                    

                    //_context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                //to be used for views to show activity of Curse
                UpdateCourse(id);

                TempData["Success"] = "Assignment Successfully Edited and Saved!";

                return RedirectToRoute(new
                {
                    controller = "Post",
                    action = "Details",
                    id = id,
                    assignmentId = assignmentId,
                    categoryId = categoryId
                });
            }
            return View(post);
        }

        // GET: Post/Delete/5
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts
        //        .SingleOrDefaultAsync(m => m.PostId == id);




        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

            

        //    return View(post);
        //}


        // POST: Post/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("/Course/{id?}/Delete/{assignmentId?}/{categoryId?}", Name ="DeletePost")]
        public async Task<IActionResult> Delete(int id, int? assignmentId, int? categoryId)
        {

            //when we want to delete a post, we delete from the post table and the table that assignment belongs to 
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.CourseId == id && p.AssignmentId == assignmentId && p.PostCategory == categoryId);
            _context.Posts.Remove(post);

            if(categoryId == 1)
            {
                var specificAssignment = await _context.MultipleChoices.SingleOrDefaultAsync(m => m.MultipleChoiceId == assignmentId);
                _context.MultipleChoices.Remove(specificAssignment);


                //delete all the submissions for this specific assignment
                var submissionsForAssignment = _context.MultipleChoiceSubmissions.Where(ms => ms.AssignmentId == assignmentId);
                foreach(MultipleChoiceSubmission submission in submissionsForAssignment)
                {
                    _context.MultipleChoiceSubmissions.Remove(submission);
                }

                //also delete data from the associative table,
                //all submissions to the specific post assignment in the specific table
                var submissions = _context.Submissions.Where(s => s.AssignmentId == assignmentId && s.CategoryId == categoryId && s.CourseId == id);
                foreach(Submission s in submissions)
                {
                    _context.Submissions.Remove(s);
                }

                
            }
            else if(categoryId == 2)
            {
                var specificAssignment = await _context.CodeSnippets.SingleOrDefaultAsync(c => c.CodeSnippetId == assignmentId);
                _context.CodeSnippets.Remove(specificAssignment);

                //delete all the submissions for this specific assignment
                var submissionsForAssignment = _context.CodeSnippetSubmissions.Where(ms => ms.AssignmentId == assignmentId);
                foreach (CodeSnippetSubmission submission in submissionsForAssignment)
                {
                    _context.CodeSnippetSubmissions.Remove(submission);
                }

                var submissions = _context.Submissions.Where(s => s.AssignmentId == assignmentId && s.CategoryId == categoryId && s.CourseId == id);
                foreach (Submission s in submissions)
                {
                    _context.Submissions.Remove(s);
                }
            }
            else if(categoryId == 3)
            {
                var specificAssignment = await _context.CodeSnippetNoAnswers.SingleOrDefaultAsync(c => c.CodeSnippetNoAnswerId == assignmentId);
                _context.CodeSnippetNoAnswers.Remove(specificAssignment);


                //delete all the submissions for this specific assignment
                var submissionsForAssignment = _context.CodeSnippetNoAnswerSubmissions.Where(ms => ms.AssignmentId == assignmentId);
                foreach (CodeSnippetNoAnswerSubmission submission in submissionsForAssignment)
                {
                    _context.CodeSnippetNoAnswerSubmissions.Remove(submission);
                }

                var submissions = _context.Submissions.Where(s => s.AssignmentId == assignmentId && s.CategoryId == categoryId && s.CourseId == id);
                foreach (Submission s in submissions)
                {
                    _context.Submissions.Remove(s);
                }
            }


            //to be used for views to show activity of Curse
            //update course date times
            UpdateCourse(id);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Assignment Post successfully deleted!";

            return RedirectToRoute(new
            {
                controller = "Course",
                action = "Show",
                id = id
            });
        }


        [HttpPost]
        [Route("/Course/{id?}/Post/{assignmentId?}/{categoryId?}/SubmitMultipleChoice", Name = "SubmitMultipleChoice")]
        public IActionResult SubmitMultipleChoice(int id, int? assignmentId, int? categoryId, MultipleChoice submission)
        {

            //get the actual answer of the assignment
            //since we know this is only for MC, we go directly to the MC table
            MultipleChoice assignment = _context.MultipleChoices.Where(m => m.MultipleChoiceId == assignmentId).FirstOrDefault();
            var assignmentAnswer = assignment.Answer;


            //we want the user to only make one submission per assignment
            //so we check if such a submission exist
            bool subExist = _context.MultipleChoiceSubmissions.Any(m => m.AssignmentId == assignmentId && m.UserEmail == User.Identity.Name);

            //only create submission if there isnt a submission already existing
            if (!subExist)
            {



                var userSelectedAnswer = Request.Form["radio"];

                MultipleChoiceSubmission newSubmission = new MultipleChoiceSubmission
                {
                    AssignmentId = (int)assignmentId,
                    Answer = userSelectedAnswer,
                    UserEmail = User.Identity.Name,
                    IsCorrect = assignmentAnswer == userSelectedAnswer
                };

                _context.MultipleChoiceSubmissions.Add(newSubmission);

                _context.SaveChanges();
            }

            //create new submission to be used for report
            Submission newSub = new Submission
            {
                AssignmentId = (int)assignmentId,
                CategoryId = 1,
                CourseId = id,
                UserEmail = User.Identity.Name
            };

            _context.Add(newSub);
            _context.SaveChanges();

            TempData["Success"] = "Assignment Successfully Submitted!";


            //for multiple choice we dont need the actual model to be submitted
            //all we need is the actual radio button selected 
            return RedirectToRoute(new
            {
                controller = "Course",
                action = "Show",
                id = id
            });

        }

        [HttpPost]
        [Route("/Course/{id?}/Post/{assignmentId?}/{categoryId?}/SubmitCodesnippet", Name = "SubmitCodeSnippet")]
        public IActionResult SubmitCodeSnippet(int id, int? assignmentId, int? categoryId)
        {
           
            //we need to grab the code form the text area
            string code = Request.Form["Code"];

            //create instance of codecontroller to reuse code for compiling and running code
            CodeController codeController = new CodeController();
            string results = codeController.GetResults(code).Trim(); //we want to trim it from escape characters ex: \n \r, only works if they are @ end or beginning, which works perfectly with what we want

            //grab the specific assignment
            var assignment = _context.CodeSnippets.Where(c => c.CodeSnippetId == assignmentId).First();

            //check if the results is equal to the answer in the specific assignment
            bool isCorrect = false;
            if(assignment.Answer == results)
            {
                isCorrect = true;
            }



            //get userEmail
            string userEmail = User.Identity.Name;

            //create new instance of submission
            CodeSnippetSubmission newSubmission = new CodeSnippetSubmission
            {
                AssignmentId = (int)assignmentId,
                UserEmail = userEmail,
                UserCode = code,
                IsCorrect = isCorrect

            };

            _context.Add(newSubmission);
            _context.SaveChanges();

            //create new submission to be used for report
            Submission newSub = new Submission
            {
                AssignmentId = (int)assignmentId,
                CategoryId = 2,
                CourseId = id,
                UserEmail = User.Identity.Name
            };

            _context.Add(newSub);
            _context.SaveChanges();


            TempData["Success"] = "Assignment Successfully Submitted!";



            return RedirectToRoute(new
            {
                controller = "Course",
                action = "Show",
                id = id
            });
        }

        [HttpPost]
        [Route("/Course/{id?}/Post/{assignmentId?}/{categoryId?}/SubmitCodesnippetNoAnswer", Name = "SubmitCodeSnippetNoAnswer")]
        public IActionResult SubmitCodeSnippetNoAnswer(int id, int assignmentId, int categoryId)
        {

            //we need to grab the code form the text area
            string answer = Request.Form["Answer"];

            bool isCorrect = false;

            //get userEmail
            string userEmail = User.Identity.Name;

            //create new instance of submission
            CodeSnippetNoAnswerSubmission newSubmission = new CodeSnippetNoAnswerSubmission
            {
                AssignmentId = assignmentId,
                UserEmail = userEmail,
                UserAnswer = answer,
                IsCorrect = isCorrect
            };

            _context.Add(newSubmission);
            _context.SaveChanges();


            //create new submission to be used for report
            Submission newSub = new Submission
            {
                AssignmentId = assignmentId,
                CategoryId = 3,
                CourseId = id,
                UserEmail = User.Identity.Name
            };


            _context.Add(newSub);
            _context.SaveChanges();



            TempData["Success"] = "Assignment Successfully Submitted!";

            return RedirectToRoute(new
            {
                controller = "Course",
                action = "Show",
                id = id
            });
        }


        [Authorize(Roles = "Admin")]
        [Route("/Course/{id?}/Post/{assignmentId?}/{categoryId?}/Results", Name = "PostResults")]
        public IActionResult Results(int id, int? assignmentId, int? categoryId)
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


            //used by the results view
            ViewBag.isMCResult = false;
            ViewBag.isCodeSnipResult = false;
            ViewBag.isCodeSnipNoAnswerResult = false;

            List<UserResultsViewModel> userResults = new List<UserResultsViewModel>();
            int correctCount = 0;
            int incorrectCount = 0;

            //used by MC
            UserAnswersViewModel userAnswers = new UserAnswersViewModel();


            //grab all the submissions from the specific category table
            //grab from MCSubmissions table
            if (categoryId == 1)
            {
                ViewBag.isMCResult = true;

                //get current question using primary key of table
                var assignment = _context.MultipleChoices.Where(mc => mc.MultipleChoiceId == assignmentId).First();

                //for chart
                ViewBag.Question = assignment.Description;
                ViewBag.ChoiceA = assignment.A;
                ViewBag.ChoiceB = assignment.B;
                ViewBag.ChoiceC = assignment.C;
                ViewBag.ChoiceD = assignment.D;


                //get all submissions from table
                IEnumerable<MultipleChoiceSubmission> mcSubmissions = _context.MultipleChoiceSubmissions.Where(m => m.AssignmentId == assignmentId);

                
                foreach(MultipleChoiceSubmission mc in mcSubmissions)
                {

                    UserResultsViewModel currentResult = new UserResultsViewModel();

                    //get current user to get Fname and Lname
                    ApplicationUser user = getUserByEmail(mc.UserEmail);

                    currentResult.FName = user.FirstName;
                    currentResult.LName = user.LastName;
                    currentResult.Answer = mc.Answer;


                    //get counts of all results
                    switch (currentResult.Answer)
                    {
                        case "A": userAnswers.ACount++;break;
                        case "B": userAnswers.BCount++;break;
                        case "C": userAnswers.CCount++;break;
                        case "D": userAnswers.DCount++;break;
                        default: break;
                       
                    }
                        

                    currentResult.IsCorrect = mc.IsCorrect;

                    if (currentResult.IsCorrect == true)
                    {
                        correctCount++;
                    }
                    else
                    {
                        incorrectCount++;
                    }

                    userResults.Add(currentResult);

                }
            }

            //for the code snippet results, we want to show who it belongs to, their code character count, and to check if it is correct
            else if(categoryId == 2)
            {
                ViewBag.isCodeSnipResult = true;

                //get all submissions from the CodeSnipetSub table for the specific assignment
                IEnumerable<CodeSnippetSubmission> submissions = _context.CodeSnippetSubmissions.Where(cs => cs.AssignmentId == assignmentId);

                foreach(CodeSnippetSubmission sub in submissions)
                {
                    UserResultsViewModel currentResult = new UserResultsViewModel();

                    //get current user to get Fname and Lname
                    ApplicationUser user = getUserByEmail(sub.UserEmail);

                    currentResult.FName = user.FirstName;
                    currentResult.LName = user.LastName;

                    currentResult.UserCodeLength = sub.UserCode.Length;
                    currentResult.UserCode = sub.UserCode;

                    currentResult.IsCorrect = sub.IsCorrect;


                    if (currentResult.IsCorrect == true)
                    {
                        correctCount++;
                    }
                    else
                    {
                        incorrectCount++;
                    }

                    userResults.Add(currentResult);

                }
            }

            else if (categoryId == 3)
            {
                ViewBag.isCodeSnipNoAnswerResult = true;

                //get all submissions from the CodeSnipetSub table for the specific assignment
                IEnumerable<CodeSnippetNoAnswerSubmission> submissions = _context.CodeSnippetNoAnswerSubmissions.Where(cs => cs.AssignmentId == assignmentId);

                foreach (CodeSnippetNoAnswerSubmission sub in submissions)
                {
                    UserResultsViewModel currentResult = new UserResultsViewModel();

                    //get current user to get Fname and Lname
                    ApplicationUser user = getUserByEmail(sub.UserEmail);

                    currentResult.FName = user.FirstName;
                    currentResult.LName = user.LastName;

                    currentResult.Answer = sub.UserAnswer;


                    //get assignment
                    var assignment = _context.CodeSnippetNoAnswers.Where(cs => cs.CodeSnippetNoAnswerId == assignmentId).First();

                    currentResult.IsCorrect = (assignment.Answer == sub.UserAnswer);

                    if (currentResult.IsCorrect == true)
                    {
                        correctCount++;
                    }
                    else
                    {
                        incorrectCount++;
                    }

                    userResults.Add(currentResult);

                }
            }

            return View( new ResultsViewModel
            {
                UserResults = userResults,
                CorrectCount = correctCount,
                IncorrentCount = incorrectCount,
                UserAnswers = userAnswers,
                
            });
        }

        private ApplicationUser getUserByEmail(string userEmail)
        {
            return _context.Users.Where(u => u.UserName == userEmail).FirstOrDefault();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
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


        private void UpdateCourse(int id)
        {

            try
            {
            //find course and change updated at to "now"
            var course = _context.Courses.Where(c => c.CourseId == id).First();

            course.WhenEdited = DateTime.Now;

            _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
           
        }

    }
}