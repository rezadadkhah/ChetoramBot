using Business.Common.Enums;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Extentions;
using Microsoft.EntityFrameworkCore;
using Business.Models;

namespace Business.Business.User
{
    public class GetUserSurveySummery : BusinessBase<List<SurveySummary>>
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
            List<SurveySummary> result = new List<SurveySummary>();
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $@"select min(S.PersianTitle) SurveyPersianTitle,count(US.SurveyId) SurveyCount,sum(US.Point) / count(US.SurveyId) as Point from usersurveys US
                                                    inner join Surveys S on S.Id = US.SurveyId
                                                    where ConsideredUserId = {userId}
                                                    group by US.SurveyId ";
                Context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            result.Add(new SurveySummary()
                            {
                                SurveyPersianName = reader.GetString(0),
                                SurveyCount = reader.GetInt32(1),
                                Point = reader.GetInt32(2)
                            });
                        }
                    }   
                }
                Result = result;
            }
        }
    }
}
