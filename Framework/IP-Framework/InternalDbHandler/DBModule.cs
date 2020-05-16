using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework.InternalDbHandler
{
    class DBModule
    {
        private static UserHandler userHandler;
        private static QuizSigsHandler sigsHandler;

        public DBModule()
        {
            userHandler = new UserHandler(Utils.Singleton<DBInstance>.Instance);
            sigsHandler = new QuizSigsHandler(Utils.Singleton<DBInstance>.Instance);
        }

        public UserHandler GetUserHandler()
        {
            return userHandler;
        }

        public QuizSigsHandler GetSigsHandler()
        {
            return sigsHandler;
        }
    }
}
