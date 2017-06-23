using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class ResultReviewModel
    {
        public int AssessmentId { get; set; }
        public int QuestionId { get; set; }

        public int? TopicId { get; set; }

        public bool? Correct { get; set; }

        public List<int> Options { get; set; } 


        public ResultReviewModel()
        {
            Options = new List<int>();
        }


        public static List<ResultReviewModel> ToResultReviewList(string input, int assessmentId)
        {
            var list = new List<ResultReviewModel>();

            var array = input.Split(';');

            foreach (var s in array)
            {
                var temp = new ResultReviewModel();
                var tempArray = s.Split(':');

                temp.QuestionId = Int32.Parse(tempArray[0]);
                temp.AssessmentId = assessmentId;

                var TempOptions = tempArray[1].Split(',');

                foreach (var tempOption in TempOptions)
                {
                    temp.Options.Add(Int32.Parse(tempOption));
                }

                list.Add(temp);
            }



            return list;
        } 
    }



   
}