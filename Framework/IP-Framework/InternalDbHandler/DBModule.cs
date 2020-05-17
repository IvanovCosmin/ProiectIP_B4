using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework.InternalDbHandler
{
    class DBModule
    {
        private static UserHandler userHandler;
        private static QuizSigsHandler sigsHandler;
        private static NotificationsHandler notifHandler;

        public DBModule()
        {
            userHandler = new UserHandler(Utils.Singleton<DBInstance>.Instance);
            sigsHandler = new QuizSigsHandler(Utils.Singleton<DBInstance>.Instance);
            notifHandler = new NotificationsHandler(Utils.Singleton<DBInstance>.Instance);
        }

        public UserHandler GetUserHandler()
        {
            return userHandler;
        }

        public QuizSigsHandler GetSigsHandler()
        {
            return sigsHandler;
        }

        public NotificationsHandler GetNotifHandler()
        {
            return notifHandler;
        }
    }
}
