using Business.Common.Enums;
using Business.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Business.User
{
    public class CreateUser : BusinessBase<bool>
    {
        private readonly DataAccess.Models.User user;

        public CreateUser(DataAccess.Models.User user)
        {
            this.user = user;
        }
        public override void Validate()
        {
            if (user.UserId.IsNotPositive() || user.UserName.IsNullOrEmptyOrWhitespace())
                ReturnInvalidResult(Types.ExceptionTypes.ValidationException);

            if (UserExists())
            {
                Continue = false;
                return;
            }
        }


        public override void Execute()
        {
            Context.User.Add(new DataAccess.Models.User
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                lastName = user.lastName
            });
            Context.SaveChanges();
            Result = true;
        }


        private bool UserExists()
        {
            GetUser getUser = new GetUser(user.UserId);
            getUser.Run();
            if(!getUser.IsSuccessful)
                ReturnInvalidResult(Types.ExceptionTypes.InvalidResultException);
            if (getUser.Result != null)
                return true;
            return false;
        }
    }
}
