using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AuthorApp.Infrastructure;
using AuthorApp.Models;


namespace AuthorApp.Services
{
    public class QuestionDifficultyLevelService : ServiceBase
    {
        public IQueryable<QuestionDifficultyLevel> Levels
        {
            get { return Context.QuestionDifficultyLevels; }
        }


        public List<QuestionDifficultyLevel> GetLevels(int AssessmentId)
        {
            return Levels.Where(x => x.AssessmentId == AssessmentId).OrderByDescending(x => x.LevelId).ToList();

        }

        public List<QuestionDifficultyLevel> GetLevels(int AssessmentId, int startRow, int maxRows)
        {
            return Levels.Where(x => x.AssessmentId == AssessmentId).OrderByDescending(x => x.LevelId).Skip(startRow).Take(maxRows).ToList();

        }

        public int Count(int AssessmentId)
        {
            return Levels.Count(x => x.AssessmentId == AssessmentId);
        }


        public QuestionDifficultyLevel GetLevel(int LevelId)
        {
            return Levels.FirstOrDefault(x => x.LevelId == LevelId);
        }

        public AppMessage Add(QuestionDifficultyLevel item)
        {
            if(Exists(item.AssessmentId, item.LevelWeight) || Exists(item.AssessmentId, item.LevelName))
            {
                return new AppMessage() { IsDone = false, Message = "A difficulty level already exists with specified scale or name.", Status = MessageStatus.Error };
            }

            try
            {
                Context.QuestionDifficultyLevels.Add(item);
                Context.SaveChanges();

                return new AppMessage() { IsDone = true, Message = "Added difficulty level successfully.", Status = MessageStatus.Success };
            }

            catch (Exception ex)
            {
                return new AppMessage() { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }


        public bool Exists(int assessmentId, int scale)
        {
            return Context.QuestionDifficultyLevels.Any(x => x.AssessmentId == assessmentId && x.LevelWeight == scale);

        }

        public bool Exists(int assessmentId, string LevelName)
        {
            return Context.QuestionDifficultyLevels.Any(x => x.AssessmentId == assessmentId && x.LevelName.Equals(LevelName));

        }

        public bool ExistsExcept(int assessmentId, int scale, int LevelId)
        {
            return Context.QuestionDifficultyLevels.Any(x => x.AssessmentId == assessmentId && x.LevelWeight == scale && x.LevelId != LevelId);

        }

        public bool ExistsExcept(int assessmentId, string LevelName, int LevelId)
        {
            return Context.QuestionDifficultyLevels.Any(x => x.AssessmentId == assessmentId && x.LevelName.Equals(LevelName) && x.LevelId != LevelId);

        }


        public AppMessage Update(QuestionDifficultyLevel item)
        {
            if (ExistsExcept(item.AssessmentId, item.LevelWeight, item.LevelId) || ExistsExcept(item.AssessmentId, item.LevelName, item.LevelId))
            {
                return new AppMessage() { IsDone = false, Message = "A difficulty level already exists with specified scale or name.", Status = MessageStatus.Error };
            }

            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage() { IsDone = true, Message = "Updated difficulty level successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage() { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public void Delete(int LevelId)
        {
            var item = Context.QuestionDifficultyLevels.Find(LevelId);

            if (item != null)
            {
                Context.QuestionDifficultyLevels.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}