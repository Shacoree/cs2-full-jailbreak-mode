using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak.EventDays
{
    internal class KnifeDay : EventDay
    {
        public KnifeDay()
        {
            StartEventDay();
        }
        public override void StartEventDay()
        {
            if (EventDay.isEventDay)
                return;

            base.StartEventDay();


            EventDay.dayType = "KnifeDay";

            foreach (var player in Utilities.GetPlayers())
            {
                if (player.PlayerPawn.Value.TeamNum == 2)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 60;
                    player.PlayerPawn.Value.ArmorValue = 0;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.GiveNamedItem("weapon_knife");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }

                if (player.PlayerPawn.Value.TeamNum == 3)
                {
                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 100;
                    player.PlayerPawn.Value.ArmorValue = 0;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.GiveNamedItem("weapon_knife");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }
            }
        }



        public override void EndEventDay()
        {
            base.EndEventDay();


        }
    }
}

