using IP_Framework;
using IP_Framework.InternalDbHandler;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using IP_Framework.Utils;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Quizzer
{
    public class SymptomsHolder
    {
        private ISet<QuSignature> signatures;
        private Dictionary<string, HashSet<QuSignature>> symptomToSignatures;
        private ISet<string> chekckedSymtpoms;
        private const double minValidPercentage = 0.5;
        private const double minCorrectPercentage = 0.75;
        
        public SymptomsHolder(ISet<QuSignature> signatures)
        {
            symptomToSignatures = new Dictionary<string, HashSet<QuSignature>>();
            this.signatures = signatures;
            foreach(QuSignature signature in signatures)
            {
                foreach(var symptom in signature.symptoms)
                {
                    if (!symptomToSignatures.ContainsKey(symptom.Key))
                    {
                        symptomToSignatures.Add(symptom.Key, new HashSet<QuSignature>());
                        symptomToSignatures[symptom.Key].Add(signature);
                    }
                    else
                    {
                        symptomToSignatures[symptom.Key].Add(signature);
                    }
                }
            }
            chekckedSymtpoms = new HashSet<string>();
        }

        public Question GetNextQuestion()
        {
            foreach(QuSignature signature in signatures)
            {
                foreach(var symptom in signature.symptoms)
                {
                    Question question = new Question(symptom.Key, symptom.Value.questionString, symptom.Value.GetQuestionType());
                    if (!chekckedSymtpoms.Contains(symptom.Key))
                    {
                        chekckedSymtpoms.Add(symptom.Key);
                        return question;
                    }
                }
            }
            return null;
        }

        private Question ComputeNextQuestion()
        {

            return null;
        }

        public int GetSignaturesCount()
        {
            return signatures.Count;
        }

        public async System.Threading.Tasks.Task<string> GetJsonVerdictAsync(int id)
        {
            JObject jObject = new JObject();
            JArray jArray = new JArray();
            jObject.Add("verdict", jArray);
            List<QuSignature> verdicts = new List<QuSignature>();
            foreach(QuSignature signature in signatures)
            {
                if(((double)signature.currentPositiveScore / (double)signature.initialScore) > minCorrectPercentage)
                {
                    verdicts.Add(signature);
                }
            }
            UserHandler userHandler = new UserHandler(Singleton<DBInstance>.Instance);

            var collection = userHandler.GetCollection();
            userHandler.ShowData();

            foreach (QuSignature signature in verdicts)
            {
                var document = new BsonDocument
                {
                    {"disease", signature.name },
                    {"date", DateTime.Now.ToString() }
                };
                jArray.Add(signature.name);
                var filter = Builders<BsonDocument>.Filter.Eq("userid", id.ToString());
                var update = Builders<BsonDocument>.Update.Push("diseases", document);
                var result = await collection.UpdateOneAsync(filter, update);
            }

           

            
            
            return jObject.ToString();
        }

        public void ProcessAnswer(Answer answer)
        {
            QuizzerStrategyContext strategyContext = new QuizzerStrategyContext();
            strategyContext.SetContext(answer.GetAnswerType());
            string correspondingSymptom = answer.GetCorrespondingSympton();
            IList<QuSignature> removedSignatures = new List<QuSignature>();


            foreach(var signature in symptomToSignatures[correspondingSymptom])
            {
                if(!strategyContext.GetStrategy().ApplyAnswerToSignature(signature, correspondingSymptom, answer))
                {
                    double percentage = (double)signature.currentScore / (double)signature.initialScore;
                    if (percentage < minValidPercentage)
                    {
                        removedSignatures.Add(signature);
                    }
                }
            }

            foreach(QuSignature signature in removedSignatures)
            {
                foreach(var symptom in signature.symptoms)
                {
                    symptomToSignatures[symptom.Key].Remove(signature);
                }
                signatures.Remove(signature);
            }
        }
    }
}
