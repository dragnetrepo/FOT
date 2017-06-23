using Fot.Admin.Infrastructure;

namespace Fot.Admin.Models
{
    public class AssessmentBundleEntryViewModel
    {
        public int EntryId { get; set; }
        public string Name { get; set; }
        public AssessmentType AssessmentType { get; set; }

    }
}