using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class MultipleChoice
    {
        public MultipleChoice()
        {
            //everytime a course is created this will be called
            WhenCreated = DateTime.Now;
            WhenEdited = DateTime.Now; //initialize edit to now
        }
        public int MultipleChoiceId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String A { get; set; }
        public String B { get; set; }
        public String C { get; set; }
        public String D { get; set; }
        public String Answer { get; set; }
        public int PostId { get; set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenEdited { get; private set; }
    }
}
