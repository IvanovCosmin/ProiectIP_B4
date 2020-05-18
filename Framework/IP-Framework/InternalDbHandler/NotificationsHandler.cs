using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IP_Framework.InternalDbHandler
{
    class NotificationsHandler
    {
        private static IMongoCollection<BsonDocument> collection = null;
        private static DBInstance dBInstance;

        public NotificationsHandler(DBInstance dBInstance)
        {
            NotificationsHandler.dBInstance = dBInstance;
            collection = dBInstance.databaseInstance.GetCollection<BsonDocument>(Config.COLLECTION_NOTIFICATIONS);
        }

        public void ShowData()
        {
            dBInstance.ShowDataInCollection(collection);
        }

        public BsonArray GetAllNotifs(String user_id)
        {
            List<Point> points = new List<Point>();

            BsonArray notifs = new BsonArray();
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {

                try
                {
                    if (doc["id_user"] == user_id)
                    {
                        doc["category"] = "Epidemie";
                        doc.Remove("_id");
                        notifs.Add(doc);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return notifs;
        }


    }
}
