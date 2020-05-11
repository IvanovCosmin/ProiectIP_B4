using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
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
}
