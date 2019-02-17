namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faults", "FaultPath", c => c.String());
            AddColumn("dbo.Faults", "InvoiceaPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faults", "InvoiceaPath");
            DropColumn("dbo.Faults", "FaultPath");
        }
    }
}
