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
        [BindNever]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please enter the name of Course" )]
        [Display(Name = "Course name:")]
        [StringLength(50)]
        public string Name { get; set; }

        public string UserEmail { get; set; }
    }
}
