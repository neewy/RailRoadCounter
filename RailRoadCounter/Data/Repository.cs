﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace RailRoadCounter
{
	public interface IRepository<T> where T : class, new()
	{
		Task<List<T>> Get();
		Task<T> Get(int id);
		Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
		Task<T> Get(Expression<Func<T, bool>> predicate);
		AsyncTableQuery<T> AsQueryable();
		Task<int> Insert(T entity);
		Task<int> Update(T entity);
		Task<int> Delete(T entity);
		Task DeleteAll();
	}

	public class Repository<T> : IRepository<T> where T : class, new()
	{
		private SQLiteAsyncConnection db;

		public Repository(SQLiteAsyncConnection db)
		{
			this.db = db;
		}

		public AsyncTableQuery<T> AsQueryable() =>
			db.Table<T>();

		public async Task<List<T>> Get() =>
			await db.Table<T>().ToListAsync();

		public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
		{
			var query = db.Table<T>();

			if (predicate != null)
				query = query.Where(predicate);

			if (orderBy != null)
				query = query.OrderBy<TValue>(orderBy);

			return await query.ToListAsync();
		}

		public async Task<List<T>> GetByQuery(string sql) {
			return await db.QueryAsync<T>(sql, new object[1]);
		}

		public async Task<T> Get(int id) =>
			 await db.FindAsync<T>(id);

		public async Task<T> Get(Expression<Func<T, bool>> predicate) =>
			await db.FindAsync<T>(predicate);

		public async Task<int> Insert(T entity) =>
			 await db.InsertAsync(entity);

		public async Task<int> Update(T entity) =>
			 await db.UpdateAsync(entity);

		public async Task<int> Delete(T entity) =>
			 await db.DeleteAsync(entity);

		public async Task DeleteAll()
		{
			await db.DropTableAsync<T>();
			await db.CreateTableAsync<T>();
		}
	}
}
