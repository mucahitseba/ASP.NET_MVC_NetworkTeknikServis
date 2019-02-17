namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faults", "InvoicePath", c => c.String());
            DropColumn("dbo.Faults", "InvoiceaPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Faults", "InvoiceaPath", c => c.String());
            DropColumn("dbo.Faults", "InvoicePath");
        }
    }
}
