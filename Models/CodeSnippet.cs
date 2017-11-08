using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class CodeSnippet
    {
        public int CodeSnippetId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Code { get; set; }
        public String Answer { get; set; }
        public int PostId { get; set; }
    }
}
