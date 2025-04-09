using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak.JailItems
{
    public class HealthShotItem : JailItem
    {
        public static readonly int costCT = JailBreak.jailbreak.Config.ItemCost.costHealthShotCT;

        public HealthShotItem(CCSPlayerController player, string name)
        {
            this.name = name;
            this.player = player;
            GiveItemToPlayer();
        }


        public override void GiveItemToPlayer()
        {
            Player.GetPlayerFromController(player).JailItems.Add(this);

            ulong steamid = player.SteamID;
            
            Task.Run(() => DataBase.Database.SubstractCredits(steamid, costCT));


            player.GiveNamedItem("weapon_healthshot");

            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + player.PlayerName + " je kupio " + this.name.Remove(this.name.Length - 4);
            Server.PrintToChatAll($"\u200B{message}");
        }
    }
}
