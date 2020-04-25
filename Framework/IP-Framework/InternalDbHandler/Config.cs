using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework.InternalDbHandler
{
    static class Config
    {
        public static string CONNECTION_STRING = "mongodb+srv://cosmin:12345666@ip0-s3dwl.mongodb.net/test?retryWrites=true&w=majority";
        public static string DB_NAME = "cancer";
        public static string COLLECTION_USERS_NAME = "users";

    }
}
