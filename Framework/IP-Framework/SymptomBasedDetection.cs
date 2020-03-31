using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class SymptomBasedDetection : IModule
    {
        private EventHandler fatherHandler;
        private String text = "SymptomBasedDetection constructor";

        public SymptomBasedDetection(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for Form SubModule");

            FormContext subModuleContextHandler = contextHandler as FormContext;
            switch (command)
            {
                case SubModuleFunctions.CreateForm:
                    // create form function
                    return true;
                case SubModuleFunctions.AskForFormResults:
                    // return formular result in a context
                    return true;
                case SubModuleFunctions.SaveFormResults:
                    // save form to DB
                    return true;
                case SubModuleFunctions.StartForm:
                    // form start
                    EventHandlerContext newContext = new EventHandlerContext(contextHandler.context, contextHandler.sizeOfContext);
                    newContext.command = EventHandlerFunctions.EpidemyAlertModule;
                    newContext.subModuleCommand = SubModuleFunctions.EpidemyAlert;
                    return fatherHandler.InvokeCommand(newContext);
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
