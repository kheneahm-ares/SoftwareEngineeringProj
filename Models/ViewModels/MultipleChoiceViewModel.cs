using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models.ViewModels
{
    public class MultipleChoiceViewModel
    {
        public int PostId { get; set; }
        public int CategoryType { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        [Display(Name="Description/Question")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "Choice A")]
        public String A { get; set; }

        [Required]
        [Display(Name = "Choice B")]
        public String B { get; set; }

        [Required]
        [Display(Name = "Choice C")]
        public String C { get; set; }

        [Required]
        [Display(Name = "Choice D")]
        public String D { get; set; }

        [Required]
        public Char Answer { get; set; }
    }
}
