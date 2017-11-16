using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models.ViewModels
{
    public class CodeSnippetNoAnswerViewModel
    {
        [Required]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Description/Question")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "Code Snippet below")]
        public String Code1 { get; set; }
    }
}
