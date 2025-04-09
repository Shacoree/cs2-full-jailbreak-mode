using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Timers;
using System.Threading.Tasks;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.UserMessages;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Runtime.Intrinsics.X86;
using System.Numerics;
using static CounterStrikeSharp.API.Core.Listeners;
using CounterStrikeSharp.API.Modules.Menu;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Collections;
using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System;
using CounterStrikeSharp.API.Modules.Utils;
namespace JailBreak
{
    internal abstract class EventDay
    {
        public static bool isEventDay = false;
        public static string dayType = string.Empty;

        public EventDay()
        {
            StartEventDay();
        }
        public virtual void EndEventDay()
        {
            isEventDay = false;
            dayType = string.Empty;
            Server.ExecuteCommand("mp_friendlyfire 0");
            foreach (var player in Utilities.GetPlayers())
            {
                player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = false;
                
            }
        }
        public virtual void OnHit(CCSPlayerController attacker, CCSPlayerController victim)
        {

        }
        public virtual void StartEventDay() 
        {
            EventDay.isEventDay = true;
        }
    }
}
