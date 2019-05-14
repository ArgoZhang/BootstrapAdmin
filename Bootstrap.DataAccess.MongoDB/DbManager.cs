using Bootstrap.Security;
using Longbow.Configuration;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DbManager
    {
        private static IMongoDatabase _db = null;
        private static bool _register = false;
        private static readonly object _locker = new object();
        /// <summary>
        /// 
        /// </summary>
        public static IMongoDatabase DBAccess
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
                            ChangeToken.OnChange(() => ConfigurationManager.AppSettings.GetReloadToken(), () => _db = null);
                            InitClassMap();
                        }
                        InitDb();
                    }
                }
                return _db;
            }
        }

        #region Collections
        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<DataAccess.Log> Logs
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Log>("Logs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<DataAccess.Exceptions> Exceptions
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Exceptions>("Exceptions");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<BootstrapDict> Dicts
        {
            get
            {
                return DBAccess.GetCollection<BootstrapDict>("Dicts");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<User> Users
        {
            get
            {
                return DBAccess.GetCollection<User>("Users");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<Group> Groups
        {
            get
            {
                return DBAccess.GetCollection<Group>("Groups");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<Role> Roles
        {
            get
            {
                return DBAccess.GetCollection<Role>("Roles");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<BootstrapMenu> Menus
        {
            get
            {
                return DBAccess.GetCollection<BootstrapMenu>("Navigations");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<DataAccess.LoginUser> LoginUsers
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.LoginUser>("LoginLogs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<DataAccess.ResetUser> ResetUsers
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.ResetUser>("ResetUsers");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<DataAccess.Trace> Traces
        {
            get
            {
                return DBAccess.GetCollection<DataAccess.Trace>("Traces");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMongoCollection<RejectUser> RejectUsers
        {
            get
            {
                return DBAccess.GetCollection<RejectUser>("RejectUsers");
            }
        }
        #endregion

        private static void InitDb()
        {
            var client = new MongoClient(Longbow.Data.DbManager.GetConnectionString());
            _db = client.GetDatabase(ConfigurationManager.AppSettings["MongoDB"]);
        }

        private static void InitClassMap()
        {
            BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);

            if (!BsonClassMap.IsClassMapRegistered(typeof(Dict)))
            {
                BsonClassMap.RegisterClassMap<Dict>(md =>
                {
                    md.AutoMap();
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(BootstrapDict)))
            {
                BsonClassMap.RegisterClassMap<BootstrapDict>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.AddKnownType(typeof(Dict));
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
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Group)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Group>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.UnmapMember(group => group.Checked);
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
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Task)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Task>(md =>
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
        }
    }
}
