using Bootstrap.Security;
using Bootstrap.Security.Mvc;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    internal static class DbManager
    {
        private static IMongoDatabase? _db;
        private static bool _register;
        private static readonly object _locker = new object();

        /// <summary>
        /// BA数据库 IMongoDatabase 实例
        /// </summary>
        private static IMongoDatabase BADatabase
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
                            _db = InitDb("ba");
                    }
                }
                return _db;
            }
        }

        #region Collections
        /// <summary>
        /// Dicts 集合
        /// </summary>
        public static IMongoCollection<BootstrapDict> Dicts
        {
            get
            {
                return BADatabase.GetCollection<BootstrapDict>("Dicts");
            }
        }

        /// <summary>
        /// Users 集合
        /// </summary>
        public static IMongoCollection<User> Users
        {
            get
            {
                return BADatabase.GetCollection<User>("Users");
            }
        }

        /// <summary>
        /// Roles 集合
        /// </summary>
        public static IMongoCollection<Role> Roles
        {
            get
            {
                return BADatabase.GetCollection<Role>("Roles");
            }
        }

        /// <summary>
        /// Groups 集合
        /// </summary>
        public static IMongoCollection<Group> Groups
        {
            get
            {
                return BADatabase.GetCollection<Group>("Groups");
            }
        }

        /// <summary>
        /// Menus 集合
        /// </summary>
        public static IMongoCollection<BootstrapMenu> Menus
        {
            get
            {
                return BADatabase.GetCollection<BootstrapMenu>("Navigations");
            }
        }
        #endregion

        private static IMongoDatabase InitDb(string name)
        {
            var (connectString, databaseName) = Longbow.Data.DbManager.GetMongoDB(name);
            var client = new MongoClient(connectString);
            return client.GetDatabase(databaseName);
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
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
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
            if (!BsonClassMap.IsClassMapRegistered(typeof(Group)))
            {
                BsonClassMap.RegisterClassMap<Group>(md =>
                {
                    md.AutoMap();
                    md.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(Role)))
            {
                BsonClassMap.RegisterClassMap<Role>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                    md.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}
