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
        private int _courseId;

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
        public IActionResult Create(int courseId)
        {
            _courseId = courseId;
            ViewBag.Categories = _categoryRepo.Categories;
            return View();
        }

        // POST: Post/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel category, int courseId)
        {
            if (ModelState.IsValid)
            {
                //post.CourseId = _courseId;
                //_context.Add(post);
                //await _context.SaveChangesAsync();
                //return RedirectToRoute(new
                //{
                //    controller = "Profile",
                //    action = "Index"
                //});

                //Post newPost = new Post();
                //newPost.CourseId = _courseId; //initialized at (get) create
                //newPost.

                String CategoryId = Request.Form["Category"];
                //if (Request.Form["category"] == "1")
                //{
                //    return RedirectToAction("Assignment",);
                //}
            }
            return View(category);
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