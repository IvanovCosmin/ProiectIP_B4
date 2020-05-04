using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    interface IQuizzerStrategy
    {
        public string GetQuestionString(string symptomName);
        public bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer);
    }

    class QuizzerStrategyContext
    {
        IQuizzerStrategy quizzerStrategy;

        public IQuizzerStrategy GetStrategy()
        {
            return quizzerStrategy;
        }

        public void SetContext(Answer.QUESTION_TYPE questionType)
        {
            switch (questionType)
            {
                case Answer.QUESTION_TYPE.QUESTION_SICKNESS_LEVEL:
                    quizzerStrategy = new QuestionSicknessLevelStrategy();
                    break;
                case Answer.QUESTION_TYPE.QUESTION_BOOLEAN:
                    quizzerStrategy = new QuestionBooleanStrategy();
                    break;
                case Answer.QUESTION_TYPE.QUESTION_NUMBER:
                    quizzerStrategy = new QuestionNumberStrategy();
                    break;

            }
        }
    }

    class QuestionSicknessLevelStrategy : IQuizzerStrategy
    {
        public bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer)
        {
            Answer.QUESTION_SICKNESS_LEVEL minRequiredSicknessLevel = signature.symptoms[symptomName].min_durere;
            Answer.QUESTION_SICKNESS_LEVEL maxRequiredSicknessLevel = signature.symptoms[symptomName].max_durere;
            Answer.QUESTION_SICKNESS_LEVEL answerValue = answer.GetAnswerSicknessLevel();
            if(answerValue>= minRequiredSicknessLevel && answerValue <= maxRequiredSicknessLevel)
            {
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
    class QuestionBooleanStrategy : IQuizzerStrategy
    {
        public bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer)
        {
            bool requiredAnswer = signature.symptoms[symptomName].valoare;
            bool answerValue = answer.GetAnswerBoolean();
            if(requiredAnswer == answerValue)
            {
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
    class QuestionNumberStrategy : IQuizzerStrategy
    {
        public bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer)
        {
            float minRequiredAnswer = signature.symptoms[symptomName].min;
            float maxRequiredAnswer = signature.symptoms[symptomName].max;
            float answerValue = answer.GetAnswerNumeric();
            if (answerValue >= minRequiredAnswer && answerValue <= maxRequiredAnswer)
            {
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
