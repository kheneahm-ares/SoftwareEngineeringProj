using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    //this is used by profile page to show all courses
    public class CourseListViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public string ProfessorName { get; set; }

    }
}
