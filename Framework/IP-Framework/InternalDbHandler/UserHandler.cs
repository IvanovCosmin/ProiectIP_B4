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
                    { "simptome", simptome },
                    { "lat", user.lat},
                    { "lon", user.lon }
                }
            );
        }

        public List<Point> GetPoints()
        {
            List<Point> points = new List<Point>();

            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                try
                {
                    double lon = (double)doc["lon"].ToDouble() * Math.PI / 180.0;
                    double lat = (double)doc["lat"].ToDouble() * Math.PI / 180.0;

                    Point p = new Point(lon, lat);
                    points.Add(p);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return points;
        }

        public List<Point> GetPointsForDisease(String disease)
        {
            List<Point> points = new List<Point>();

            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {

                try
                {
                    var simptoms = doc["simptome"].AsBsonArray;
                    foreach (var simptom in simptoms)
                    {
                        if (simptom == disease)
                        {
                            double lon = (double)doc["lon"].ToDouble() * Math.PI / 180.0;
                            double lat = (double)doc["lat"].ToDouble() * Math.PI / 180.0;

                            Point p = new Point(lon, lat);
                            points.Add(p);
                            break;
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return points;
        }

        public Point GetPointsForUser(String user_id)
        {
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {

                try
                {
                    var id = doc["userid"].ToString();
                    if (id == user_id)
                    {
                        double lon = (double)doc["lon"].ToDouble() * Math.PI / 180.0;
                        double lat = (double)doc["lat"].ToDouble() * Math.PI / 180.0;

                        Point p = new Point(lon, lat);
                        return p;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return null;
        }


        public List<BsonDocument> GetCollectionData()
        {
            var documents = collection.Find(new BsonDocument()).ToList();
            return documents;
        }

    }
}
