using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{

    //this is used to show all the posts of the current course
    public class CourseViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<MultipleChoice> MultipleChoices { get; set; }
        public Course Course { get; set; }

    }
}
