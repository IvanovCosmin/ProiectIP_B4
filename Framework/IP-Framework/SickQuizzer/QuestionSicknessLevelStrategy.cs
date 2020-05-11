using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    class QuestionSicknessLevelStrategy : IQuizzerStrategy
    {
        public bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer)
        {
            Answer.QUESTION_SICKNESS_LEVEL minRequiredSicknessLevel = signature.symptoms[symptomName].min_durere;
            Answer.QUESTION_SICKNESS_LEVEL maxRequiredSicknessLevel = signature.symptoms[symptomName].max_durere;
            Answer.QUESTION_SICKNESS_LEVEL answerValue = answer.GetAnswerSicknessLevel();
            if (answerValue >= minRequiredSicknessLevel && answerValue <= maxRequiredSicknessLevel)
            {
                signature.currentPositiveScore += signature.symptoms[symptomName].importanta;
                return true;
            }
            signature.currentScore -= signature.symptoms[symptomName].importanta;
            return false;
        }

        public string GetQuestionString(string symptomName)
        {
            return ("Te doare " + symptomName + "? Daca da, cat de tare?");
        }
    }
}
