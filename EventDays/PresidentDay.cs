using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak.EventDays
{
    internal class PresidentDay : EventDay
    {
        public PresidentDay()
        {
            StartEventDay();
        }
        public override void StartEventDay()
        {
            if (EventDay.isEventDay)
                return;

            base.StartEventDay();


            EventDay.dayType = "PresidentDay";

            foreach (var player in Utilities.GetPlayers())
            {
                if (player.PlayerPawn.Value.TeamNum == 2)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 100;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.GiveNamedItem("weapon_knife");
                    player.GiveNamedItem("weapon_ak47");
                    player.GiveNamedItem("weapon_deagle");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;

                    Server.ExecuteCommand("css_freeze " + player.PlayerName);

                }
                if (player.PlayerPawn.Value.TeamNum == 3)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 200;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.GiveNamedItem("weapon_knife");
                    player.GiveNamedItem("weapon_m4a1");
                    player.GiveNamedItem("weapon_deagle");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;

                }
            }
            JailBreak.jailbreak.AddTimer(30.0f, () => 
            {
                if (!EventDay.dayType.Equals("PresidentDay")) return;
                foreach (var player in Utilities.GetPlayers())
                {
                    if (player.PlayerPawn.Value.TeamNum == 2)
                    {
                        Server.ExecuteCommand("css_unfreeze " + player.PlayerName);
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
