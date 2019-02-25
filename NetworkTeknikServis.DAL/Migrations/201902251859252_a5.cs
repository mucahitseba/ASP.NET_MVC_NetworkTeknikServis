namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faults", "TeknisyenBilgiPuani", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "TeknisyenDavranisPuani", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "DavranisPuani", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "OMNetHizmetPuanı", c => c.Int(nullable: false));
            AddColumn("dbo.Faults", "OMNetHakkindakiGorusler", c => c.String());
            AddColumn("dbo.Faults", "AnketYapildimi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faults", "AnketYapildimi");
            DropColumn("dbo.Faults", "OMNetHakkindakiGorusler");
            DropColumn("dbo.Faults", "OMNetHizmetPuanı");
            DropColumn("dbo.Faults", "DavranisPuani");
            DropColumn("dbo.Faults", "TeknisyenDavranisPuani");
            DropColumn("dbo.Faults", "TeknisyenBilgiPuani");
        }
    }
}
