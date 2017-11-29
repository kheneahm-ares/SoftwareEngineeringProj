using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class Submission
    {
        public Submission()
        {
            DateCreated = DateTime.Now;
        }

        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public int CategoryId { get; set; }
        public int CourseId { get; set; }
        public string UserEmail { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
