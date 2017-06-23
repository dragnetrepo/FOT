using System;
using System.Collections.Generic;
using Fot.Admin.Models;

namespace Fot.Admin.Infrastructure
{
    

    public class FixedDataSources
    {
        public String Text { get; set; }
        public int Value { get; set; }

        private List<FixedDataSources> GetList<TEnum>() where TEnum : struct
        {
            var items = new List<FixedDataSources>();
            foreach (int value in Enum.GetValues(typeof (TEnum)))
            {
                items.Add(new FixedDataSources
                    {
                        Text = Enum.GetName(typeof (TEnum), value).Replace("_", " "),
                        Value = value
                    });
            }
            return items;
        }


        public List<FixedDataSources> GetAssesmentTypes()
        {
            return GetList<AssessmentType>();
        }
    }
}