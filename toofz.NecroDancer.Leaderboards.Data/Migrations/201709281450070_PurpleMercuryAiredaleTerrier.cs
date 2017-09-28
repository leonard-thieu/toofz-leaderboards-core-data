namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurpleMercuryAiredaleTerrier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaderboards", "IsProduction", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "ModeId", c => c.Int(nullable: false));
            AddColumn("dbo.Replays", "Uri", c => c.String());
            AlterColumn("dbo.DailyLeaderboards", "LastUpdate", c => c.DateTime());
            AlterColumn("dbo.Leaderboards", "LastUpdate", c => c.DateTime());
            DropColumn("dbo.Leaderboards", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leaderboards", "Date", c => c.DateTime());
            AlterColumn("dbo.Leaderboards", "LastUpdate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DailyLeaderboards", "LastUpdate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Replays", "Uri");
            DropColumn("dbo.Leaderboards", "ModeId");
            DropColumn("dbo.Leaderboards", "ProductId");
            DropColumn("dbo.Leaderboards", "IsProduction");
        }
    }
}
