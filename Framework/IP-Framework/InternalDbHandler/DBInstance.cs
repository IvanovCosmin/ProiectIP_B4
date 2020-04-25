using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace IP_Framework.InternalDbHandler
{
    class DBInstance
    {
        
        public IMongoDatabase databaseInstance = null;

        public DBInstance()
        {
            MongoClient client = new MongoClient(Config.CONNECTION_STRING);

            databaseInstance = client.GetDatabase(Config.DB_NAME);
        }
        public void InsertDocument(IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            collection.InsertOne(document);
        }

        public void ShowDataInCollection(IMongoCollection<BsonDocument> collection)
        {
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                Console.WriteLine(doc.ToString());
            }
        }
    }
}
