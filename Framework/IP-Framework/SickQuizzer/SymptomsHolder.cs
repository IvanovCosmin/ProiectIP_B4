using IP_Framework;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

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

        public string GetJsonVerdict()
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
            foreach(QuSignature signature in verdicts)
            {
                jArray.Add(signature.name);
            }
            if(jArray.Count == 0)
            {
                jArray.Add("nimic"); // verdictul o sa contina "nimic" daca nu ai adaugat nimic.
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
