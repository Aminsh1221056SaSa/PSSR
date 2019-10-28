
using System.Data;
using Microsoft.Data.SqlClient;

namespace PSSR.Security.Helpers
{
    public interface IDatabaseService
    {
        string ConnectionString { get; set; }
        int ExecuteNonQuery(string cmdText, CommandType cmdType, SqlParameter[] cmdParms);
        int ExecuteNonQuery(string cmdText, CommandType cmdType, DataTable dt);
        SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, SqlParameter[] cmdParms);
        object ExecuteScalar(string cmdText, CommandType cmdType, SqlParameter[] cmdParms);
    }
}
