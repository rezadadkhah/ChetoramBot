using System;
using System.Collections.Generic;
using System.Text;

namespace ChetoramBot.Test
{
    public class GetUserBusiness : BusinessBase<int>
    {
        private readonly int x;

        public GetUserBusiness(int x)
        {
            this.x = x;
        }

        public override int Run()
        {
            return Business.GetUser(x);
        }
    }
}
