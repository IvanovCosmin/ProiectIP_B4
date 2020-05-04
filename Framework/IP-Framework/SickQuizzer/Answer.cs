using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    public class Answer
    {
        public enum QUESTION_TYPE
        {
            QUESTION_SICKNESS_LEVEL, 
            QUESTION_BOOLEAN,
            QUESTION_NUMBER
        }
        public enum QUESTION_SICKNESS_LEVEL
        {
            NONE,
            ABSENT,
            LITTLE,
            MEDIUM,
            HIGH
        }

        public enum QUESTION_BOOLEAN
        {
            NONE,
            TRUE, 
            FALSE
        }

        private int correspondingQuestionID;
        private QUESTION_TYPE answerType;
        private QUESTION_SICKNESS_LEVEL answerSicknessLevel;
        private QUESTION_BOOLEAN answerBoolean;
        private float answerNumeric;
        private string correspondingSymptom;

        public string GetCorrespondingSympton()
        {
            return correspondingSymptom;
        }

        public Answer(QUESTION_TYPE answerType, string correspondingSymptom)
        {
            this.answerType = answerType;
            this.correspondingSymptom = correspondingSymptom;
            this.correspondingQuestionID = correspondingQuestionID;
        }

        public int GetCorrespondingQuestionID()
        {
            return correspondingQuestionID;
        }

        public QUESTION_TYPE GetAnswerType()
        {
            return answerType;
        }

        public QUESTION_SICKNESS_LEVEL GetAnswerSicknessLevel()
        {
            if (answerType != QUESTION_TYPE.QUESTION_SICKNESS_LEVEL)
            {
                return QUESTION_SICKNESS_LEVEL.NONE;
            }
            return answerSicknessLevel;
        }

        public bool SetAnswerSicknessLevel(QUESTION_SICKNESS_LEVEL answerSicknessLevel)
        {
            if (answerType != QUESTION_TYPE.QUESTION_SICKNESS_LEVEL)
            {
                return false;
            }
            this.answerSicknessLevel = answerSicknessLevel;
            return true;
        }

        public bool GetAnswerBoolean()
        {
            if (answerType != QUESTION_TYPE.QUESTION_BOOLEAN)
            {
                return true;
            }
            if(answerBoolean == QUESTION_BOOLEAN.TRUE)
            {
                return true;
            }
            return false;
        }

        public bool SetAnswerBoolean(QUESTION_BOOLEAN answerBoolean)
        {
            if (answerType != QUESTION_TYPE.QUESTION_BOOLEAN)
            {
                return false;
            }
            this.answerBoolean = answerBoolean;
            return true;
        }

        public float GetAnswerNumeric()
        {
            if(answerType != QUESTION_TYPE.QUESTION_NUMBER)
            {
                return -1;
            }
            return this.answerNumeric;
        }

        public bool SetAnswerNumeric(float answerNumeric)
        {
            if (answerType != QUESTION_TYPE.QUESTION_NUMBER)
            {
                return false;
            }
            this.answerNumeric = answerNumeric;
            return true;
        }
    }
}
