﻿namespace DotNetGrate.Nugets.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class SQLConnectionCommand : SQLCommand
    {
        private readonly IDbConnection dbConnection;

        internal SQLConnectionCommand(IDbConnection dbConnection, string commandText)
        {
            this.dbConnection = dbConnection;
            this.dbCommand = this.dbConnection.CreateCommand();
            this.dbCommand.CommandText = commandText;
        }

        /// <summary>
        /// <inheritdoc cref="IDbCommand.ExecuteNonQuery()"/>
        /// </summary>
        public override int ExecuteNonQuery()
        {
            var rowsAffected = default(int);

            try
            {
                using (this.dbConnection)
                {
                    this.dbConnection.Open();

                    using (this.dbCommand)
                    {
                        rowsAffected = this.dbCommand.ExecuteNonQuery();

                        this.dbCommand.Log();
                    }
                }
            }
            catch (Exception e)
            {
                e.Log();
            }

            return rowsAffected;
        }

        /// <summary>
        /// <inheritdoc cref="IDbCommand.ExecuteReader()"/>
        /// </summary>
        public override void ExecuteReader(Action<SQLReader> readHandler)
        {
            try
            {
                using (this.dbConnection)
                {
                    this.dbConnection.Open();

                    using (this.dbCommand)
                    {
                        using (var dataReader = this.dbCommand.ExecuteReader())
                        {
                            var dbReader = new SQLReader(dataReader);

                            while (dbReader.Read())
                            {
                                readHandler(dbReader);
                            }
                        }

                        this.dbCommand.Log();
                    }
                }
            }
            catch (Exception e)
            {
                e.Log();
            }
        }

        /// <summary>
        /// <inheritdoc cref="IDbCommand.ExecuteReader()"/>
        /// </summary>
        public override List<T> ExecuteReader<T>(Func<SQLReader, T> readHandler)
        {
            var results = new List<T>();

            try
            {
                using (this.dbConnection)
                {
                    this.dbConnection.Open();

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
                        }

                        this.dbCommand.Log();
                    }
                }
            }
            catch (Exception e)
            {
                e.Log();
            }

            return results;
        }

        /// <summary>
        /// <inheritdoc cref="IDbCommand.ExecuteScalar()"/>
        /// </summary>
        public override T ExecuteScalar<T>()
        {
            var result = default(T);

            try
            {
                using (this.dbConnection)
                {
                    this.dbConnection.Open();

                    using (this.dbCommand)
                    {
                        result = this.dbCommand
                            .ExecuteScalar()
                            .TryParseValueOrDefault<T>();

                        this.dbCommand.Log();
                    }
                }
            }
            catch (Exception e)
            {
                e.Log();
            }

            return result;
        }
    }
}
