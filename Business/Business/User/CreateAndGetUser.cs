using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Business.Business.User
{
    public class CreateAndGetUser 
    {
        private readonly Models.User user;

        public CreateAndGetUser(Models.User user)
        {
            this.user = user;
        }

        public Models.User Run()
        {
            try
            {
            BotDbContext context = new BotDbContext();
            Models.User userEntity;
            userEntity = context.User.FirstOrDefault(u=>u.BotId == user.BotId);
            if(userEntity != null)
            {
                return userEntity;
            }

            context.User.Add(user);
            context.SaveChanges();
            return user;
            }
            catch
            {
                throw;
            }
        }
    }
}
