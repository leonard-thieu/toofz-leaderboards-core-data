namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TealBrassLabrador : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Leaderboards", "IX_Leaderboards");
            AddColumn("dbo.Leaderboards", "IsCustomMusic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsCoOp", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Leaderboards", new[] { "CharacterId", "RunId", "ModeId", "ProductId", "IsProduction", "IsCustomMusic", "IsCoOp" }, unique: true, name: "IX_Leaderboards");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Leaderboards", "IX_Leaderboards");
            DropColumn("dbo.Leaderboards", "IsCoOp");
            DropColumn("dbo.Leaderboards", "IsCustomMusic");
            CreateIndex("dbo.Leaderboards", new[] { "CharacterId", "RunId", "ModeId", "ProductId", "IsProduction" }, unique: true, name: "IX_Leaderboards");
        }
    }
}
