using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Quizzer
{
    public class Quiz
    {
        public enum QUIZ_STATE 
        {
            INIT,
            ANSWERING,
            ANSWERED,
            FINISHED
        }

        QUIZ_STATE quizState;
        private IList<Answer> answers;
        private IList<Question> questions;
        private int currentQuestionNumber;
        private ISet<int> askedQuestions;
        private SymptomsHolder symptomsHolder;
        private Answer.QUESTION_TYPE currentQuestionType;

        public Quiz(SymptomsHolder symptomsHolder)
        {
            quizState = QUIZ_STATE.INIT;
            questions = new List<Question>();
            answers = new List<Answer>();
            askedQuestions = new HashSet<int>();
            currentQuestionNumber = 0;
            this.symptomsHolder = symptomsHolder;
        }

        public bool BeginQuiz()
        {
            if (quizState != QUIZ_STATE.INIT)
            {
                return false;
            }
            quizState = QUIZ_STATE.ANSWERED;
            return true;
        }

        public Question GetNextQuestion()
        {
            if (quizState != QUIZ_STATE.ANSWERED)
            {
                return null;
            }
            quizState = QUIZ_STATE.ANSWERING;
            Question nextQuestion = symptomsHolder.GetNextQuestion();
            currentQuestionType = nextQuestion.GetQuestionType();
            return nextQuestion;
        }

        public bool IsQuizFinished()
        {
            return quizState == QUIZ_STATE.FINISHED;
        }

        public bool ProcessAnswer(Answer answer)
        {

            if (quizState != QUIZ_STATE.ANSWERING || answer.GetAnswerType() != currentQuestionType)
            {
                return false;
            }
            symptomsHolder.ProcessAnswer(answer);
            answers.Add(answer);
            currentQuestionNumber++;
            quizState = QUIZ_STATE.ANSWERED;

            return true;
        }

        public IList<Answer> GetAnswers()
        {
            if (quizState != QUIZ_STATE.FINISHED)
            {
                return null;
            }
            return answers;
        }
    }
}
