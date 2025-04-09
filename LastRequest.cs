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
namespace JailBreak;

internal class LastRequest
{
    public static bool isLastRequestOnDeathEvent(CCSPlayerController player)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null || player.PlayerPawn.Value == null) return false;

        if (player.PlayerPawn.Value.TeamNum != 2) return false;

        if (EventDay.isEventDay) return false;

        int tCount = 0;
        CCSPlayerController? LR = null;
        foreach (var temp in Utilities.GetPlayers())
        {
            if (temp == null || !temp.IsValid || temp.AuthorizedSteamID == null || temp.PlayerPawn.Value == null) continue;

            if (temp.PlayerPawn.Value.TeamNum != 2) continue;

            if (temp == player) continue;

            if (temp.PawnIsAlive)
            {
                tCount++;
                LR = temp;
            }
        }
        if (tCount != 1) return false;

        if (JailBreak.isLr) return false;

        JailBreak.isLr = true;
        JailBreak.LRPlayer = LR;
        string message = ChatColors.Green + "[LastRequest] " + ChatColors.Default + "Last request aktiviran, ukucaj !lr";
        Server.PrintToChatAll($"\u200B{message}");
        return true;
    }
    public static bool PlayerHasLR(CCSPlayerController player)
    {
        if (!JailBreak.isLr || JailBreak.LRPlayer == null) return false;

        if (player.PawnIsAlive && player == JailBreak.LRPlayer)
        {
            return true;
        }
        else
            return false;

    }
    public static void StartLRS4S(CCSPlayerController? t, CCSPlayerController? ct)
    {
        JailBreak.ActiveLRS4S = true;

        if (t.PlayerPawn.Value == null || t.PlayerPawn.Value.WeaponServices == null || ct.PlayerPawn.Value == null || ct.PlayerPawn.Value.WeaponServices == null) return;


        ct.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();
        t.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

        Random random = new Random();

        ct.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 0, 125);
        t.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 0, 0);

        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");


        t.PlayerPawn.Value.Health = 100;
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
        ct.PlayerPawn.Value.Health = 100;
        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

        t.GiveNamedItem("weapon_deagle");
        ct.GiveNamedItem("weapon_deagle");



        t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 0;
        ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 0;

        if (random.Next(0, 2) == 1)
        {
            t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 1;
            ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 0;
            string message = ChatColors.Green + "[LastRequest] " + ChatColors.Default + " Shot4Shot(" + t.PlayerName + " vs " + ct.PlayerName + ") " + "- Igrac " + t.PlayerName + " je random izabran da puca prvi!";
            Server.PrintToChatAll($"\u200B{message}");
        }
        else
        {
            string message = ChatColors.Green + "[LastRequest] " + ChatColors.Default + " Shot4Shot(" + t.PlayerName + " vs " + ct.PlayerName + ") " + "- Igrac " + ct.PlayerName + " je random izabran da puca prvi!";
            Server.PrintToChatAll($"\u200B{message}");
            t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 0;
            ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 1;
        }

        Utilities.SetStateChanged(t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_iClip1");
        Utilities.SetStateChanged(t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");
        Utilities.SetStateChanged(ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_iClip1");
        Utilities.SetStateChanged(ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");



        t.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
        ct.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;


        return;
    }
    
    public static void BulletCycleLRS4S(CCSPlayerController? player)
    {
        if (!JailBreak.ActiveLRS4S) return;

        if (player!= JailBreak.LRPlayer && player != JailBreak.LRPlayerCT) return;
        

        if(player == JailBreak.LRPlayerCT)
        { 
            JailBreak.LRPlayer.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 1;
            Utilities.SetStateChanged(JailBreak.LRPlayer.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");
        }
        else
        {
            JailBreak.LRPlayerCT.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 1;
            Utilities.SetStateChanged(JailBreak.LRPlayerCT.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");
        }
        
        return;
    }
    public static void StartLRKnife(CCSPlayerController? t, CCSPlayerController? ct)
    {
        JailBreak.ActiveLRKnife = true;

        if (t.PlayerPawn.Value == null || t.PlayerPawn.Value.WeaponServices == null || ct.PlayerPawn.Value == null || ct.PlayerPawn.Value.WeaponServices == null) return;


        ct.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();
        t.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

        ct.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 0, 125);
        t.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 0, 0);

        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");


        t.PlayerPawn.Value.Health = 100;
        t.PlayerPawn.Value.ArmorValue = 0;
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
        ct.PlayerPawn.Value.ArmorValue = 0;
        ct.PlayerPawn.Value.Health = 100;
        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

        t.GiveNamedItem("weapon_knife");
        ct.GiveNamedItem("weapon_knife");


        t.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
        ct.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;


        return;
    }
    public static void StartLRNoScope(CCSPlayerController? t, CCSPlayerController? ct)
    {
        
        JailBreak.ActiveLRNoScope = true;

        if (t.PlayerPawn.Value == null || t.PlayerPawn.Value.WeaponServices == null || ct.PlayerPawn.Value == null || ct.PlayerPawn.Value.WeaponServices == null) return;


        ct.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();
        t.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

        ct.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 0, 125);
        t.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 0, 0);

        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");


        t.PlayerPawn.Value.Health = 100;
        t.PlayerPawn.Value.ArmorValue = 100;
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
        ct.PlayerPawn.Value.ArmorValue = 100;
        ct.PlayerPawn.Value.Health = 100;
        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

        t.GiveNamedItem("weapon_awp");
        ct.GiveNamedItem("weapon_awp");

        t.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
        ct.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;


        return;
    }
    public static void LRNoScopeAtScope(CCSPlayerController player)
    {
        if (!JailBreak.ActiveLRNoScope) return;

        if (player != JailBreak.LRPlayer && player != JailBreak.LRPlayerCT) return;

        player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = false;

        player.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();
        player.GiveNamedItem("weapon_awp");
        player.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 0;
        Utilities.SetStateChanged(player.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_iClip1");

        player.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
        return;
    }

    public static void StartLRGunToss(CCSPlayerController? t, CCSPlayerController? ct)
    {
        
        JailBreak.ActiveLRGunToss = true;

        if (t.PlayerPawn.Value == null || t.PlayerPawn.Value.WeaponServices == null || ct.PlayerPawn.Value == null || ct.PlayerPawn.Value.WeaponServices == null) return;


        ct.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();
        t.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

        ct.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 0, 125);
        t.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 0, 0);

        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");


        t.PlayerPawn.Value.Health = 100;
        t.PlayerPawn.Value.ArmorValue = 100;
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
        ct.PlayerPawn.Value.ArmorValue = 100;
        ct.PlayerPawn.Value.Health = 100;
        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

        t.GiveNamedItem("weapon_deagle");
        t.GiveNamedItem("weapon_knife");
        ct.GiveNamedItem("weapon_deagle");
        ct.GiveNamedItem("weapon_knife");

        t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 0;
        ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 0;

        
        t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 0;
        ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 0;

        Utilities.SetStateChanged(t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_iClip1");
        Utilities.SetStateChanged(t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");
        Utilities.SetStateChanged(ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_iClip1");
        Utilities.SetStateChanged(ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");

        return;
    }
    public static void StartLRRebel(CCSPlayerController t)
    {
        if (t.PlayerPawn.Value == null || t.PlayerPawn.Value.WeaponServices == null) return;
        
        JailBreak.ActiveLRRebel = true;

        t.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

        t.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 0, 0);
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");

        t.GiveNamedItem("weapon_ak47");
        t.PlayerPawn.Value.Health = 150;
        t.PlayerPawn.Value.ArmorValue = 100;
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

    }
    public static void StartLRRussianRoulette(CCSPlayerController? t, CCSPlayerController? ct)
    {

        JailBreak.ActiveLRRussianRoulette = true;

        if (t.PlayerPawn.Value == null || t.PlayerPawn.Value.WeaponServices == null || ct.PlayerPawn.Value == null || ct.PlayerPawn.Value.WeaponServices == null) return;


        ct.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();
        t.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

        ct.PlayerPawn.Value.Render = Color.FromArgb(255, 0, 0, 125);
        t.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 0, 0);

        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");


        t.PlayerPawn.Value.Health = 100;
        t.PlayerPawn.Value.ArmorValue = 100;
        Utilities.SetStateChanged(t.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
        ct.PlayerPawn.Value.ArmorValue = 100;
        ct.PlayerPawn.Value.Health = 100;
        Utilities.SetStateChanged(ct.PlayerPawn.Value, "CBaseEntity", "m_iHealth");

        t.GiveNamedItem("weapon_deagle");
        ct.GiveNamedItem("weapon_deagle");

        t.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;
        ct.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = true;

        t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 0;
        ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 0;


        t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 1;
        ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.Clip1 = 0;

        Utilities.SetStateChanged(t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_iClip1");
        Utilities.SetStateChanged(t.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");
        Utilities.SetStateChanged(ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_iClip1");
        Utilities.SetStateChanged(ct.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");

        QAngle tNewAngle = new QAngle(t.PlayerPawn.Value.EyeAngles.X, 90.0f, t.PlayerPawn.Value.EyeAngles.Z);
        QAngle ctNewAngle = new QAngle(t.PlayerPawn.Value.EyeAngles.X, t.PlayerPawn.Value.EyeAngles.Y * -1, t.PlayerPawn.Value.EyeAngles.Z);
        CounterStrikeSharp.API.Modules.Utils.Vector ctNewPos = new CounterStrikeSharp.API.Modules.Utils.Vector(t.PlayerPawn.Value.AbsOrigin.X, t.PlayerPawn.Value.AbsOrigin.Y + 70, t.PlayerPawn.Value.AbsOrigin.Z);

        Server.ExecuteCommand("css_god " + t.PlayerName + " 1");
        Server.ExecuteCommand("css_god " + ct.PlayerName + " 1");

        t.PlayerPawn.Value.Teleport(t.PlayerPawn.Value.AbsOrigin, tNewAngle);
        ct.PlayerPawn.Value.Teleport(ctNewPos, ctNewAngle);

        Server.ExecuteCommand("css_freeze " + t.PlayerName);
        Server.ExecuteCommand("css_freeze " + ct.PlayerName);

        return;
    }
    public static void OnShotLRRussianRoulette(CCSPlayerController player)
    {
        if(!JailBreak.ActiveLRRussianRoulette) return;

        if (player != JailBreak.LRPlayer && player != JailBreak.LRPlayerCT) return;



        Random rnd = new Random();
        int number = rnd.Next(0, 7);
        // Bang!
        if (number == 6)
        {
            
            Server.ExecuteCommand("css_unfreeze " + JailBreak.LRPlayer.PlayerName);
            Server.ExecuteCommand("css_unfreeze " + JailBreak.LRPlayerCT.PlayerName);
            if (player == JailBreak.LRPlayer)
            {
                Server.ExecuteCommand("css_slay " + JailBreak.LRPlayerCT.PlayerName);
                string message = ChatColors.Green + "[LastRequest] " + ChatColors.Default + JailBreak.LRPlayerCT.PlayerName + " glava je raznesena";
                Server.PrintToChatAll($"\u200B{message}");
            }
            else
            {
                Server.ExecuteCommand("css_slay " + JailBreak.LRPlayer.PlayerName);
                string message = ChatColors.Green + "[LastRequest] " + ChatColors.Default + JailBreak.LRPlayer.PlayerName + " glava je raznesena";
                Server.PrintToChatAll($"\u200B{message}");
            }
        }
        else
        { 
            if (player == JailBreak.LRPlayerCT)
            {
                JailBreak.LRPlayer.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 1;
                Utilities.SetStateChanged(JailBreak.LRPlayer.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");
            }
            else
            {
                JailBreak.LRPlayerCT.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value.ReserveAmmo[0] = 1;
                Utilities.SetStateChanged(JailBreak.LRPlayerCT.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value, "CBasePlayerWeapon", "m_pReserveAmmo");
            }
            player.PrintToChat("Click!");
        }
    }
    public static void EndLR(CCSPlayerController? player)
    {
        if(JailBreak.ActiveLRRebel) 
        {
            if (player != JailBreak.LRPlayer) return;

            JailBreak.LRPlayer.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
            Utilities.SetStateChanged(JailBreak.LRPlayer.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
            JailBreak.ActiveLRRebel = false;
        }

        if (!JailBreak.ActiveLRS4S && !JailBreak.ActiveLRKnife && !JailBreak.ActiveLRNoScope && !JailBreak.ActiveLRGunToss && !JailBreak.ActiveLRRussianRoulette) return;

        if (JailBreak.ActiveLRRussianRoulette) 
        {
            Server.ExecuteCommand("css_god " + JailBreak.LRPlayerCT.PlayerName + " 0");
            Server.ExecuteCommand("css_god " + JailBreak.LRPlayer.PlayerName + " 0");
        }

        if (player != JailBreak.LRPlayer && player != JailBreak.LRPlayerCT) return;

        JailBreak.ActiveLRS4S = false;
        JailBreak.ActiveLRKnife = false;
        JailBreak.ActiveLRNoScope = false;
        JailBreak.ActiveLRGunToss = false;
        JailBreak.ActiveLRRussianRoulette = false;
        JailBreak.LRPlayer.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = false;
        JailBreak.LRPlayerCT.PlayerPawn.Value.WeaponServices.PreventWeaponPickup = false;



        JailBreak.LRPlayerCT.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
        JailBreak.LRPlayer.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);

        Utilities.SetStateChanged(JailBreak.LRPlayer.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        Utilities.SetStateChanged(JailBreak.LRPlayerCT.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");

        if (player == JailBreak.LRPlayer)
        {
            string message = ChatColors.Green + "[LastRequest] " + ChatColors.Default + "Pobednik LR-a je : " + JailBreak.LRPlayerCT.PlayerName;
            Server.PrintToChatAll($"\u200B{message}");
        }
        else
        {
            string message = ChatColors.Green + "[LastRequest] " + ChatColors.Default + "Pobednik LR-a je : " + JailBreak.LRPlayerCT.PlayerName;
            Server.PrintToChatAll($"\u200B{message}");

            if (Utilities.GetPlayers().Count >= JailBreak.jailbreak.Config.ShopCoins.minPlayers)
            {
                if (JailBreak.jailbreak.Config.ShopCoins.toggleTWinLR)
                {
                    if (Player.playerHasFlag(JailBreak.LRPlayer, "@css/reservation") && JailBreak.jailbreak.Config.ShopCoins.toggleTWinLRVip)
                    {
                        ulong steamid = JailBreak.LRPlayer.SteamID;
                        int amount = JailBreak.jailbreak.Config.ShopCoins.amountTWinLRVip;
                        Task.Run(() => DataBase.Database.AddCredits(steamid, amount));
                        message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Pobedio si LR i dobio si " + amount + " kredita";
                        player.PrintToChat($"\u200B{message}");
                    }
                    else
                    {
                        ulong steamid = JailBreak.LRPlayer.SteamID;
                        int amount = JailBreak.jailbreak.Config.ShopCoins.amountTWinLR;
                        Task.Run(() => DataBase.Database.AddCredits(steamid, amount));
                        message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Pobedio si LR i dobio si " + amount + " kredita";
                        player.PrintToChat($"\u200B{message}");
                    }
                }
            }
        }
        return;
    }
}
