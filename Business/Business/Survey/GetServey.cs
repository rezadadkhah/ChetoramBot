using Business.Common.Enums;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Business.Business.User
{
    public class GetServey : BusinessBase<List<Survey>>
    {
    
        public override void Execute()
        {
            Result = Context.Survey.ToList();
            return;
        }
    }
}
