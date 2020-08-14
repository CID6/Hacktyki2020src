using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EFCarsDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
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

        public string SearchString { get; set; }

        private System.Timers.Timer timer;

        const string queryString = "select Year, VIN, CarModels.Name as ModelName, CarFactories.Name as FactoryName from [dbo].CarProducts" + "\n" +
            "inner join [dbo].CarModels on CarModelId = [dbo].CarModels.Id" + "\n" +
            "inner join [dbo].CarFactories on FactoryId = [dbo].CarFactories.Id";

        public SqlDependencyManager(ILogger<SqlDependencyManager> logger)
        {
            _logger = logger;
            EstablishConnection();
            StartTimer();
        }

        public Task SetAction(Action<SqlDataReader> action)
        {
            dependencyEvent = action;


            if (dependencyReader!=null)
            {
                dependencyReader.Close(); 
            }
            CreateNewDependency();

            return Task.FromResult(0);
        }

        public Task UnsetAction()
        {
            dependencyEvent = null;

            return Task.FromResult(0);
        }

        public void StartTimer() 
        {
            timer = new System.Timers.Timer(1000 * 60 * 5);
            timer.Elapsed += HandleTimerEvent;
            timer.Start();
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

        private void HandleTimerEvent(object sender, EventArgs e)
        {
            queryCommand = new SqlCommand(queryString, sqlConnection);
            _logger.LogInformation("Timer");

            dependencyReader.Close();
            queryReader = queryCommand.ExecuteReader();
            dependencyEvent(queryReader);
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
