using System;
using System.Threading;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CsMySql.Data.Query
{
    class Scalar : BaseScript
    {
        public Scalar()
        {
            Exports.Add("Scalar_Sync", new Action<string>(Scalar_Sync));
            Exports.Add("Scalar_Async", new Action<string>(Scalar_Async));
        }

        //
        // Scalar_Sync
        // Execute a single SQL statement synchronously and returns first result
        //
        private void Scalar_Sync(string query)
        {
            Thread t = new Thread(() => ScalarQuery(query, false));
            t.Start();
        }

        //
        // Scalar_Async
        // Execute a single SQL statement asynchronously and returns first result
        //
        private void Scalar_Async(string query)
        {
            Thread t = new Thread(() => ScalarQuery(query, true));
            t.Start();
        }

        private async void ScalarQuery(string query, bool async)
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
                            var result = await cmd.ExecuteScalarAsync();
                            // todo return result
                        }
                        else
                        {
                            var result = cmd.ExecuteScalar();
                            // todo return result
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
