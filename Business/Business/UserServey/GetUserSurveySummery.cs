using Business.Common.Enums;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Business.Business.User
{
    public class GetUserSurveySummery : BusinessBase<bool>
    {
        private readonly int userId;

        public GetUserSurveySummery(int userId)
        {
            this.userId = userId;
        }
        public override void Validate()
        {
            if (userId.IsNotPositive())
            {
                ReturnInvalidResult(Types.ExceptionTypes.ValidationException);
                Continue = false;
            }
        }
        public override void Execute()
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $@"select min(S.PersianTitle) SurveyPersianTitle,count(US.SurveyId) SurveyCount,sum(US.Point) / count(US.SurveyId) as Point from usersurveys US
                                                    inner join Surveys S on S.Id = US.SurveyId
                                                    where ConsideredUserId = {userId}
                                                    group by US.SurveyId ";
                Context.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                        
                }
            }
        }
    }
}
