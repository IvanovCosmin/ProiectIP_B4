using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class SymptomLearning : IModule
    {
        private EventHandler fatherHandler;
        private String text = "SymptomLearning constructor";

        public SymptomLearning(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for SymptomLearning subModule");

            SymptomLearningContext subModuleContextHandler = contextHandler as SymptomLearningContext;
            switch (command)
            {
                case SubModuleFunctions.MachineLearningAdapt:
                    // improve machine learning
                    return true;
                case SubModuleFunctions.MachineLearningAsk:
                    // query machine learning
                    return true;
                case SubModuleFunctions.MachineLearningGetResults:
                    // query results
                    return true;
                case SubModuleFunctions.MachineLearningStoreResults:
                    // store results for improvements
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
