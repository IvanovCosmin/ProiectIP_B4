using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    class QuestionBooleanStrategy : IQuizzerStrategy
    {
        public bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer)
        {
            bool requiredAnswer = signature.symptoms[symptomName].valoare;
            bool answerValue = answer.GetAnswerBoolean();
            if (requiredAnswer == answerValue)
            {
                signature.currentPositiveScore += signature.symptoms[symptomName].importanta;
                return true;
            }
            signature.currentScore -= signature.symptoms[symptomName].importanta;
            return false;
        }

        public string GetQuestionString(string symptomName)
        {
            return ("Te doare " + symptomName + "?");
        }
    }
}
