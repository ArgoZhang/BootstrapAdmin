namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginUser : DataAccess.LoginUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool Log(DataAccess.LoginUser user)
        {
            DbManager.LoginUsers.InsertOne(user);
            return true;
        }
    }
}
