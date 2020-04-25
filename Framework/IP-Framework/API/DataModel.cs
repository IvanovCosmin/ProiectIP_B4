using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IP_Framework.API
{
    public class DataModel
    {
        public int ID;
        public int type;
        public String content;
        public DataModel(int id, int type, String content)
        {
            this.ID = id;
            this.type = type;
            this.content = content;
        }

    }
}
