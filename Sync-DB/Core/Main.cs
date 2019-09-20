using System;
using System.Threading;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CsMySql
{
    class Main : BaseScript
    {
        public static MySqlConnectionStringBuilder ConnectionStringBldr = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            UserID = "root",
            Password = "Overfl0w1@",
            Database = ""
        };
        public static bool connection_validated = false;

        public Main()
        {
            EventHandlers["onResourceStart"] += new Action<string>(check_db);
        }

        private void check_db(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Thread t = new Thread(init_db);
            t.Start();
        }

        private void init_db()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionStringBldr.ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'sharpdb';";
                        Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count < 1) // Database doesn't exist, create it now
                        {
                            cmd.CommandText = "CREATE DATABASE IF NOT EXISTS sharpdb;";
                            cmd.ExecuteNonQuery();
                        }
                    }
                        
                    Debug.WriteLine("[MySQL] Database connection succesful");
                    connection_validated = true;
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}