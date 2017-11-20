using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class MultipleChoiceSubmission
    {
        public MultipleChoiceSubmission()
        {
            //everytime a course is created this will be called
            WhenCreated = DateTime.Now;
            WhenEdited = DateTime.Now; //initialize edit to now
        }
        public int MultipleChoiceSubmissionId { get; set; }

        public int AssignmentId { get; set; }
        public String UserEmail { get; set; }
        public String Answer { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenEdited { get; private set; }
    }
}
