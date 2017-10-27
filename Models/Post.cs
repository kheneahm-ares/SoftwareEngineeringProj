using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }
        public int CourseId { get; set; }
    }
}
