using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    public class QuSymptom
    {
        public static class SymptomTypes
        {
            public static string NIVEL_DURERE = "NIVEL_DURERE";
        }
        public enum NivelDurere : int
        {
            DURERE_DELOC = 0,
            DURERE_MICA = 1,
            DURERE_MEDIE = 2,
            DURERE_MARE = 3
        }

        public Answer.QUESTION_SICKNESS_LEVEL min_durere;
        public Answer.QUESTION_SICKNESS_LEVEL max_durere;
        public string tip;
        public string questionString;
        public int importanta;
        public float min;
        public float max;
        public bool valoare;

        public Answer.QUESTION_TYPE GetQuestionType()
        {
            if (tip == "interval")
            {
                return Answer.QUESTION_TYPE.QUESTION_NUMBER;
            }
            if (tip == "existenta")
            {
                return Answer.QUESTION_TYPE.QUESTION_BOOLEAN;
            }
            if (tip == "nivel_durere")
            {
                return Answer.QUESTION_TYPE.QUESTION_SICKNESS_LEVEL;
            }
            return Answer.QUESTION_TYPE.QUESTION_BOOLEAN;
        }
    };
}
