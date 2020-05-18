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

        public IMongoCollection<BsonDocument> GetCollection()
        {
            return collection;
        }

        public List<Point> GetPoints()
        {
            List<Point> points = new List<Point>();

            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                try
                {
                    double lon = (double)doc["lon"] * Math.PI / 180.0;
                    double lat = (double)doc["lat"] * Math.PI / 180.0;

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
                    if (doc["disease"] == disease)
                    {
                        double lon = (double) doc["lon"] * Math.PI / 180.0;
                        double lat = (double) doc["lat"] * Math.PI / 180.0;

                        Point p = new Point(lon, lat);
                        points.Add(p);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return points;
        }





    }
}
