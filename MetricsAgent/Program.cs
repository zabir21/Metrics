using AutoMapper;
using FluentMigrator.Runner;
using Metrics.Models;
using MetricsAgent.Convecters;
using MetricsAgent.Jobs;
using MetricsAgent.Models;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton<AgentPool>();

            builder.Services.Configure<DatabaseOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
            });

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

            builder.Services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
            builder.Services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
            builder.Services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
            builder.Services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();

            builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            builder.Services.AddSingleton<CpuMetricJob>();
            builder.Services.AddSingleton<HddMetricJob>();
            builder.Services.AddSingleton<NetworkMetricJob>();
            builder.Services.AddSingleton<RamMetricJob>();

            builder.Services.AddSingleton(new JobSchedule(typeof(CpuMetricJob), "0/5 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(HddMetricJob), "0/5 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(NetworkMetricJob), "0/5 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(RamMetricJob), "0/5 * * ? * * *"));

            builder.Services.AddHostedService<QuartzHostedService>();

            //ConfigureNpgsqlConnection(builder.Services);

            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                rb.AddSQLite()
                .WithGlobalConnectionString(builder.Configuration["Settings:DatabaseOptions:ConnectionString"].ToString())
                .ScanIn(typeof(Program).Assembly).For.Migrations()
                ).AddLogging(ib => ib.AddFluentMigratorConsole());

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();

            }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
                logging.RequestHeaders.Add("X-Forwarded-For");
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });

                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseHttpLogging();

            app.MapControllers();

            var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using (IServiceScope serviceScope = serviceScopeFactory.CreateScope())
            {
                var migrationRunner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                migrationRunner.MigrateUp();
            }

            app.Run();
        }

        //private static void ConfigureNpgsqlConnection(WebApplicationBuilder? builder)
        //{            
        //    var connection = new SQLiteConnection(builder.Configuration["Settings:DatabaseOptions:ConnectionString"].ToString());

        //    connection.Open();
        //    //PrepareSchema(connection);
        //}

        //private static void PrepareSchema(SQLiteConnection connection)
        //{
        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        command.CommandText = "DROP TABLE IF EXISTS cpumetrics";

        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE cpumetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }
        //}
    }
}