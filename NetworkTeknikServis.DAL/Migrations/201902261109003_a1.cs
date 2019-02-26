namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Faults", "OMNetHakkindakiGorusler", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Faults", "OMNetHakkindakiGorusler", c => c.String());
        }
    }
}
