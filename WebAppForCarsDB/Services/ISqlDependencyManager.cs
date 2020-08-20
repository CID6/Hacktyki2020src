using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace WebAppForCarsDB.Services
{
    public interface ISqlDependencyManager
    {
        Task SetAction(Action<SqlDataReader> action);
        Task UnsetAction();
    }
}
