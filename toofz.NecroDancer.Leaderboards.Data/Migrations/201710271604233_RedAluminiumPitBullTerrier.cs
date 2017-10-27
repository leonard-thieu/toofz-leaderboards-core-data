namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedAluminiumPitBullTerrier : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Leaderboards", "IX_Leaderboards");
            CreateIndex("dbo.Leaderboards", new[] { "CharacterId", "RunId", "ModeId", "ProductId", "IsProduction", "IsCoOp", "IsCustomMusic" }, unique: true, name: "IX_Leaderboards");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Leaderboards", "IX_Leaderboards");
            CreateIndex("dbo.Leaderboards", new[] { "CharacterId", "RunId", "ModeId", "ProductId", "IsProduction", "IsCustomMusic", "IsCoOp" }, unique: true, name: "IX_Leaderboards");
        }
    }
}
