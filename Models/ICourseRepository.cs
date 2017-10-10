using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public interface ICourseRepository
    {
        void AddCourse(Course newCourse);

        IEnumerable<Course> Courses { get; }
    }
}
