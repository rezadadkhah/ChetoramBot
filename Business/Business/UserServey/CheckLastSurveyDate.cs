using Business.Common.Enums;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Business.User
{
    public class CheckLastSurveyDate : BusinessBase<bool>
    {
        private readonly int voterUserId;
        private readonly int consideredUserId;

        public CheckLastSurveyDate(int VoterUserId, int ConsideredUserId)
        {
            voterUserId = VoterUserId;
            consideredUserId = ConsideredUserId;
        }

        public override void Execute()
        {
            ////var userSurvey = Context.UserServey.LastOrDefault(u => u.VoterUserId == voterUserId && u.ConsideredUserId == consideredUserId);
            //if (userSurvey == null)
            //{
            //    Result = true;
            //    return;
            //}
            //if (userSurvey.SurveyDate > DateTime.Now.AddDays(-1))
            //{
            //    Result = false;
            //    return;
            //}

            //Result = true;
            //return;
        }
    }
}
