using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class Report
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int Submissions { get; set; }
        public DateTime SubmissionTime { get; set; }

        public int CoursePostTotal { get; set; }
        public double SubmissionActivityPercentage { get; set; }

        public string UserEmail { get; set; }
    }
}
