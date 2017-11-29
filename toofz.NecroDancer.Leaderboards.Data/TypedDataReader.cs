using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class TypedDataReader<T> : IDataReader
    {
        private static Func<object, object> CreatePropertyGetter(Type type, string propertyName)
        {
            var propertyInfo = type.GetTypeInfo().GetDeclaredProperty(propertyName) ??
                throw new ArgumentException($"Unable to find property '{propertyName}' on '{type.Name}'.");

            var propertyType = propertyInfo.PropertyType;
            var entityParameter = Expression.Parameter(typeof(object), "entity");
            Expression getterExpression = Expression.Property(Expression.Convert(entityParameter, type), propertyInfo);

            if (propertyType.GetTypeInfo().IsValueType)
            {
                getterExpression = Expression.Convert(getterExpression, typeof(object));
            }

            return Expression.Lambda<Func<object, object>>(getterExpression, entityParameter).Compile();
        }

        public TypedDataReader(IEnumerable<ScalarPropertyMapping> scalarPropertyMappings, IEnumerable<T> items)
        {
            var type = typeof(T);
            foreach (var scalarPropertyMapping in scalarPropertyMappings)
            {
                var getter = CreatePropertyGetter(type, scalarPropertyMapping.Property.Name);
                getters.Add(getter);
                ordinals.Add(scalarPropertyMapping.Column.Name, fieldCount);
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

        [ExcludeFromCodeCoverage] string IDataRecord.GetName(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] string IDataRecord.GetDataTypeName(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] Type IDataRecord.GetFieldType(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] int IDataRecord.GetValues(object[] values) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] bool IDataRecord.GetBoolean(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] byte IDataRecord.GetByte(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] char IDataRecord.GetChar(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] Guid IDataRecord.GetGuid(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] short IDataRecord.GetInt16(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] int IDataRecord.GetInt32(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] long IDataRecord.GetInt64(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] float IDataRecord.GetFloat(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] double IDataRecord.GetDouble(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] string IDataRecord.GetString(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] decimal IDataRecord.GetDecimal(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] DateTime IDataRecord.GetDateTime(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] IDataReader IDataRecord.GetData(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] bool IDataRecord.IsDBNull(int i) => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] object IDataRecord.this[int i] => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] object IDataRecord.this[string name] => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] void IDataReader.Close() => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] DataTable IDataReader.GetSchemaTable() => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] bool IDataReader.NextResult() => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] int IDataReader.Depth => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] bool IDataReader.IsClosed => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] int IDataReader.RecordsAffected => throw new NotImplementedException();
        [ExcludeFromCodeCoverage] void IDisposable.Dispose() { }

        #endregion
    }
}