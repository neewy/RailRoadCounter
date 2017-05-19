using System;
using SQLite;
using System.Threading.Tasks;

namespace RailRoadCounter
{
	public class Database
	{
		public readonly SQLiteAsyncConnection sqlite;

		public Database(string dbPath)
		{
			sqlite = new SQLiteAsyncConnection(dbPath);
		}

		public async Task CreateInitialDatabase()
		{
			await sqlite.CreateTablesAsync<Cargo, Station>();
		}
	}
}
