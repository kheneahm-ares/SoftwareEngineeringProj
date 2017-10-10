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
            var user = _appDbContext.Users.SingleOrDefault(p => p.Email == userEmail);

            return user.FirstName;
            
            throw new NotImplementedException();
        }

        public bool IsAdmin(string userEmail)
        {
            var user  = _appDbContext.Users.SingleOrDefault(p => p.Email == userEmail);

            return user.IsAdmin;
        }



        public ApplicationUser getUserByEmail(string userEmail)
        {
            return _appDbContext.Users
                .Where(p => p.Email == userEmail)
                .FirstOrDefault();
        }
    }
}
