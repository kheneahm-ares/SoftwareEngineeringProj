using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public interface IPostRepository
    {
        void AddPost(Post newPost);

        IEnumerable<Post> Posts { get; }
    }
}
