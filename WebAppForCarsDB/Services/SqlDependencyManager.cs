using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppForCarsDB.Services
{
    public class SqlDependencyManager : ISqlDependencyManager
    {
        private readonly ILogger<SqlDependencyManager> _logger;
        private SqlDependency sqlDependency;
        private SqlCommand dependencyCommand;
        private SqlConnection sqlConnection;
        private Action<SqlDataReader> dependencyEvent = null;
        private SqlDataReader dependencyReader;
        private SqlCommand queryCommand;
        private SqlDataReader queryReader;

        const string queryString = "select Year, VIN, CarModels.Name as ModelName, CarFactories.Name as FactoryName from [dbo].CarProducts" + "\n" +
            "inner join [dbo].CarModels on CarModelId = [dbo].CarModels.Id" + "\n" +
            "inner join [dbo].CarFactories on FactoryId = [dbo].CarFactories.Id";

        public SqlDependencyManager(ILogger<SqlDependencyManager> logger)
        {
            _logger = logger;


            EstablishConnection();
        }

        public Task SetAction(Action<SqlDataReader> action)
        {
            dependencyEvent = action;

            CreateNewDependency();

            return Task.FromResult(0);
        }

        public Task UnsetAction()
        {
            dependencyEvent = null;

            return Task.FromResult(0);
        }

        public Task WriteMessage(string message)
        {
            _logger.LogInformation("Writemessage called. Message: {MESSAGE}", message);

            return Task.FromResult(0);
        }

        private void EstablishConnection()
        {
            sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Integrated Security=true;Database=CarsDB;";
            sqlConnection.Open();


            SqlDependency.Start("Data Source=(LocalDb)\\MSSQLLocalDB;Integrated Security=true;Database=CarsDB;");
        }

        private void HandleDependencyEvent(object sender, SqlNotificationEventArgs e)
        {
            queryCommand = new SqlCommand(queryString, sqlConnection);
            _logger.LogInformation(e.Info.ToString());

            sqlDependency.OnChange -= HandleDependencyEvent;
            dependencyReader.Close();

            queryReader = queryCommand.ExecuteReader();

            dependencyEvent(queryReader);

            CreateNewDependency();
        }

        public void CreateNewDependency()
        {
            dependencyCommand = new SqlCommand("SELECT Year FROM [dbo].CarProducts", sqlConnection);
            sqlDependency = new SqlDependency(dependencyCommand);
            sqlDependency.OnChange += HandleDependencyEvent;

            dependencyReader = dependencyCommand.ExecuteReader();
        }

    }
}
