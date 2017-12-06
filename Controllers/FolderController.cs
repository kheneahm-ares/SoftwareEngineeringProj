using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodingBlogDemo2.Data;
using CodingBlogDemo2.Models;
using Microsoft.AspNetCore.Authorization;
using CodingBlogDemo2.Models.ViewModels;

namespace CodingBlogDemo2.Controllers
{
    public class FolderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int _courseId;

        public FolderController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Folder
        [Route("/Course/{id}/Folder")]
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.course = _context.Courses.Where(c => c.CourseId == id).SingleOrDefault();
            return View(await _context.Folders.Where(f => f.CourseId == id).ToListAsync());
        }

        // GET: Folder/Details/5
        [Route("/Course/{courseId}/Folder/{id}/Details/", Name = "FolderDetails")]
        public IActionResult Details(int? id, int courseId)
        {
            if (id == null)
            {
                return NotFound();
            }

            AssignmentViewModel assignmentViewModel = new AssignmentViewModel();

            // Get all posts for this course and this folder
            IEnumerable<Post> posts = _context.Posts.Where(p => p.CourseId == courseId && p.FolderId == id);
            // Store posts by their type
            List<MultipleChoice> multipleChoice = new List<MultipleChoice>();
            List<CodeSnippet> codeSnippet = new List<CodeSnippet>();
            List<CodeSnippetNoAnswer> codeSnippetNoAnswer = new List<CodeSnippetNoAnswer>();

            // Separate posts by type, adding them to their respective lists
            foreach (Post post in posts)
            {               
                if (post.PostCategory == 1)
                {
                    multipleChoice.Add(_context.MultipleChoices.Where(m => m.MultipleChoiceId == post.AssignmentId).SingleOrDefault());
                }       
                else if (post.PostCategory == 2)
                {
                    codeSnippet.Add(_context.CodeSnippets.Where(c => c.CodeSnippetId == post.AssignmentId).SingleOrDefault());
                }
                else if (post.PostCategory == 3)
                {
                    codeSnippetNoAnswer.Add(_context.CodeSnippetNoAnswers.Where
                        (c => c.CodeSnippetNoAnswerId == post.AssignmentId).SingleOrDefault());
                }
            }

            ViewBag.courseId = courseId;

            if (id == 0)
            {
                ViewBag.folder = new Folder { FolderId = 0, Name = "Other", CourseId = courseId };
            }
            else
            {
                ViewBag.folder = _context.Folders.Where(f => f.FolderId == id).SingleOrDefault();
            }        

            return View(new AssignmentViewModel
            {
                MultipleChoices = multipleChoice,
                CodeSnippets = codeSnippet,
                CodeSnippetNoAnswers = codeSnippetNoAnswer
            });
        }

        // GET: Folder/Create
        [Authorize(Roles = "Admin")]
        [Route("/Course/{id}/Folder/Create")]
        public IActionResult Create(int id)
        {
            _courseId = id;
            return View();
        }

        // POST: Folder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{id}/Folder/Create")]
        public async Task<IActionResult> Create(Folder folder)
        {
            if (ModelState.IsValid)
            {
                Folder f = new Folder
                {
                    CourseId = _courseId,
                    Name = folder.Name
                };

                _context.Folders.Add(f);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Folder Successfully Created!";
                return RedirectToAction("Index", new { id = _courseId });
            }
            return View(folder);
        }

        // GET: Folder/Edit/5
        [Authorize(Roles = "Admin")]
        [Route("/Course/{courseId}/Folder/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id, int courseId)
        {
            _courseId = courseId;

            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders.SingleOrDefaultAsync(m => m.FolderId == id);
            if (folder == null)
            {
                return NotFound();
            }
            return View(folder);
        }

        // POST: Folder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Course/{courseId?}/Folder/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Folder folder)
        {
            if (id != folder.FolderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolderExists(folder.FolderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = "Folder Successfully Updated!";
                return RedirectToAction("Index", new { id = _courseId });
            }
            return View(folder);
        }

        // POST: Folder/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int courseId)
        {
            var folder = await _context.Folders.SingleOrDefaultAsync(m => m.FolderId == id);
            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Folder Successfully Deleted!";
            return RedirectToAction("Index", new { id = courseId });
        }

        private bool FolderExists(int id)
        {
            return _context.Folders.Any(e => e.FolderId == id);
        }
    }
}
