namespace ErkinStudy.Infrastructure.Helpers
{
    public static class UtilHelper
    {
        public static string RemoveInputMaskFromPhoneNumber(string phoneNumber)
        {
            return phoneNumber?.Replace("+", "").Replace("(", "").Replace(")","").Replace("-", "");
        }

        public static bool CheckUserAnswer(string answer, string correctAnswer)
        {
            return FormatAnswer(answer) == FormatAnswer(correctAnswer);
        }

        public static string FormatAnswer(string answer)
        {
            return answer.Replace(" ", "").ToLower();
        }
    }
}
