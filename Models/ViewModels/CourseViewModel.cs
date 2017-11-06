using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class CourseViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public Course Course { get; set; }

    }
}
