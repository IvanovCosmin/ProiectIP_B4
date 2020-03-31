using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class EpidemyAlert : IModule
    {
        private EventHandler fatherHandler;
        private String text = "EpidemyAlert constructor";

        public EpidemyAlert(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for EpidemyAlert subModule");

            EpidemyContext subModuleContextHandler = contextHandler as EpidemyContext;
            switch (command)
            {
                case SubModuleFunctions.EpidemyAlert:
                    // alert user for epidemy
                    return true;
                case SubModuleFunctions.EpidemyAskData:
                    // check data for epidemy
                    return true;
                case SubModuleFunctions.EpidemyCheckForAlert:
                    // alert watcher
                    return true;
                case SubModuleFunctions.EpidemyStoreData:
                    // save data about the epidemy
                    return true;
                default:
                    return false;
            }
        }

        public override bool Init(byte[] context, int sizeOfContext)
        {
            Console.WriteLine("Init execution");
            return true;
        }

        public override bool UnInit()
        {
            Console.WriteLine("UnInit execution");
            return true;
        }
    }
}
