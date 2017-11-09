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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
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

                newPost.PostCategory = 1;
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


        // GET: Post/Edit/5'
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        //[Authorize(Roles = "Admin")]
        //// POST: Post/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Post post)
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