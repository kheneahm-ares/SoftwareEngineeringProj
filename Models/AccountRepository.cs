using CodingBlogDemo2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class AccountRepository : IAccountRepository
    {
        private ApplicationDbContext _appDbContext;

        public AccountRepository(ApplicationDbContext context)
        {
            _appDbContext = context;
        }

        public String getUserFName(string userEmail)
        {
            var userFName = _appDbContext.Users.SingleOrDefault(p => p.Email == userEmail);

            return userFName.FirstName;
            
            throw new NotImplementedException();
        }
    }
}
