using Bootstrap.Security;
using Longbow.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public static class MongoDbAccessManager
    {
        private static IMongoDatabase _db = null;
        private static bool _register = false;
        /// <summary>
        /// 
        /// </summary>
        public static IMongoDatabase DBAccess
        {
            get
            {
                if (_db == null)
                {
                    if (!_register)
                    {
                        _register = true;
                        DbAdapterManager.RegisterConfigChangeCallback(InitDb);
                    }
                    InitDb();
                }
                return _db;
            }
        }

        private static void InitDb()
        {
            var connectString = DbAdapterManager.GetConnectionString("ba");
            if (string.IsNullOrEmpty(connectString)) throw new InvalidOperationException("Please set the BA default value in configuration file.");

            var seq = connectString.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if (seq.Length != 2) throw new InvalidOperationException("");
            var client = new MongoClient(seq[0]);
            _db = client.GetDatabase(seq[1].Split("=", StringSplitOptions.RemoveEmptyEntries).LastOrDefault());

            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local));
            if (!BsonClassMap.IsClassMapRegistered(typeof(BootstrapDict)))
            {
                BsonClassMap.RegisterClassMap<BootstrapDict>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(BootstrapUser)))
            {
                BsonClassMap.RegisterClassMap<BootstrapUser>(md =>
                {
                    md.AutoMap();
                    md.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.User)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.User>(md =>
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
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataAccess.Log)))
            {
                BsonClassMap.RegisterClassMap<DataAccess.Log>(md =>
                {
                    md.AutoMap();
                    md.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    md.IdMemberMap.SetIgnoreIfDefault(true);
                });
            }
        }
    }
}
