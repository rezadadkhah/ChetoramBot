using Business.Common.Enums;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Business.Business.User
{
    public class InsertUserSurvey : BusinessBase<bool>
    {
        private readonly UserSurvey userSurvey;

        public InsertUserSurvey(UserSurvey userSurvey)
        {
            this.userSurvey = userSurvey;
        }
        public override void Validate()
        {
            if (userSurvey == null)
            {
                ReturnInvalidResult(Types.ExceptionTypes.ValidationException);
                Continue = false;
            }
        }
        public override void Execute()
        {
            Context.UserSurveys.Add(userSurvey);
            Context.SaveChanges();
            Result = true;
            return;
        }
    }
}
