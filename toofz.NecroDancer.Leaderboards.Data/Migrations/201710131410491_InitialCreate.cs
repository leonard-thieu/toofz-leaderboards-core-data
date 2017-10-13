namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                {
                    CharacterId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.CharacterId);

            CreateTable(
                "dbo.DailyEntries",
                c => new
                {
                    LeaderboardId = c.Int(nullable: false),
                    Rank = c.Int(nullable: false),
                    SteamId = c.Long(nullable: false),
                    ReplayId = c.Long(),
                    Score = c.Int(nullable: false),
                    Zone = c.Int(nullable: false),
                    Level = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.LeaderboardId, t.Rank })
                .ForeignKey("dbo.DailyLeaderboards", t => t.LeaderboardId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.SteamId, cascadeDelete: true)
                .Index(t => t.LeaderboardId)
                .Index(t => t.SteamId);

            CreateTable(
                "dbo.DailyLeaderboards",
                c => new
                {
                    LeaderboardId = c.Int(nullable: false),
                    LastUpdate = c.DateTime(),
                    DisplayName = c.String(nullable: false),
                    IsProduction = c.Boolean(nullable: false),
                    ProductId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false, storeType: "date"),
                })
                .PrimaryKey(t => t.LeaderboardId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => new { t.Date, t.ProductId, t.IsProduction }, unique: true, name: "IX_DailyLeaderboards");

            CreateTable(
                "dbo.Products",
                c => new
                {
                    ProductId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.ProductId);

            CreateTable(
                "dbo.Players",
                c => new
                {
                    SteamId = c.Long(nullable: false),
                    LastUpdate = c.DateTime(),
                    Exists = c.Boolean(),
                    Name = c.String(maxLength: 64),
                    Avatar = c.String(),
                })
                .PrimaryKey(t => t.SteamId);

            CreateTable(
                "dbo.Entries",
                c => new
                {
                    LeaderboardId = c.Int(nullable: false),
                    Rank = c.Int(nullable: false),
                    SteamId = c.Long(nullable: false),
                    ReplayId = c.Long(),
                    Score = c.Int(nullable: false),
                    Zone = c.Int(nullable: false),
                    Level = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.LeaderboardId, t.Rank })
                .ForeignKey("dbo.Leaderboards", t => t.LeaderboardId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.SteamId, cascadeDelete: true)
                .Index(t => t.LeaderboardId)
                .Index(t => t.SteamId);

            CreateTable(
                "dbo.Leaderboards",
                c => new
                {
                    LeaderboardId = c.Int(nullable: false),
                    LastUpdate = c.DateTime(),
                    DisplayName = c.String(nullable: false),
                    IsProduction = c.Boolean(nullable: false),
                    ProductId = c.Int(nullable: false),
                    ModeId = c.Int(nullable: false),
                    RunId = c.Int(nullable: false),
                    CharacterId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.LeaderboardId)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .ForeignKey("dbo.Modes", t => t.ModeId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Runs", t => t.RunId, cascadeDelete: true)
                .Index(t => new { t.CharacterId, t.RunId, t.ModeId, t.ProductId, t.IsProduction }, unique: true, name: "IX_Leaderboards");

            CreateTable(
                "dbo.Modes",
                c => new
                {
                    ModeId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.ModeId);

            CreateTable(
                "dbo.Runs",
                c => new
                {
                    RunId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.RunId);

            CreateTable(
                "dbo.Replays",
                c => new
                {
                    ReplayId = c.Long(nullable: false),
                    ErrorCode = c.Int(),
                    Seed = c.Int(),
                    Version = c.Int(),
                    KilledBy = c.String(),
                    Uri = c.String(),
                })
                .PrimaryKey(t => t.ReplayId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Entries", "SteamId", "dbo.Players");
            DropForeignKey("dbo.Leaderboards", "RunId", "dbo.Runs");
            DropForeignKey("dbo.Leaderboards", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Leaderboards", "ModeId", "dbo.Modes");
            DropForeignKey("dbo.Entries", "LeaderboardId", "dbo.Leaderboards");
            DropForeignKey("dbo.Leaderboards", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.DailyEntries", "SteamId", "dbo.Players");
            DropForeignKey("dbo.DailyLeaderboards", "ProductId", "dbo.Products");
            DropForeignKey("dbo.DailyEntries", "LeaderboardId", "dbo.DailyLeaderboards");
            DropIndex("dbo.Leaderboards", "IX_Leaderboards");
            DropIndex("dbo.Entries", new[] { "SteamId" });
            DropIndex("dbo.Entries", new[] { "LeaderboardId" });
            DropIndex("dbo.DailyLeaderboards", "IX_DailyLeaderboards");
            DropIndex("dbo.DailyEntries", new[] { "SteamId" });
            DropIndex("dbo.DailyEntries", new[] { "LeaderboardId" });
            DropTable("dbo.Replays");
            DropTable("dbo.Runs");
            DropTable("dbo.Modes");
            DropTable("dbo.Leaderboards");
            DropTable("dbo.Entries");
            DropTable("dbo.Players");
            DropTable("dbo.Products");
            DropTable("dbo.DailyLeaderboards");
            DropTable("dbo.DailyEntries");
            DropTable("dbo.Characters");
        }
    }
}
