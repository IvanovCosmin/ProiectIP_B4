using System;
using System.Net;
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

        [HttpPost("send-command")]
        public string Post( [FromForm] Command command)
        {
            var json = command.ToString();
            EventHandler eventHandler = new EventHandler();
            IContext context = new IContext(json);
            EventHandlerContext eventHandlerContext = new EventHandlerContext();
            eventHandlerContext.contextHandler = context;
            eventHandlerContext.command = EventHandlerFunctions.InvokeCommand;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.AskForFormResults;
            eventHandler.InvokeCommand(eventHandlerContext);
            return json;
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
            return "succes";
        }

        [HttpPost("get-diagnosis")]
        public String Post([FromForm] String imageUrl)
        {
            using (var webClient = new WebClient())
            {
               byte[] imageBytes = webClient.DownloadData(imageUrl);
                EventHandler eventHandler = new EventHandler();
                EventHandlerContext eventHandlerContext = new EventHandlerContext(imageBytes, imageBytes.Length)
                {
                    command = EventHandlerFunctions.ImageProcessingModule,
                    subModuleCommand = SubModuleFunctions.ImageComparePhoto
            };
                
                eventHandler.InvokeCommand(eventHandlerContext);
            }
           
            return "succes";
        }

    }
}
