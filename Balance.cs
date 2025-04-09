using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak
{
    internal class Balance
    {
        public static bool JoinTeam(CCSPlayerController player, int team)
        {

            if (team == 3)
            {
                if(TCount() !=0 && CtCount() != 0 && (CtCount() * 3) > TCount())
                {
                    string message = ChatColors.Green + "[Jailbreak] " + ChatColors.Default + "Ct ima previse igraca, ratio je 3:1";
                    player.PrintToChat($"\u200B{message}");
                    return false;
                }
            }
            return true;
        }
        public static int CtCount()
        {
            int count = 0;
            if (Utilities.GetPlayers().Count > 0) 
            { 
                foreach (var item in Utilities.GetPlayers())
                {
                    if (item == null && !item.IsValid) continue;

                    if (item.TeamNum == 3)
                        count++;
                }
            }
            return count;
        }
        public static int TCount()
        {
            int count = 0;
            if (Utilities.GetPlayers().Count > 0)
            {
                foreach (var item in Utilities.GetPlayers())
                {
                    if (item == null && !item.IsValid) continue;

                    if (item.TeamNum == 2)
                        count++;
                }
            }
            return count;
        }
    }
}
