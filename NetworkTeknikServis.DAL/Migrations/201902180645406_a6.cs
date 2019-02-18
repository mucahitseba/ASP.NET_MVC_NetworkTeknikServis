namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faults", "FaultDescription", c => c.String());
            AddColumn("dbo.Faults", "Adress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faults", "Adress");
            DropColumn("dbo.Faults", "FaultDescription");
        }
    }
}
