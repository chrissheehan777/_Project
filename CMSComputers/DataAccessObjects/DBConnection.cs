using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccessObjects
{
    internal class DBConnection  //only place for sql conn
    {
        //const string ConnectionString = @"Data Source=localhost;Initial Catalog=CMSComputers;Integrated Security=True";
        const string ConnectionString = @"Data Source=chris-laptop\sqlexpress;Initial Catalog=CMSComputers;User ID=sa;Password=Sva17079";
        public static SqlConnection GetDBConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
