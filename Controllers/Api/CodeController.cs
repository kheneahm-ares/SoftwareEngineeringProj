using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodingBlogDemo2.Data;

namespace CodingBlogDemo2.Controllers.Api
{
    public class CodeController : Controller
    {
        //in this api controller, we will grab the code from text area, run the code, then return the output

        private ApplicationDbContext _context;

        public CodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/api/compile/{code?}")]
        public string CompileCode(String code)
        {
            //we are assuming that the code is wholesome, meaning it can just be ran with javac as is
            
            //before anything, we need to know what operating system this application is hosted on
            
            return code;
        }


        
    }
}