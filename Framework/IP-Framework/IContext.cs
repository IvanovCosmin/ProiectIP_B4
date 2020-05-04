using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class IContext
    {
        public byte[] context;
        public int sizeOfContext;
        public String json;

        public IContext(byte[] initContext, int initSizeOfContext)
        {
            context = initContext;
            sizeOfContext = initSizeOfContext;
        }

        public IContext()
        {
            context = null;
            sizeOfContext = 0;
        }

        public IContext(String json)
        {
            this.json = json;
        }
    }

    class ImageContext : IContext
    {
        // to be discussed and implemented
    }

    class EpidemyContext : IContext
    {
        // to be discussed and implemented
    }

    class SymptomLearningContext : IContext
    {
        // to be discussed and implemented
    }

    class FormContext : IContext
    {
        // to be discussed and implemented
    }
}
