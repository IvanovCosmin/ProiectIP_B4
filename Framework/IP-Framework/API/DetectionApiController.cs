using System;
using IP_Framework.InternalDbHandler;
using IP_Framework.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using MongoDB.Bson;

namespace IP_Framework.API
{
    [Route("api/v1/detectionapi")]
    [ApiController]
    public class DetectionApiController : ControllerBase
    {
        
        [HttpPost("example")]
        public string Post([FromForm] Symptomes symptomeList)
        {
            var json = symptomeList.ToString();
            IContext context = new IContext(json);
            EventHandlerContext eventHandlerContext = new EventHandlerContext();
            eventHandlerContext.contextHandler = context;
            eventHandlerContext.command = EventHandlerFunctions.SymptomLearningModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.MachineLearningStoreResults;
            EventHandler eventHandler = new EventHandler();
            eventHandler.InvokeCommand(eventHandlerContext);
            var response = HttpContext.Response;

            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return "succes";
        }

        [HttpPost("check-epidemic-haul")]
        public string PostHaul( [FromBody] JObject data)
        {
            EventHandler eventHandler = new EventHandler();
            EpidemyContext context = new EpidemyContext();
            if (data.ContainsKey("disease"))
            {
                context.specificSearch = data["disease"].ToObject<String>();
            }
            else
            {
                context.specificSearch = null;
            }
            EventHandlerContext eventHandlerContext = new EventHandlerContext();
            eventHandlerContext.contextHandler = context;
            eventHandlerContext.command = EventHandlerFunctions.EpidemyAlertModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.EpidemyCheckForAreas;
            eventHandler.InvokeCommand(eventHandlerContext);
            var response = HttpContext.Response;

            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return context.json;
        }

        [HttpPost("get-question")]
        public string Post([FromBody] JObject data) {
            int id = data["id"].ToObject<int>();
            byte[] idBytes = BitConverter.GetBytes(id);
            IContext context = new SymptomContext(id, 0);
            EventHandlerContext eventHandlerContext = new EventHandlerContext(idBytes, idBytes.Length);
            eventHandlerContext.command = EventHandlerFunctions.SymptomBasedDetectionModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.GetQuestion;
            eventHandlerContext.contextHandler = context;
            EventHandler eventHandler = EventHandler.GetInstance();
            eventHandler.InvokeCommand(eventHandlerContext);

            var response = HttpContext.Response;
             if(response != null) {
           response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
           response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
           response.Headers.Add("Access-Control-Allow-Origin", "*");}
            return (context as SymptomContext).response;
        }

        [HttpOptions("get-question")]
        public void QuestionOptions()
        {
            var response = HttpContext.Response;

            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return;
        }

        [HttpOptions("send-response")]
        public void ResponseOptions()
        {
            var response = HttpContext.Response;

            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return;
        }

        [HttpPost("send-response")]
        public string PostResponse([FromBody] JObject data)
        {
            //TODO: de scos cod redundant
            int id = data["id"].ToObject<int>();
            float status = data["status"].ToObject<float>();
            byte[] idBytes = BitConverter.GetBytes(id);
            IContext context = new SymptomContext(id, status);
            EventHandlerContext eventHandlerContext = new EventHandlerContext(idBytes, idBytes.Length);
            eventHandlerContext.command = EventHandlerFunctions.SymptomBasedDetectionModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.SendResponse;
            eventHandlerContext.contextHandler = context;
            EventHandler eventHandler = EventHandler.GetInstance();
            eventHandler.InvokeCommand(eventHandlerContext);
            var response = HttpContext.Response;
            if (response != null)
            {
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            return (context as SymptomContext).response;
        }

        [HttpPost("check-epidemic")]
        public String GetEpidemic([FromBody] JObject data)
        {
            EventHandler eventHandler = new EventHandler();
            EpidemyContext context = new EpidemyContext();
            context.specificSearch = data["disease"].ToObject<String>();
            context.user_id = data["user_id"].ToObject<String>();
            EventHandlerContext eventHandlerContext = new EventHandlerContext();
            eventHandlerContext.contextHandler = context;
            eventHandlerContext.command = EventHandlerFunctions.EpidemyAlertModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.EpidemyCheckForAlert;
            eventHandler.InvokeCommand(eventHandlerContext);
            var response = HttpContext.Response;

            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return context.json;
        }

        [HttpPost("get-notifications")]
        public string GetNotifs([FromBody] JObject data)
        {
            EventHandler eventHandler = new EventHandler();
            EpidemyContext context = new EpidemyContext();
            context.specificSearch = data["id"].ToObject<String>();
            EventHandlerContext eventHandlerContext = new EventHandlerContext();
            eventHandlerContext.contextHandler = context;
            eventHandlerContext.command = EventHandlerFunctions.EpidemyAlertModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.GetAllNotifications;
            eventHandler.InvokeCommand(eventHandlerContext);
            var response = HttpContext.Response;

            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
     
            return context.json;

        }
        [HttpOptions("get-notifications")]
        public void NotificationOptions()
        {
            var response = HttpContext.Response;

            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return;
        }

        [HttpPost("get-internal-id")]
        public async System.Threading.Tasks.Task<string> GetEpidemicAsync([FromBody] String id)
        {
            DBModule internalDB = Singleton<DBModule>.Instance;
            Console.WriteLine(id);
            var documents = internalDB.GetUserHandler().GetCollectionData();
            
                foreach (BsonDocument doc in documents)
        try { 
                {
                    Console.WriteLine(doc.ToString());
                    if ((String)doc["userid"] == id)
                        return "Exists";
                }
            }
        catch(Exception e)
            {
                Console.WriteLine("Bad data");
            }
            //return "exists";
            HttpClient client = new HttpClient();
            var responseString = await client.GetStringAsync("https://auth-service-ip.herokuapp.com/dbAPI/diagnosisInfo/" + id);
            UserWrapper user = new UserWrapper(id);
            string toBeSearched = "\"longitude\":";
            string lon = responseString.Substring(responseString.IndexOf(toBeSearched) + toBeSearched.Length);           
            lon = lon.Remove(lon.Length - 1);
            Console.WriteLine(lon);
            String lat = responseString.Split(',')[1];
            toBeSearched = "\"latitude\":";
            lat = lat.Substring(lat.IndexOf(toBeSearched) + toBeSearched.Length);
            Console.WriteLine(lat);
            user.SetLat(lat);
            user.SetLon(lon);
            internalDB.GetUserHandler().InsertUser(user);
            return "Created";
        }


    }
}
