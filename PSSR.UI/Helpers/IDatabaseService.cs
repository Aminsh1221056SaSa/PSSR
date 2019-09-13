using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PSSR.UI.Helpers
{
    public interface IDatabaseService
    {
        //int Insert_EventLog(EventLog eventLog);
        //int UpdateStuffOnUpdateFromServer();
        string ConnectionString { get; set; }
        int ExecuteNonQuery(string cmdText, CommandType cmdType, SqlParameter[] cmdParms);
        int ExecuteNonQuery(string cmdText, CommandType cmdType, DataTable dt);
        SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, SqlParameter[] cmdParms);
        object ExecuteScalar(string cmdText, CommandType cmdType, SqlParameter[] cmdParms);
    }
}
