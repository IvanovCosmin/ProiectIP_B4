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
                    Question question = new Question(symptom.Key, symptom.Value.GetQuestionType());
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
                    if (((double)signature.currentScore / (double)signature.initialScore) < 0.5)
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
