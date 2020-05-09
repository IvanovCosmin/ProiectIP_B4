using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework.API
{
    public class DiagnosticReport
    {
        private String name;
        private Double percentage;
        public DiagnosticReport(String name, Double percentage)
        {
            this.name = name;
            this.percentage = percentage;
        }
        public string GetName()
        {
            return name;
        }

        public Double GetPercentage()
        {
            return percentage;
        }
    }
}
