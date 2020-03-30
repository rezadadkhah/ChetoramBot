using System;
using System.Collections.Generic;
using System.Text;

namespace ChetoramBot.Test
{
    public static class Business
    {
        public static int GetUser(int x)
        {
            GetUserBusiness getUserBusiness = new GetUserBusiness(x);
            return getUserBusiness.Run();
        }
    }
}
