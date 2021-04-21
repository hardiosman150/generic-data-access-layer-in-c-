namespace POS.DataAccessLayer
{
    using System.Data;
    interface IDatabaseHandler
    {
        IDbConnection CreateDBConnection();   
        IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection);
        IDbCommand CreateCommand(string commandText, CommandType commandType);
        IDbTransaction dbTransaction(string commandText, CommandType commandType, IDbConnection connection);
        void Open(IDbConnection connection);
        void Close(IDbConnection connection);
        IDataAdapter dataAdapter(IDbCommand command);
        IDataReader DataReader(string commandText, CommandType commandType, IDbConnection connection);
    }
}
