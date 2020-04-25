using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework.InternalDbHandler
{
    class DBModule
    {
        private static UserHandler userHandler;
        
        public DBModule()
        {
            userHandler = new UserHandler(Utils.Singleton<DBInstance>.Instance);
        }

        public UserHandler GetUserHandler()
        {
            return userHandler;
        }

    }
}
