using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PSSR.Security.Helpers
{
    public class DatabaseService : IDatabaseService
    {
        public string ConnectionString { get; set; }

        private SqlConnection DatabaseConnection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = DatabaseConnection.CreateCommand();
            using (DatabaseConnection)
            {
                CreateCommand(cmd, DatabaseConnection, null, cmdType, cmdText, cmdParms, null);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = DatabaseConnection.CreateCommand();
            CreateCommand(cmd, DatabaseConnection, null, cmdType, cmdText, cmdParms, null);
            var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return rdr;
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = DatabaseConnection.CreateCommand();
            CreateCommand(cmd, DatabaseConnection, null, cmdType, cmdText, cmdParms, null);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        private void CreateCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters, DataTable dt, bool isTvp = false)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = cmdType;
            if (isTvp)
            {
                SqlParameter tvpParam = cmd.Parameters.AddWithValue(dt.TableName, dt); //Needed TVP
                tvpParam.SqlDbType = SqlDbType.Structured; //tells ADO.NET we are passing TVP
            }
            else
            {
                if (commandParameters != null)
                {
                    AddSqlParameters(cmd, commandParameters);
                }
            }
        }

        private void AddSqlParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType, DataTable dt)
        {
            SqlCommand cmd = DatabaseConnection.CreateCommand();
            using (DatabaseConnection)
            {
                CreateCommand(cmd, DatabaseConnection, null, cmdType, cmdText, null, dt, true);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
    }
}
