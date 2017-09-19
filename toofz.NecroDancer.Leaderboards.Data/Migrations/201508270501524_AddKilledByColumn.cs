namespace toofz.NecroDancer.Leaderboards.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKilledByColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Replays", "KilledBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Replays", "KilledBy");
        }
    }
}