using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak.EventDays
{
    internal class SniperDay : EventDay
    {
        public SniperDay()
        {
            StartEventDay();
        }
        public override void StartEventDay()
        {
            if (EventDay.isEventDay)
                return;

            base.StartEventDay();


            EventDay.dayType = "SniperDay";

            foreach (var player in Utilities.GetPlayers())
            {
                if (player.PlayerPawn.Value.TeamNum == 2)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 100;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.GiveNamedItem("weapon_knife");
                    player.GiveNamedItem("weapon_ssg08");

                    player.PlayerPawn.Value.GravityScale = 0.5f;

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;

                    Server.ExecuteCommand("css_freeze " + player.PlayerName);

                }
                if (player.PlayerPawn.Value.TeamNum == 3)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 150;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.GiveNamedItem("weapon_knife");
                    player.GiveNamedItem("weapon_awp");

                    player.PlayerPawn.Value.GravityScale = 0.5f;

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;

                }
            }
            JailBreak.jailbreak.AddTimer(30.0f, () =>
            {
                if (!EventDay.dayType.Equals("SniperDay")) return;
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