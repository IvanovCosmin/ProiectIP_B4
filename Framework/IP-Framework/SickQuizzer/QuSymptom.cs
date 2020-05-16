using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        public string min_pain;
        public string max_pain;

        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public double min;
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public double max;
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

        public void SetPainLevel()
        {
            if(min_pain == null)
            {
                return;
            }
            switch (min_pain)
            {
                case "deloc":
                    min_durere = Answer.QUESTION_SICKNESS_LEVEL.ABSENT;
                    break;
                case "mic":
                    min_durere = Answer.QUESTION_SICKNESS_LEVEL.LITTLE;
                    break;

                case "mediu":
                    min_durere = Answer.QUESTION_SICKNESS_LEVEL.MEDIUM;
                    break;
                case "mare":
                    min_durere = Answer.QUESTION_SICKNESS_LEVEL.HIGH;
                    break;
            }
            if (max_pain == null)
            {
                return;
            }
            switch (max_pain)
            {
                case "deloc":
                    max_durere = Answer.QUESTION_SICKNESS_LEVEL.ABSENT;
                    break;
                case "mic":
                    max_durere = Answer.QUESTION_SICKNESS_LEVEL.LITTLE;
                    break;
                case "mediu":
                    max_durere = Answer.QUESTION_SICKNESS_LEVEL.MEDIUM;
                    break;
                case "mare":
                    max_durere = Answer.QUESTION_SICKNESS_LEVEL.HIGH;
                    break;
            }
        }
    };
}
