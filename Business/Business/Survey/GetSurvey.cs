using System.Collections.Generic;
using System.Linq;

namespace Business.Business.Survey
{
    public class GetSurvey : BusinessBase<List<DataAccess.Models.Survey>>
    {
    
        public override void Execute()
        {
            Result = Context.Surveys.ToList();
            return;
        }
    }
}
