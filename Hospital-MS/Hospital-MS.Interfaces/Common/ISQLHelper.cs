using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.Common
{
    public interface ISQLHelper
    {
        DataTable ExecuteDataTable(string commandText, params SqlParameter[] Parameters);
        DataSet ExecuteDataset(string commandText, SqlParameter[] commandParameters);
        List<TElement> SQLQuery<TElement>(string commandText, params SqlParameter[] parameters);
    }
}
