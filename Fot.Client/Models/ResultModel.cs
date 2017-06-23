using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Client.Models
{
    public class ResultModel
    {
        public string AssessmentName { get; set; }
        public int Score { get; set; }
    }




    public class ResultResponse
    {
        public bool Succeeded { get; set; }
        public List<ResultModel> resultList { get; set; }

        public string ErrorMessage { get; set; }


        public ResultResponse()
        {
            resultList = new List<ResultModel>();
        }
    }
}