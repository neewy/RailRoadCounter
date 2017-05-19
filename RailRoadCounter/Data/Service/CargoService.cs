using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace RailRoadCounter
{
	public class CargoService : IService<Cargo>
	{

		private readonly Repository<Cargo> _cargoRepository;

		public CargoService(SQLiteAsyncConnection db)
		{
			_cargoRepository = new Repository<Cargo>(db);
		}

		public async Task<int> Delete(Cargo item)
			=> await _cargoRepository.Delete(item);

		public async Task DeleteAll()
			=> await _cargoRepository.DeleteAll();

		public async Task<List<Cargo>> Find(Expression<Func<Cargo, bool>> predicate)
			=> await _cargoRepository.Get<Cargo>(predicate);

		public async Task<List<Cargo>> FindAll()
			=> await _cargoRepository.Get<Cargo>();

		public async Task<List<Cargo>> FindByName(string name)
			=> await _cargoRepository.GetByQuery($"SELECT * FROM Cargo WHERE Name like '{name}%';");

		public async Task<List<Cargo>> FindByCode(string code)
			=> await _cargoRepository.GetByQuery($"SELECT * FROM Cargo WHERE Code like '{code}%';");


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
