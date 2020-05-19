using System;
using System.Collections.Generic;
using IP_Framework.InternalDbHandler;
using MongoDB.Bson;

namespace IP_Framework
{
    class EpidemyAlert : IModule
    {
        private EventHandler fatherHandler;

        public static double AreaAroundYuu = 0.2;
        public static int AreaAroundYuuCases = 5;

        public static double NeighourHood = 1.5;
        public static int NeighourHoodCases = 20;

        public static double Town = 10;
        public static int TownCases = 150;

        public static double Country = 100;
        public static int CountryCases = 1000;

        public EpidemyAlert(EventHandler father)
        {
            fatherHandler = father;
        }

        public String CreateConvexHauls(List<Point> points)
        {

            List<List<Point>> finalResultList = new List<List<Point>>();

            foreach (List<Point> list in ConvexHaul.CalculateHull(points, AreaAroundYuu))
            {
                finalResultList.Add(list);
            }
            foreach (List<Point> list in ConvexHaul.CalculateHull(points, NeighourHood))
            {
                finalResultList.Add(list);
            }
            foreach (List<Point> list in ConvexHaul.CalculateHull(points, Town))
            {
                finalResultList.Add(list);
            }
            foreach (List<Point> list in ConvexHaul.CalculateHull(points, Country))
            {
                finalResultList.Add(list);
            }

            String JSON = "{result : [";

            foreach (var list in finalResultList)
            {
                JSON = JSON + "[";
                foreach (var point in list)
                {
                    JSON = JSON + "{" + point.x + ", " + point.y + "},";
                }
                JSON = JSON.TrimEnd(',');
                JSON = JSON + "],";
            }

            JSON = JSON.TrimEnd(',');
            JSON = JSON + "]}";

            Console.WriteLine(JSON);

            return JSON;
        }

        public String CheckIfPointsCauseAlert(List<Point> points, Point user, String disease)
        {
            String JSON = "{areas : [";

            int counterForAreaAroundYuu = 0;
            int counterForNeighourHood = 0;
            int counterForTown = 0;
            int counterForCountry = 0;

            foreach (Point point in points)
            {
                if (ConvexHaul.Distance(point, user) < AreaAroundYuu)
                    counterForAreaAroundYuu++;
                if (ConvexHaul.Distance(point, user) < NeighourHood)
                    counterForNeighourHood++;
                if (ConvexHaul.Distance(point, user) < Town)
                    counterForTown++;
                if (ConvexHaul.Distance(point, user) < Country)
                    counterForCountry++;
            }

            DBModule instance = Utils.Singleton<DBModule>.Instance;
            NotificationsHandler notifHandler = instance.GetNotifHandler();

            if (counterForAreaAroundYuu >= AreaAroundYuuCases)
            {
                JSON = JSON + "{\"AreaAroundYou\" : 1},";
                notifHandler.InsertNotificationToAllAffectedUsers(user, AreaAroundYuu, disease);
            }
            else
            {
                JSON = JSON + "{\"AreaAroundYou\" : 0},";
            }

            if (counterForNeighourHood >= NeighourHoodCases)
            {
                JSON = JSON + "{\"NeighourHood\" : 1},";
                notifHandler.InsertNotificationToAllAffectedUsers(user, NeighourHood, disease);
            }
            else
            {
                JSON = JSON + "{\"NeighourHood\" : 0},";
            }

            if (counterForTown >= TownCases)
            {
                JSON = JSON + "{\"Town\" : 1},";
                notifHandler.InsertNotificationToAllAffectedUsers(user, Town, disease);
            }
            else
            {
                JSON = JSON + "{\"Town\" : 0},";
            }

            if (counterForCountry >= CountryCases)
            {
                JSON = JSON + "{\"Country\" : 1}]}";
                notifHandler.InsertNotificationToAllAffectedUsers(user, Country, disease);
            }
            else
            {
                JSON = JSON + "{\"Country\" : 0}]}";
            }

            return JSON;
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for EpidemyAlert subModule");

            EpidemyContext subModuleContextHandler = contextHandler as EpidemyContext;


            if (subModuleContextHandler == null)
                return false;

            DBModule instance = Utils.Singleton<DBModule>.Instance;
            UserHandler userHandler = instance.GetUserHandler();
            NotificationsHandler notifHandler = instance.GetNotifHandler();
            List<Point> points;

            switch (command)
            {

                case SubModuleFunctions.EpidemyCheckForAreas:

                    if (subModuleContextHandler.specificSearch != null)
                    {
                        points = userHandler.GetPointsForDisease(subModuleContextHandler.specificSearch);
                    }
                    else
                    {
                        points = userHandler.GetPoints();
                    }
                    subModuleContextHandler.json = CreateConvexHauls(points);
                    return true;

                case SubModuleFunctions.EpidemyCheckForAlert:

                    points = userHandler.GetPointsForDisease(subModuleContextHandler.specificSearch);

                    String user_id = subModuleContextHandler.user_id;
                    Point user = userHandler.GetPointsForUser(user_id);

                    if(user != null)
                        subModuleContextHandler.json = CheckIfPointsCauseAlert(points, user, subModuleContextHandler.specificSearch);
                    return true;

                case SubModuleFunctions.GetAllNotifications:

                    BsonArray notifs = notifHandler.GetAllNotifs(subModuleContextHandler.specificSearch);

                    subModuleContextHandler.json = notifs.ToString();

                    return true;

                default:

                    return false;
            }
        }

        public override bool Init(byte[] context, int sizeOfContext)
        {
            Console.WriteLine("Init execution");
            return true;
        }

        public override bool UnInit()
        {
            Console.WriteLine("UnInit execution");
            return true;
        }
    }
}
