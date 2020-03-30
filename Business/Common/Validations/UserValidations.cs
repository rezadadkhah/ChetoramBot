
using System;

namespace Business.Common.Validations
{
    public class UserValidations : ValidationBase
    {
        private readonly DataAccess.Models.User user;

        public UserValidations(DataAccess.Models.User user)
        {
            this.user = user;
        }


        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
