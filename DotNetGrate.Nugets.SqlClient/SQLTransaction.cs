namespace DotNetGrate.Nugets.SqlClient
{
    using System.Data;

    public class SQLTransaction
    {
        private readonly IDbTransaction dbTransaction;

        internal SQLTransaction(IDbTransaction dbTransaction) => this.dbTransaction = dbTransaction;

        public void CommitTransaction() => this.dbTransaction.Commit();

        /// <summary>
        /// <inheritdoc cref="IDbConnection.CreateCommand()"/>
        /// </summary>
        public SQLCommand CreateCommand(string commandText)
        {
            var dbCommand = this.dbTransaction.Connection.CreateCommand();
            dbCommand.Transaction = this.dbTransaction;

            return new SQLTransactionCommand(dbCommand, commandText);
        }

        public void RollbackTransaction() => this.dbTransaction.Rollback();
    }
}
