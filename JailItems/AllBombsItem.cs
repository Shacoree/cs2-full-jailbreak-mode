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
    public class AllBombsItem : JailItem
    {
        public static readonly int costT = JailBreak.jailbreak.Config.ItemCost.costAllBombsT;
        public static readonly int costCT = JailBreak.jailbreak.Config.ItemCost.costAllBombsCT;

        public AllBombsItem(CCSPlayerController player, string name)
        {
            this.name = name;
            this.player = player;
            GiveItemToPlayer();
        }


        public override void GiveItemToPlayer()
        {
            Player.GetPlayerFromController(player).JailItems.Add(this);

            ulong steamid = player.SteamID;
            if(player.PlayerPawn.Value.TeamNum == 2)
                Task.Run(() => DataBase.Database.SubstractCredits(steamid, costT));

            if (player.PlayerPawn.Value.TeamNum == 3)
                Task.Run(() => DataBase.Database.SubstractCredits(steamid, costCT));

            player.GiveNamedItem("weapon_hegrenade");
            player.GiveNamedItem("weapon_molotov");
            player.GiveNamedItem("weapon_smokegrenade");
            player.GiveNamedItem("weapon_flashbang");

            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + player.PlayerName + " je kupio " + this.name.Remove(this.name.Length - 4);
            Server.PrintToChatAll($"\u200B{message}");
        }
    }
}
