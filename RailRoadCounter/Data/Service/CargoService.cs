using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace RailRoadCounter
{
	public class CargoService : IService<Cargo>
	{

		private readonly IRepository<Cargo> _cargoRepository;

		public CargoService(SQLiteAsyncConnection db)
		{
			_cargoRepository = new Repository<Cargo>(db);
		}

		public async Task<int> Delete(Cargo item)
			=> await _cargoRepository.Delete(item);

		public async Task DeleteAll()
			=> await _cargoRepository.DeleteAll();

		public Task<List<Cargo>> Find(Expression<Func<Cargo, bool>> predicate)
			=> _cargoRepository.Get<Cargo>(predicate);

		public Task<List<Cargo>> FindAll()
			=> _cargoRepository.Get<Cargo>();

		public async Task<int> Save(Cargo item)
		{
			var rowsAffected = await _cargoRepository.Update(item);
			if (rowsAffected == 0)
			{
				return await _cargoRepository.Insert(item);
			}
			return rowsAffected;
		}
	}
}
