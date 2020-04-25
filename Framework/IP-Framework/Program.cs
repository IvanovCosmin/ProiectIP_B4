using System;

namespace IP_Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            InternalDbHandler.DBModule internalDB = Utils.Singleton<InternalDbHandler.DBModule>.Instance;

            internalDB.GetUserHandler().ShowData();

            UserWrapper user = new UserWrapper("2");

            internalDB.GetUserHandler().InsertUser(user);

            internalDB.GetUserHandler().ShowData();

        }
    }
}
