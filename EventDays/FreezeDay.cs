using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API.Modules.Entities;
using System.Numerics;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailBreak.EventDays
{
    internal class FreezeDay : EventDay
    {
        public FreezeDay()
        {
            StartEventDay();
        }
        public override void StartEventDay()
        {
            if (EventDay.isEventDay)
                return;

            base.StartEventDay();


            EventDay.dayType = "FreezeDay";
            Server.ExecuteCommand("mp_friendlyfire 1");

            foreach (var player in Utilities.GetPlayers())
            {

                if (player.PlayerPawn.Value.TeamNum == 2)
                {
                    

                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

                    player.PlayerPawn.Value.Health = 1000;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.PlayerPawn.Value.GravityScale = 0.5f;
                    player.PlayerPawn.Value.Speed = 2.0f;

                    player.GiveNamedItem("weapon_knife");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }

                if (player.PlayerPawn.Value.TeamNum == 3)
                {

                    player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();


                    player.PlayerPawn.Value.Health = 100;
                    player.PlayerPawn.Value.ArmorValue = 100;
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

                    player.PlayerPawn.Value.GravityScale = 0.5f;
                    player.PlayerPawn.Value.Speed = 2.0f;

                    player.GiveNamedItem("weapon_knife");

                    player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
                }
            }
            JailBreak.jailbreak.AddTimer(180.0f, () =>
            {
                if (!EventDay.dayType.Equals("FreezeDay")) return;

                string message = ChatColors.Green + "[EventDay] " + ChatColors.Default + "Vreme od 3 minuta je proslo, T pobedjuje!";
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

        public override void OnHit(CCSPlayerController attacker, CCSPlayerController victim)
        {
            if (Player.GetPlayerFromController(victim) == null || Player.GetPlayerFromController(attacker) == null) return;
            if (!EventDay.isEventDay) return;
            if (!EventDay.dayType.Equals("FreezeDay")) return;


            if(attacker.PlayerPawn.Value.TeamNum == 3 && victim.PlayerPawn.Value.TeamNum == 2)
            {

                Server.ExecuteCommand("css_freeze " + victim.PlayerName);
                Player.GetPlayerFromController(victim).isFroze = true;

                JailBreak.jailbreak.AddTimer(15.0f, () =>
                {
                    if(Player.GetPlayerFromController(victim).isFroze)
                    {
                        Server.ExecuteCommand("css_slay " + victim.PlayerName);
                    }    
                });
            }
            if(attacker.PlayerPawn.Value.TeamNum == 2 && victim.PlayerPawn.Value.TeamNum == 2)
            {
                if (Player.GetPlayerFromController(victim).isFroze)
                {
                    Server.ExecuteCommand("css_unfreeze " + victim.PlayerName);
                    Player.GetPlayerFromController(victim).isFroze = false;
                    if (JailBreak.jailbreak.Timers.Count > 0)
                    {
                        foreach (CounterStrikeSharp.API.Modules.Timers.Timer timer in JailBreak.jailbreak.Timers)
                        {
                            timer.Kill();
                        }
                        JailShop.IdleCredits();
                    }
                }
            }
        }

        public override void EndEventDay()
        {
            base.EndEventDay();
        }
    }
}