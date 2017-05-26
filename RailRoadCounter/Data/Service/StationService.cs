using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace RailRoadCounter
{
	public class StationService : IService<Station>
	{

		private readonly Repository<Station> _stationRepository;

		public StationService(SQLiteAsyncConnection db)
		{
			_stationRepository = new Repository<Station>(db);
		}

		public async Task<int> Delete(Station item)
			=> await _stationRepository.Delete(item);

		public async Task DeleteAll()
		=> await _stationRepository.DeleteAll();

		public async Task<List<Station>> Find(Expression<Func<Station, bool>> predicate)
		=> await _stationRepository.Get<Station>(predicate);

		public async Task<List<Station>> FindAll()
		=> await _stationRepository.Get<Station>();

		public async Task<List<Station>> FindByName(string name)
			=> await _stationRepository.GetByQuery($"SELECT * FROM Station WHERE Name like '{name}%';");

		public async Task<List<Station>> FindByCode(string code)
 			=> await _stationRepository.GetByQuery($"SELECT * FROM Station WHERE Code like '{code}%';");


		public async Task<int> Save(Station item)
		{
			try
			{
				var rowsAffected = await _stationRepository.Update(item);
				if (rowsAffected == 0)
				{
					return await _stationRepository.Insert(item);
				}
				return rowsAffected;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
			}
			return 0;
		}
	}
}
