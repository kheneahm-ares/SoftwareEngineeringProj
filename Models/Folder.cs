using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class Folder
    {
        public Folder()
        {
            //everytime a course is created this will be called
            WhenCreated = DateTime.Now;
            WhenEdited = DateTime.Now; //initialize edit to now
        }
        public int FolderId { get; set; }
        public String Name { get; set; }
        public int CourseId { get; set; }
        public DateTime WhenCreated { get; set; }
        public DateTime WhenEdited { get; set; }
    }
}
