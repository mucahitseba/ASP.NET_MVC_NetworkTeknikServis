namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faults", "TechnicianDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faults", "TechnicianDescription");
        }
    }
}
