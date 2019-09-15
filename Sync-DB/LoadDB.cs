using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Sync_DB
{
    public class LoadDB : BaseScript
    {
        public MySqlConnection conn = null;
        private string cs = @"server=localhost;userid=root;password=Overfl0w1@"; // TODO: put this into a local resource file

        public LoadDB()
        {
            EventHandlers["onResourceStart"] += new Action<string>(loadDatabase);
            EventHandlers["onResourceStop"] += new Action<string>(unloadDatabase);
        }

        private void loadDatabase(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
            Debug.WriteLine("Time to load db");

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                Debug.WriteLine("==========================");
                Debug.WriteLine("Sync DB Connection Successful");
                Debug.WriteLine("Mysql version: {0}", conn.ServerVersion);
                Debug.WriteLine("==========================");

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS SharpDB; Use SharpDB;";
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Players(playerid INT(10) NULL, position INT(10) NULL) COLLATE = 'latin1_swedish_ci' ENGINE = InnoDB;";
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine($"MySql Errror: {ex}");
            }
        }

        private void unloadDatabase(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Debug.WriteLine("Sync DB Connection Closed");
            conn.Close();
        }
    }
}
