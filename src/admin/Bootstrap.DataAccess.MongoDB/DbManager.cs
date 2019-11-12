using Bootstrap.Security;
using Bootstrap.Security.Mvc;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    internal static class DbManager
    {
        private static IMongoDatabase? _db = null;
        private static bool _register = false;
        private static readonly object _locker = new object();

        /// <summary>
        /// IMongoDatabase 实例
        /// </summary>
        private static IMongoDatabase DBAccess
        {
            get
            {
                if (_db == null)
                {
                    lock (_locker)
                    {
                        if (!_register)
                        {
                            _register = true;
                            ChangeToken.OnChange(() => BootstrapAppContext.Configuration.GetReloadToken(), () => _db = null);
                            InitClassMap();
                        }
                        if (_db == null)
                            InitDb();
                    }
                }
#pragma warning disable CS8603 // 可能的 null 引用返回。
                return _db;
#pragma warning restore CS8603 // 可能的 null 引用返回。
            }
        }

        #region Collections
        /// <summary>
        /// Logs 集合
        /// </summary>
        public static IMongoCollection<DataAccess.Log> Logs
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Log>("Logs");
            }
        }

        /// <summary>
        /// Exceptions 集合
        /// </summary>
        public static IMongoCollection<DataAccess.Exceptions> Exceptions
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Exceptions>("Exceptions");
            }
        }
        /// <summary>
        /// Dicts 集合
        /// </summary>
        public static IMongoCollection<BootstrapDict> Dicts
        {
            get
            {
                return DBAccess.GetCollection<BootstrapDict>("Dicts");
            }
        }

        /// <summary>
        /// Users 集合
        /// </summary>
        public static IMongoCollection<User> Users
        {
            get
            {
                return DBAccess.GetCollection<User>("Users");
            }
        }

        /// <summary>
        /// Groups 集合
        /// </summary>
        public static IMongoCollection<Group> Groups
        {
            get
            {
                return DBAccess.GetCollection<Group>("Groups");
            }
        }

        /// <summary>
        /// Roles 集合
        /// </summary>
        public static IMongoCollection<Role> Roles
        {
            get
            {
                return DBAccess.GetCollection<Role>("Roles");
            }
        }

        /// <summary>
        /// Menus 集合
        /// </summary>
        public static IMongoCollection<BootstrapMenu> Menus
        {
            get
            {
                return DBAccess.GetCollection<BootstrapMenu>("Navigations");
            }
        }

        /// <summary>
        /// LoginUsers 集合
        /// </summary>
        public static IMongoCollection<DataAccess.LoginUser> LoginUsers
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.LoginUser>("LoginLogs");
            }
        }

        /// <summary>
        /// ResetUsers 集合
        /// </summary>
        public static IMongoCollection<DataAccess.ResetUser> ResetUsers
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.ResetUser>("ResetUsers");
            }
        }

        /// <summary>
        /// Traces 集合
        /// </summary>
        public static IMongoCollection<DataAccess.Trace> Traces
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Trace>("Traces");
            }
        }

        /// <summary>
        /// RejectUsers 集合
        /// </summary>
        public static IMongoCollection<RejectUser> RejectUsers
        {
            get
            {
                return DBAccess.GetCollection<RejectUser>("RejectUsers");
            }
        }

        /// <summary>
        /// Messages 集合
        /// </summary>
        public static IMongoCollection<DataAccess.Message> Messages
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Message>("Messages");
            }
        }

        /// <summary>
        /// Tasks 集合
        /// </summary>
        public static IMongoCollection<DataAccess.Task> Tasks
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Task>("Tasks");
            }
        }
        #endregion

        private static void InitDb()
        {
            var (connectString, databaseName) = Longbow.Data.DbManager.GetMongoDB();
            var client = new MongoClient(connectString);
            _db = client.GetDatabase(databaseName);
        }

        private static void InitClassMap()
        {
            BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);

            if (!BsonClassMap.IsClassMapRegistered(typeof(BootstrapDict)))
            {
                BsonClassMap.RegisterClassMap<BootstrapDict>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.User)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.User>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.UnmapMember(user => user.Checked);
                    md.UnmapMember(user => user.Period);
                    md.UnmapMember(user => user.NewPassword);
                    md.UnmapMember(user => user.UserStatus);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(BootstrapMenu)))
            {
                BsonClassMap.RegisterClassMap<BootstrapMenu>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.UnmapMember(m => m.CategoryName);
                    md.UnmapMember(m => m.Active);
                    md.UnmapMember(m => m.ParentName);
                    md.UnmapMember(m => m.Menus);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(BootstrapGroup)))
            {
                BsonClassMap.RegisterClassMap<BootstrapGroup>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Group)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Group>(md =>
                {
                    md.AutoMap();
                    md.UnmapMember(group => group.Checked);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(Group)))
            {
                BsonClassMap.RegisterClassMap<Group>(md =>
                {
                    md.AutoMap();
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Role)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Role>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.UnmapMember(role => role.Checked);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Exceptions)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Exceptions>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.UnmapMember(ex => ex.Period);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Log)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Log>(md =>
                {
                    md.AutoMap();
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Trace)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Trace>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.AddKnownType(typeof(DataAccess.Log));
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.LoginUser)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.LoginUser>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.ResetUser)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.ResetUser>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(RejectUser)))
            {
                BsonClassMap.RegisterClassMap<RejectUser>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Message)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Message>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.UnmapMember(t => t.LabelName);
                    md.UnmapMember(t => t.Period);
                    md.UnmapMember(t => t.FromIcon);
                    md.UnmapMember(t => t.FromDisplayName);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Task)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Task>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.UnmapMember(t => t.AssignDisplayName);
                });
            }
        }
    }
}
