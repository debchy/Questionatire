using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionaire.Models
{
    public class UserAnswer
    {
        public int UserAnswerID { get; set; }

        public string UserName { get; set; }

        public DateTime AnswerDate { get; set; }

        public int QuestionID { get; set; }

        virtual public Question Question { get; set; }

        public int QuestionOptionID { get; set; }

        virtual public QuestionOption QuestionOption { get; set; }

        public Boolean IsFinalSubmission { get; set; }
    }
}