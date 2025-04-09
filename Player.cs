using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Core;
using System.IO;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Cvars;
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
using JailBreak.JailItems;

namespace JailBreak;

public class Player
{
    public CCSPlayerController controller;
    public bool isWarden;
    public bool isFreeday;
    public bool isRebel;
    public bool isBlueTeam;
    public bool isRedTeam;
    public bool isFroze;
    public List<JailItem> JailItems = new List<JailItem>();
    public static ArrayList Players = new ArrayList();
    public static ArrayList PlayersBlueTeam = new ArrayList();
    public static ArrayList PlayersRedTeam = new ArrayList();
    
    public Player(CCSPlayerController player) 
    {
        this.controller = player;
        this.isWarden = false;
        this.isFreeday = false;
        this.isRebel = false;
        this.isBlueTeam = false;
        this.isRedTeam = false;
        this.isFroze = false;
    }
    public static Player? GetPlayerFromController(CCSPlayerController player)
    {
        int index = -1;
        if (Players.Count > 0)
        {
            foreach (Player item in Players)
            {
                if (item.controller == player)
                {
                    index = Players.IndexOf(item);
                    break;
                }
            }
            if(index != -1)
                return (Player)Players[index];
        }

        return null;
    }
    public static bool playerHasFlag(CCSPlayerController? player, string flag)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return false;
        if (AdminManager.GetPlayerAdminData(player) == null) return false;
        if (AdminManager.GetPlayerAdminData(player).Flags["css"].Contains(flag))
            return true;
        return false;
    }
    public static bool IsPlayerRebel(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return false;
        if (GetPlayerFromController(player) == null) return false;
        if (GetPlayerFromController(player).isRebel == true)
            return true;

        return false;
    }
    public static bool IsPlayerWarden(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return false;
        if (GetPlayerFromController(player) == null) return false;
        if (GetPlayerFromController(player).isWarden == true)
            return true;

        return false;
    }
    public static bool IsPlayerFreeday(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return false;
        if (GetPlayerFromController(player) == null) return false;
        if (GetPlayerFromController(player).isFreeday == true)
            return true;

        return false;
    }
    public static bool IsPlayerBlueTeam(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return false;
        if (GetPlayerFromController(player) == null) return false;
        if (GetPlayerFromController(player).isBlueTeam == true)
            return true;

        return false;
    }
    public static bool IsPlayerRedTeam(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return false;
        if (GetPlayerFromController(player) == null) return false;
        if (GetPlayerFromController(player).isRedTeam == true)
            return true;

        return false;
    }
    public static bool IsPlayerAliveLegal(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return false;
        if (!player.PawnIsAlive || player.Connected != PlayerConnectedState.PlayerConnected || !player.PlayerPawn.IsValid || player.PlayerPawn.Value == null || !player.PlayerPawn.Value?.IsValid == true || player.PlayerPawn.Value?.LifeState != (byte)LifeState_t.LIFE_ALIVE) return false;

        return true;
    }
    public static void BecomeRebel(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (Player.GetPlayerFromController(player).isRebel)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac je vec rebel";
            Server.PrintToChatAll($"\u200B{message1}");
            return;
        }
        Player.GetPlayerFromController(player).isRebel = true;
        Player.GetPlayerFromController(player).isFreeday = false;


        player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 0, 0);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Igrac " + player.PlayerName + " je postao rebel";
        Server.PrintToChatAll($"\u200B{message}");

    }
    public static void GiveFreeday(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (Player.GetPlayerFromController(player).isFreeday)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac vec ima Freeday";
            player.PrintToChat($"\u200B{message1}");
            return;
        }
        Player.GetPlayerFromController(player).isFreeday = true;
        Player.GetPlayerFromController(player).isRebel = false;
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 255, 0);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");

        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Igrac " + player.PlayerName + " je sada freeday";
        Server.PrintToChatAll($"\u200B{message}");
    }
    public static void RemoveFreeday(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (!Player.GetPlayerFromController(player).isFreeday)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac nema Freeday";
            player.PrintToChat($"\u200B{message1}");
     
            return;
        }
        Player.GetPlayerFromController(player).isFreeday = false;
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");

        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Igrac " + player.PlayerName + " vise nije freeday";
        Server.PrintToChatAll($"\u200B{message}");
    }
    public static void RemoveRebel(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (!Player.GetPlayerFromController(player).isRebel)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac nije Rebel";
            player.PrintToChat($"\u200B{message1}");
            return;
        }
        Player.GetPlayerFromController(player).isRebel = false;
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");

        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Igrac " + player.PlayerName + " vise nije rebel";
        Server.PrintToChatAll($"\u200B{message}");
     
    }
    public static void BecomeBlueTeam(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (Player.GetPlayerFromController(player).isBlueTeam)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac je vec u plavom timu";
            player.PrintToChat($"\u200B{message1}");

            return;
        }
        Player.GetPlayerFromController(player).isRedTeam = false;
        Player.GetPlayerFromController(player).isBlueTeam = true;
        PlayersBlueTeam.Add(Player.GetPlayerFromController(player));
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 0, 255);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Igrac " + player.PlayerName + " je sada u plavom timu";
        Server.PrintToChatAll($"\u200B{message}");
    }
    public static void RemoveBlueTeam(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (!Player.GetPlayerFromController(player).isBlueTeam)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac nije u plavom timu";
            player.PrintToChat($"\u200B{message1}");
            return;
        }
        Player.GetPlayerFromController(player).isBlueTeam = false;
        PlayersBlueTeam.Remove(Player.GetPlayerFromController(player));
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
    }
    public static void BecomeRedTeam(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (Player.GetPlayerFromController(player).isRedTeam)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac je vec u crvenom timu";
            player.PrintToChat($"\u200B{message1}");
            return;
        }
        Player.GetPlayerFromController(player).isBlueTeam = false;
        Player.GetPlayerFromController(player).isRedTeam = true;
        PlayersRedTeam.Add(Player.GetPlayerFromController(player));
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 94, 0, 11);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Igrac " + player.PlayerName + " je sada u crvenom timu";
        Server.PrintToChatAll($"\u200B{message}");
    }
    public static void RemoveRedTeam(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return;
        if (GetPlayerFromController(player) == null) return;
        if (!Player.GetPlayerFromController(player).isRedTeam)
        {
            string message1 = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Taj igrac nije u crvenom timu";
            player.PrintToChat($"\u200B{message1}");
            return;
        }
        Player.GetPlayerFromController(player).isRedTeam = false;
        PlayersRedTeam.Remove(Player.GetPlayerFromController(player));
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
        /*Server.NextFrame(() =>
        { 
            player.PlayerPawn.Value.SetModel("characters\\models\\nozb1\\jail_police_player_model\\jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
    }
    public static void GiveWarden(CCSPlayerController player)
    {
        if (GetPlayerFromController(player) == null) return;
        Player.GetPlayerFromController(player).isWarden = true;
        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + player.PlayerName + " je sada Warden!";
        Server.PrintToChatAll($"\u200B{message}");

        player.Clan = "[Warden]";

        if (player.PlayerPawn.Value == null)
            return;
        player.PlayerPawn.Value.MaxHealth = 200;
        player.PlayerPawn.Value.Health = 200;
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 0, 255);
        /*Server.NextFrame(() =>
        {
            player.PlayerPawn.Value.SetModel("models/characters/models/nozb1/jail_police_player_model/jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
    }
    public static void RemoveWarden(CCSPlayerController player)
    {
        if (GetPlayerFromController(player) == null) return;
        Player.GetPlayerFromController(player).isWarden = false;
        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + player.PlayerName + " vise nije Warden!";
        Server.PrintToChatAll($"\u200B{message}");

        player.Clan = "";

        if (player.PlayerPawn.Value == null)
            return;
        player.PlayerPawn.Value.MaxHealth = 100;
        player.PlayerPawn.Value.Health = 100;
        player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
        /*Server.NextFrame(() =>
        {
            player.PlayerPawn.Value.SetModel("models/characters/models/nozb1/jail_police_player_model/jail_police_player_model.vmdl");
        });*/
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
    }
}
