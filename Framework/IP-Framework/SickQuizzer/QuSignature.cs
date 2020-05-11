using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    public class QuSignature
    {
        public Dictionary<string, QuSymptom> symptoms;
        public string name;
        public int initialScore;
        public int currentScore;
        public int currentPositiveScore;
        public int id;

        public QuSignature()
        {
            initialScore = 0;
            currentPositiveScore = 0;


        }

        public void ComputeInitialScore()
        {
            foreach (var symptom in symptoms)
            {
                initialScore += symptom.Value.importanta;
            }
            currentScore = initialScore;
        }
    }
}
