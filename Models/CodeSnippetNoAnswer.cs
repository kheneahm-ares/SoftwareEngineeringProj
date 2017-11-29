using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class CodeSnippetNoAnswer
    {
        public CodeSnippetNoAnswer()
        {
            //everytime a course is created this will be called
            WhenCreated = DateTime.Now;
            WhenEdited = DateTime.Now; //initialize edit to now
        }
        public int CodeSnippetNoAnswerId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Code { get; set; }
        public String Answer { get; set; }
        public int PostId { get; set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenEdited { get; private set; }
    }
}
