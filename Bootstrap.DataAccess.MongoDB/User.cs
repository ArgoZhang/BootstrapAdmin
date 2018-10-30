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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrieveUsers()
        {
            var users = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.User>("Users");
            return users.Find(user => user.ApprovedTime != DateTime.MinValue).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool SaveUser(DataAccess.User user)
        {
            if (user.Description.Length > 500) user.Description = user.Description.Substring(0, 500);
            if (user.UserName.Length > 50) user.UserName = user.UserName.Substring(0, 50);
            user.Id = null;
            user.PassSalt = LgbCryptography.GenerateSalt();
            user.Password = LgbCryptography.ComputeHash(user.Password, user.PassSalt);
            user.RegisterTime = DateTime.Now;
            user.ApprovedTime = DateTime.Now;
            user.Icon = $"{DictHelper.RetrieveIconFolderPath().Code}default.jpg";
            var users = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.User>("Users");
            users.InsertOne(user);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public override bool UpdateUser(string id, string password, string displayName)
        {
            var passSalt = LgbCryptography.GenerateSalt();
            var newPassword = LgbCryptography.ComputeHash(password, passSalt);
            var update = Builders<DataAccess.User>.Update.Set(u => u.Password, newPassword).Set(u => u.PassSalt, passSalt).Set(u => u.DisplayName, displayName);
            var users = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.User>("Users");
            users.FindOneAndUpdate(u => u.Id == id, update);
            return true;
        }
    }
}
