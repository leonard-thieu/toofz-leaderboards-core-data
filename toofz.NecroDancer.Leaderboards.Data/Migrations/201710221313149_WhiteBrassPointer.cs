namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class WhiteBrassPointer : DbMigration
    {
        public override void Up()
        {
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "CharacterId"));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "DisplayName"));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "IsCoOp"));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "IsCustomMusic"));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "IsProduction"));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "ModeId"));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "ProductId"));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "RunId"));

            AddColumn("dbo.DailyLeaderboards", "Name", c => c.String(nullable: false));
            Sql(Util.DropDefaultConstraint("dbo.DailyLeaderboards", "Name"));
            Sql($@"UPDATE dbo.DailyLeaderboards
SET Name = FORMAT(Date, 'd/M/yyyy', 'en-us')
WHERE ProductId = 0 AND IsProduction = 0;");
            Sql($@"UPDATE dbo.DailyLeaderboards
SET Name = FORMAT(Date, 'd/M/yyyy', 'en-us') + '_PROD'
WHERE ProductId = 0 AND IsProduction = 1;");
            Sql($@"UPDATE dbo.DailyLeaderboards
SET Name = 'DLC ' + FORMAT(Date, 'd/M/yyyy', 'en-us') + '_PROD'
WHERE ProductId = 1;");

            AddColumn("dbo.Leaderboards", "Name", c => c.String(nullable: false));
            Sql(Util.DropDefaultConstraint("dbo.Leaderboards", "Name"));
            // This doesn't handle Classic Early Access, Custom Music, or Co-op leaderboards.
            // While the schema supports it, leaderboards of those types did not exist in the production database.
            Sql($@"UPDATE dbo.Leaderboards
SET Name = RTRIM(CASE ProductId
               WHEN 1 THEN 'DLC '
               ELSE ''
           END + 
           CASE RunId
               WHEN 0 THEN 'HARDCORE '
               WHEN 1 THEN 'SPEEDRUN '
               WHEN 2 THEN 'HARDCORE SEEDED '
               WHEN 3 THEN 'SEEDED SPEEDRUN '
               WHEN 4 THEN 'HARDCORE '
               ELSE ''
           END + 
           CASE CharacterId
               WHEN -4 THEN 'All Chars '
               WHEN -3 THEN 'All Chars DLC '
               WHEN -2 THEN 'Story Mode '
               WHEN 0 THEN ''
               WHEN 1 THEN 'Melody '
               WHEN 2 THEN 'Aria '
               WHEN 3 THEN 'Dorian '
               WHEN 4 THEN 'Eli '
               WHEN 5 THEN 'Monk '
               WHEN 6 THEN 'DOVE '
               WHEN 7 THEN 'Coda '
               WHEN 8 THEN 'Bolt '
               WHEN 9 THEN 'Bard '
               WHEN 10 THEN 'Nocturna '
               WHEN 11 THEN 'Diamond '
               WHEN 12 THEN 'Mary '
               WHEN 13 THEN 'Tempo '
               ELSE ''
           END + 
           CASE RunId
               WHEN 4 THEN 'DEATHLESS '
               ELSE ''
           END +
           CASE ModeId
               WHEN 1 THEN 'NO RETURN'
               WHEN 2 THEN 'HARD'
               WHEN 3 THEN 'PHASING'
               WHEN 4 THEN 'RANDOMIZER'
               WHEN 5 THEN 'MYSTERY'
               ELSE ''
           END) + '_PROD'");
        }

        public override void Down()
        {
            DropColumn("dbo.Leaderboards", "Name");
            DropColumn("dbo.DailyLeaderboards", "Name");
        }
    }
}
