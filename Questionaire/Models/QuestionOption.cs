using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionaire.Models
{
    public class QuestionOption
    {
        public int QuestionOptionID { get; set; }
        public int QuestionID { get; set; }
        public Question Question { get; set; }

        public string QuestionOptionTitle { get; set; }

        public Boolean IsAnswer { get; set; }
    }
}