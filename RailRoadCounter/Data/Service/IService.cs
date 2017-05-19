using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RailRoadCounter
{
	public interface IService<T>
	{
		Task<List<T>> FindAll();

		Task<List<T>> Find(Expression<Func<T, bool>> predicate);

		Task<int> Save(T item);

		Task<int> Delete(T item);

		Task DeleteAll();
	}
}