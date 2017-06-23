using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Lan.Models
{
    [Serializable]
    public class QuestionStatus
    {
        public AnswerStatus[] answers;
        public int question_id;
        public bool seen;
        public bool answered;
    }


    [Serializable]
    public class BundleStatus
    {
        public AssessmentStatus[] assessments;
        public int bundle_id;
        public int current_assessment_index;
    }

    [Serializable]
    public class AssessmentStatus
    {
        public int assessment_id;
        public QuestionStatus[] questions;

        public bool started;
        public int time_remaining;
        public int current_question_index;

        public EssayStatus essayStatus;
    }

    [Serializable]
    public class EssayStatus
    {
        public int essay_id;
        public string candidate_response;
    }


    [Serializable]
    public class AnswerStatus
    {
        public int answer_id;
        public bool selected;
    }
}