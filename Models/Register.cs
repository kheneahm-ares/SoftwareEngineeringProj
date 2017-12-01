using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class Register
    {

        public Register()
        {
            //everytime a course is created this will be called
            WhenCreated = DateTime.Now;
            WhenEdited = DateTime.Now; //initialize edit to now
        }
        public int RegisterId { get; set; }
        public string UserEmail { get; set; }
        public int CourseId { get; set; }
        public DateTime WhenCreated { get; set; }
        public DateTime WhenEdited { get; set; }

        //[ForeignKey("UserEmail")]
        //public virtual ApplicationUser User { get; set; }
    }
}
