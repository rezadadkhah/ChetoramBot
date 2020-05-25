using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Models
{
    public class UserSurvey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int VoterUserId { get; set; }
        public int ConsideredUserId { get; set; }
        public int SurveyId { get; set; }
        public int Point { get; set; }
    }
}
