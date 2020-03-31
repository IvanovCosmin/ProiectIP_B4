using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class ImageProcessing : IModule
    {
        private EventHandler fatherHandler;
        private String text = "ImageProcessing constructor";

        public ImageProcessing(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for Image subModule");

            ImageContext subModuleContextHandler = contextHandler as ImageContext;
            switch (command)
            {
                case SubModuleFunctions.ImageAdapt:
                    // improve machine learning
                    return true;
                case SubModuleFunctions.ImageAddPhoto:
                    // add data to machine learning
                    return true;
                case SubModuleFunctions.ImageComparePhoto:
                    // query machine learning
                    return true;
                case SubModuleFunctions.ImageStoreResults:
                    // store results for machine learning
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
