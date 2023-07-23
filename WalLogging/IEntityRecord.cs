using System.Security.Cryptography;
using System.Text;

namespace WalLogging
{
    public class EntityRecord<T> : IEntityRecord<T> where T : class
    {
        public EntityRecord(
            T entity,
            byte[]? id = null,
            byte[]? typeHash = null,
            Func<T, T, T>? aggregateFunction = null)
        {
            Entity = entity;

            Id = id ?? Guid.NewGuid().ToByteArray();
            TypeHash = typeHash ?? CalcTypeHash();
            RecordType =
               id == null && typeHash == null ?
               RecordType.NewRecord :
               RecordType.OldRecord;
            if (aggregateFunction != null)
                _aggregateFunction = aggregateFunction;
        }

        private byte[] CalcTypeHash()
        {
            return MD5.HashData(Encoding.ASCII.GetBytes(typeof(T).FullName));
        }

        public byte[] Id { get; set; }

        public T Entity { get; }

        public byte[] TypeHash { get; }

        //public int ContentLength { get; }

        public RecordType RecordType { get; }

        private Func<T, T, T> _aggregateFunction;
        public T AggregateFunction(T x, T y)
        {
            if (_aggregateFunction == null)
                throw new NotImplementedException();
            return _aggregateFunction(x, y);
        }
    }
    public interface IEntityRecord<T> where T : class
    {
        byte[] Id { get; }
        T Entity { get; }
        byte[] TypeHash { get; }
        //int ContentLength { get; }
        T AggregateFunction(T x, T y);
        RecordType RecordType { get; }
    }
}