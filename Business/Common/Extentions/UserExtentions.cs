using Business.Common.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Common.Extentions
{
    public class UserExtentions
    {
        public void Validate(DataAccess.Models.User user)
        {
            UserValidations validation = new UserValidations(user);
            validation.Validate();

        }
    }
}
