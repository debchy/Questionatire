namespace Questionaire.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    internal sealed class Configuration : DbMigrationsConfiguration<Questionaire.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Questionaire.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            List<Question> Questions = new List<Question>();
            for (int i =0; i < 100; i++)
            {
                
                Questions.Add(new Question { QuestionTitle = "Question " + (i + 1), AvailableFrom = DateTime.Now.AddDays(Math.Floor((double)(i / 25))).Date, AvailableTo = DateTime.Now.AddDays(Math.Floor((double)(i / 25))).Date, QuestionOptions= new List<QuestionOption>() });
                
            }
            
            for(int i =0; i < 100; i++)
            {
                var Question = Questions[i];
                var QuestionOptions = Question.QuestionOptions;
                for (int n = 0; n < 4; n++)
                {
                    QuestionOptions.Add(new QuestionOption { QuestionID = Question.QuestionID, QuestionOptionTitle = "Answer " + 1, IsAnswer = false });
                }
            }

            foreach(Question question in Questions)
            {
                context.Questions.AddOrUpdate(q=>q.QuestionID, question);
            }
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
