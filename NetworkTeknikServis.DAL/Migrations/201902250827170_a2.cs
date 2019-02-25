namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FaultsLog", "FaultId", c => c.String());
            AlterColumn("dbo.FaultsLog", "CustomerId", c => c.String());
            AlterColumn("dbo.FaultsLog", "TechnicianId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FaultsLog", "TechnicianId", c => c.Int(nullable: false));
            AlterColumn("dbo.FaultsLog", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.FaultsLog", "FaultId", c => c.Int(nullable: false));
        }
    }
}
