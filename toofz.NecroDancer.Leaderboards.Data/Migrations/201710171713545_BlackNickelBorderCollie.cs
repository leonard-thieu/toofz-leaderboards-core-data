namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlackNickelBorderCollie : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Modes", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Runs", "Name", c => c.String(nullable: false, maxLength: 32));
            CreateIndex("dbo.Characters", "Name", unique: true);
            CreateIndex("dbo.Products", "Name", unique: true);
            CreateIndex("dbo.Players", "Name");
            CreateIndex("dbo.Modes", "Name", unique: true);
            CreateIndex("dbo.Runs", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Runs", new[] { "Name" });
            DropIndex("dbo.Modes", new[] { "Name" });
            DropIndex("dbo.Players", new[] { "Name" });
            DropIndex("dbo.Products", new[] { "Name" });
            DropIndex("dbo.Characters", new[] { "Name" });
            AlterColumn("dbo.Runs", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Modes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Characters", "Name", c => c.String(nullable: false));
        }
    }
}
