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
using Dapper;
using MenuManager;
using CounterStrikeSharp.API.Core.Capabilities;
using System.Web;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using System.Diagnostics;
using JailBreak.EventDays;
using System.Reflection.Emit;
namespace JailBreak;
public partial class JailBreak
{
    
    Color defModColor;
    EventDay day;
    public void onWardenStart()
    {

    }
    [ConsoleCommand("css_wmenu")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_WardenMenu(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return;
        if (!Player.IsPlayerWarden(player))
        {
            string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Ova komanda je za wardena";
            player.PrintToChat($"\u200B{message}");
            return;
        }
        if (!player.PawnIsAlive)
        {
            string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Ova komanda je moguca samo dok ste zivi";
            Server.PrintToChatAll($"\u200B{message}");
        }
        if (_api == null) return;
        var menu = _api.NewMenuForcetype("Warden Menu", MenuType.ButtonMenu);

        menu.AddMenuOption("Freeday", (player, action) =>
        {
            var menu1 = _api.NewMenuForcetype("Freeday", MenuType.ButtonMenu);
            menu1.AddMenuOption("Give Freeday", (player, action) =>
            {
                var menu2 = _api.NewMenuForcetype("Give Freeday", MenuType.ButtonMenu);
                menu2.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 2)
                        continue;
                    if (Player.IsPlayerFreeday(item))
                        continue;
                    menu2.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Player.GiveFreeday(item);
                    });
                }
                menu2.Open(player);
            });
            menu1.AddMenuOption("Remove Freeday", (player, action) =>
            {
                var menu3 = _api.NewMenuForcetype("Remove Freeday", MenuType.ButtonMenu);
                menu3.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 2)
                        continue;
                    if (!Player.IsPlayerFreeday(item))
                        continue;
                    menu3.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Player.RemoveFreeday(item);
                    });
                }
                menu3.Open(player);
            });

            menu1.Open(player);
        });
        menu.AddMenuOption("Rebel", (player, action) =>
        {
            var menu4 = _api.NewMenuForcetype("Rebel", MenuType.ButtonMenu);
            menu4.AddMenuOption("Give Rebel", (player, action) =>
            {
                var menu5 = _api.NewMenuForcetype("Give Rebel", MenuType.ButtonMenu);
                menu5.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 2)
                        continue;
                    if (Player.IsPlayerRebel(item))
                        continue;
                    menu5.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Player.BecomeRebel(item);
                    });
                }
                menu5.Open(player);
            });
            menu4.AddMenuOption("Remove Rebel", (player, action) =>
            {
                var menu6 = _api.NewMenuForcetype("Remove Rebel", MenuType.ButtonMenu);
                menu6.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 2)
                        continue;
                    if (!Player.IsPlayerRebel(item))
                        continue;
                    menu6.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Player.RemoveRebel(item);
                    });
                }
                menu6.Open(player);
            });
            menu4.Open(player);
        });
        menu.AddMenuOption("TeamGames", (player, action) =>
        {
            var menu7 = _api.NewMenuForcetype("TeamGames", MenuType.ButtonMenu);
            menu7.AddMenuOption("Start", (player, action) =>
            {
                string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "TeamGames Igra je pocela!";
                Server.PrintToChatAll($"\u200B{message}");
                AddTimer(60.0f, () =>
                {
                    foreach (var item in Utilities.GetPlayers())
                    {
                        if (Player.IsPlayerBlueTeam(item))
                        {
                            Player.RemoveBlueTeam(item);
                        }
                        if (Player.IsPlayerRedTeam(item))
                        {
                            Player.RemoveRedTeam(item);
                        }
                    }
                    string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "TeamGames Igra se zavrsila!";
                    Server.PrintToChatAll($"\u200B{message}");
                });
            });
            menu7.AddMenuOption("Blue Team", (player, action) =>
            {
                var menu8 = _api.NewMenuForcetype("Blue Team", MenuType.ButtonMenu);
                menu8.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 2)
                        continue;
                    if (Player.IsPlayerRebel(item))
                        continue;
                    if (Player.IsPlayerFreeday(item))
                        continue;
                    if (Player.IsPlayerBlueTeam(item))
                        continue;
                    menu8.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Player.BecomeBlueTeam(item);
                    });
                }
                menu8.Open(player);
            });
            menu7.AddMenuOption("Red Team", (player, action) =>
            {
                var menu9 = _api.NewMenuForcetype("Red Team", MenuType.ButtonMenu);
                menu9.PostSelectAction = PostSelectAction.Close;
                foreach (var item in Utilities.GetPlayers())
                {
                    if (!item.PawnIsAlive)
                        continue;
                    if (item.TeamNum != 2)
                        continue;
                    if (Player.IsPlayerRebel(item))
                        continue;
                    if (Player.IsPlayerFreeday(item))
                        continue;
                    if (Player.IsPlayerRedTeam(item))
                        continue;
                    menu9.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Player.BecomeRedTeam(item);
                    });
                }
                menu9.Open(player);
            });
            menu7.Open(player);
        });
        menu.AddMenuOption("Glava / Pismo", (player, action) =>
        {
            Random rand = new Random();
            if (rand.Next(0, 2) == 0)
            {
                string message = ChatColors.Green + "[Coinflip] " + ChatColors.Default + "Glava";
                Server.PrintToChatAll($"\u200B{message}");
            }
            else
            {
                string message = ChatColors.Green + "[Coinflip] " + ChatColors.Default + "Pismo";
                Server.PrintToChatAll($"\u200B{message}");
            }
        });
        menu.AddMenuOption("Start EventDay", (player, action) =>
        {
            var menu10 = _api.NewMenuForcetype("Start EventDay", MenuType.ButtonMenu);
            menu10.PostSelectAction = PostSelectAction.Close;
            menu10.AddMenuOption("Free Day", (player, action) =>
            {
                day = new FreeDay();
            });
            menu10.AddMenuOption("President Day", (player, action) =>
            {
                day = new PresidentDay();
            });
            menu10.AddMenuOption("Freeze Day", (player, action) =>
            {
                day = new FreezeDay();
            });
            menu10.AddMenuOption("Hide And Seek", (player, action) =>
            {
                day = new HideDay();
            });
            menu10.AddMenuOption("Knife", (player, action) =>
            {
                day = new KnifeDay();
            });
            menu10.AddMenuOption("Predator Day", (player, action) =>
            {
                day = new PredatorDay();
            });
            menu10.AddMenuOption("Sniper Day", (player, action) =>
            {
                day = new SniperDay();
            });
            menu10.Open(player);
        });

        /*menu.AddMenuOption("GunToss", (player, action) =>
        {
            foreach(var item in Utilities.GetPlayers())
            {
                if (item == null || !item.IsValid || item.AuthorizedSteamID == null || !item.PawnIsAlive) continue;
                if (Player.IsPlayerFreeday(item) || Player.IsPlayerRebel(item))
                    continue;
                item.GiveNamedItem("weapon_deagle");

                
            }
        });*/
        menu.AddMenuOption("Swap CT", (player, action) =>
        {
            var menu10 = _api.NewMenuForcetype("Swap CT", MenuType.ButtonMenu);
            menu10.PostSelectAction = PostSelectAction.Close;
            foreach (var item in Utilities.GetPlayers())
            {
                if (item == null || !item.IsValid || item.AuthorizedSteamID == null || item.PlayerPawn.Value == null) continue;
                
                if(item.PlayerPawn.Value.TeamNum == 3 && item!=player)
                {
                    menu10.AddMenuOption(item.PlayerName, (player, action) =>
                    {
                        Server.ExecuteCommand("css_swap " + item.PlayerName);
                        Server.ExecuteCommand("css_slay " + item.PlayerName);
                    });
                }
                
            }
            menu10.Open(player);
        });
        menu.AddMenuOption("Toggle Block", (player, action) =>
        {
 
            var menu2 = JailBreak._api.NewMenuForcetype("On/Off", MenuType.ButtonMenu);
            menu2.PostSelectAction = PostSelectAction.Close;
            menu2.AddMenuOption("On", (player, action) =>
            {
                Server.ExecuteCommand("mp_solid_teammates 1");
            });
            menu2.AddMenuOption("Off", (player, action) =>
            {
                Server.ExecuteCommand("mp_solid_teammates 0");
            });

            menu2.Open(player);

        });

        menu.Open(player);
    }
    [ConsoleCommand("css_uw")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_UnBecomeWarden(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return;
        if (Player.IsPlayerWarden(player))
        {
            if (_api != null) 
                _api.CloseMenu(player);
            Player.RemoveWarden(player);
            return;
        }
        string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Morate biti warden!";
        player.PrintToChat($"\u200B{message}");
    }
    [ConsoleCommand("css_w")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_BecomeWarden(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return;


        if (Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").ElementAt(0).GameRules.WarmupPeriod)
        {
            string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Ova komanda je iskljucena u warmupu!";
            player.PrintToChat($"\u200B{message}");
            return;
        }

        if (player.TeamNum != 3)
        {
            string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Morate biti u ct da biste postali warden!";
            player.PrintToChat($"\u200B{message}");

            return;
        }
        if (!player.PawnIsAlive)
        {
            string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Ova komanda je moguca samo dok ste zivi!";
            player.PrintToChat($"\u200B{message}");

            return;
        }
        if (Player.IsPlayerWarden(player))
        {
            string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Vi ste vec warden!";
            player.PrintToChat($"\u200B{message}");
            return;
        }
        if (Player.Players.Count > 0)
        {
            foreach (Player item in Player.Players)
            {
                if (item.isWarden == true)
                {
                    string message = ChatColors.Green + "[JailBreak] " + ChatColors.Default + "Warden vec postoji!";
                    player.PrintToChat($"\u200B{message}");
                    return;
                }
            }
        }
        JailBreak.jailbreak.AddTimer(0.1f, LaserTick, TimerFlags.REPEAT);
        Player.GiveWarden(player);
    }
    [GameEventHandler(mode: HookMode.Pre)]
    public HookResult OnEventRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        JailShop.ResetPlayersRoundEnd();
        Player.Players.Clear();
        isLr = false;
        if(EventDay.isEventDay)
            day.EndEventDay();

        freezetimepassed = false;
        return HookResult.Continue;
    }
    [GameEventHandler(mode: HookMode.Post)]
    public HookResult OnEventRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        /*foreach (string Weapons in WeaponsList)
        {
            foreach (var entity in Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(Weapons))
            {
                if (entity == null) continue;
                if (!entity.IsValid) continue;
                if (entity.Entity == null) continue;
                if (entity.OwnerEntity == null) continue;
                if (!entity.OwnerEntity.IsValid) continue;
                if (entity.OwnerEntity.Value == null) continue;
                if (!entity.OwnerEntity.Value.IsValid) continue;
                if (entity.OwnerEntity.Value.TeamNum !=2) continue;
                entity.AcceptInput("Kill");
            }
        }*/
        foreach(var item in Utilities.GetPlayers())
        {
            item.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().RemoveWeapons();
            item.GiveNamedItem("weapon_knife");

        }    
        JailShop.timeOk = true;
        AddTimer(60.0f, () =>
        {
            JailShop.timeOk = false;
        });


        return HookResult.Continue;
    }
    [GameEventHandler(mode:HookMode.Post)]
    public HookResult OnEventPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if (@event.Userid == null || !@event.Userid.IsValid || @event.Userid.AuthorizedSteamID == null) return HookResult.Continue;
        CCSPlayerController temp = @event.Userid;
        if (_api != null)
            _api.CloseMenu(temp);

        
        

        if (temp.PlayerPawn.Value != null && !Player.IsPlayerWarden(temp) && !Player.IsPlayerFreeday(temp)) 
        { 
            temp.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
        }

        if (Player.Players.Count > 0)
        {
            foreach (Player playerr in Player.Players)
            {
                if (playerr.controller == temp)
                    return HookResult.Continue;
            }
        }
        

        Player player = new Player(temp);
        Player.Players.Add(player);


        return HookResult.Continue;
    }
    [GameEventHandler(mode: HookMode.Post)]
    public HookResult OnEventPlayerSpawned(EventPlayerSpawned @event, GameEventInfo info)
    {
        if (@event.Userid == null || !@event.Userid.IsValid || @event.Userid.AuthorizedSteamID == null) return HookResult.Continue;

        return HookResult.Continue;
    }
    [GameEventHandler(mode:HookMode.Post)]
    public HookResult OnEventPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        if (Player.GetPlayerFromController(@event.Userid) == null) return HookResult.Continue;
        if (@event.Userid == null || !@event.Userid.IsValid || @event.Userid.AuthorizedSteamID == null) return HookResult.Continue;

        if (_api != null)
            _api.CloseMenu(@event.Userid);
        Player.GetPlayerFromController(@event.Userid).isWarden = false;
        Player.GetPlayerFromController(@event.Userid).isFreeday = false;
        Player.GetPlayerFromController(@event.Userid).isRebel = false;
        Player.GetPlayerFromController(@event.Userid).isBlueTeam = false;
        Player.GetPlayerFromController(@event.Userid).isRedTeam = false;
        @event.Userid.Clan = "";


        LastRequest.isLastRequestOnDeathEvent(@event.Userid);
        LastRequest.EndLR(@event.Userid);
        

        return HookResult.Continue;
    }
    [GameEventHandler]
    public HookResult OnEventPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (@event.Userid == null || !@event.Userid.IsValid || @event.Userid.AuthorizedSteamID == null) return HookResult.Continue;

        return HookResult.Continue;
    }
    private HookResult OnPlayerChat(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsValid || player.AuthorizedSteamID == null) return HookResult.Continue;
        if (info.GetArg(1).Length == 0) return HookResult.Continue;
        if (info.GetArg(1).StartsWith("!") || info.GetArg(1).StartsWith("@") || info.GetArg(1).StartsWith("/") || info.GetArg(1).StartsWith("."))
        {
            return HookResult.Continue;
        }
        string message = player.PlayerName + ChatColors.Default + " : " + info.GetArg(1);
        if (Player.IsPlayerWarden(player))
        {
            message = ChatColors.DarkBlue + "[Warden] " + message;
        }
        if (Player.playerHasFlag(player, "@css/reservation"))
        {
            message = ChatColors.Gold + "[VIP] " + message;
        }
        if (Player.playerHasFlag(player, "@css/admin"))
        {
            message = ChatColors.Green + "[ADMIN] " + message;
        }
        if (Player.playerHasFlag(player, "@css/headadmin"))
        {
            message = ChatColors.LightBlue + "[HEAD ADMIN] " + message;
        }
        if (Player.playerHasFlag(player, "@css/owner"))
        {
            message = ChatColors.Darkred + "[OWNER] " + message;
        }
        if (Player.playerHasFlag(player, "@css/developer"))
        {
            message = ChatColors.Darkred + "[DEVELOPER] " + message;
        }
        Server.PrintToChatAll($"\u200B{message}");
        return HookResult.Handled;
    }

    public void LaserTick()
    {
        if (Utilities.GetPlayers().Count < 1) return;
        if (Player.Players.Count < 1) return;
        CCSPlayerController player = new CCSPlayerController(0);
        bool x = false;
        foreach(var temp in Utilities.GetPlayers())
        {
            if (!temp.PawnIsAlive) continue;
            if(Player.IsPlayerWarden(temp))
            {
                x = true;
                player = temp;
            }
        }
        if (!x)
            return;
        if (Player.IsPlayerWarden(player) && Player.IsPlayerAliveLegal(player))
        {
            bool useKey = (player.Buttons & PlayerButtons.Use) == PlayerButtons.Use;
            CCSPlayerPawn? pawn = player.PlayerPawn.Value;
            CPlayer_CameraServices? camera = pawn?.CameraServices;
            CEnvBeam? laser = Utilities.CreateEntityByName<CEnvBeam>("env_beam");
            if (pawn != null && pawn.AbsOrigin != null && camera != null && useKey)
            {
                CounterStrikeSharp.API.Modules.Utils.Vector eye = new CounterStrikeSharp.API.Modules.Utils.Vector(pawn.AbsOrigin.X, pawn.AbsOrigin.Y, pawn.AbsOrigin.Z + camera.OldPlayerViewOffsetZ);

                if (laser == null) return;
                if(player.PlayerPawn.Value == null) return;
                QAngle viewAngles = player.PlayerPawn.Value.EyeAngles;
                CounterStrikeSharp.API.Modules.Utils.Vector direction = QAngleToVector(viewAngles);

                QAngle temp = new QAngle(0.0f, 0.0f, 0.0f);
                CounterStrikeSharp.API.Modules.Utils.Vector temp2 = new CounterStrikeSharp.API.Modules.Utils.Vector(0.0f, 0.0f, 0.0f);

                laser.Render = Color.Blue;
                laser.Width = 2.0f;

                laser.Teleport(eye, temp, temp2);

                CounterStrikeSharp.API.Modules.Utils.Vector end = eye + (direction * 10000);

                
                laser.EndPos.X = end.X; 
                laser.EndPos.Y = end.Y;
                laser.EndPos.Z = end.Z;

                Utilities.SetStateChanged(laser, "CBeam", "m_vecEndPos");

                laser.DispatchSpawn();
            }
            if (laser == null) return;
            CBaseEntity? ent = Utilities.GetEntityFromIndex<CBaseEntity>((int)laser.Index);
            if (ent != null && ent.DesignerName == "env_beam")
            {
                AddTimer(0.15f, () =>
                {
                    if (ent == null) return;
                    ent.Remove();
                });
            }
        }
    }

   
    private CounterStrikeSharp.API.Modules.Utils.Vector QAngleToVector(QAngle angles)
    {
        double pitch = -angles.X * (Math.PI / 180.0); 
        double yaw = angles.Y * (Math.PI / 180.0);

        double x = Math.Cos(yaw) * Math.Cos(pitch);
        double y = Math.Sin(yaw) * Math.Cos(pitch);
        double z = Math.Sin(pitch);

        return new CounterStrikeSharp.API.Modules.Utils.Vector((float)x, (float)y, (float)z);
    }
   
    public void RegisterListenersWarden()
    {
        RegisterEventHandler<EventRoundEnd>(OnEventRoundEnd);
        RegisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawn);
        RegisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
        RegisterEventHandler<EventPlayerDeath>(OnEventPlayerDeath);
        RegisterEventHandler<EventPlayerSpawned>(OnEventPlayerSpawned);
        AddCommandListener("say", OnPlayerChat);
    }
    public void RegisterCommandsWarden()
    {
        AddCommand("css_w", "Become warden", (player, info) =>
        {
        });
        AddCommand("css_uw", "Unbecome warden", (player, info) =>
        {
        });
        AddCommand("css_wmenu", "Warden menu", (player, info) =>
        {
        });
        
    }
}
