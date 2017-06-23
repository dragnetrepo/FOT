using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fot.Admin.Services;
using Hangfire;
using Newtonsoft.Json;

namespace Fot.Admin.Infrastructure
{
    public class Processor
    {
        private static List<QuestionModel> questions { get; set; }


        public static async Task ProcessSummary()
        {
            var ctx = new ServiceBase().Context;

            ctx.Database.CommandTimeout = 0;

            questions = ctx.AssessmentQuestions.Select(x => new QuestionModel
            {
                QuestionId = x.QuestionId,
                Answers = x.AssessmentAnswers.Where(t => t.IsCorrect).Select(y => y.AnswerId)
            }).ToList();


            while (ctx.Database.SqlQuery<int>("Select count(*) from ShownQuestion where Correct Is Null").First() > 0) // (ShownQuestions.Any(x => x.Correct.HasValue == false))
            {

                var list = await ctx.ShownQuestions.Where(x => x.Correct.HasValue == false)
                    .Select(x => new QuestionModel { QuestionId = x.QuestionId.Value, EntryId = x.EntryId, Answers = x.ChosenOptions.Select(y => y.AnswerId.Value) }).Take(5000).ToListAsync();


                StringBuilder b = new StringBuilder();
                foreach (var item in list)
                {


                    bool? flag = Correct(item);

                    int? val = flag.HasValue ? (flag.Value ? 1 : 0) : default(int?);

                    if (val.HasValue)
                    {

                        b.Append(string.Format("UPDATE ShownQuestion set Correct = {0} where EntryId = {1};", val, item.EntryId));
                    }
                    else
                    {
                        b.Append(string.Format("DELETE FROM ShownQuestion where EntryId = {0};", item.EntryId));
                    }


                }

                ctx.Database.ExecuteSqlCommand(b.ToString());



            }




        }




        private static bool? Correct(QuestionModel question)
        {

            var first = questions.FirstOrDefault(x => x.QuestionId == question.QuestionId);

            if (first != null)
            {
                var list1 = first.Answers.OrderBy(x => x).ToList();

                var list2 = question.Answers.OrderBy(x => x).ToList();

                return Enumerable.SequenceEqual(list1, list2);
            }
            else
            {
                // question.QuestionId.Dump();
                return default(bool?);
            }



        }



    }

    public class QuestionModel
    {
        public int EntryId { get; set; }
        public int QuestionId { get; set; }

        public IEnumerable<int> Answers { get; set; }

    }
}