using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JailBreak.Config
{
    public class Config : BasePluginConfig
    {
        public class Config_Database
        {
            public string Host { get; set; } = string.Empty;
            public uint Port { get; set; } = 3306;
            public string User { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        public class Config_ShopCoins
        {
            public int minPlayers { get; set; } = 4;
            public bool toggleIdleCredits { get; set; } = true;
            public int amountIdleCredits { get; set; } = 3;
            public int everyXSeconds { get; set; } = 600;

            public bool toggleIdleCreditsVip { get; set; } = true;
            public int amountIdleCreditsVip { get; set; } = 6;

            public bool toggleTWinLR { get; set; } = true;
            public int amountTWinLR { get; set; } = 10;

            public bool toggleTWinLRVip { get; set; } = true;
            public int amountTWinLRVip { get; set; } = 15;
        }

        public class Config_ItemCost
        {
            public bool toggleFreeDayT { get; set; } = true;
            public int costFreeDayT { get; set; } = 1000;

            public bool toggleAllBombsT { get; set; } = true;
            public bool toggleAllBombsCT { get; set; } = true;
            public int costAllBombsT { get; set; } = 250;
            public int costAllBombsCT { get; set; } = 50;

            public bool toggleNoClipT { get; set; } = true;
            public int costNoClipT { get; set; } = 800;
            public bool vipOnlyNoClipT { get; set; } = true;

            public bool toggleGravityT { get; set; } = true;
            public bool toggleGravityCT { get; set; } = true;
            public int costGravityT { get; set; } = 500;
            public int costGravityCT { get; set; } = 250;

            public bool toggleSpeedT { get; set; } = true;
            public bool toggleSpeedCT { get; set; } = true;
            public int costSpeedT { get; set; } = 600;
            public int costSpeedCT { get; set; } = 450;

            public bool toggleGodCT { get; set; } = true;
            public int costGodCT { get; set; } = 400;

            public bool toggleHealthShotCT { get; set; } = true;
            public int costHealthShotCT { get; set; } = 100;

            public bool toggleHPCT { get; set; } = true;
            public int valueHPCT { get; set; } = 300;
            public int costHPCT { get; set; } = 350;
            public bool toggleHPT { get; set; } = false;
            public int valueHPT { get; set; } = 300;
            public int costHPT { get; set; } = 350;
        }

        [JsonPropertyName("Tag")] public string Tag { get; set; } = "{red}[CSS] ";
        
        [JsonPropertyName("Database")] public Config_Database Database { get; set; } = new Config_Database();

        [JsonPropertyName("ShopCoins")] public Config_ShopCoins ShopCoins { get; set; } = new Config_ShopCoins();

        [JsonPropertyName("ItemCost")] public Config_ItemCost ItemCost { get; set; } = new Config_ItemCost();
    }
}
