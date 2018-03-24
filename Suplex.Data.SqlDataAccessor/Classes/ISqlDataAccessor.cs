using System;
using System.Data.SqlClient;

namespace Suplex.Data
{
    public interface ISqlDataAccessor
    {
        string ConnectionString { get; set; }
        SqlConnection Connection { get; }

        void OpenConnection();
        void CloseConnection();
    }
}