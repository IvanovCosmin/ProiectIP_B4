using System;

namespace IP_Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            EventHandler newHandler = new EventHandler();


            byte[] array = new byte[100];

            EventHandlerContext context = new EventHandlerContext(array, 100);
            context.command = EventHandlerFunctions.SymptomBasedDetectionModule;
            context.subModuleCommand = SubModuleFunctions.StartForm;

            Console.WriteLine(newHandler.InvokeCommand(context));

        }
    }
}
