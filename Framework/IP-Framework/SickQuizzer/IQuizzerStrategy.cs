using System;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    interface IQuizzerStrategy
    {
        string GetQuestionString(string symptomName);
        bool ApplyAnswerToSignature(QuSignature signature, string symptomName, Answer answer);
    }
}
