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
    public class NoClipItem : JailItem
    {
        public static new readonly int costT = JailBreak.jailbreak.Config.ItemCost.costNoClipT;

        public NoClipItem(CCSPlayerController player, string name)
        {
            this.name = name;
            this.player = player;
            GiveItemToPlayer();
        }


        public override void GiveItemToPlayer()
        {
            Player.GetPlayerFromController(player).JailItems.Add(this);

            ulong steamid = player.SteamID;

            Task.Run(() => DataBase.Database.SubstractCredits(steamid, costT));

            Server.ExecuteCommand("css_noclip " + player.PlayerName);
            JailBreak.jailbreak.AddTimer(5.0f, () => 
            {
                Server.ExecuteCommand("css_noclip " + player.PlayerName);
            });
            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + player.PlayerName + " je kupio " + this.name.Remove(this.name.Length - 4);
            Server.PrintToChatAll($"\u200B{message}");
        }
    }
}
