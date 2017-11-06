using CodingBlogDemo2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                return _context.Categories;
            }
        }
    }
}
