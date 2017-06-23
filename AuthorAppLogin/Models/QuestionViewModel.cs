namespace AuthorApp.Models
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }

        public string AnswerType { get; set; }

        public int OptionCount { get; set; }

        public byte[] QuestionImage { get; set; }

        public bool ValidQuestion { get; set; }

        public bool Approved { get; set; }
    }
}
