namespace DotNetGrate.Nugets.SqlClient
{
    using System.Data;

    public class SQLReader
    {
        private readonly IDataReader dataReader;

        internal SQLReader(IDataReader dataReader) => this.dataReader = dataReader;

        public T GetValue<T>(int columnIndex)
        {
            if (0 <= columnIndex && columnIndex < this.dataReader.FieldCount && !this.dataReader.IsDBNull(columnIndex))
            {
                return this.dataReader
                    .GetValue(columnIndex)
                    .TryParseValueOrDefault<T>();
            }

            return default;
        }

        public bool Read() => this.dataReader.Read();
    }
}