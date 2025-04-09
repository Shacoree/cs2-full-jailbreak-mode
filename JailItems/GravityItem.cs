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
    public class GravityItem : JailItem
    {
        public static new readonly int costT = JailBreak.jailbreak.Config.ItemCost.costGravityT;
        public static new readonly int costCT = JailBreak.jailbreak.Config.ItemCost.costGravityCT;

        public GravityItem(CCSPlayerController player, string name)
        {
            this.name = name;
            this.player = player;
            GiveItemToPlayer();
        }


        public override void GiveItemToPlayer()
        {
            Player.GetPlayerFromController(player).JailItems.Add(this);

            ulong steamid = player.SteamID;

            if (player.PlayerPawn.Value.TeamNum == 2)
                Task.Run(() => DataBase.Database.SubstractCredits(steamid, costT));

            if (player.PlayerPawn.Value.TeamNum == 3)
                Task.Run(() => DataBase.Database.SubstractCredits(steamid, costCT));


            player.PlayerPawn.Value.GravityScale = 0.5f;

            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + player.PlayerName + " je kupio " + this.name.Remove(this.name.Length - 4);
            Server.PrintToChatAll($"\u200B{message}");
        }
    }
}
