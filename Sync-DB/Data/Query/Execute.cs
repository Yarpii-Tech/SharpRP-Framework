using System;
using System.Threading;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CsMySql
{
    class Execute : BaseScript
    {
        public Execute()
        {
            Exports.Add("Execute_Sync", new Action<string>(Execute_Sync));
            Exports.Add("Execute_Async", new Action<string>(Execute_Async));
        }

        //
        // Execute_Sync
        // Execute a single SQL statement synchronously without a return
        //
        private void Execute_Sync(string query)
        {
            Thread t = new Thread(()=>ExecuteQuery(query, false));
            t.Start();
        }

        //
        // Execute_Async
        // Execute a single SQL statement asynchronously without a return
        //
        private void Execute_Async(string query)
        {
            Thread t = new Thread(() => ExecuteQuery(query, true));
            t.Start();
        }

        private async void ExecuteQuery(string query, bool async)
        {
            if (!Main.connection_validated)
            {
                Debug.WriteLine("[Cs-MySql] Error: Database failed to initialize");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Main.ConnectionStringBldr.ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        
                        if (async)
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        else
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
