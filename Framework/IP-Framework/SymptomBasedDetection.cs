using Microsoft.AspNetCore.Antiforgery.Internal;
using MongoDB.Driver.Core.Events;
using Quizzer;
using System;
using System.Collections.Generic;
using System.Text;

namespace IP_Framework
{
    class SymptomBasedDetection : IModule
    {
        private EventHandler fatherHandler;
        private String text = "SymptomBasedDetection constructor";
        private QuizHandler quizHandler;

        public SymptomBasedDetection(EventHandler father)
        {
            fatherHandler = father;
            Console.WriteLine(text);
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            Console.WriteLine("InvokeCommand execution for Form SubModule");
            Question question;
            SymptomContext symptomContext = contextHandler as SymptomContext;
            switch (command)
            {
                case SubModuleFunctions.GetQuestion:
                    question = quizHandler.GetQuestion(symptomContext.id);
                    if (question == null)
                    {
                        symptomContext.response = "invalid";
                        return false;
                    }
                    symptomContext.response = question.ToJson(symptomContext.id);
                    return true;
                case SubModuleFunctions.SendResponse:
                    Answer answer = InitAnswer(symptomContext);
                    quizHandler.ProcessAnswer(symptomContext.id, answer);
                    question = quizHandler.GetQuestion(symptomContext.id);
                    if(question == null && quizHandler.GetQuizById(symptomContext.id).IsQuizFinished())
                    {
                        symptomContext.response = quizHandler.GetQuizById(symptomContext.id).GetSymptomsHolder().GetJsonVerdict();
                        quizHandler.RemoveById(symptomContext.id);
                        return true;

                    }
                    symptomContext.response = question.ToJson(symptomContext.id);
                    return true;
                default:
                    return false;
            }
        }

        private Answer InitAnswer(SymptomContext context)
        {
            Quiz currentQuiz = quizHandler.GetQuizById(context.id);
            Answer.QUESTION_TYPE answerType = currentQuiz.GetCurrentQuestionType();
            Answer answer = new Answer(answerType, currentQuiz.GetCurrentSymptom());
            switch (answerType)
            {
                case Answer.QUESTION_TYPE.QUESTION_BOOLEAN:
                    if (context.status > 0)
                    {
                        answer.SetAnswerBoolean(Answer.QUESTION_BOOLEAN.TRUE);
                    }
                    else
                    {
                        answer.SetAnswerBoolean(Answer.QUESTION_BOOLEAN.FALSE);
                    }
                    
                    break;
                case Answer.QUESTION_TYPE.QUESTION_NUMBER:
                    answer.SetAnswerNumeric(context.status);
                    break;
                case Answer.QUESTION_TYPE.QUESTION_SICKNESS_LEVEL:
                    switch (context.status)
                    {
                        case 0:
                            answer.SetAnswerSicknessLevel(Answer.QUESTION_SICKNESS_LEVEL.ABSENT);
                            break;
                        case 1:
                            answer.SetAnswerSicknessLevel(Answer.QUESTION_SICKNESS_LEVEL.LITTLE);
                            break;
                        case 2:
                            answer.SetAnswerSicknessLevel(Answer.QUESTION_SICKNESS_LEVEL.MEDIUM);
                            break;
                        case 3:
                            answer.SetAnswerSicknessLevel(Answer.QUESTION_SICKNESS_LEVEL.HIGH);
                            break;
                    }
                    break;
            }
            return answer;
        }

        public override bool Init(byte[] context, int sizeOfContext)
        {
            Console.WriteLine("Init execution");
            quizHandler = new QuizHandler();
            return true;
        }

        public override bool UnInit()
        {
            Console.WriteLine("UnInit execution");
            return true;
        }

        
    }
}
