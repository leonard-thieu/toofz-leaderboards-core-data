using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Mapping;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class TypedDataReader<T> : IDataReader
    {
        public TypedDataReader(IEnumerable<ScalarPropertyMapping> scalarPropertyMappings, IEnumerable<T> items)
        {
            var type = typeof(T);
            foreach (var scalarPropertyMapping in scalarPropertyMappings)
            {
                ordinals.Add(scalarPropertyMapping.Column.Name, fieldCount);
                var context = new ScalarPropertyMappingContext(scalarPropertyMapping, type);
                getters.Add(context.ValueGetter);
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