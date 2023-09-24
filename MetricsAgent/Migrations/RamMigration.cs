using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(4)]
    public class RamMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("rammetrics");
        }

        public override void Up()
        {
            Create.Table("rammetrics")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
