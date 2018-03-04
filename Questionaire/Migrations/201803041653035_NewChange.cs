namespace Questionaire.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAnswers", "IsFinalSubmission", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAnswers", "IsFinalSubmission");
        }
    }
}
