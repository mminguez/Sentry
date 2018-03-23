﻿using System.Linq;
using System.Reflection;
using Dolittle.Concepts;
using Dolittle.ReadModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadModelRepositoryFor<T> : IReadModelRepositoryFor<T> where T : Dolittle.ReadModels.IReadModel
    {
        readonly string _collectionName = typeof(T).Name;
        readonly Connection _connection;
        readonly IMongoCollection<T> _collection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public ReadModelRepositoryFor(Connection connection)
        {
            _connection = connection;
            _collection = connection.Database.GetCollection<T>(_collectionName);
        }

        /// <inheritdoc/>
        public IQueryable<T> Query => _collection.AsQueryable<T>();

        /// <inheritdoc/>
        public void Delete(T readModel)
        {
            var objectId = GetObjectIdFrom(readModel);
            _collection.DeleteOne(Builders<T>.Filter.Eq("_id", objectId));
        }

        /// <inheritdoc/>
        public T GetById(object id)
        {
            var objectId = GetIdAsBsonValue(id);
            return _collection.Find(Builders<T>.Filter.Eq("_id", objectId)).FirstOrDefault();
        }

        /// <inheritdoc/>
        public void Insert(T readModel)
        {
            _collection.InsertOne(readModel);
        }

        /// <inheritdoc/>
        public void Update(T readModel)
        {
            var id = GetObjectIdFrom(readModel);

            var filter = Builders<T>.Filter.Eq("_id", id);
            _collection.ReplaceOne(filter, readModel, new UpdateOptions() { IsUpsert = true });
        }

        BsonValue GetObjectIdFrom(T entity)
        {
            var propInfo = GetIdProperty(entity);
            object id = propInfo.GetValue(entity);

            return GetIdAsBsonValue(id);
        }

        BsonValue GetIdAsBsonValue(object id)
        {
            var idVal = id;
            if (id.IsConcept())
                idVal = id.GetConceptValue();

            var idAsValue = BsonValue.Create(idVal);
            return idAsValue;
        }

        PropertyInfo GetIdProperty(T entity)
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name.ToLowerInvariant()== "id").First();
        }

    }
}