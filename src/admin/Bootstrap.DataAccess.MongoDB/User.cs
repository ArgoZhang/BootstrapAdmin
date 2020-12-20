using Bootstrap.Security;
using Bootstrap.Security.Mvc;
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
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Groups { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override BootstrapUser? RetrieveUserByUserName(string userName)
        {
            var project = Builders<User>.Projection.Include(u => u.Id)
               .Include(u => u.UserName)
               .Include(u => u.DisplayName)
               .Include(u => u.Icon)
               .Include(u => u.Css)
               .Include(u => u.App);
            var ret = DbManager.Users.Find(user => user.UserName.ToLowerInvariant() == userName.ToLowerInvariant()).Project<DataAccess.User>(project).FirstOrDefault();
            if (ret != null)
            {
                if (string.IsNullOrEmpty(ret.Icon)) ret.Icon = "default.jpg";
                if (string.IsNullOrEmpty(ret.App)) ret.App = BootstrapAppContext.AppId;
            }
            return ret;
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

            var u = DbManager.Users.Find(user => user.UserName.ToLowerInvariant() == userName.ToLowerInvariant()).FirstOrDefault();
            return u != null && !string.IsNullOrEmpty(u.PassSalt) && u.Password == LgbCryptography.ComputeHash(password, u.PassSalt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public override bool SaveApp(string userName, string app)
        {
            var update = Builders<User>.Update.Set(u => u.App, app);
            DbManager.Users.FindOneAndUpdate(u => u.UserName.ToLowerInvariant() == userName.ToLowerInvariant(), update);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrieveNewUsers() => DbManager.Users.Find(user => !user.ApprovedTime.HasValue).SortByDescending(user => user.RegisterTime).ToList();

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
                .Include(u => u.Roles)
                .Include(u => u.IsReset);
            return DbManager.Users.Find(user => user.ApprovedTime != DateTime.MinValue).Project<User>(project).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.User user)
        {
            user.PassSalt = LgbCryptography.GenerateSalt();
            user.Password = LgbCryptography.ComputeHash(user.Password, user.PassSalt);

            var newUser = new User()
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                PassSalt = user.PassSalt,
                Password = user.Password,
                RegisterTime = DateTime.Now,
                ApprovedTime = user.ApprovedTime,
                ApprovedBy = user.ApprovedBy,
                Icon = user.Icon,
                Description = user.Description,
                IsReset = 0
            };
            DbManager.Users.InsertOne(newUser);
            user.Id = DbManager.Users.Find(r => r.UserName.ToLowerInvariant() == user.UserName.ToLowerInvariant()).FirstOrDefault().Id;
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
                var newPassword = LgbCryptography.ComputeHash(newPass, passSalt);
                var update = Builders<User>.Update.Set(u => u.Password, newPassword).Set(u => u.PassSalt, passSalt);
                DbManager.Users.FindOneAndUpdate(u => u.UserName.ToLowerInvariant() == userName.ToLowerInvariant(), update);
                ret = true;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ResetPassword(string userName, string password)
        {
            var ret = false;
            var resetUser = UserHelper.RetrieveResetUserByUserName(userName);
            if (resetUser == null) return ret;

            var passSalt = LgbCryptography.GenerateSalt();
            var newPassword = LgbCryptography.ComputeHash(password, passSalt);
            DbManager.Users.UpdateOne(User => User.UserName.ToLowerInvariant() == userName.ToLowerInvariant(), Builders<User>.Update.Set(md => md.Password, newPassword).Set(md => md.PassSalt, passSalt).Set(md => md.IsReset, 0));
            DbManager.ResetUsers.DeleteMany(user => user.UserName.ToLowerInvariant() == userName.ToLowerInvariant());
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        public override bool Approve(string id, string approvedBy)
        {
            DbManager.Users.UpdateOne(User => User.Id == id, Builders<User>.Update.Set(md => md.ApprovedTime, DateTime.Now).Set(md => md.ApprovedBy, approvedBy));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public override bool SaveDisplayName(string userName, string displayName)
        {
            DbManager.Users.UpdateOne(User => User.UserName.ToLowerInvariant() == userName.ToLowerInvariant(), Builders<User>.Update.Set(md => md.DisplayName, displayName));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public override bool SaveUserCssByName(string userName, string cssName)
        {
            DbManager.Users.UpdateOne(User => User.UserName.ToLowerInvariant() == userName.ToLowerInvariant(), Builders<User>.Update.Set(md => md.Css, cssName));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public override bool SaveUserIconByName(string userName, string iconName)
        {
            DbManager.Users.UpdateOne(User => User.UserName.ToLowerInvariant() == userName.ToLowerInvariant(), Builders<User>.Update.Set(md => md.Icon, iconName));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rejectBy"></param>
        /// <returns></returns>
        public override bool Reject(string id, string rejectBy)
        {
            var user = UserHelper.RetrieveNewUsers().FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                DbManager.RejectUsers.InsertOne(new RejectUser()
                {
                    DisplayName = user.DisplayName,
                    RegisterTime = user.RegisterTime,
                    RejectedBy = rejectBy,
                    RejectedReason = "",
                    RejectedTime = DateTime.Now,
                    UserName = user.UserName
                });
                DbManager.Users.DeleteOne(User => User.Id == id);
            }
            return true;
        }
    }
}
