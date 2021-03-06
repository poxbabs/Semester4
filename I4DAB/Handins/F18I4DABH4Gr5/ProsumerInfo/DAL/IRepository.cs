﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProsumerInfo.DAL
{
	public interface IRepository<T> where T : class
	{
		T Add(T t);
		//Task<T> AddAsync(T t);
		int Count();
		Task<int> CountAsync();
		void Delete(T t);
		void Dispose();
		T Find(Expression<Func<T, bool>> match);
		ICollection<T> FindAll(Expression<Func<T, bool>> match);
		Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
		IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
		Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
		T Get(int id);
		IQueryable<T> GetAll();
		Task<ICollection<T>> GetAllAsync();
		IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
		Task<T> GetAsync(int id);
		T Update(T t, object key);
		Task<T> UpdateAsync(T t, object key);
	}
}
