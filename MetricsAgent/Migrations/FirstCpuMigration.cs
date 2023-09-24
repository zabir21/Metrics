using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(1)]
    public class FirstCpuMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("cpumetrics");
        }

        public override void Up()
        {
            Create.Table("cpumetrics")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
