using CodingBlogDemo2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> Posts
        {
            get
            {
                return _context.Posts;
            }
        }

        public void AddPost(Post newPost)
        {
            _context.Posts.Add(newPost);
            _context.SaveChanges();
        }
    }
}
