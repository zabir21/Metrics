using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(3)]
    public class NetworkMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("networkmetrics");
        }

        public override void Up()
        {
            Create.Table("networkmetrics")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
