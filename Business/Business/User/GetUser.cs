using Business.Common.Enums;
using Business.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Business.User
{
    public class GetUser : BusinessBase<DataAccess.Models.User>
    {
        private readonly int userId;
        private readonly string userName;

        public GetUser(int userId)
        {
            this.userId = userId;
        }
        public GetUser(string userName)
        {
            this.userName = userName;
        }

        

        public override void Validate()
        {
            if (userId.IsNotPositive() && userName.IsNullOrEmptyOrWhitespace())
                ReturnInvalidResult(Types.ExceptionTypes.ValidationException);
        }

        public override void Execute()
        {
            Result = Context.User.FirstOrDefault(u => 
            (userId <= 0 || u.UserId == userId) &&
            (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName) || u.UserName == userName) 
            );
            return;
        }
    }
}
