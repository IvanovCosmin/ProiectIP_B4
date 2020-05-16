using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Quizzer;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace IP_Framework.InternalDbHandler
{
    class QuizSigsHandler
    {
        private static IMongoCollection<BsonDocument> collection = null;
        private static DBInstance dBInstance;

        public QuizSigsHandler(DBInstance dBInstance)
        {
            QuizSigsHandler.dBInstance = dBInstance;
            collection = dBInstance.databaseInstance.GetCollection<BsonDocument>(Config.COLLECTION_QUIZSIGS);
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
                        double lon = (double)doc["lon"] * Math.PI / 180.0;
                        double lat = (double)doc["lat"] * Math.PI / 180.0;

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

        public ISet<QuSignature> GetSignatures()
        {
            ISet<QuSignature> signatures = new HashSet<QuSignature>();
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (BsonDocument doc in documents)
            {
                QuSignature signature = new QuSignature();
                signature  = BsonSerializer.Deserialize<QuSignature>(doc);
                foreach(QuSymptom symptom in signature.symptoms.Values)
                {
                    symptom.SetPainLevel();
                }
                signature.ComputeInitialScore();
                signatures.Add(signature);
            }
            return signatures;
        } 
    }
}
