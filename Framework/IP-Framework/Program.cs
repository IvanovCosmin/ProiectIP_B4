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
            InternalDbHandler.DBModule internalDB = Utils.Singleton<InternalDbHandler.DBModule>.Instance;            
            internalDB.GetUserHandler().ShowData();
            UserWrapper user = new UserWrapper("3");
            internalDB.GetUserHandler().InsertUser(user);

            CreateWebHostBuilder(args).Build().Run();
         

        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<WebStartup>();
    }
}
