using System;
using System.Data;
using System.Net;
using System.Windows.Forms;
namespace POS.DataAccessLayer
{
    class DbManger
    {
        private IDatabaseHandler database;
        private FactoryDatabase FactoryDatabase;
        private Enum Provide;
        public DbManger(Enum Provider)
        {
            this.Provide = Provider;
            FactoryDatabase = new FactoryDatabase(Provider);
            database = FactoryDatabase.Database();
        }
        public IDbDataParameter CreateParameter(string name,DbType dbType,object value,ParameterDirection direction=ParameterDirection.Input)
        {
         
            return ParameterManger.CreateParameter(this.Provide, name, dbType, value, direction);
        }

        public IDbDataParameter CreateParameter(string name, DbType dbType,int siz, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            return ParameterManger.CreateParameter(this.Provide, name,siz, dbType, value, direction);
        }
        public bool ExuteNoneQuery(string SQL,CommandType commandType,params IDbDataParameter[] parameters)
        {
            bool Result = false;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    using (var cmd = database.CreateCommand(SQL, commandType, con))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                   
                       
                        try
                        {
                            con.Open();
                            int n = cmd.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
                            if (n > -1)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception Exp)
                        {
                            database.Close(con);
                            MSGError(Exp);
                            return false;
                        }   
                    }
                }
            }
            catch (Exception Ex)
            {
                MSGError(Ex);
               
                return Result;
            } 

        }
        private void MSGError(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }

        public bool Delete(string SQL, CommandType commandType, params IDbDataParameter[] parameters)
        {

            bool Result = false;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    using (var cmd = database.CreateCommand(SQL, commandType, con))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }


                        try
                        {
                            con.Open();
                            int n = cmd.ExecuteNonQuery();
                            con.Close();
                            if (n > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception Exp)
                        {
                            database.Close(con);
                            MSGError(Exp);
                            return false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MSGError(Ex);

                return Result;
            }
        }


        public bool Update(string SQL, CommandType commandType, params IDbDataParameter[] parameters)
        {


            bool Result = false;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    using (var cmd = database.CreateCommand(SQL, commandType, con))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }


                        try
                        {
                            con.Open();
                            int n = cmd.ExecuteNonQuery();
                            con.Close();
                            if (n > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception Exp)
                        {
                            database.Close(con);
                            MSGError(Exp);
                            return false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MSGError(Ex);

                return Result;
            }
        }

        //public Int64 LastId()
        //{

        //}
        public DataTable GetDataTable(string Query, CommandType commandType, params IDbDataParameter[] parameters)
        {
          
            try
            {
               
                using (var con = database.CreateDBConnection())
                {
                    using (var cmd = database.CreateCommand(Query, commandType, con))
                    {

                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        var adapter = database.dataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet.Tables[0];
                    }
                }

            }
            catch (Exception Ex)
            {
                MSGError(Ex);
                return null;
            }
           
        }

        public bool DbTransaction(string SQL, CommandType commandType, params IDbDataParameter[] parameters)
        {
            try
            {
                bool Result = false;
                using (var con = database.CreateDBConnection())
                {
                    var tra = con.BeginTransaction();
                    using (var cmd = database.CreateCommand(SQL, commandType, con))
                    {
                        cmd.Transaction = tra;
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        try
                        {
                            con.Open();
                            int n = cmd.ExecuteNonQuery();
                            con.Close();
                            tra.Commit();
                            if (n>0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                            

                        }
                        catch (Exception Exc)
                        {
                            database.Close(con);
                            tra.Rollback();
                            MSGError(Exc);
                            return false;
                        }
                   
                    }
                }
            }
            catch (Exception ex)
            {
                MSGError(ex);
                return false;
            }
        }

       

        public DataSet GetDataSet(string Query, CommandType commandType, params IDbDataParameter[] parameters)
        {

            try
            {

                using (var con = database.CreateDBConnection())
                {
                    using (var cmd = database.CreateCommand(Query, commandType, con))
                    {

                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        var adapter = database.dataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }

            }
            catch (Exception Ex)
            {
                MSGError(Ex);
                return null;
            }
        }
        public void Open(IDbConnection dbConnection)
        {
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                MSGError(ex);
            }

        }
        public void Close(IDbConnection dbConnection)
        {
                if (dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                dbConnection.Dispose();
                }
            
        }

        private bool CancelTransaction(bool resultisCancel,IDbTransaction dbTransaction,IDbConnection dbConnection)
        {
            if(!resultisCancel)
            {
                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbConnection.Close();
                dbConnection.Dispose();
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ExuteMoreRow(string SQL,IDbTransaction dbTransaction,IDbConnection dbConnection,CommandType commandType,params IDbDataParameter[] parameters)
        {

            var cmd = database.CreateCommand(SQL, commandType);
                cmd.Connection = dbConnection;
                cmd.Transaction = dbTransaction;
                if (parameters != null)
                {
                try
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                 

                }
                try
                {
                    int n = cmd.ExecuteNonQuery();

                    if (n > 0)
                        return true;
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MSGError(ex);
                    return false;
                }

            

        }

        public bool transactionImplement(IDbTransaction dbTransaction)
        {
            try
            {
                dbTransaction.Commit();
                dbTransaction.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public IDbConnection Getconection()
        {
            try
            {
                var connection = database.CreateDBConnection();
                
                    return connection;
               
            }
            catch (Exception Ex)
            {
                MSGError(Ex);
                return null;
            }
        }
        public IDataReader DataReader(string Query, CommandType commandType, params IDbDataParameter[] parameters)
        {
            IDataReader reader=null;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    try
                    {
                        using(var cmd=database.CreateCommand(Query,commandType,con))
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                            con.Open();
                        reader = database.DataReader(Query, commandType, con);
                        con.Close();
                        }
                        
                    }
                    catch (Exception exp)
                    {
                        database.Close(con);
                        MSGError(exp);
                        return null;
                    }
                    return reader;
                }
            }
            catch (Exception ex)
            {
                MSGError(ex);
                return null;
            }
        }
        public IDataReader RDataReader(string Query, CommandType commandType, params IDbDataParameter[] parameters)
        {
            IDataReader reader = null;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    try
                    {
                        con.Open();
                        using (var cmd = database.CreateCommand(Query, commandType, con))
                        {
                            DataSet dt = new DataSet();

                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }

                            var sa = database.dataAdapter(cmd);
                           
                            sa.Fill(dt);
                            reader = dt.Tables[0].CreateDataReader();
                            con.Close();
                            con.Dispose();
                            return reader;
                        }
                    }
                    catch (Exception exp)
                    {
                        database.Close(con);
                        MSGError(exp);
                        return null;
                    }
                    return reader;
                }
            }
            catch (Exception ex)
            {
                MSGError(ex);
                return null;
            }
        }
        public object GetScallerValue(string Query, CommandType commandType, params IDbDataParameter[] parameters)
        {
            try
            {
                using(var con=database.CreateDBConnection())
                {
                    using(var cmd=database.CreateCommand(Query,commandType,con))
                    {
                        if(parameters!=null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        try
                        {
                            con.Open();
                       var rr=  cmd.ExecuteScalar();
                       /*MessageBox.Show(rr.ToString());
                       if (rr == DBNull.Value)
                       {
                           return 0;
                       }*/
                       con.Close();
                       return rr;
                        }
                        catch (Exception exp) 
                        {
                            database.Close(con);
                            MSGError(exp);
                            return null;
                        }
                        
                    }
                }
            }
            catch (Exception Ex)
            {
                MSGError(Ex);
                return null;
            }
        }
        public long Insert(string SQL, CommandType commandType,  IDbDataParameter[] parameters,out long lastID)
        {
            lastID = 0;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    using(var cmd=database.CreateCommand(SQL,commandType,con))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        try
                        {
                            con.Open();
                            object newID = cmd.ExecuteScalar();
                            con.Close();
                            lastID = Convert.ToInt64(newID);
                            return lastID;
                        }
                        catch (Exception exp)
                        {
                            database.Close(con);
                            MSGError(exp);
                            return lastID;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MSGError(exp);
                return lastID;
            }
        }
        public int Insert(string SQL, CommandType commandType, IDbDataParameter[] parameters, out int lastID)
        {
            lastID = 0;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    using (var cmd = database.CreateCommand(SQL, commandType, con))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        try
                        {
                            con.Open();
                            object newID = cmd.ExecuteScalar();
                            con.Close();
                            lastID = Convert.ToInt32(newID);
                            return lastID;
                        }
                        catch (Exception exp)
                        {
                            database.Close(con);
                            MSGError(exp);
                            return lastID;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MSGError(exp);
                return lastID;
            }
        }

        //public bool ImportDataGridview(string column,object Data,CommandType commandType)
        //{
            
        //}

        public long lastID(string SQL, CommandType commandType, IDbDataParameter[] parameters)
        {
            long lastID = 0;
            try
            {
                using (var con = database.CreateDBConnection())
                {
                    using (var cmd = database.CreateCommand(SQL, commandType, con))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        try
                        {
                            con.Open();

                            lastID=Convert.ToInt64(cmd.ExecuteScalar());
                            con.Close();
                            return lastID;
                        }
                        catch (Exception exp)
                        {
                            database.Close(con);
                            MSGError(exp);
                            return lastID;
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                MSGError(Ex);
                return lastID;
            }
        }
        public long lastID(string SQL, CommandType commandType,IDbConnection dbConnection,IDbTransaction dbTransaction,params IDbDataParameter[] parameters)
        {
            long lastID = 0;
            try
            {                
                    using (var cmd = database.CreateCommand(SQL, commandType, dbConnection))
                    {
                    cmd.Transaction = dbTransaction;
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        try
                        {              
                            lastID = Convert.ToInt64(cmd.ExecuteScalar());
                            return lastID;
                        }
                        catch (Exception exp)
                        {
                            MSGError(exp);
                            return lastID;
                        }

                    }
                
            }
            catch (Exception Ex)
            {
                MSGError(Ex);
                return lastID;
            }
        }
        public DataTable Mergtable(string Query1,string nameTable,string Query2,string nameTable2,CommandType commandType,params IDbDataParameter []parameters)
        {
           DataTable dt=new DataTable();
            DataSet ds = new DataSet();
            using(var con=database.CreateDBConnection())
            {
                using (var cmd = database.CreateCommand(Query1, commandType, con))
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    var da = database.dataAdapter(cmd)as System.Data.SqlClient.SqlDataAdapter;
                    da.Fill(ds,nameTable.Trim());
                    da.SelectCommand.CommandText = Query2;
                    da.SelectCommand.CommandType = commandType;
                    da.Fill(ds, nameTable2);
                    da.Dispose();
                }

                ds.Tables[0].Merge(ds.Tables[1]);
                dt = ds.Tables[0];
                ds.Dispose();
                
            }
            return dt;
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var r = client.OpenRead(@"https://azure.microsoft.com/en-us/free/students/"))// check is server
                    if (r.CanRead)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            catch
            {
                return false;
            }
        }
    }
}
