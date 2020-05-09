using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework.API
{
    public class FormQuestion
    {
        private String question;
        private int id;
        private String type;
        public FormQuestion(String question, int id, String type)
        {
            this.question = question;
            this.id = id;
            this.type = type;
        }

    }
}
