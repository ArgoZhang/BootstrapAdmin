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
            return DbManager.Users.Find(user => user.UserName == userName).Project<DataAccess.User>(project).FirstOrDefault();
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

            var u = DbManager.Users.Find(user => user.UserName == userName).FirstOrDefault();
            return !string.IsNullOrEmpty(u.PassSalt) && u.Password == LgbCryptography.ComputeHash(password, u.PassSalt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrieveNewUsers() => DbManager.Users.Find(user => user.ApprovedTime == DateTime.MinValue).SortByDescending(user => user.RegisterTime).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> Retrieves()
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
            return DbManager.Users.Find(user => user.ApprovedTime != DateTime.MinValue).Project<User>(project).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.User user)
        {
            // 已经存在或者已经在新用户中了
            if (UserHelper.RetrieveUserByUserName(user.UserName) != null || UserHelper.RetrieveNewUsers().Any(u => u.UserName == user.UserName)) return false;

            if (user.Description.Length > 500) user.Description = user.Description.Substring(0, 500);
            if (user.UserName.Length > 50) user.UserName = user.UserName.Substring(0, 50);
            DbManager.Users.InsertOne(new User()
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                PassSalt = LgbCryptography.GenerateSalt(),
                Password = LgbCryptography.ComputeHash(user.Password, user.PassSalt),
                RegisterTime = DateTime.Now,
                ApprovedTime = DateTime.Now,
                ApprovedBy = user.ApprovedBy,
                Roles = new List<string>(),
                Groups = new List<string>(),
                Icon = $"{DictHelper.RetrieveIconFolderPath()}default.jpg",
                Description = user.Description
            });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public override bool Update(string id, string password, string displayName)
        {
            var passSalt = LgbCryptography.GenerateSalt();
            var newPassword = LgbCryptography.ComputeHash(password, passSalt);
            var update = Builders<User>.Update.Set(u => u.Password, newPassword).Set(u => u.PassSalt, passSalt).Set(u => u.DisplayName, displayName);
            DbManager.Users.FindOneAndUpdate(u => u.Id == id, update);
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
                DbManager.Users.FindOneAndUpdate(u => u.UserName == UserName, update);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Delete(IEnumerable<string> value)
        {
            var list = new List<WriteModel<User>>();
            foreach (var id in value)
            {
                list.Add(new DeleteOneModel<User>(Builders<User>.Filter.Eq(u => u.Id, id)));
            }
            DbManager.Users.BulkWrite(list);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrievesByRoleId(string roleId)
        {
            var users = UserHelper.Retrieves().Cast<User>().ToList();
            users.ForEach(p => p.Checked = (p.Roles != null && p.Roles.Contains(roleId)) ? "checked" : "");
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public override bool SaveByRoleId(string roleId, IEnumerable<string> userIds)
        {
            var users = DbManager.Users.Find(md => md.Roles != null && md.Roles.Contains(roleId)).ToList();

            // Remove roles
            users.ForEach(p =>
            {
                var roles = p.Roles == null ? new List<string>() : p.Roles.ToList();
                roles.Remove(roleId);
                DbManager.Users.UpdateOne(md => md.Id == p.Id, Builders<User>.Update.Set(md => md.Roles, roles));
            });

            users = DbManager.Users.Find(md => userIds.Contains(md.Id)).ToList();
            // Add roles
            users.ForEach(p =>
            {
                var roles = p.Roles == null ? new List<string>() : p.Roles.ToList();
                roles.Add(roleId);
                DbManager.Users.UpdateOne(md => md.Id == p.Id, Builders<User>.Update.Set(md => md.Roles, roles));
            });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrievesByGroupId(string groupId)
        {
            var users = UserHelper.Retrieves().Cast<User>().ToList();
            users.ForEach(p => p.Checked = (p.Groups != null && p.Groups.Contains(groupId)) ? "checked" : "");
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public override bool SaveByGroupId(string groupId, IEnumerable<string> userIds)
        {
            var users = DbManager.Users.Find(md => md.Groups != null && md.Groups.Contains(groupId)).ToList();

            // Remove roles
            users.ForEach(p =>
            {
                var groups = p.Groups == null ? new List<string>() : p.Groups.ToList();
                groups.Remove(groupId);
                DbManager.Users.UpdateOne(md => md.Id == p.Id, Builders<User>.Update.Set(md => md.Groups, groups));
            });

            users = DbManager.Users.Find(md => userIds.Contains(md.Id)).ToList();
            // Add roles
            users.ForEach(p =>
            {
                var groups = p.Groups == null ? new List<string>() : p.Groups.ToList();
                groups.Add(groupId);
                DbManager.Users.UpdateOne(md => md.Id == p.Id, Builders<User>.Update.Set(md => md.Groups, groups));
            });
            return true;
        }
    }
}
