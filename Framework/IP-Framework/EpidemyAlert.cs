using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IP_Framework
{
    class EpidemyAlert : IModule
    {
        private EventHandler fatherHandler;
        private String text = "EpidemyAlert constructor";

        public static double AreaAroundYuu = 0.2;
        public static double NeighourHood = 1.5;
        public static double Town = 10;
        public static double Country = 100;


        public EpidemyAlert(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for EpidemyAlert subModule");

            EpidemyContext subModuleContextHandler = contextHandler as EpidemyContext;

            DataBaseContext context = new DataBaseContext();
            Dictionary<string, string> dictionary = new Dictionary<string, string>(); // the params
            EventHandlerContext commandContext = new EventHandlerContext();

            switch (command)
            {

                case SubModuleFunctions.EpidemyCheckForAreas:

                    context.databaseId = DataBaseDefines.DatabaseDiseases; // the database id;
                    context.databaseFunctionId = DataBaseDefines.GetAllDots; // function ID


                    dictionary.Add("x", subModuleContextHandler.x.ToString());
                    dictionary.Add("y", subModuleContextHandler.y.ToString());
                    dictionary.Add("disease", subModuleContextHandler.specificSearch);
                    context.ParametersDictionary = dictionary;


                    commandContext.command = EventHandlerFunctions.DataBaseModule;
                    commandContext.subModuleCommand = SubModuleFunctions.DataBaseQueryData;
                    commandContext.coreCommand = CoreKnownFunctions.InvalidCommand;
                    commandContext.contextHandler = context;


                    bool resultcontextHool = fatherHandler.InvokeCommand(commandContext);
                    if (!resultcontextHool)
                    {
                        return false;
                    }

                    resultcontextHool = fatherHandler.InvokeCommand(commandContext);

                    List<Point> points = context.points;

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

                    // CALL frontend care primeste List<List<Point>> si afiseaza valorile si x-y nostru


                    return true;

                case SubModuleFunctions.EpidemyCheckForSpecificAlert:

                    context.databaseId = DataBaseDefines.DatabaseDiseases; // the database id;
                    context.databaseFunctionId = DataBaseDefines.DiseasesSpecificQueryDisease; // function ID

                    dictionary.Add("x", subModuleContextHandler.x.ToString());
                    dictionary.Add("y", subModuleContextHandler.y.ToString());
                    dictionary.Add("disease", subModuleContextHandler.specificSearch);
                    context.ParametersDictionary = dictionary;

                    commandContext.command = EventHandlerFunctions.DataBaseModule;
                    commandContext.subModuleCommand = SubModuleFunctions.DataBaseQueryData;
                    commandContext.coreCommand = CoreKnownFunctions.InvalidCommand;
                    commandContext.contextHandler = context;

                    bool resultSpecific = fatherHandler.InvokeCommand(commandContext);

                    if (!resultSpecific)
                    {
                        return false;
                    }

                    foreach (KeyValuePair<string, string> entry in context.AnswerDictionary)
                    {
                        // Validate each response

                        subModuleContextHandler.AnswerDictionary[entry.Key] = entry.Value; // perechi boala - zona
                    }

                    if (subModuleContextHandler.AnswerDictionary.Count == 0)
                    {
                        return false;
                    }

                    return true;
                case SubModuleFunctions.EpidemyCheckForAlert:

                    context.databaseId = DataBaseDefines.DatabaseDiseases; // the database id;
                    context.databaseFunctionId = DataBaseDefines.DiseasesFullQuery; // function ID

                    dictionary.Add("x", subModuleContextHandler.x.ToString());
                    dictionary.Add("y", subModuleContextHandler.y.ToString());
                    context.ParametersDictionary = dictionary;

                    commandContext.command = EventHandlerFunctions.DataBaseModule;
                    commandContext.subModuleCommand = SubModuleFunctions.DataBaseQueryData;
                    commandContext.coreCommand = CoreKnownFunctions.InvalidCommand;
                    commandContext.contextHandler = context;

                    bool result = fatherHandler.InvokeCommand(commandContext);

                    if (!result)
                    {
                        return false;
                    }

                    foreach (KeyValuePair<string, string> entry in context.AnswerDictionary)
                    {
                        // Validate each response

                        subModuleContextHandler.AnswerDictionary[entry.Key] = entry.Value; // perechi boala - zona
                    }

                    if (subModuleContextHandler.AnswerDictionary.Count == 0)
                    {
                        return false;
                    }

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
