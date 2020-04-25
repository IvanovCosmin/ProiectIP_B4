using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework.Utils
{
    public class Singleton<T> where T : class, new()
    {
        private Singleton() { }
       
        private static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        public static T Instance { get { return instance.Value; } }
    }
}
