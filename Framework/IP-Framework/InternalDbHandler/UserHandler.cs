using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace IP_Framework.InternalDbHandler
{
    class UserHandler
    {
        private static IMongoCollection<BsonDocument> collection = null;
        private static DBInstance dBInstance;

        public UserHandler(DBInstance dBInstance)
        {
            UserHandler.dBInstance = dBInstance;
            collection = dBInstance.databaseInstance.GetCollection<BsonDocument>(Config.COLLECTION_USERS_NAME);
        }

        public void ShowData()
        {
            dBInstance.ShowDataInCollection(collection);
        }

        public void InsertUser(UserWrapper user)
        {
            // TODO :)
            BsonArray simptome = new BsonArray { };
            dBInstance.InsertDocument(collection,
                new BsonDocument
                {
                    { "userid", user.userid },
                    { "simptome", simptome }
                }
            );
        }

    }
}
