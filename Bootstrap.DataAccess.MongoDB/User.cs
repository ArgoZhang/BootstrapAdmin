using Bootstrap.Security;
using Longbow.Security.Cryptography;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class User : DataAccess.User
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override BootstrapUser RetrieveUserByUserName(string userName)
        {
            var users = MongoDbAccessManager.DBAccess.GetCollection<BootstrapUser>("Users");
            return users.Find(user => user.UserName == userName).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool Authenticate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password)) return false;

            var users = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.User>("Users");
            var u = users.Find(user => user.UserName == userName).FirstOrDefault();
            return !string.IsNullOrEmpty(u.PassSalt) && u.Password == LgbCryptography.ComputeHash(password, u.PassSalt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrieveNewUsers()
        {
            var users = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.User>("Users");
            return users.Find(user => user.ApprovedTime == DateTime.MinValue).SortByDescending(user => user.RegisterTime).ToList();
        }
    }
}
