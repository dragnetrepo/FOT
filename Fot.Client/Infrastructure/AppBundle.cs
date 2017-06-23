using System;

namespace Fot.Client.Infrastructure
{
    [Serializable]
    public class AssessmentQuestion
    {
        public string answer_type { get; set; }
        public AssessmentAnswer[] answers { get; set; }
        public byte[] item_img { get; set; }
        public int question_id { get; set; }
        public string question_text { get; set; }
        public bool options_layout_is_vertical { get; set; }

        public bool seen { get; set; }
        public bool answered { get; set; }
    }

    [Serializable]
    public class AppBundle
    {
        public AppAssessment[] assessments { get; set; }
        public byte[] bundle_icon { get; set; }
        public byte[] candidate_photo { get; set; }
        public int bundle_id { get; set; }
        public string bundle_name { get; set; }
        public bool save_as_you_go { get; set; }
        public bool show_results_on_submit { get; set; }
        public bool flagsuccess { get; set; }
        public string error_message { get; set; }
        public int? min_aggregate_score { get; set; }

        public int current_assessment_index { get; set; }
    }

    [Serializable]
    public class AppAssessment
    {
        public int assessment_id { get; set; }
        public string assessment_name { get; set; }
        public int duration { get; set; }
        public string assessment_type { get; set; }
        public byte[] instruction_image { get; set; }
        public AssessmentQuestion[] questions { get; set; }
        public AppEssay[] essays { get; set; }
        public bool timed { get; set; }
        public bool show_calculator { get; set; }

        public bool started { get; set; }

        public bool completed { get; set; }
        public int time_remaining { get; set; }
        public int current_question_index { get; set; }
    }

    [Serializable]
    public class AssessmentAnswer
    {
        public int answer_id { get; set; }
        public byte[] answer_image { get; set; }
        public string answer_text { get; set; }
        public bool is_image { get; set; }
        public bool selected { get; set; }
    }

    [Serializable]
    public class AppEssay
    {
        public int essay_id { get; set; }
        public string candidate_response { get; set; }
        public bool selected { get; set; }

        public string topic { get; set; }
    }


    public class AssessmentResponse
    {
        public int assessment_id { get; set; }
        public bool is_essay { get; set; }
        public string result { get; set; }

        public int essay_id { get; set; }

    }
}