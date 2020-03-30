using Business.Common.Enums;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Business.Business.User
{
    public class InsertUserServey : BusinessBase<bool>
    {
        private readonly UserServey userServey;

        public InsertUserServey(UserServey userServey)
        {
            this.userServey = userServey;
        }
        public override void Validate()
        {
            if (userServey == null)
            {
                ReturnInvalidResult(Types.ExceptionTypes.ValidationException);
                Continue = false;
            }
        }
        public override void Execute()
        {
            Context.UserServey.Add(userServey);
            Context.SaveChanges();
            Result = true;
            return;
        }
    }
}
