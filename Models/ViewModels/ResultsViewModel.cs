using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBlogDemo2.Models.ViewModels
{
    public class ResultsViewModel
    {
        public IEnumerable<UserResultsViewModel> UserResults { get; set; }
        public UserAnswersViewModel UserAnswers { get; set; }
        public int CorrectCount { get; set; }
        public int IncorrentCount { get; set; }
    }
}
