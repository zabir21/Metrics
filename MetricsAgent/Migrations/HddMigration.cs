using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(2)]
    public class HddMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("hddmetrics");
        }

        public override void Up()
        {
            Create.Table("hddmetrics")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
