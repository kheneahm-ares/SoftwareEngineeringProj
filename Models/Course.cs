using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class Course
    {

        public Course()
        {

            //everytime a course is created this will be called
            WhenCreated = DateTime.Now;
            WhenEdited = DateTime.Now; //initialize edit to now
        }
        [BindNever]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please enter the name of Course" )]
        [Display(Name = "Course name:")]
        [StringLength(50)]
        public string Name { get; set; }

        public string UserEmail { get; set; }

        public DateTime WhenCreated { get; set; }
        public DateTime WhenEdited { get; set; }
    }
}
