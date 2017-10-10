using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public interface IAccountRepository
    {
        String getUserFName(string userEmail);
        bool IsAdmin(string userEmail);

        ApplicationUser getUserByEmail(string userEmail);
    }
}
