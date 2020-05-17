using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
            return context.json;
        }

        [HttpPost("get-question")]
        public String Post([FromBody] JObject data) {
            int id = data["id"].ToObject<int>();
            byte[] idBytes = BitConverter.GetBytes(id);
            IContext context = new SymptomContext(id, 0);
            EventHandlerContext eventHandlerContext = new EventHandlerContext(idBytes, idBytes.Length);
            eventHandlerContext.command = EventHandlerFunctions.SymptomBasedDetectionModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.GetQuestion;
            eventHandlerContext.contextHandler = context;
            EventHandler eventHandler = EventHandler.GetInstance();
            eventHandler.InvokeCommand(eventHandlerContext);
            return (context as SymptomContext).response;
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
            return (context as SymptomContext).response;
        }

        [HttpPost("check-epidemic")]
        public String GetEpidemic([FromBody] JObject data)
        {
            EventHandler eventHandler = new EventHandler();
            EpidemyContext context = new EpidemyContext();
            context.specificSearch = data["disease"].ToObject<String>();
            EventHandlerContext eventHandlerContext = new EventHandlerContext();
            eventHandlerContext.contextHandler = context;
            eventHandlerContext.command = EventHandlerFunctions.EpidemyAlertModule;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.EpidemyCheckForAlert;
            eventHandler.InvokeCommand(eventHandlerContext);
            return context.json;
        }

        [HttpGet("get-notifications")]
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
            return context.json;

        }

    }
}
