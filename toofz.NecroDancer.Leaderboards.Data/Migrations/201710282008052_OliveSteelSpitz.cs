namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class OliveSteelSpitz : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DailyEntries", "ReplayId");
            CreateIndex("dbo.Entries_A", "ReplayId");
            CreateIndex("dbo.Entries_B", "ReplayId");
            AddForeignKey("dbo.Entries_A", "ReplayId", "dbo.Replays", "ReplayId");
            AddForeignKey("dbo.Entries_B", "ReplayId", "dbo.Replays", "ReplayId");
            AddForeignKey("dbo.DailyEntries", "ReplayId", "dbo.Replays", "ReplayId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.DailyEntries", "ReplayId", "dbo.Replays");
            DropForeignKey("dbo.Entries_B", "ReplayId", "dbo.Replays");
            DropForeignKey("dbo.Entries_A", "ReplayId", "dbo.Replays");
            DropIndex("dbo.Entries_B", new[] { "ReplayId" });
            DropIndex("dbo.Entries_A", new[] { "ReplayId" });
            DropIndex("dbo.DailyEntries", new[] { "ReplayId" });
        }
    }
}
