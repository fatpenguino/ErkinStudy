using System.Collections.Generic;

namespace ErkinStudy.Web.Models
{
    public class QuestionAnswerModel
    {
        public long QuestionId { get; set; }
        public List<long> Answers { get; set; } = new List<long>();
    }
}
