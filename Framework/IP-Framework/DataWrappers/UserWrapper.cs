using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    // the user wrapper. todo everything here
    class UserWrapper
    {
        public string userid { get; set; }
        List<string> Symptoms { get; set; }
        public String lat { get; set; }
        public String lon { get; set; }
        public UserWrapper(string userid)
        {
            this.userid = userid;
            Symptoms = new List<string>();
            lat = "";
            lon = "";
        }
        public void AddNewSymptom(string symptom)
        {
            Symptoms.Add(symptom);
        }
        public bool HasSymptom(string Symptom)
        {
            return Symptoms.Contains(Symptom);
        }

        public bool HasSymptomInArea(string Symptom)
        {
            return true;
        }

        public void SetLon(String lon)
        {
            this.lon = lon;
        }

        public void SetLat(String lat)
        {
            this.lat = lat;
        }
    }
}
