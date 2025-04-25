namespace DotNetGrate.Nugets.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class SQLTransactionCommand : SQLCommand
    {
        internal SQLTransactionCommand(IDbCommand dbCommand, string commandText)
        {
            this.dbCommand = dbCommand;
            this.dbCommand.CommandText = commandText;

            this.dbCommand.Parameters.Clear();
        }

        public override int ExecuteNonQuery()
        {
            var rowsAffected = default(int);

            using (this.dbCommand)
            {
                rowsAffected = this.dbCommand.ExecuteNonQuery();

                this.dbCommand.Log();
            }

            return rowsAffected;
        }

        public override void ExecuteReader(Action<SQLReader> readHandler)
        {
            using (this.dbCommand)
            {
                using (var dataReader = this.dbCommand.ExecuteReader())
                {
                    var dbReader = new SQLReader(dataReader);

                    while (dbReader.Read())
                    {
                        readHandler(dbReader);
                    }

                    this.dbCommand.Log();
                }
            }
        }

        public override List<T> ExecuteReader<T>(Func<SQLReader, T> readHandler)
        {
            var results = new List<T>();

            using (this.dbCommand)
            {
                using (var dataReader = this.dbCommand.ExecuteReader())
                {
                    var dbReader = new SQLReader(dataReader);

                    while (dbReader.Read())
                    {
                        var value = readHandler(dbReader);

                        results.Add(value);
                    }

                    this.dbCommand.Log();
                }
            }

            return results;
        }

        public override T ExecuteScalar<T>()
        {
            var result = default(T);

            using (this.dbCommand)
            {
                result = this.dbCommand
                    .ExecuteScalar()
                    .TryParseValueOrDefault<T>();

                this.dbCommand.Log();
            }

            return result;
        }
    }
}
