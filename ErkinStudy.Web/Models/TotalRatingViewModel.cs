using System.Collections.Generic;

namespace ErkinStudy.Web.Models
{
    public class TotalRatingViewModel
    {
        public long FolderId { get; set; }
        public string FolderName { get; set; }
        public long[] QuizIds { get; set; }
        public string[] QuizTitles { get; set; }
        public List<UserScoreViewModel> UserScores { get; set; }
    }

    public class UserScoreViewModel
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public Dictionary<long, int> Scores { get; set; }
        public int TotalPoint { get; set; }
    }
}
