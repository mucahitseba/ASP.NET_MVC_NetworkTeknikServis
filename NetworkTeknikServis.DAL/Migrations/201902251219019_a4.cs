namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faults", "TechnicianState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faults", "TechnicianState");
        }
    }
}
