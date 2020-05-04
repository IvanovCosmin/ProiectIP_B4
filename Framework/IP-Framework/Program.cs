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
            //InternalDbHandler.DBModule internalDB = Utils.Singleton<InternalDbHandler.DBModule>.Instance;            
            //internalDB.GetUserHandler().ShowData();
            //UserWrapper user = new UserWrapper("3");
            //internalDB.GetUserHandler().InsertUser(user);

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"logs.txt", true))
            {
                file.WriteLine("scriu aici chestii");
            }

            CreateWebHostBuilder(args).Build().Run();

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"logs.txt", true))
            {
                file.WriteLine("a pornit serverul");
            }


        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<WebStartup>();
    }
}
