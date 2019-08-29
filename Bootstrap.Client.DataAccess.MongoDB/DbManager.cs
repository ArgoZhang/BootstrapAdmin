using Bootstrap.Security;
using Longbow.Configuration;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Bootstrap.Client.DataAccess.MongoDB
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
        public static IMongoCollection<BootstrapMenu> Menus
        {
            get
            {
                return DBAccess.GetCollection<BootstrapMenu>("Navigations");
            }
        }
        #endregion

        private static void InitDb()
        {
            var client = new MongoClient(Longbow.Data.DbManager.GetConnectionString("ba"));
            _db = client.GetDatabase(ConfigurationManager.AppSettings["MongoDB"]);
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
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(Role)))
            {
                BsonClassMap.RegisterClassMap<Role>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
        }
    }
}
