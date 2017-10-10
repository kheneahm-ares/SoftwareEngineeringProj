using CodingBlogDemo2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodingBlogDemo2.Models
{
    public class CourseRepository : ICourseRepository
    {
        private ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public IEnumerable<Course> Courses
        {
            get
            {
                return _context.Courses;
            }
        }
        public void AddCourse(Course newCourse)
        {

     
            _context.Courses.Add(newCourse);
            _context.SaveChanges();
        }
    }
}
