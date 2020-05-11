using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    public class Question
    {
        private string questionText;
        private string correspondingSymptom;
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

        public Question(string symptomName, string questionText,  Answer.QUESTION_TYPE questionType)
        {
            QuizzerStrategyContext quizzerStrategyContext = new QuizzerStrategyContext();
            quizzerStrategyContext.SetContext(questionType);

            this.questionText = questionText;
            this.questionType = questionType;
            this.correspondingSymptom = symptomName;
        }
        public string ToJson(int id)
        {
            dynamic jsonObject = new JObject();
            jsonObject.id = id;
            jsonObject.question = questionText;
            switch (questionType)
            {
                case Answer.QUESTION_TYPE.QUESTION_NUMBER:
                    jsonObject.tip = "interval";
                    break;
                case Answer.QUESTION_TYPE.QUESTION_SICKNESS_LEVEL:
                    jsonObject.tip = "nivel_durere";
                    break;
                case Answer.QUESTION_TYPE.QUESTION_BOOLEAN:
                    jsonObject.tip = "existenta";
                    break;
            }

            return jsonObject.ToString();
        }
    }
}
