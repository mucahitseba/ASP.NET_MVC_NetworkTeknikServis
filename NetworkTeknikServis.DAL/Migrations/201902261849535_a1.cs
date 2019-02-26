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
                        FaultId = c.Guid(nullable: false),
                        CustomerId = c.String(),
                        TechnicianId = c.String(),
                        History = c.DateTime(nullable: false),
                        Operation = c.String(),
                        OperationDescription = c.String(),
                    })
                .PrimaryKey(t => t.FaultLogId);
            
            AddColumn("dbo.Faults", "TechnicianState", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "TeknisyenBilgiPuani", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "TeknisyenDavranisPuani", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "DavranisPuani", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "OMNetHizmetPuanı", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "OMNetHakkindakiGorusler", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "SurveyCode", c => c.String());
            AddColumn("dbo.Faults", "AnketYapildimi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faults", "AnketYapildimi");
            DropColumn("dbo.Faults", "SurveyCode");
            DropColumn("dbo.Faults", "OMNetHakkindakiGorusler");
            DropColumn("dbo.Faults", "OMNetHizmetPuanı");
            DropColumn("dbo.Faults", "DavranisPuani");
            DropColumn("dbo.Faults", "TeknisyenDavranisPuani");
            DropColumn("dbo.Faults", "TeknisyenBilgiPuani");
            DropColumn("dbo.Faults", "TechnicianState");
            DropTable("dbo.FaultsLog");
        }
    }
}
