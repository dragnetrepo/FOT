﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fot.Lan.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LanContext : DbContext
    {
        public LanContext()
            : base("name=LanContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<AssessmentPackage> AssessmentPackages { get; set; }
        public virtual DbSet<AssessmentResult> AssessmentResults { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<CandidateAssessment> CandidateAssessments { get; set; }
        public virtual DbSet<PhotoLog> PhotoLogs { get; set; }
        public virtual DbSet<RequiredAssessment> RequiredAssessments { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TestFeedback> TestFeedbacks { get; set; }
    }
}
