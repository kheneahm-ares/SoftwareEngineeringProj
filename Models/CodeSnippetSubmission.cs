using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class CodeSnippetSubmission
    {

        public CodeSnippetSubmission()
        {
            //everytime a course is created this will be called
            WhenCreated = DateTime.Now;
            WhenEdited = DateTime.Now; //initialize edit to now
        }
        public int CodeSnippetSubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public string UserEmail { get; set; }
        public string UserCode { get; set; }
        public bool IsCorrect { get; set; }

        public DateTime WhenCreated { get; private set; }
        public DateTime WhenEdited { get; private set; }
    }
}
