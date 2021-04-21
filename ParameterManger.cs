
using System;
using System.Data;
namespace POS.DataAccessLayer
{
    using Provider;
    using System.Data.SqlClient;

    class ParameterManger
    {
        public static IDbDataParameter CreateParameter(Enum provide,string name,DbType dbType,object value,ParameterDirection direction=ParameterDirection.Input)
        {
           
            switch (provide)
            {
                case provideServer.MNSQL:
                    return SqlParameter(name, dbType, value, direction);
                default:
                    return null;
            }
        }

        public static IDbDataParameter CreateParameter(Enum provide, string name,int size, DbType dbType, object value, ParameterDirection direction = ParameterDirection.Input)
        {

            switch (provide)
            {
                case provideServer.MNSQL:
                    return SqlParameter(name, dbType,size, value, direction);
                default:
                    return null;
            }
        }


        private static SqlParameter SqlParameter(string Name,DbType dbType,object value,ParameterDirection direction=ParameterDirection.Input)
        {
            return new SqlParameter { ParameterName = Name, DbType = dbType, Value = value, Direction = direction };
        }
        private static SqlParameter SqlParameter(string Name, DbType dbType,int size, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter { ParameterName = Name, DbType = dbType, Value = value,Size=size, Direction = direction };
        }
    }
}
