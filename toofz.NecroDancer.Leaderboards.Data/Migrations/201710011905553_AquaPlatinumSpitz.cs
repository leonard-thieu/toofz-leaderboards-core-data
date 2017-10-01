namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    public partial class AquaPlatinumSpitz : DbMigration
    {
        public override void Up()
        {
            var leaderboardHeaders = LeaderboardsResources.ReadLeaderboardHeaders();

            // Remove old leaderboards/dailies from Leaderboards table.
            {
                var lbids = from l in leaderboardHeaders
                            select l.Id;
                var sql = $@"DELETE FROM dbo.Leaderboards
WHERE LeaderboardId NOT IN ({string.Join(", ", lbids)});";
                Sql(sql);
            }

            var leaderboardCategories = LeaderboardsResources.ReadLeaderboardCategories();

            CreateTable(
                "dbo.Products",
                c => new
                {
                    ProductId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.ProductId);
            // Seed products
            foreach (var product in leaderboardCategories["products"])
            {
                var sql = $@"INSERT INTO dbo.Products(ProductId, Name, DisplayName) 
VALUES ({product.Value.Id}, '{product.Key}', '{product.Value.DisplayName}');";
                Sql(sql);
            }

            CreateTable(
                "dbo.Modes",
                c => new
                {
                    ModeId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.ModeId);
            // Seed modes
            foreach (var mode in leaderboardCategories["modes"])
            {
                var sql = $@"INSERT INTO dbo.Modes(ModeId, Name, DisplayName) 
VALUES ({mode.Value.Id}, '{mode.Key}', '{mode.Value.DisplayName}');";
                Sql(sql);
            }

            CreateTable(
                "dbo.Runs",
                c => new
                {
                    RunId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.RunId);
            // Seed runs
            foreach (var run in leaderboardCategories["runs"])
            {
                var sql = $@"INSERT INTO dbo.Runs(RunId, Name, DisplayName) 
VALUES ({run.Value.Id}, '{run.Key}', '{run.Value.DisplayName}');";
                Sql(sql);
            }

            CreateTable(
                "dbo.Characters",
                c => new
                {
                    CharacterId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    DisplayName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.CharacterId);
            // Seed characters
            foreach (var character in leaderboardCategories["characters"])
            {
                var sql = $@"INSERT INTO dbo.Characters(CharacterId, Name, DisplayName) 
VALUES ({character.Value.Id}, '{character.Key}', '{character.Value.DisplayName}');";
                Sql(sql);
            }

            AddColumn("dbo.DailyLeaderboards", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.Leaderboards", "DisplayName", c => c.String(nullable: false));

            foreach (var leaderboardHeader in leaderboardHeaders)
            {
                var productId = leaderboardCategories["products"][leaderboardHeader.Product].Id;
                var modeId = leaderboardCategories["modes"][leaderboardHeader.Mode].Id;
                var runId = leaderboardCategories["runs"][leaderboardHeader.Run].Id;
                var characterId = leaderboardCategories["characters"][leaderboardHeader.Character].Id;
                var displayName = leaderboardHeader.DisplayName;
                var leaderboardId = leaderboardHeader.Id;
                var sql = $@"UPDATE dbo.Leaderboards
SET IsProduction = 1,
    ProductId = {productId},
    ModeId = {modeId},
    RunId = {runId},
    CharacterId = {characterId},
    DisplayName = '{displayName}'
WHERE LeaderboardId = {leaderboardId};";
                Sql(sql);
            }

            AlterColumn("dbo.DailyLeaderboards", "Date", c => c.DateTime(nullable: false, storeType: "date"));

            {
                var sql = $@"UPDATE dbo.DailyLeaderboards
SET DisplayName = 'Daily (' + CONVERT(VARCHAR(10), Date, 120) + ') (Early Access)'
WHERE ProductId = 0 AND IsProduction = 0;";
                Sql(sql);
            }

            {
                var sql = $@"UPDATE dbo.DailyLeaderboards
SET DisplayName = 'Daily (' + CONVERT(VARCHAR(10), Date, 120) + ')'
WHERE ProductId = 0 AND IsProduction = 1;";
                Sql(sql);
            }

            {
                var sql = $@"UPDATE dbo.DailyLeaderboards
SET DisplayName = 'Daily (' + CONVERT(VARCHAR(10), Date, 120) + ') (Amplified) (Early Access)'
WHERE ProductId = 1 AND IsProduction = 0;";
                Sql(sql);
            }

            {
                var sql = $@"UPDATE dbo.DailyLeaderboards
SET DisplayName = 'Daily (' + CONVERT(VARCHAR(10), Date, 120) + ') (Amplified)'
WHERE ProductId = 1 AND IsProduction = 1;";
                Sql(sql);
            }

            CreateIndex("dbo.DailyLeaderboards", new[] { "Date", "ProductId", "IsProduction" }, unique: true, name: "IX_DailyLeaderboards");
            CreateIndex("dbo.Leaderboards", new[] { "CharacterId", "RunId", "ModeId", "ProductId", "IsProduction" }, unique: true, name: "IX_Leaderboards");
            AddForeignKey("dbo.DailyLeaderboards", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.Leaderboards", "CharacterId", "dbo.Characters", "CharacterId", cascadeDelete: true);
            AddForeignKey("dbo.Leaderboards", "ModeId", "dbo.Modes", "ModeId", cascadeDelete: true);
            AddForeignKey("dbo.Leaderboards", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.Leaderboards", "RunId", "dbo.Runs", "RunId", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Leaderboards", "RunId", "dbo.Runs");
            DropForeignKey("dbo.Leaderboards", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Leaderboards", "ModeId", "dbo.Modes");
            DropForeignKey("dbo.Leaderboards", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.DailyLeaderboards", "ProductId", "dbo.Products");
            DropIndex("dbo.Leaderboards", "IX_Leaderboards");
            DropIndex("dbo.DailyLeaderboards", "IX_DailyLeaderboards");
            AlterColumn("dbo.DailyLeaderboards", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Leaderboards", "DisplayName");
            DropColumn("dbo.DailyLeaderboards", "DisplayName");
            DropTable("dbo.Characters");
            DropTable("dbo.Runs");
            DropTable("dbo.Modes");
            DropTable("dbo.Products");
        }
    }
}
