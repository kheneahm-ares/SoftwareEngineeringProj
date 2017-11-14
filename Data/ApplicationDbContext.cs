using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CodingBlogDemo2.Models;

namespace CodingBlogDemo2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MultipleChoice> MultipleChoices { get; set; }
        public DbSet<CodeSnippet> CodeSnippets { get; set; }
        public DbSet<CodeSnippetNoAnswer> CodeSnippetNoAnswers { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }



        public DbSet<CodingBlogDemo2.Models.CodeSnippet> CodeSnippet { get; set; }
    }
}
