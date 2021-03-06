//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fot.Admin.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FotContext : DbContext
    {
        public FotContext()
            : base("name=FotContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<AssessmentAnswer> AssessmentAnswers { get; set; }
        public DbSet<AssessmentBundle> AssessmentBundles { get; set; }
        public DbSet<AssessmentBundleEntry> AssessmentBundleEntries { get; set; }
        public DbSet<AssessmentOutputConfig> AssessmentOutputConfigs { get; set; }
        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; }
        public DbSet<AssessmentResult> AssessmentResults { get; set; }
        public DbSet<AssessmentTopic> AssessmentTopics { get; set; }
        public DbSet<AuthorAssignedAssessment> AuthorAssignedAssessments { get; set; }
        public DbSet<CampaignEntry> CampaignEntries { get; set; }
        public DbSet<CandidateAssessment> CandidateAssessments { get; set; }
        public DbSet<CandidateFeedback> CandidateFeedbacks { get; set; }
        public DbSet<Center> Centers { get; set; }
        public DbSet<ChosenOption> ChosenOptions { get; set; }
        public DbSet<EssayTopic> EssayTopics { get; set; }
        public DbSet<FeedbackType> FeedbackTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerAssignedAssessment> PartnerAssignedAssessments { get; set; }
        public DbSet<PartnerWalletDebit> PartnerWalletDebits { get; set; }
        public DbSet<PartnerWalletEntry> PartnerWalletEntries { get; set; }
        public DbSet<QuestionDifficultyLevel> QuestionDifficultyLevels { get; set; }
        public DbSet<QuestionGroup> QuestionGroups { get; set; }
        public DbSet<ScheduleDownload> ScheduleDownloads { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ShownQuestion> ShownQuestions { get; set; }
        public DbSet<TestSession> TestSessions { get; set; }
        public DbSet<PartnerAssignedCenter> PartnerAssignedCenters { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }
        public DbSet<CampaignSession> CampaignSessions { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateScheduleResponse> CandidateScheduleResponses { get; set; }
        public DbSet<EmailBatch> EmailBatches { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }
        public DbSet<MessageBatch> MessageBatches { get; set; }
        public DbSet<MessageQueue> MessageQueues { get; set; }
        public DbSet<CenterUser> CenterUsers { get; set; }
        public DbSet<TestDayPhoto> TestDayPhotoes { get; set; }
        public DbSet<FixedQuestion> FixedQuestions { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<TestFeedback> TestFeedbacks { get; set; }
        public DbSet<AssessmentAuthor> AssessmentAuthors { get; set; }
        public DbSet<PhotoLog> PhotoLogs { get; set; }
    }
}
