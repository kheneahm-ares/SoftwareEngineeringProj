using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class MultipleChoiceSubmission
    {
        public int MultipleChoiceSubmissionId { get; set; }

        public int AssignmentId { get; set; }
        public String UserEmail { get; set; }
        public String Answer { get; set; }
        public bool IsCorrect { get; set; }


    }
}
