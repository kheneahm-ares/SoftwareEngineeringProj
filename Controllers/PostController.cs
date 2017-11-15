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
                    //codensippnoresult
                }
            }

            return View(new AssignmentViewModel
            {
                MultipleChoices = mcs,
                CodeSnippets = codeSnips
            });
        }

        // GET: Post/Details/5
        [Route("/Course/{id}/Post/{assignmentId?}/{categoryId?}", Name = "PostDetails")]
        public IActionResult Details(int? id, int? assignmentId, int? categoryId)
        {
            //NOTE THAT assignmentId != postId, postId is the id for the Post table whereas assignmentId is the actual instance of the assignment post

            //define an view model that will be used by the view
            AssignmentViewModel newModel = new AssignmentViewModel();

            //we will use this in our views, instead of creating another view model
            ViewBag.courseId = id;
            ViewBag.categoryId = categoryId;

            //we first check whether there exists an assignment based on the assignmentId, categoryId, and courseId
            var coursesPosts = _context.Posts.Where(p => p.CourseId == id && p.AssignmentId == assignmentId && p.PostCategory == p.PostCategory);
            
            if(coursesPosts.Count() == 0)
            {
                return NotFound();
            }

            //if requests for a category of type Multiple Choice
            if (categoryId == 1)
            {
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

                var codeSnip = _context.CodeSnippets.Where(c => c.CodeSnippetId == assignmentId).SingleOrDefault();
                newModel.CodeSnippet = codeSnip;

                return View(new AssignmentViewModel
                {
                    CodeSnippet = newModel.CodeSnippet
                });
            }

            //if requests for category of type code snippet without an answer
            else if(categoryId == 3)
            {

            }


            return NotFound();
             


        }


        // GET: Post/Create
        [Authorize(Roles = "Admin")]
        [Route("/Course/{id}/Create")]
        public IActionResult Create(int id)
        {
            _courseId = id;
            ViewBag.Categories = _categoryRepo.Categories;
            return View();
        }

        // POST: Post/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id}/Create/MultipleChoice", Name ="MultipleChoice")]
        public async Task<IActionResult> CreateMultipleChoice(MultipleChoiceViewModel model)
        {
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

                _context.Posts.Add(newPost);
                await _context.SaveChangesAsync();

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



        // POST: Post/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id}/Create/CodeSnippet", Name = "CodeSnippet")]
        public async Task<IActionResult> CreateCodeSnippet(CodeSnippetViewModel model)
        {
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

                _context.Posts.Add(newPost);
                await _context.SaveChangesAsync();

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
        [Route("/Course/{id?}/Edit/{assignmentId?}/{categoryId?}", Name ="EditPost")]
        public async Task<IActionResult> Edit(int? id, int? assignmentId, int? categoryId)
        {

            AssignmentViewModel newModel = new AssignmentViewModel();
            if (id == null)
            {
                return NotFound();
            }
           

            if(categoryId == 1)
            {
              newModel.MC = _context.MultipleChoices.Where(m => m.MultipleChoiceId == assignmentId).SingleOrDefault();

            }
            else if(categoryId == 2)
            {
                newModel.CodeSnippet = _context.CodeSnippets.Where(c => c.CodeSnippetId == assignmentId).SingleOrDefault();

            }
            else if(categoryId == 3)
            {
                //do something for no result cs
            }
            else
            {
                return NotFound();
            }

            return View(new AssignmentViewModel
            {
                MC = newModel.MC,
                CodeSnippet = newModel.CodeSnippet
            });
            
        }

        //[Authorize(Roles = "Admin")]
        //// POST: Post/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditMultipleChoice(int id, MultipleChoice post)
        //{
        //    if (id != post.PostId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var postToUpdate = _context.Set<Post>().Where(c => c.PostId == id).SingleOrDefault();

        //            postToUpdate.Title = post.Title;
        //            postToUpdate.Description = post.Description;

        //            //_context.Update(post);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PostExists(post.PostId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToRoute(new
        //        {
        //            controller = "Profile",
        //            action = "Index"
        //        });
        //    }
        //    return View(post);
        //}

        // GET: Post/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .SingleOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.PostId == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToRoute(new
            {
                controller = "Profile",
                action = "Index"
            });
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}