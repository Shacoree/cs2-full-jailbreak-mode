using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak.EventDays
{
    internal class PredatorDay : EventDay
    {
        public PredatorDay()
        {
            StartEventDay();
        }
        public override void StartEventDay()
        {

            if (EventDay.isEventDay)
                return;

            base.StartEventDay();


            EventDay.dayType = "PredatorDay";

            foreach (var player in Utilities.GetPlayers())
            {
                if (player.PlayerPawn.Value.TeamNum == 2)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

                    player.PlayerPawn.Value.Health = 100;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.PlayerPawn.Value.GravityScale = 0.7f;
                    player.PlayerPawn.Value.Speed = 1.5f;

                    player.GiveNamedItem("weapon_knife");
                    player.GiveNamedItem("weapon_ak47");
                    player.GiveNamedItem("weapon_elite");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }

                if (player.PlayerPawn.Value.TeamNum == 3)
                {
                    Server.ExecuteCommand("css_freeze " + player.PlayerName);

                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 5000;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.PlayerPawn.Value.GravityScale = 0.5f;
                    player.PlayerPawn.Value.Speed = 2.0f;

                    player.GiveNamedItem("weapon_knife");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }
            }
            JailBreak.jailbreak.AddTimer(30.0f, () =>
            {
                if (!EventDay.dayType.Equals("PredatorDay")) return;

                foreach (var player in Utilities.GetPlayers())
                {
                    if (player.PlayerPawn.Value.TeamNum == 3)
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