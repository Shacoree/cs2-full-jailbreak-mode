using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak.EventDays
{
    internal class FreeDay : EventDay
    {
        public FreeDay() 
        {
            StartEventDay();
        }
        public override void StartEventDay()
        {
            if (EventDay.isEventDay)
                return;

            base.StartEventDay();

            
            EventDay.dayType = "FreeDay";

            foreach (var player in Utilities.GetPlayers()) 
            { 
                if(player.PlayerPawn.Value.TeamNum == 2) 
                {
                    Player.GiveFreeday(player);
                }
            }
            JailBreak.jailbreak.AddTimer(120.0f, () =>
            {
                if (!EventDay.dayType.Equals("FreeDay")) return;
                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "FreeDay svima je gotov, zatvorenici vratite se u kavez!";
                Server.PrintToChatAll($"\u200B{message}");
                foreach (var player in Utilities.GetPlayers())
                {
                    if (player.PlayerPawn.Value.TeamNum == 2 && Player.GetPlayerFromController(player).isFreeday)
                    {
                       Player.RemoveFreeday(player);
                    }
                }
            });
        }

        public override void EndEventDay()
        {
            base.EndEventDay();


        }
    }
}
