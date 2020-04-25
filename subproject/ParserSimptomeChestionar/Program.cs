using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class QuSymptom
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

    public NivelDurere min_durere;
    public NivelDurere max_durere;
    public string tip;
    public int importanta;
    public float min;
    public float max;
    public bool valoare;
    
};

class QuSignature
{
    public Dictionary<string, QuSymptom> symptoms;
}



class QuSymptomsParser
{
    List<QuSymptom> symptoms;
    public QuSymptomsParser()
    {
        symptoms = new List<QuSymptom>();
    }

    private string NormalizeJson(string json)
    {
        return json.Replace("\"deloc\"", "0")
            .Replace("\"mic\"", "1")
            .Replace("\"mediu\"", "2")
            .Replace("\"tare\"", "3");
    }

    private QuSymptom.NivelDurere DurereNumToEnum(int num)
    {

        switch(num)
        {
            case 0:
                return QuSymptom.NivelDurere.DURERE_DELOC;
            case 1:
                return QuSymptom.NivelDurere.DURERE_MICA;
            case 2:
                return QuSymptom.NivelDurere.DURERE_MEDIE;
            case 3:
                return QuSymptom.NivelDurere.DURERE_MARE;
            default:
                return QuSymptom.NivelDurere.DURERE_DELOC;
        }

    }

    public void ParseJson(string json)
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
        Console.WriteLine(symptoms["glicemie"].tip);
    }
}




namespace ParserSimptomeChestionar
{
    class Program
    {
        static void Main(string[] args)
        {
            QuSymptomsParser qp = new QuSymptomsParser();

            using (StreamReader sr = new StreamReader("test.json.txt"))
            {
                String file = sr.ReadToEnd();
                qp.ParseJson(file);

            }
        }
    }
}
