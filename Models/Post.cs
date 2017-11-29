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
        public int CourseId { get; set; }

        //category instead of type because type is so widely used in programming,
        //dont want system to get confused
        public int PostCategory { get; set; }


        //the id in each of the assignment types
        public int AssignmentId { get; set; }


    }
}
