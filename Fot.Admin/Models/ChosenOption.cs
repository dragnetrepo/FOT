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
    using System.Collections.Generic;
    
    public partial class ChosenOption
    {
        public int EntryId { get; set; }
        public Nullable<int> ShowQuestionEntryId { get; set; }
        public Nullable<int> AnswerId { get; set; }
    
        public virtual ShownQuestion ShownQuestion { get; set; }
    }
}
