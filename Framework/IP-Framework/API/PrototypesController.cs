using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace IP_Framework.API
{
    [Route("api/prototypes")]
    [ApiController]
    public class PrototypesController : ControllerBase
    {
      
        
        // POST: api/Prototype
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

        // PUT: api/Prototype/5
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
        public String Post([FromForm] int id) {
            byte[] idBytes = BitConverter.GetBytes(id);
            EventHandlerContext eventHandlerContext = new EventHandlerContext(idBytes, idBytes.Length);
            eventHandlerContext.command = EventHandlerFunctions.RequestCommand;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.MachineLearningAsk;
            return "succes";
        }
        [HttpPost("send-response")]
        public string Post([FromForm] Response response)
        {
            var json = response.ToString();
            IContext context = new IContext(json);
            EventHandlerContext eventHandlerContext = new EventHandlerContext();
            eventHandlerContext.contextHandler = context;
            eventHandlerContext.command = EventHandlerFunctions.InvokeCommand;
            eventHandlerContext.subModuleCommand = SubModuleFunctions.MachineLearningGetResults;
            EventHandler eventHandler = new EventHandler();
            eventHandler.InvokeCommand(eventHandlerContext);
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
