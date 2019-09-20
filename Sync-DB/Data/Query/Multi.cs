using System;
using System.Threading;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CsMySql.Data.Query
{
    class Multi : BaseScript
    {
        public Multi()
        {
            Exports.Add("Multi_Sync", new Action<string>(Multi_Sync));
            Exports.Add("Multi_Async", new Action<string>(Multi_Async));
        }

        //
        // Multi_Sync
        // Execute a single SQL statement synchronously and returns first result
        //
        private void Multi_Sync(string query)
        {
            Thread t = new Thread(() => MultiQuery(query, false));
            t.Start();
        }

        //
        // Multi_Async
        // Execute a single SQL statement asynchronously and returns first result
        //
        private void Multi_Async(string query)
        {
            Thread t = new Thread(() => MultiQuery(query, true));
            t.Start();
        }

        private async void MultiQuery(string query, bool async)
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
                    if (async)
                    {
                        await conn.OpenAsync();
                    }
                    else
                    {
                        conn.Open();
                    }
                    
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    Debug.WriteLine($"{0} \t {1}", rdr.GetInt32(0), rdr.GetString(1));
                                }
                            }
                            else
                            {
                                // Return 0 rows
                            }
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
