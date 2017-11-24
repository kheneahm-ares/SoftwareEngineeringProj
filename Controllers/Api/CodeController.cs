using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodingBlogDemo2.Data;
using System.IO;
using System.Diagnostics;
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
        public string GetResults(String code)
        {
            WriteToFile(code);
            CompileCode();
            string output = RunCode();

            return output;
        }

        /* Writes code to file named "Code.java" (can be written anywhere, I just chose "temp" folder I created)
         * All incoming code must match the following template (or else it will not compile):
         
                File must be named Code.java
                              |
                              V
                public class Code {
                  public static void main(String[] args) {
                    code goes here...
                  }
                }

         */
        private void WriteToFile(String code)
        {
            System.IO.File.WriteAllText("./temp/Code.java", code);
        }

        // Compiles a java file
        private void CompileCode()
        {
            // Compile code using javac
            Process process = new Process();
            process.StartInfo.FileName = "javac.exe";       // Name of process (assumes javac is executable anywhere)
            process.StartInfo.WorkingDirectory = "./temp";  // Directory where java file is located
            process.StartInfo.Arguments = "Code.java";      // Name of java file
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the error (if compile fails)
            string err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
        }

        // Runs a compiled java file
        private string RunCode()
        {
            // Run code using java
            Process process = new Process();
            process.StartInfo.FileName = "java.exe";        // Name of process
            process.StartInfo.WorkingDirectory = "./temp";  // Directory where compiled (.class) file is located
            process.StartInfo.Arguments = "Code";           // Name of java file (without .java)
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
            return output;
        }

    }
}