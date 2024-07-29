using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace SupabaseArduinoDataDisplay
{
	public class DataService
	{
		private string connectionString = "Host=aws-0-eu-central-1.pooler.supabase.com;Port=6543;Username=postgres.pokmjfgwexrzniphrnie;Password=HC_iEJbPVyrG5Bs;Database=postgres;SslMode=Require;Trust Server Certificate=true;";

		public async Task<List<TemperatureLog>> GetTemperatureLogsAsync()
		{
			var logs = new List<TemperatureLog>();

			using (var connection = new NpgsqlConnection(connectionString))
			{
				await connection.OpenAsync();

				var query = "SELECT temperature, timestamp FROM temperature_log";
				using (var command = new NpgsqlCommand(query, connection))
				{
					var reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						var log = new TemperatureLog
						{
							Temperature = reader.GetString(0),
							Timestamp = reader.GetDateTime(1)
						};
						logs.Add(log);
					}
				}
			}

			return logs;
		}
	}

	public class TemperatureLog
	{
		public string Temperature { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
