using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models.ViewModels
{
    public class UserResultsViewModel
    {
        public String FName { get; set; }
        public String LName { get; set; }
        public bool IsCorrect { get; set; }

        //used by MC
        public String Answer { get; set; }

        //used by CodeSnippet
        public int UserCodeLength { get; set; }
        public string UserCode { get; set; }

    }
}
