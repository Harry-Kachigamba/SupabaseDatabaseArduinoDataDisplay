using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace SupabaseArduinoDataDisplay
{
	public class Program
	{
		private static SerialPort _serialPort;
		private static DataService dataService = new DataService();

		static async Task Main(string[] args)
		{
			try
			{
				// Adjust COM port as necessary
				_serialPort = new SerialPort("COM7", 9600);
				_serialPort.DataReceived += DataReceivedHandler;
				_serialPort.Open();

				Console.WriteLine("Listening for data...");

				// Fetch and display data from Supabase
				var logs = await dataService.GetTemperatureLogsAsync();
				DisplayLogs(logs);

				Console.ReadLine(); // Keep the application running
			}
			catch (UnauthorizedAccessException ex)
			{
				Console.WriteLine($"Access to the port is denied: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}

		private static  void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
		{
			try
			{
				string data = _serialPort.ReadLine();
				Console.WriteLine($"Data Received: {data}");

				if (data.StartsWith("Temperature: "))
				{
					string temperatureString = data.Replace("Temperature: ", "").Trim();
					//await SaveDataToSupabaseAsync(temperatureString);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while reading data: {ex.Message}");
			}
		}

		private static void DisplayLogs(List<TemperatureLog> logs)
		{
			Console.WriteLine("Temperature Logs:");
			foreach (var log in logs)
			{
				Console.WriteLine($"Temperature: {log.Temperature}, Timestamp: {log.Timestamp}");
			}
		}
	}
}
