using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API.Modules.Utils;
namespace JailBreak.EventDays
{
    internal class HideDay : EventDay
    {
        public HideDay()
        {
            StartEventDay();
        }
        public override void StartEventDay()
        {
            if (EventDay.isEventDay)
                return;

            base.StartEventDay();


            EventDay.dayType = "HideDay";

            foreach (var player in Utilities.GetPlayers())
            {
                if (player.PlayerPawn.Value.TeamNum == 2)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

                    player.PlayerPawn.Value.Health = 100;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.GiveNamedItem("weapon_knife");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }

                if (player.PlayerPawn.Value.TeamNum == 3)
                {
                    Server.ExecuteCommand("css_freeze " + player.PlayerName);

                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 100;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");


                    player.GiveNamedItem("weapon_knife");
                    player.GiveNamedItem("weapon_m4a1");
                    player.GiveNamedItem("weapon_usp_silencer");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }
            }
            JailBreak.jailbreak.AddTimer(120.0f, () =>
            {
                if (!EventDay.dayType.Equals("HideDay")) return;

                foreach (var player in Utilities.GetPlayers())
                {
                    if (player.PlayerPawn.Value.TeamNum == 3)
                    {
                        Server.ExecuteCommand("css_unfreeze " + player.PlayerName);
                    }
                    if (player.PlayerPawn.Value.TeamNum == 2)
                    {
                        Server.ExecuteCommand("css_freeze " + player.PlayerName);
                    }
                }
            });
            JailBreak.jailbreak.AddTimer(420.0f, () =>
            {
                if (!EventDay.dayType.Equals("HideDay")) return;

                string message = ChatColors.Green + "[EventDay] " + ChatColors.Default + "Vreme od 5 minuta je proslo, T pobedjuje!";
                Server.PrintToChatAll($"\u200B{message}");
 
                foreach (var player in Utilities.GetPlayers())
                {
                    if (player.PlayerPawn.Value.TeamNum == 3)
                    {
                        Server.ExecuteCommand("css_slay " + player.PlayerName);
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