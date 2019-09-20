using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;

namespace CsMySql
{
    class Query : BaseScript
    {
        public Query()
        {
            Exports.Add("Query_Sync", new Action<dynamic, Boolean>(Query_Sync));
            //Tick += onTick;
        }

        private void Query_Sync(dynamic query, Boolean resultExpected)
        {
            Debug.WriteLine($"QueryMessage: {query.msg}");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Main.ConnectionStringBldr.ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        if (resultExpected)
                        {
                            using (MySqlDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    // Put together a response
                                }
                            }
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

        private async Task onTick()
        {
            await Delay(1000);
            Exports["SharpRPF"].Query_Sync(new { msg = "Hi this is a test" });
        }
    }
}