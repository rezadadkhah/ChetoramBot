using Business.Business.User;
using Business;
using DataAccess.Models;

namespace ChetoramBot.Helpers
{
    public static class Business
    {
        public static bool InsertUserSurvey(UserSurvey userSurvey)
        {
            InsertUserSurvey insertUserSurvey = new InsertUserSurvey(userSurvey);
            insertUserSurvey.Run();
            return insertUserSurvey.Result;
        }
        public static User GetUser(int consideredUserId)
        {
            GetUser getUser = new GetUser(consideredUserId);
            getUser.Run();
            return getUser.Result;
        }
        public static bool CreateUser(User user)
        {
            CreateUser createUser = new CreateUser(user);
            createUser.Run();
            return createUser.Result;
        }

        public static bool GetUserSurveySummery(int userId)
        {
            GetUserSurveySummery getUserSurveySummery = new GetUserSurveySummery(userId);
            getUserSurveySummery.Run();
            return getUserSurveySummery.Result;
        }
    }
}
