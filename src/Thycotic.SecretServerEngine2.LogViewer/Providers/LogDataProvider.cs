using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Thycotic.SecretServerEngine2.LogViewer.Models;
using Thycotic.SecretServerEngine2.LogViewer.Views;

namespace Thycotic.SecretServerEngine2.LogViewer.Providers
{
    public class LogDataProvider
    {
        private readonly Database _database;


        public LogDataProvider(string connectionStringName)
        {
            _database = new Database(connectionStringName);
        }

        public T Execute<T>(Func<T> func)
        {
            try
            {
                lock (_database)
                {
                    return func.Invoke();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                return default(T);
            }
        }

        public IEnumerable<LogEntry> GetLatestCorrelations(string logLevel)
        {
            return Execute(() =>
            {
                var sb = new StringBuilder();

                sb.Append(@"
                    SELECT TOP 50
                           [Date] = MIN([Date])
                             ,[Correlation]
                          ,[Level]      
                    FROM ( ");

                //inner
                sb.Append(@"
                        SELECT TOP 1000 
                            [Date] = [Date]
                            ,[Correlation]
                            ,[Level]  
                                         FROM Log
                    ");

                if (logLevel != LogLevelViewModel.CatchAllLevel)
                {
                    sb.Append(@"
                        WHERE Level=@0 AND Correlation IS NOT NULL 
                    ");
                }

                sb.Append(@"
                        ORDER BY [Date] DESC");

                //end inner
                sb.Append(@" ) raw
                    GROUP BY [Correlation], [Level]
                    ORDER BY [Date] DESC
                ");

                return _database.Query<LogEntry>(sb.ToString(), logLevel);
            });
        }

        public IEnumerable<LogEntry> GetLogEntriesInCorrelation(string correlation)
        {
            return Execute(() =>
            {
                var sb = new StringBuilder();

                sb.Append(@"
                    SELECT 
                       [Id]
                      ,[Date]
                      ,[UserID]
                      ,[ServiceRole]
                      ,[Correlation]
                      ,[Context]
                      ,[Thread]
                      ,[Level]
                      ,[Logger]
                      ,[Message]
                      ,[Exception]    
                    FROM Log 
                    WHERE Correlation = @0
                    ORDER BY [Date] DESC
                ");

                return _database.Query<LogEntry>(sb.ToString(), correlation);
            });
        }
    }
}