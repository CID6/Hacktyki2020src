using Microsoft.Data.SqlClient;
using RabbitEntityConsumer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppForCarsDB.Services
{
    public interface ISqlDependencyManager
    {
        Task WriteMessage(string message);
        Task SetAction(Action<SqlDataReader> action);
        Task UnsetAction();
    }
}
