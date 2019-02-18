namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Faults",
                c => new
                    {
                        FaultID = c.Guid(nullable: false),
                        CustomerId = c.String(nullable: false),
                        OperatorId = c.String(),
                        TechnicianId = c.String(),
                        FaultNotifyDate = c.DateTime(nullable: false),
                        FaultResultDate = c.DateTime(),
                        FaultState = c.Int(nullable: false),
                        AssignedOperator = c.Boolean(nullable: false),
                        FaultPath = c.String(),
                        InvoicePath = c.String(),
                    })
                .PrimaryKey(t => t.FaultID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Faults");
        }
    }
}
