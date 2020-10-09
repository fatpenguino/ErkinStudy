using System.Collections.Generic;

namespace ErkinStudy.Infrastructure.DTOs.Quiz
{
    public class TotalRatingDto
    {
        public long FolderId { get; set; }
        public string FolderName { get; set; }
        public List<long> QuizIds { get; set; } = new List<long>();
        public List<string> QuizTitles { get; set; } = new List<string>();
        public List<UserScoreDto> UserScores { get; set; }
    }

    public class UserScoreDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public Dictionary<long, int> Scores { get; set; }
        public int TotalPoint { get; set; }
}

}

