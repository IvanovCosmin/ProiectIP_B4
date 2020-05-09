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
            //InternalDbHandler.DBModule internalDB = Utils.Singleton<InternalDbHandler.DBModule>.Instance;            
            //internalDB.GetUserHandler().ShowData();
            //UserWrapper user = new UserWrapper("3");
            //internalDB.GetUserHandler().InsertUser(user);
            Console.WriteLine(newHandler.InvokeCommand(context));
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<WebStartup>();
    }
}
