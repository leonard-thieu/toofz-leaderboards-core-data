using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class TypedDataReader<T> : IDataReader
    {
        private static Func<object, object> CreatePropertyGetter(PropertyInfo propertyInfo, Type declaringEntityType)
        {
            var getter = propertyInfo.GetMethod;
            var propertyType = propertyInfo.PropertyType;
            var entityParameter = Expression.Parameter(typeof(object), "entity");
            Expression getterExpression = Expression.Property(Expression.Convert(entityParameter, declaringEntityType), propertyInfo);

            if (propertyType.GetTypeInfo().IsValueType)
            {
                getterExpression = Expression.Convert(getterExpression, typeof(object));
            }

            return Expression.Lambda<Func<object, object>>(getterExpression, entityParameter).Compile();
        }

        public TypedDataReader(IEntityType entityType, IEnumerable<T> items)
        {
            var type = typeof(T);
            foreach (var property in entityType.GetProperties())
            {
                ordinals.Add(property.Name, fieldCount);
                var getter = CreatePropertyGetter(property.PropertyInfo, property.DeclaringEntityType.ClrType);
                getters.Add(getter);
                fieldCount++;
            }
            this.items = items.GetEnumerator();
        }

        private readonly List<Func<object, object>> getters = new List<Func<object, object>>();
        private readonly Dictionary<string, int> ordinals = new Dictionary<string, int>();
        private readonly IEnumerator<T> items;

        public int FieldCount => fieldCount;
        private readonly int fieldCount;

        public int GetOrdinal(string name) => ordinals[name];
        public object GetValue(int i) => getters[i](items.Current);
        public bool Read() => items.MoveNext();

        #region Not used by SqlBulkCopy (satisfying interface only)

        string IDataRecord.GetName(int i) => throw new NotImplementedException();
        string IDataRecord.GetDataTypeName(int i) => throw new NotImplementedException();
        Type IDataRecord.GetFieldType(int i) => throw new NotImplementedException();
        int IDataRecord.GetValues(object[] values) => throw new NotImplementedException();
        bool IDataRecord.GetBoolean(int i) => throw new NotImplementedException();
        byte IDataRecord.GetByte(int i) => throw new NotImplementedException();
        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        char IDataRecord.GetChar(int i) => throw new NotImplementedException();
        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        Guid IDataRecord.GetGuid(int i) => throw new NotImplementedException();
        short IDataRecord.GetInt16(int i) => throw new NotImplementedException();
        int IDataRecord.GetInt32(int i) => throw new NotImplementedException();
        long IDataRecord.GetInt64(int i) => throw new NotImplementedException();
        float IDataRecord.GetFloat(int i) => throw new NotImplementedException();
        double IDataRecord.GetDouble(int i) => throw new NotImplementedException();
        string IDataRecord.GetString(int i) => throw new NotImplementedException();
        decimal IDataRecord.GetDecimal(int i) => throw new NotImplementedException();
        DateTime IDataRecord.GetDateTime(int i) => throw new NotImplementedException();
        IDataReader IDataRecord.GetData(int i) => throw new NotImplementedException();
        bool IDataRecord.IsDBNull(int i) => throw new NotImplementedException();
        object IDataRecord.this[int i] => throw new NotImplementedException();
        object IDataRecord.this[string name] => throw new NotImplementedException();
        void IDataReader.Close() => throw new NotImplementedException();
        DataTable IDataReader.GetSchemaTable() => throw new NotImplementedException();
        bool IDataReader.NextResult() => throw new NotImplementedException();
        int IDataReader.Depth => throw new NotImplementedException();
        bool IDataReader.IsClosed => throw new NotImplementedException();
        int IDataReader.RecordsAffected => throw new NotImplementedException();
        void IDisposable.Dispose() { }

        #endregion
    }
}