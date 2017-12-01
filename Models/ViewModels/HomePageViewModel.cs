using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models.ViewModels
{
    public class HomePageViewModel
    {
        public CourseListViewModel CourseList { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
    }
}
