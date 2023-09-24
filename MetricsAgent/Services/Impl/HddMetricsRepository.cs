using Dapper;
using MetricsAgent.Models;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace MetricsAgent.Services.Impl
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public HddMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }
        public void Create(HddMetrics item)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Open();

            connection.Execute("INSERT INTO hddmetrics(value, time) VALUES(@value, @time)", new
            {
                value = item.Value,
                time = item.Time
            });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Open();

            connection.Execute("DELETE FROM hddmetrics WHERE id=@id", new
            {
                id = id
            });
        }

        public IList<HddMetrics> GetAll()
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Open();

            return connection.Query<HddMetrics>("SELECT id, Time, Value FROM hddmetrics").ToList();
        }

        public HddMetrics GetById(int id)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Open();

            HddMetrics metric = connection.QuerySingle<HddMetrics>("SELECT id, Time,Value FROM hddmetrics WHERE id=@id",
                new
                {
                    id = id
                });
            return metric;
        }

        /// <summary>
        /// Получение данных по нагрузке на ЦП за период
        /// </summary>
        /// <param name="timeFrom">Время начала периода</param>
        /// <param name="timeTo">Время окончания периода</param>
        /// <returns></returns>
        public IList<HddMetrics> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Open();

            List<HddMetrics> metrics = connection.Query<HddMetrics>("SELECT * FROM hddmetrics where time >= @timeFrom and time <= @timeTo",
                new { timeFrom = timeFrom.TotalSeconds, timeTo = timeTo.TotalSeconds }).ToList();
            return metrics;
        }

        public void Update(HddMetrics item)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE id = @id", new
            {
                value = item.Value,
                time = item.Time,
                id = item.Id
            });
        }
    }
}
