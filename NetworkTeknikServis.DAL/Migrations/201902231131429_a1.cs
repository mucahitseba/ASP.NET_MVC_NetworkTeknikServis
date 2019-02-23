namespace NetworkTeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Faults", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Faults", new[] { "User_Id" });
            AddColumn("dbo.Faults", "haveJob", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Faults", "FaultDescription", c => c.String());
            AlterColumn("dbo.Faults", "Adress", c => c.String());
            DropColumn("dbo.Faults", "Number");
            DropColumn("dbo.Faults", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Faults", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Faults", "Number", c => c.String());
            AlterColumn("dbo.Faults", "Adress", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Faults", "FaultDescription", c => c.String(nullable: false, maxLength: 1000));
            DropColumn("dbo.Faults", "haveJob");
            CreateIndex("dbo.Faults", "User_Id");
            AddForeignKey("dbo.Faults", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
