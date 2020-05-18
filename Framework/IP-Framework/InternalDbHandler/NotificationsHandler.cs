using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace IP_Framework.InternalDbHandler
{
    class NotificationsHandler
    {
        private static IMongoCollection<BsonDocument> collection = null;
        private static DBInstance dBInstance;

        public bool InsertNotification(BsonDocument doc)
        {
            collection.InsertOne(doc);
            return true;
        }

        public bool InsertNotificationToAllAffectedUsers(Point user, double distance, String disease)
        {

            Point point = new Point();

            DBModule instance = Utils.Singleton<DBModule>.Instance;
            UserHandler userHandler = instance.GetUserHandler();
            var documents = userHandler.GetCollectionData();
            foreach (BsonDocument doc in documents)
            {

                try
                {
                    point.x = (double)doc["lon"].ToDouble() * Math.PI / 180.0;
                    point.y = (double)doc["lat"].ToDouble() * Math.PI / 180.0;

                    if (ConvexHaul.Distance(user, point) < distance)
                    {
                        BsonDocument document = new BsonDocument();
                        document["id_user"] = doc["userid"];
                        document["text"] = disease;
                        BsonArray temp = new BsonArray();
                        document["links"] = temp;

                        InsertNotification(document);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }


            return true;
        }

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
