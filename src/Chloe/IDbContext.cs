﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Chloe
{
    /// <summary>
    /// 注：DbContext 对象非线程安全，多线程不能共享一个 DbContext 对象。
    /// </summary>
    public interface IDbContext : IDisposable
    {
        IDbSession Session { get; }

        void HasQueryFilter<TEntity>(Expression<Func<TEntity, bool>> filter);

        IQuery<TEntity> Query<TEntity>();
        IQuery<TEntity> Query<TEntity>(string table);
        IQuery<TEntity> Query<TEntity>(LockType @lock);
        IQuery<TEntity> Query<TEntity>(string table, LockType @lock);

        TEntity QueryByKey<TEntity>(object key, bool tracking = false);
        TEntity QueryByKey<TEntity>(object key, string table, bool tracking = false);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key">If the entity just has one primary key, input a value that it's type is same as the primary key. If the entity has multiple keys, input an instance that defines the same properties as the keys like 'new { Key1 = "1", Key2 = "2" }'.</param>
        /// <param name="table"></param>
        /// <param name="lock"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        TEntity QueryByKey<TEntity>(object key, string table, LockType @lock, bool tracking = false);

        /// <summary>
        /// context.JoinQuery&lt;User, City&gt;((user, city) => new object[] 
        /// { 
        ///     JoinType.LeftJoin, user.CityId == city.Id 
        /// })
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="joinInfo"></param>
        /// <returns></returns>
        IJoinQuery<T1, T2> JoinQuery<T1, T2>(Expression<Func<T1, T2, object[]>> joinInfo);
        /// <summary>
        /// context.JoinQuery&lt;User, City, Province&gt;((user, city, province) => new object[] 
        /// { 
        ///     JoinType.LeftJoin, user.CityId == city.Id, 
        ///     JoinType.LeftJoin, city.ProvinceId == province.Id 
        /// })
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="joinInfo"></param>
        /// <returns></returns>
        IJoinQuery<T1, T2, T3> JoinQuery<T1, T2, T3>(Expression<Func<T1, T2, T3, object[]>> joinInfo);
        IJoinQuery<T1, T2, T3, T4> JoinQuery<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, object[]>> joinInfo);
        IJoinQuery<T1, T2, T3, T4, T5> JoinQuery<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, object[]>> joinInfo);

        IEnumerable<T> SqlQuery<T>(string sql, params DbParam[] parameters);
        IEnumerable<T> SqlQuery<T>(string sql, CommandType cmdType, params DbParam[] parameters);
        /// <summary>
        /// dbContext.SqlQuery&lt;User&gt;("select * from Users where Id=@Id", new { Id = 1 }).ToList();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IEnumerable<T> SqlQuery<T>(string sql, object parameter);
        IEnumerable<T> SqlQuery<T>(string sql, CommandType cmdType, object parameter);

        /// <summary>
        /// 插入数据，连同导航属性一并插入。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Save<TEntity>(TEntity entity);

        /// <summary>
        /// 插入数据，但不包括导航属性。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity);
        TEntity Insert<TEntity>(TEntity entity, string table);
        /// <summary>
        /// context.Insert&lt;User&gt;(() => new User() { Name = "lu", Age = 18, Gender = Gender.Woman, CityId = 1, OpTime = DateTime.Now })
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="content"></param>
        /// <returns>It will return null if an entity does not define primary key,other wise return primary key value.</returns>
        object Insert<TEntity>(Expression<Func<TEntity>> content);
        object Insert<TEntity>(Expression<Func<TEntity>> content, string table);

        void InsertRange<TEntity>(List<TEntity> entities);
        void InsertRange<TEntity>(List<TEntity> entities, string table);

        int Update<TEntity>(TEntity entity);
        int Update<TEntity>(TEntity entity, string table);
        /// <summary>
        /// context.Update&lt;User&gt;(a => a.Id == 1, a => new User() { Name = "lu", Age = a.Age + 1, Gender = Gender.Woman, OpTime = DateTime.Now })
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        int Update<TEntity>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TEntity>> content);
        int Update<TEntity>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TEntity>> content, string table);

        int Delete<TEntity>(TEntity entity);
        int Delete<TEntity>(TEntity entity, string table);
        /// <summary>
        /// context.Delete&lt;User&gt;(a => a.Id == 1)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        int Delete<TEntity>(Expression<Func<TEntity, bool>> condition);
        int Delete<TEntity>(Expression<Func<TEntity, bool>> condition, string table);
        int DeleteByKey<TEntity>(object key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key">If the entity just has one primary key, input a value that it's type is same as the primary key. If the entity has multiple keys, input an instance that defines the same properties as the keys like 'new { Key1 = "1", Key2 = "2" }'.</param>
        /// <param name="table"></param>
        /// <returns></returns>
        int DeleteByKey<TEntity>(object key, string table);

        ITransientTransaction BeginTransaction();
        ITransientTransaction BeginTransaction(IsolationLevel il);
        void UseTransaction(Action action);
        void UseTransaction(Action action, IsolationLevel il);
#if !net40
        Task UseTransactionAsync(Func<Task> action);
        Task UseTransactionAsync(Func<Task> action, IsolationLevel il);
#endif

        void TrackEntity(object entity);
    }
}
