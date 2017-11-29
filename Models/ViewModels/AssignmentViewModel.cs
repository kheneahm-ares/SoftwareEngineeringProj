using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models.ViewModels
{
    //this assignment view model is used to show each assignment or all assignments
    //if it is used to show a specific assignment then it is called by the details and only one model is defined
    //else, they are all defined and it is used by the index to show all posts to that course
    //by the details in post controller which calls the partials in the view 
    public class AssignmentViewModel
    {
        public MultipleChoice MC { get; set; }
        public CodeSnippet CodeSnippet { get; set; }
        public CodeSnippetNoAnswer CodeSnippetNoAnswer { get; set; }



        //these are enumerable sets of each model
        //this will be used to show all models respectively
        public IEnumerable<MultipleChoice> MultipleChoices { get; set; }
        public IEnumerable<CodeSnippet> CodeSnippets { get; set; }
        public IEnumerable<CodeSnippetNoAnswer> CodeSnippetNoAnswers { get; set; }
    }
}
