namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FaultsLog",
                c => new
                    {
                        FaultLogId = c.Int(nullable: false, identity: true),
                        FaultId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        TechnicianId = c.Int(nullable: false),
                        History = c.DateTime(nullable: false),
                        Operation = c.String(),
                        OperationDescription = c.String(),
                    })
                .PrimaryKey(t => t.FaultLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FaultsLog");
        }
    }
}
