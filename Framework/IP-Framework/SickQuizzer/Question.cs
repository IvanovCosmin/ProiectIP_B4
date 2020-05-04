using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    public class Question
    {
        private string questionText;
        private string correspondingSymptom;
        private int questionID;
        private Answer.QUESTION_TYPE questionType;

        public string GetCorrespondingSymptom()
        {
            return correspondingSymptom;
        }

        public Answer.QUESTION_TYPE GetQuestionType()
        {
            return questionType;
        }

        public string GetQuestionText()
        {
            return questionText;
        }

        private void SetQuestionText(string questionText)
        {
            this.questionText = questionText;
        }

        public Question(string symptomName,  Answer.QUESTION_TYPE questionType)
        {
            QuizzerStrategyContext quizzerStrategyContext = new QuizzerStrategyContext();
            quizzerStrategyContext.SetContext(questionType);

            questionText = quizzerStrategyContext.GetStrategy().GetQuestionString(symptomName);
            this.questionType = questionType;
            this.correspondingSymptom = symptomName;
        }
    }
}
