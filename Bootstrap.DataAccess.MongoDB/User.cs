using Bootstrap.Security;
using Longbow.Security.Cryptography;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Groups { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override BootstrapUser RetrieveUserByUserName(string userName)
        {
            var project = Builders<User>.Projection.Include(u => u.Id)
               .Include(u => u.UserName)
               .Include(u => u.DisplayName)
               .Include(u => u.Icon)
               .Include(u => u.Css);
            return MongoDbAccessManager.Users.Find(user => user.UserName == userName).Project<DataAccess.User>(project).FirstOrDefault();
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

            var u = MongoDbAccessManager.Users.Find(user => user.UserName == userName).FirstOrDefault();
            return !string.IsNullOrEmpty(u.PassSalt) && u.Password == LgbCryptography.ComputeHash(password, u.PassSalt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrieveNewUsers() => MongoDbAccessManager.Users.Find(user => user.ApprovedTime == DateTime.MinValue).SortByDescending(user => user.RegisterTime).ToList();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrieveUsers()
        {
            var project = Builders<User>.Projection.Include(u => u.Id)
                .Include(u => u.UserName)
                .Include(u => u.DisplayName)
                .Include(u => u.RegisterTime)
                .Include(u => u.ApprovedTime)
                .Include(u => u.ApprovedBy)
                .Include(u => u.Description)
                .Include(u => u.Groups)
                .Include(u => u.Roles);
            return MongoDbAccessManager.Users.Find(user => user.ApprovedTime != DateTime.MinValue).Project<User>(project).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool SaveUser(DataAccess.User user)
        {
            // 已经存在或者已经在新用户中了
            if (UserHelper.RetrieveUserByUserName(user.UserName) != null || UserHelper.RetrieveNewUsers().Any(u => u.UserName == user.UserName)) return false;

            if (user.Description.Length > 500) user.Description = user.Description.Substring(0, 500);
            if (user.UserName.Length > 50) user.UserName = user.UserName.Substring(0, 50);
            user.Id = null;
            user.PassSalt = LgbCryptography.GenerateSalt();
            user.Password = LgbCryptography.ComputeHash(user.Password, user.PassSalt);
            user.RegisterTime = DateTime.Now;
            user.ApprovedTime = DateTime.Now;
            user.Icon = $"{DictHelper.RetrieveIconFolderPath().Code}default.jpg";
            MongoDbAccessManager.Users.InsertOne(user as User);
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
            var update = Builders<User>.Update.Set(u => u.Password, newPassword).Set(u => u.PassSalt, passSalt).Set(u => u.DisplayName, displayName);
            MongoDbAccessManager.Users.FindOneAndUpdate(u => u.Id == id, update);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public override bool ChangePassword(string userName, string password, string newPass)
        {
            bool ret = false;
            if (Authenticate(userName, password))
            {
                var passSalt = LgbCryptography.GenerateSalt();
                var newPassword = LgbCryptography.ComputeHash(password, passSalt);
                var update = Builders<User>.Update.Set(u => u.Password, newPassword).Set(u => u.PassSalt, passSalt);
                MongoDbAccessManager.Users.FindOneAndUpdate(u => u.UserName == UserName, update);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool DeleteUser(IEnumerable<string> value)
        {
            var list = new List<WriteModel<User>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<User>(Builders<User>.Filter.Eq(u => u.Id, id)));
            }
            MongoDbAccessManager.Users.BulkWrite(list);
            return true;
        }
    }
}
