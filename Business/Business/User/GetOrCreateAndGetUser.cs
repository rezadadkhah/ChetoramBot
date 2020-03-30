namespace Business.Business.User
{
    public class GetOrCreateAndGetUser : BusinessBase<DataAccess.Models.User>
    {
        private readonly DataAccess.Models.User user;

        public GetOrCreateAndGetUser(DataAccess.Models.User user)
        {
            this.user = user;
        }

        public override void Execute()
        {
            if (GetUser() == false) return;
            if (Result != null)
                return;
            if (CreateUser() == false) return;
            return;
        }

        private bool GetUser()
        {
            GetUser getUser = new GetUser(user.UserId);
            getUser.Run();
            if (!getUser.IsSuccessful)
                ReturnInvalidResult(Common.Enums.Types.ExceptionTypes.InvalidResultException);
            if (getUser.Result != null)
            {
                Result = getUser.Result;
                return false;
            }
            return true;
        }

        private bool CreateUser()
        {
            CreateUser createUser = new CreateUser(user);
            createUser.Run();
            if (!createUser.IsSuccessful)
                ReturnInvalidResult(Common.Enums.Types.ExceptionTypes.InvalidResultException);
            if (createUser.Result == false)
            {
                Result = null;
                return false;
            }
            return true;
        }

    }
}
