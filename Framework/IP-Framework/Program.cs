using System;
using IP_Framework.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace IP_Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            EventHandler newHandler = new EventHandler();
            


            byte[] array = new byte[100];

            EventHandlerContext context = new EventHandlerContext(array, 100);
            context.command = EventHandlerFunctions.SymptomBasedDetectionModule;
            context.subModuleCommand = SubModuleFunctions.StartForm;

            Console.WriteLine(newHandler.InvokeCommand(context));
            CreateWebHostBuilder(args).Build().Run();
            

        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<WebStartup>();
    }
}
