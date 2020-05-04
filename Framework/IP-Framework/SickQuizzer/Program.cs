using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks.Sources;
using Newtonsoft.Json;
using Quizzer;

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
    public int importanta;
    public float min;
    public float max;
    public bool valoare;

    public Answer.QUESTION_TYPE GetQuestionType()
    {
        if(tip == "interval")
        {
            return Answer.QUESTION_TYPE.QUESTION_NUMBER;
        }
        if(tip == "existenta")
        {
            return Answer.QUESTION_TYPE.QUESTION_BOOLEAN;
        }
        if(tip == "nivel_durere")
        {
            return Answer.QUESTION_TYPE.QUESTION_SICKNESS_LEVEL;
        }
        return Answer.QUESTION_TYPE.QUESTION_BOOLEAN;
    }
};

public class QuSignature
{
    public Dictionary<string, QuSymptom> symptoms;
    public int initialScore;
    public int currentScore;
    public int id;

    public QuSignature(Dictionary<string, QuSymptom> symptoms)
    {
        initialScore = 0;
        this.symptoms = symptoms;
        foreach(var symptom in symptoms)
        {
            initialScore += symptom.Value.importanta;
        }
        currentScore = initialScore;
    }
}



public class QuSymptomsParser
{
     private ISet<QuSignature> signatures;

    public ISet<QuSignature> GetSignatures()
    {
        return signatures;
    }
    public QuSymptomsParser()
    {
        signatures = new HashSet<QuSignature>();
    }

    private string NormalizeJson(string json)
    {
        return json.Replace("\"deloc\"", "0")
            .Replace("\"mic\"", "1")
            .Replace("\"mediu\"", "2")
            .Replace("\"tare\"", "3");
    }

    private Answer.QUESTION_SICKNESS_LEVEL DurereNumToEnum(int num)
    {

        switch(num)
        {
            case 0:
                return Answer.QUESTION_SICKNESS_LEVEL.ABSENT;
            case 1:
                return Answer.QUESTION_SICKNESS_LEVEL.LITTLE;
            case 2:
                return Answer.QUESTION_SICKNESS_LEVEL.MEDIUM;
            case 3:
                return Answer.QUESTION_SICKNESS_LEVEL.HIGH;
            default:
                return Answer.QUESTION_SICKNESS_LEVEL.ABSENT;
        }

    }

    private void ParseJson(string json)
    {
        Dictionary<string, QuSymptom> symptoms;
        json = NormalizeJson(json);
        symptoms = JsonConvert.DeserializeObject<Dictionary<string, QuSymptom>>(json);
        foreach (var item in symptoms)
        {
            if (item.Value.tip == QuSymptom.SymptomTypes.NIVEL_DURERE)
            {
                
                item.Value.min_durere = DurereNumToEnum((int)(item.Value.min));
                item.Value.max_durere = DurereNumToEnum((int)(item.Value.max));
            }
        }
        QuSignature quSignature = new QuSignature(symptoms);
       // quSignature.symptoms = symptoms;
        signatures.Add(quSignature);
       // Console.WriteLine(symptoms["glicemie"].tip);
    }

    public void FeedSignatures(string path)
    {
        FileAttributes fileAttributes = File.GetAttributes(path);
        if(fileAttributes.HasFlag(FileAttributes.Directory))
        {
            string[] filePaths = Directory.GetFiles(path, "*");

            foreach(string filePath in filePaths)
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string file = sr.ReadToEnd();
                    ParseJson(file);
                }
            }
        }
        else
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string file = sr.ReadToEnd();
                ParseJson(file);
            }
        }
    }
}




namespace ParserSimptomeChestionar
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    QuSymptomsParser symptomsParser = new QuSymptomsParser();

        //    symptomsParser.FeedSignatures("C:\\Users\\fanghel\\source\\repos\\Quizzer\\Quizzer\\test.json.txt");

        //    ISet<QuSignature> signatures = symptomsParser.GetSignatures();
        //}
    }
}
