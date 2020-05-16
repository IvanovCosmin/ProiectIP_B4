using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    class QuestionNumberStrategy : IQuizzerStrategy
    {
        public bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer)
        {
            double minRequiredAnswer = signature.symptoms[symptomName].min;
            double maxRequiredAnswer = signature.symptoms[symptomName].max;
            double answerValue = answer.GetAnswerNumeric();
            if (answerValue >= minRequiredAnswer && answerValue <= maxRequiredAnswer)
            {
                signature.currentPositiveScore += signature.symptoms[symptomName].importanta;
                return true;
            }
            signature.currentScore -= signature.symptoms[symptomName].importanta;
            return false;
        }

        public string GetQuestionString(string symptomName)
        {
            return ("Estimeaza numeric valoarea " + symptomName + ".");
        }
    }
}
