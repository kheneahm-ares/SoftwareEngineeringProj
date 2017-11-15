using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models
{
    public class CodeSnippetNoAnswer
    {
        public int CodeSnippetNoAnswerId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Code { get; set; }
        public int PostId { get; set; }
    }
}
