using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class DataBaseHandler : IModule
    {
        private EventHandler fatherHandler;
        private String text = "DataBaseHandler constructor";

        public DataBaseHandler(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for Database subModule");

            DataBaseContext subModuleContextHandler = contextHandler as DataBaseContext;

            string answer;

            switch (command)
            {
                case SubModuleFunctions.DataBaseAlterData:
                    // modify data

                    answer = "Ai apelat db-ul cu comanda AlterData, dar inca nu e definit un raspuns";
                    contextHandler.answer = Encoding.ASCII.GetBytes(answer);
                    contextHandler.sizeOfAnswer = answer.Length;

                    return true;
                case SubModuleFunctions.DataBaseDestroyData:
                    // remove data

                    answer = "Ai apelat db-ul cu comanda DestroyData, dar inca nu e definit un raspuns";
                    contextHandler.answer = Encoding.ASCII.GetBytes(answer);
                    contextHandler.sizeOfAnswer = answer.Length;

                    return true;
                case SubModuleFunctions.DataBaseQueryData:
                    // search data


                    return true;
                case SubModuleFunctions.DataBaseSaveData:
                    // save data 

                    answer = "Ai apelat db-ul cu comanda SaveData, dar inca nu e definit un raspuns";
                    contextHandler.answer = Encoding.ASCII.GetBytes(answer);
                    contextHandler.sizeOfAnswer = answer.Length;

                    return true;
                default:
                    return false;
            }
        }

        public override bool Init(byte[] context, int sizeOfContext)
        {
            Console.WriteLine("Init execution");
            // here we should create the databases needed, or import them, actually
            return true;
        }

        public override bool UnInit()
        {
            Console.WriteLine("UnInit execution");
            return true;
        }

        public Dictionary<DataBaseDefines, string> DefinesTranslationDictionary = new Dictionary<DataBaseDefines, string>
        (

        );

    }
}
