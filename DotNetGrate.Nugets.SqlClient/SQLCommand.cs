namespace DotNetGrate.Nugets.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public abstract class SQLCommand
    {
        protected IDbCommand dbCommand;

        public abstract int ExecuteNonQuery();

        public abstract void ExecuteReader(Action<SQLReader> readHandler);

        public abstract List<T> ExecuteReader<T>(Func<SQLReader, T> readHandler);

        public abstract T ExecuteScalar<T>();

        public SQLCommand SetCommandTimeout(int commandTimeout)
        {
            this.dbCommand.CommandTimeout = commandTimeout;

            return this;
        }

        public SQLCommand SetCommandType(CommandType commandType)
        {
            this.dbCommand.CommandType = commandType;

            return this;
        }

        public SQLCommand SetParameter(string name, object value)
        {
#if NET40_OR_GREATER || NET5_0_OR_GREATER 
            if (!string.IsNullOrWhiteSpace(name))
#else
            if (!string.IsNullOrEmpty(name))
#endif
            {
                if (value == null)
                {
                    value = DBNull.Value;
                }

                var parameter = this.dbCommand.CreateParameter();
                parameter.ParameterName = name;
                parameter.Value = value;

                _ = this.dbCommand.Parameters.Add(parameter);
            }

            return this;
        }
    }
}
