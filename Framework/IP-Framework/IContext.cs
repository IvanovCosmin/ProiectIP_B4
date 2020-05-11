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

        public byte[] answer;
        public int sizeOfAnswer;

        public IContext(byte[] initContext, int initSizeOfContext)
        {
            context = initContext;
            sizeOfContext = initSizeOfContext;

            string raspuns = "Not implemented yet, but here should stay the answer for the query";
            sizeOfAnswer = raspuns.Length;
            answer = Encoding.ASCII.GetBytes(raspuns);
        }

        public IContext()
        {
            context = null;
            sizeOfContext = 0;

            string raspuns = "Not implemented yet, but here should stay the answer for the query";
            sizeOfAnswer = raspuns.Length;
            answer = Encoding.ASCII.GetBytes(raspuns);
        }

        public static implicit operator IContext(Dictionary<string, string> v)
        {
            throw new NotImplementedException();
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
        public double x;
        public double y;

        public string specificSearch;

        public Dictionary<string, string> AnswerDictionary;

        public EpidemyContext(String json)
        {
            this.json = json;
        }
        public EpidemyContext()
        {

        }

    }

    class SymptomLearningContext : IContext
    {
        // to be discussed and implemented
    }
    class SymptomContext : IContext
    {
        public int id;
        public float status;
        public String response;

        public SymptomContext(int id, float status)
        {
            this.id = id;
            this.status = status;
        }
    }

    class FormContext : IContext
    {
        // to be discussed and implemented
    }

    class DataBaseContext : IContext
    {
        public DataBaseDefines databaseId;
        public DataBaseDefines databaseFunctionId;
        public Dictionary<string, string> ParametersDictionary;

        public Dictionary<string, string> AnswerDictionary;
        public List<Point> points;
    }
}