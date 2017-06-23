namespace AuthorApp.Models
{
   public class AnswerViewModel
    {
        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

        public byte[] AnswerImage { get; set; }

        public string AnswerType { get; set; }

        public bool IsCorrect { get; set; }

        public bool IsImage { get; set; }
    }
}
