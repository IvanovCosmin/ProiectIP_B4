using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks.Sources;
using Newtonsoft.Json;
using Quizzer;
using IP_Framework.InternalDbHandler;

using IP_Framework.Utils;

namespace Quizzer
{
    public class QuSymptomsParser
    {
        private ISet<QuSignature> signatures;

        public ISet<QuSignature> GetSignatures()
        {
            return signatures;
        }
        public QuSymptomsParser()
        {
            signatures = new HashSet<QuSignature>();
        }

        private string NormalizeJson(string json)
        {
            return json.Replace("\"deloc\"", "0")
                .Replace("\"mic\"", "1")
                .Replace("\"mediu\"", "2")
                .Replace("\"tare\"", "3");
        }

        private Answer.QUESTION_SICKNESS_LEVEL DurereNumToEnum(int num)
        {

            switch (num)
            {
                case 0:
                    return Answer.QUESTION_SICKNESS_LEVEL.ABSENT;
                case 1:
                    return Answer.QUESTION_SICKNESS_LEVEL.LITTLE;
                case 2:
                    return Answer.QUESTION_SICKNESS_LEVEL.MEDIUM;
                case 3:
                    return Answer.QUESTION_SICKNESS_LEVEL.HIGH;
                default:
                    return Answer.QUESTION_SICKNESS_LEVEL.ABSENT;
            }

        }

        private void ParseJson(string json)
        {
            Dictionary<string, QuSymptom> symptoms;
            json = NormalizeJson(json);
            QuSignature quSignature = new QuSignature();
            quSignature = JsonConvert.DeserializeObject<QuSignature>(json);
            quSignature.ComputeInitialScore();
            foreach (var item in quSignature.symptoms)
            {
                if (item.Value.tip == QuSymptom.SymptomTypes.NIVEL_DURERE)
                {

                    item.Value.min_durere = DurereNumToEnum((int)(item.Value.min));
                    item.Value.max_durere = DurereNumToEnum((int)(item.Value.max));
                }
            }
            // QuSignature quSignature = new QuSignature(symptoms);
            // quSignature.symptoms = symptoms;
            signatures.Add(quSignature);
            // Console.WriteLine(symptoms["glicemie"].tip);
        }

        public void FeedSignatures()
        {
            DBModule instance = Singleton<DBModule>.Instance;
            QuizSigsHandler sigsHandler = instance.GetSigsHandler();

            signatures = sigsHandler.GetSignatures();
        }
    }
}
