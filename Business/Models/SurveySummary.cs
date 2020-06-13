using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Business.Models
{
    public class SurveySummary
    {
        public string SurveyPersianName { get; set; }
        public int SurveyCount { get; set; }
        public int Point { get; set; }
    }
}
