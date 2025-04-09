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
using System.Drawing;
using CounterStrikeSharp.API.Core.Capabilities;
using Dapper;
using MenuManager;
using System;
using CounterStrikeSharp.API.Core.Plugin.Host;
using McMaster.NETCore.Plugins;
using System.Net.Http.Headers;
using CounterStrikeSharp.API.Core.Translations;
using JailBreak.DataBase;
using JailBreak.JailItems;
namespace JailBreak;

public partial class JailBreak : BasePlugin, IPluginConfig<Config.Config>
{
    public static JailBreak? jailbreak;
    public static CounterStrikeSharp.API.Modules.Timers.Timer? LrTimer;
    public override string ModuleName => "Jailbreak Plugin";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Shacore";
    public override string ModuleDescription => "Full Jailbreak Mod";
    private const string MyLink = "https://github.com/Shacoree";
    public static IMenuApi? _api;
    public static bool ActiveLRS4S = false;
    public static bool ActiveLRKnife = false;
    public static bool ActiveLRNoScope = false;
    public static bool ActiveLRGunToss = false;
    public static bool ActiveLRRebel = false;
    public static bool ActiveLRRussianRoulette = false;
    public static PluginCapability<IMenuApi> _pluginCapability = new("menu:nfcore");
    public static bool isLr;
    public static CCSPlayerController? LRPlayer;
    public static CCSPlayerController? LRPlayerCT;
    public static bool freezetimepassed = false;

    public Config.Config Config { get; set; } = new Config.Config();

    public static readonly string[] WeaponsList =
    {
        "weapon_ak47", "weapon_aug", "weapon_awp", "weapon_bizon", "weapon_cz75a", "weapon_deagle", "weapon_elite", "weapon_famas", "weapon_fiveseven", "weapon_g3sg1", "weapon_galilar",
        "weapon_glock", "weapon_hkp2000", "weapon_m249", "weapon_m4a1", "weapon_m4a1_silencer", "weapon_mac10", "weapon_mag7", "weapon_mp5sd", "weapon_mp7", "weapon_mp9", "weapon_negev",
        "weapon_nova", "weapon_p250", "weapon_p90", "weapon_revolver", "weapon_sawedoff", "weapon_scar20", "weapon_sg556", "weapon_ssg08", "weapon_tec9", "weapon_ump45", "weapon_usp_silencer", "weapon_xm1014",
        "weapon_decoy", "weapon_flashbang", "weapon_hegrenade", "weapon_incgrenade", "weapon_molotov", "weapon_smokegrenade","item_defuser", "item_cutters"
    };


    public override void Load(bool hotReload)
    {
        jailbreak = this;
        AddTimer(2.0f, () =>
        {
            isLr = false;
            _api = _pluginCapability.Get();
            if (_api == null)
            {
                Console.WriteLine("MenuManager Core not found...");
            }
            PrintInfo();
            onWardenStart();
            RegisterListenersJailbreak();
            RegisterListenersWarden();
            RegisterCommandsWarden();
            Server.ExecuteCommand("mp_ct_default_melee weapon_knife");
            Server.ExecuteCommand("mp_t_default_melee weapon_knife");
            JailShop.IdleCredits();

            AddCommand("css_lr", "Lr Menu", (player, info) =>
            {
            });
        });
    }
    public async void OnConfigParsed(Config.Config config)
    {
        await Database.CreateDatabaseAsync(config);
        config.Tag = StringExtensions.ReplaceColorTags(config.Tag);
        Config = config;
    }
    [ConsoleCommand("css_shop")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_SHOP(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) return;
        if (_api == null) return;

        JailShop.OpenShop(player);
    }
    [ConsoleCommand("css_lr")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_LR(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) return;
        if (_api == null) return;

        if (ActiveLRS4S && ActiveLRKnife && ActiveLRNoScope) return;

        if (LastRequest.PlayerHasLR(player))
        {
            var LRmenu = _api.NewMenuForcetype("Last request", MenuType.ButtonMenu);

            LRmenu.AddMenuOption("Shot4Shot", (player, action) =>
            {
                var chooseS4Smenu = _api.NewMenuForcetype("Izaberi Igraca", MenuType.ButtonMenu);
                chooseS4Smenu.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (item == null || !item.IsValid || item.AuthorizedSteamID == null)
                        continue;
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 3)
                        continue;
                    chooseS4Smenu.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        LRPlayerCT = item;
                        LastRequest.StartLRS4S(player, item);
                    });
                }
                chooseS4Smenu.Open(player);
            });
            LRmenu.AddMenuOption("No Scope", (player, action) =>
            {
                var chooseNoScopemenu = _api.NewMenuForcetype("Izaberi Igraca", MenuType.ButtonMenu);
                chooseNoScopemenu.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 3)
                        continue;
                    chooseNoScopemenu.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        LRPlayerCT = item;
                        LastRequest.StartLRNoScope(player, item);
                    });
                }
                chooseNoScopemenu.Open(player);
            });
            LRmenu.AddMenuOption("Knife Fight", (player, action) =>
            {
                var chooseKnifemenu = _api.NewMenuForcetype("Izaberi Igraca", MenuType.ButtonMenu);
                chooseKnifemenu.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 3)
                        continue;
                    chooseKnifemenu.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        LRPlayerCT = item;
                        LastRequest.StartLRKnife(player, item);
                    });
                }
                chooseKnifemenu.Open(player);
            });
            LRmenu.AddMenuOption("Ruski Rulet", (player, action) =>
            {
                var chooseRuletmenu = _api.NewMenuForcetype("Izaberi Igraca", MenuType.ButtonMenu);
                chooseRuletmenu.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 3)
                        continue;
                    chooseRuletmenu.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        LRPlayerCT = item;
                        LastRequest.StartLRRussianRoulette(player, item);
                    });
                }
                chooseRuletmenu.Open(player);
            });
            LRmenu.AddMenuOption("Gun Toss", (player, action) =>
            {
                var chooseGunTossmenu = _api.NewMenuForcetype("Izaberi Igraca", MenuType.ButtonMenu);
                chooseGunTossmenu.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 3)
                        continue;
                    chooseGunTossmenu.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        LRPlayerCT = item;
                        LastRequest.StartLRGunToss(player, item);
                    });
                }
                chooseGunTossmenu.Open(player);
            });
            LRmenu.AddMenuOption("Rebel", (player, action) =>
            {
                LastRequest.StartLRRebel(player);
                _api.CloseMenu(player);
            });

            LRmenu.Open(player);
        }
    }
    [GameEventHandler]
    public HookResult OnEventPlayerHurt(EventPlayerHurt @event, GameEventInfo info)
    {
        if (@event.Attacker == null || !@event.Attacker.IsValid || @event.Attacker.AuthorizedSteamID == null || @event.Userid == null || !@event.Userid.IsValid || @event.Userid.AuthorizedSteamID == null) return HookResult.Continue;

        if (@event.Attacker.TeamNum == 2 && @event.Userid.TeamNum == 3)
        {
            if (!Player.IsPlayerRebel(@event.Attacker))
                Player.BecomeRebel(@event.Attacker);
        }

        if (EventDay.isEventDay && EventDay.dayType.Equals("FreezeDay"))
        {
            day.OnHit(@event.Attacker, @event.Userid);
        }

        return HookResult.Continue;
    }
    [GameEventHandler(mode: HookMode.Post)]
    public HookResult OnEventWeaponFire(EventWeaponFire @event, GameEventInfo info)
    {

        LastRequest.BulletCycleLRS4S(@event.Userid);
        LastRequest.OnShotLRRussianRoulette(@event.Userid);

        return HookResult.Continue;
    }

    [GameEventHandler(mode: HookMode.Pre)]
    public HookResult OnEventWeaponZoom(EventWeaponZoom @event, GameEventInfo info)
    {

        LastRequest.LRNoScopeAtScope(@event.Userid);
        return HookResult.Continue;
    }
    [GameEventHandler(mode: HookMode.Post)]
    public HookResult OnEventPlayerConnect(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event.Userid is not CCSPlayerController temp)
        {
            return HookResult.Continue;
        }
        if (@event.Userid == null || !@event.Userid.IsValid || @event.Userid.AuthorizedSteamID == null) 
            return HookResult.Continue;

        ulong steamid = @event.Userid.SteamID;
        string playername = @event.Userid.PlayerName;
        CCSPlayerController player = @event.Userid;
        Task.Run(() => Database.SetupPlayer(steamid, playername, 0));
        
        return HookResult.Continue;
    }
    [ConsoleCommand("css_admin")]
    [RequiresPermissions("@css/jbadmin")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_AdminMenu(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) return;
        if (_api == null) return;

        if (player == null || !player.IsValid || player.AuthorizedSteamID == null || !player.PawnIsAlive) return;

        var menu = JailBreak._api.NewMenuForcetype("Admin Menu", MenuType.ButtonMenu);

        menu.AddMenuOption("Teleport to player", (player, action) =>
        {
            var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
            menu1.PostSelectAction = PostSelectAction.Close;
            foreach (var item in Utilities.GetPlayers())
            {
                if (!item.PawnIsAlive || !item.IsValid || item == null || item.AuthorizedSteamID == null)
                    continue;
                menu1.AddMenuOption(item.PlayerName, (player, action) =>
                {
                    player.PlayerPawn.Value.Teleport(item.PlayerPawn.Value.AbsOrigin, item.PlayerPawn.Value.AbsRotation, item.PlayerPawn.Value.AbsVelocity);
                });
            }
            menu1.Open(player);
        });
        menu.AddMenuOption("Remove Warden", (player, action) =>
        { 
            foreach(var item in Utilities.GetPlayers())
            {
                if(Player.IsPlayerWarden(item))
                {
                    Player.RemoveWarden(item);
                }
            }
        });
        menu.AddMenuOption("CT Ban", (player, action) =>
        {
            var menu2 = JailBreak._api.NewMenuForcetype("CT Ban", MenuType.ButtonMenu);
            menu2.AddMenuOption("Ban a player", (player, action) =>
            {
                var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
                menu1.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.IsValid || item == null || item.AuthorizedSteamID == null)
                        continue;

                    menu1.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Server.ExecuteCommand("css_ctban " + item.SteamID + " 1440");
                    });
                }
                menu1.Open(player);
            });
            menu2.AddMenuOption("Unban a player", (player, action) =>
            {
                var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
                menu1.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.IsValid || item == null || item.AuthorizedSteamID == null)
                        continue;

                    menu1.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Server.ExecuteCommand("css_unctban " + item.SteamID);
                    });
                }
                menu1.Open(player);
            });
            menu2.AddMenuOption("Check if banned", (player, action) =>
            {
                var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
                menu1.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.IsValid || item == null || item.AuthorizedSteamID == null)
                        continue;

                    menu1.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Server.ExecuteCommand("css_isctbanned " + item.SteamID);
                    });
                }
                menu1.Open(player);
            });
            menu2.AddMenuOption("Session ban", (player, action) =>
            {
                var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
                menu1.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.IsValid || item == null || item.AuthorizedSteamID == null)
                        continue;

                    menu1.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Server.ExecuteCommand("css_sessionban " + item.SteamID);
                    });
                }
                menu1.Open(player);
            });

            menu2.Open(player);
        });

        menu.AddMenuOption("Give Freeday", (player, action) =>
        {

            var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
            menu1.PostSelectAction = PostSelectAction.Close;
            foreach (var item in Utilities.GetPlayers())
            {
                if (!item.PawnIsAlive || !item.IsValid || item == null || item.AuthorizedSteamID == null)
                    continue;
                menu1.AddMenuOption(item.PlayerName, (player, action) =>
                {
                    Player.GiveFreeday(item);
                });
            }
            menu1.Open(player);
        });
        menu.AddMenuOption("Disarm a player", (player, action) =>
        {
            var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
            menu1.PostSelectAction = PostSelectAction.Close;
            foreach (var item in Utilities.GetPlayers())
            {
                if (!item.PawnIsAlive || !item.IsValid || item == null || item.AuthorizedSteamID == null)
                    continue;
                menu1.AddMenuOption(item.PlayerName, (player, action) =>
                {
                    item.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();

                    item.GiveNamedItem("weapon_knife");
                });
            }
            menu1.Open(player);
        });
        menu.AddMenuOption("Set HP", (player, action) =>
        {
            var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);
            
            foreach (var item in Utilities.GetPlayers())
            {
                if (!item.PawnIsAlive || !item.IsValid || item == null || item.AuthorizedSteamID == null)
                    continue;
                menu1.AddMenuOption(item.PlayerName, (player, action) =>
                {
                    var menu2 = JailBreak._api.NewMenuForcetype("Select HP amount", MenuType.ButtonMenu);
                    menu2.PostSelectAction = PostSelectAction.Close;
                    menu2.AddMenuOption("100 HP", (player, action) =>
                    {
                        item.PlayerPawn.Value.Health = 100;
                    });
                    menu2.AddMenuOption("500 HP", (player, action) =>
                    {
                        item.PlayerPawn.Value.Health = 500;
                    });
                    menu2.AddMenuOption("1000 HP", (player, action) =>
                    {
                        item.PlayerPawn.Value.Health = 1000;
                    });
                    menu2.AddMenuOption("5000 HP", (player, action) =>
                    {
                        item.PlayerPawn.Value.Health = 5000;
                    });
                    menu2.AddMenuOption("10000 HP", (player, action) =>
                    {
                        item.PlayerPawn.Value.Health = 10000;
                    });
                    Utilities.SetStateChanged(item.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
                    menu2.Open(player);
                });
            }
            menu1.Open(player);
        });
        menu.AddMenuOption("Set GOD to player", (player, action) =>
        {
            var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);

            foreach (var item in Utilities.GetPlayers())
            {
                if (!item.PawnIsAlive || !item.IsValid || item == null || item.AuthorizedSteamID == null)
                    continue;
                menu1.AddMenuOption(item.PlayerName, (player, action) =>
                {
                    var menu2 = JailBreak._api.NewMenuForcetype("On/Off", MenuType.ButtonMenu);
                    menu2.PostSelectAction = PostSelectAction.Close;
                    menu2.AddMenuOption("On", (player, action) =>
                    {
                        Server.ExecuteCommand("css_god " + item.PlayerName + " 1");
                    });
                    menu2.AddMenuOption("Off", (player, action) =>
                    {
                        Server.ExecuteCommand("css_god " + item.PlayerName + " 0");
                    });
                    
                    menu2.Open(player);
                });
            }
            menu1.Open(player);
        });
        menu.AddMenuOption("Freeze a player", (player, action) =>
        {
            var menu1 = JailBreak._api.NewMenuForcetype("Select a player", MenuType.ButtonMenu);

            foreach (var item in Utilities.GetPlayers())
            {
                if (!item.PawnIsAlive || !item.IsValid || item == null || item.AuthorizedSteamID == null)
                    continue;
                menu1.AddMenuOption(item.PlayerName, (player, action) =>
                {
                    var menu2 = JailBreak._api.NewMenuForcetype("On/Off", MenuType.ButtonMenu);
                    menu2.PostSelectAction = PostSelectAction.Close;
                    menu2.AddMenuOption("On", (player, action) =>
                    {
                        Server.ExecuteCommand("css_freeze " + item.PlayerName);
                    });
                    menu2.AddMenuOption("Off", (player, action) =>
                    {
                        Server.ExecuteCommand("css_unfreeze " + item.PlayerName);
                    });

                    menu2.Open(player);
                });
            }
            menu1.Open(player);
        });

        menu.Open(player);
    }
    public HookResult JoinTeam(CCSPlayerController? player, CommandInfo command)
    {
        if(!Int32.TryParse(command.ArgByIndex(1), out int team)) return HookResult.Continue;


        if (Balance.JoinTeam(player, team))
            return HookResult.Continue;

        return HookResult.Handled;
    }
    [ConsoleCommand("css_gift")]
    [CommandHelper(minArgs: 2, usage: "<userid> <amount>")]
    public void Command_GiftCredits(CCSPlayerController? player, CommandInfo command)
    {
        JailShop.GiftCredits(player, command);
    }

        private void PrintInfo()
    {
        Server.PrintToConsole(" ");
        Server.PrintToConsole("#####################################");
        Server.PrintToConsole($"Plugin name - {ModuleName} [v{ModuleVersion}]");
        Server.PrintToConsole($"Author - {ModuleAuthor}");
        Server.PrintToConsole($"Description - {ModuleDescription}");
        Server.PrintToConsole($"Github - {MyLink}");
        Server.PrintToConsole("#####################################");
        Server.PrintToConsole(" ");
    }
    public void RegisterListenersJailbreak()
    {
        RegisterEventHandler<EventPlayerHurt>(OnEventPlayerHurt);
        RegisterEventHandler<EventWeaponFire>(OnEventWeaponFire);
        RegisterEventHandler<EventWeaponZoom>(OnEventWeaponZoom);
        RegisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnect);
        AddCommandListener("jointeam", JoinTeam);
    }
}
