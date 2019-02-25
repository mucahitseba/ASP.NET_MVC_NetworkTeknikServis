namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faults", "SurveyCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faults", "SurveyCode");
        }
    }
}
