using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Net.NetworkInformation;
using CounterStrikeSharp.API;

namespace JailBreak.DataBase
{
    internal class Database
    {
        private static string DatabaseConnectionString { get; set; } = string.Empty;

        private static async Task<MySqlConnection> ConnectAsync()
        {
            MySqlConnection connection = new(DatabaseConnectionString);
            await connection.OpenAsync();
            return connection;
        }
        
        public static async Task CreateDatabaseAsync(Config.Config config)
        {
            if (string.IsNullOrEmpty(config.Database.Host) || string.IsNullOrEmpty(config.Database.Name) || string.IsNullOrEmpty(config.Database.User))
            {
                throw new Exception("You need to setup Database credentials in config.");
            }

            MySqlConnectionStringBuilder builder = new()
            {
                Server = config.Database.Host,
                Database = config.Database.Name,
                UserID = config.Database.User,
                Password = config.Database.Password,
                Port = config.Database.Port,
                Pooling = true,
                MinimumPoolSize = 0,
                MaximumPoolSize = 600,
                ConnectionIdleTimeout = 30,
                AllowZeroDateTime = true
            };

            DatabaseConnectionString = builder.ConnectionString;

            await using DbConnection connection = await ConnectAsync();
            await connection.ExecuteAsync(CreateShopTableSql);
        }
        
        public static async Task SetupPlayer(ulong steamid, string playername, int credits)
        {
            
            if (!IsPlayerSetup(steamid))
            {
                await using MySqlConnection connection = await ConnectAsync();

                await connection.ExecuteAsync(InsertShopTableSql, new
                {
                    steamid,
                    playername,
                    credits
                });
            }
            
        }
        public static bool IsPlayerSetup(ulong steamid)
        {
            return IsPlayerSetupAsync(steamid).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static async Task<bool> IsPlayerSetupAsync(ulong steamid)
        {
            await using DbConnection connection = await ConnectAsync().ConfigureAwait(false);

            bool exists = await connection.ExecuteScalarAsync<bool>(CheckPlayerExistsSql, new { steamid }).ConfigureAwait(false);
            return exists;
        }
        public static int GetPlayerCredits(ulong steamid)
        {
            return GetPlayerCreditsAsync(steamid).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public static async Task<int> GetPlayerCreditsAsync(ulong steamid)
        {
            await using DbConnection connection = await ConnectAsync().ConfigureAwait(false);

            int credits = await connection.ExecuteScalarAsync<int>(CheckPlayerCreditsSql, new { steamid }).ConfigureAwait(false);
            return credits;
        }
        public static async Task AddCredits(ulong steamid, int amount)
        {
            await using DbConnection connection = await ConnectAsync();
            await connection.ExecuteAsync(AddPlayerCreditsSQL, new { amount, steamid });
        }
        public static async Task SubstractCredits(ulong steamid, int amount)
        {
            await using DbConnection connection = await ConnectAsync();
            await connection.ExecuteAsync(SubstractPlayerCreditsSQL, new { amount, steamid });
        }

        private const string CreateShopTableSql = @"
            CREATE TABLE IF NOT EXISTS jailshop (
                id INT AUTO_INCREMENT PRIMARY KEY,
                steamid BIGINT UNSIGNED NOT NULL,
                playername VARCHAR(128) NOT NULL,
                credits INT NOT NULL
            );
        ";

        private const string InsertShopTableSql = @"
            INSERT INTO jailshop (steamid, playername, credits) 
            VALUES (@steamid, @playername, @credits);
        ";

        private const string CheckPlayerExistsSql = @"
            SELECT EXISTS(SELECT 1 FROM jailshop WHERE steamid = @steamid LIMIT 1);
        ";

        private const string AddPlayerCreditsSQL = @"
            UPDATE jailshop
            SET credits = credits + @amount
            WHERE steamid = @steamid;
        ";

        private const string SubstractPlayerCreditsSQL = @"
            UPDATE jailshop
            SET credits = credits - @amount
            WHERE steamid = @steamid;
        ";
        private const string CheckPlayerCreditsSql = @"
            SELECT credits FROM jailshop WHERE steamid = @steamid LIMIT 1;
        ";
    }
}
