using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models.ViewModels
{
    //this model is used when creating a category
    public class CategoryViewModel
    {

        [Required(ErrorMessage = "Please enter the type of Post")]
        [Display(Name = "Choose Post Category:")]
        public IEnumerable<Category> Categories { get; set; }
    }
}
