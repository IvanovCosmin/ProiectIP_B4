using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class GloballyAvailableFunctions
    {

    }
    abstract class IModule
    {
        
        public abstract bool InvokeCommand(SubModuleFunctions command, IContext contextHandler); 
        public abstract bool Init(byte[] context, int sizeOfContext);
        public abstract bool UnInit();
    }
}
