using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using JailBreak.JailItems;
using MenuManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak
{
    internal class JailShop
    {
        public static bool timeOk = false;
        public static bool OpenShop(CCSPlayerController player)
        {
            if (player == null || !player.IsValid || player.AuthorizedSteamID == null || !player.PawnIsAlive) return false;

            if (!timeOk)
            {
                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + " Vreme za kupovinu je isteklo";
                player.PrintToChat($"\u200B{message}");
                return false;
            }

            var menu = JailBreak._api.NewMenuForcetype("Jail Shop, Credits: " + DataBase.Database.GetPlayerCredits(player.SteamID), MenuType.ButtonMenu);
            menu.PostSelectAction = PostSelectAction.Close;
            if (player.PlayerPawn.Value.TeamNum == 2)
            {
                if (JailBreak.jailbreak.Config.ItemCost.toggleFreeDayT)
                {
                    menu.AddMenuOption("Free Day - " + FreeDayItem.costT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, FreeDayItem.costT))
                        {
                            FreeDayItem freeDayItem = new FreeDayItem(player, "FreeDayItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                           
                        }
                    });
                }
                if(JailBreak.jailbreak.Config.ItemCost.toggleAllBombsT) 
                {
                    menu.AddMenuOption("All Bombs - " + AllBombsItem.costT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, AllBombsItem.costT))
                        {
                            AllBombsItem allBombsItem = new AllBombsItem(player, "AllBombsItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleNoClipT) 
                {
                    if (JailBreak.jailbreak.Config.ItemCost.vipOnlyNoClipT) 
                    {
                        if(Player.playerHasFlag(player ,"@css/reservation"))
                        {
                            menu.AddMenuOption("NoClip - " + NoClipItem.costT, (player, action) =>
                            {
                                if (PlayerCanBuyJailItem(player, NoClipItem.costT))
                                {
                                    NoClipItem noClipItem = new NoClipItem(player, "NoClipItem");
                                }
                                else
                                {
                                    string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                                    player.PrintToChat($"\u200B{message}");
                                }
                            });
                        }
                    }
                    else
                    {
                        menu.AddMenuOption("NoClip 5sec - " + NoClipItem.costT, (player, action) =>
                        {
                            if (PlayerCanBuyJailItem(player, NoClipItem.costT))
                            {
                                NoClipItem noClipItem = new NoClipItem(player, "NoClipItem");
                            }
                            else
                            {
                                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                                player.PrintToChat($"\u200B{message}");
                            }
                        });
                    }
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleGravityT)
                {
                    menu.AddMenuOption("Gravity - " + GravityItem.costT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, GravityItem.costT))
                        {
                            GravityItem gravityItem = new GravityItem(player, "GravityItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleSpeedT)
                {
                    menu.AddMenuOption("Speed - " + SpeedItem.costT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, SpeedItem.costT))
                        {
                            SpeedItem speedItem = new SpeedItem(player, "SpeedItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
            }
            if (player.PlayerPawn.Value.TeamNum == 3)
            {
                if (JailBreak.jailbreak.Config.ItemCost.toggleGodCT)
                {
                    menu.AddMenuOption("God 60sec - " + GodItem.costCT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, GodItem.costCT))
                        {
                            GodItem godItem = new GodItem(player, "GodItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleHPCT)
                {
                    menu.AddMenuOption("HP (" + JailBreak.jailbreak.Config.ItemCost.valueHPCT + ")" + " - " + HPItem.costCT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, HPItem.costCT))
                        {
                            HPItem hpItem = new HPItem(player, "HPItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleHealthShotCT)
                {
                    menu.AddMenuOption("HealthShot - " + HealthShotItem.costCT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, HealthShotItem.costCT))
                        {
                            HealthShotItem healthShotItem = new HealthShotItem(player, "HealthShotItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleGravityCT)
                {
                    menu.AddMenuOption("Gravity - " + GravityItem.costCT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, GravityItem.costCT))
                        {
                            GravityItem gravityItem = new GravityItem(player, "GravityItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleSpeedCT)
                {
                    menu.AddMenuOption("Speed - " + SpeedItem.costCT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, SpeedItem.costCT))
                        {
                            SpeedItem speedItem = new SpeedItem(player, "SpeedItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
                if (JailBreak.jailbreak.Config.ItemCost.toggleAllBombsCT)
                {
                    menu.AddMenuOption("All Bombs - " + AllBombsItem.costCT, (player, action) =>
                    {
                        if (PlayerCanBuyJailItem(player, AllBombsItem.costCT))
                        {
                            AllBombsItem allBombsItem = new AllBombsItem(player, "AllBombsItem");
                        }
                        else
                        {
                            string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                            player.PrintToChat($"\u200B{message}");
                        }
                    });
                }
            }
            menu.Open(player);

            return true;
        }
        public static void GiftCredits(CCSPlayerController? player, CommandInfo command)
        {
            CCSPlayerController? target = Utilities.GetPlayerFromUserid(Convert.ToInt32(command.GetArg(1)));
            if (target == null)
            {
                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Igrac nije pronadjen!";
                player.PrintToChat($"\u200B{message}");
                return;
            }
            ulong playersteamid = player.SteamID;
            ulong targetsteamid = target.SteamID;

            if (!DataBase.Database.IsPlayerSetup(targetsteamid))
            {
                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Igrac nije pronadjen!";
                player.PrintToChat($"\u200B{message}");
                return;
            }
            if(DataBase.Database.GetPlayerCredits(playersteamid) < Convert.ToInt32(command.GetArg(2)))
            {
                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Nemate dovoljno kredita";
                player.PrintToChat($"\u200B{message}");
                return;
            }

            Task.Run(() => DataBase.Database.AddCredits(targetsteamid, Convert.ToInt32(command.GetArg(2))));
            Task.Run(() => DataBase.Database.SubstractCredits(playersteamid, Convert.ToInt32(command.GetArg(2))));
        }
        public static bool PlayerCanBuyJailItem(CCSPlayerController player, int cost)
        {
            if (DataBase.Database.GetPlayerCredits(player.SteamID) < cost)
                return false;
            return true;
        }
        public static void ResetPlayersRoundEnd()
        {
            foreach(var player in Utilities.GetPlayers())
            {
                player.PlayerPawn.Value.Speed = 1.0f;
                player.PlayerPawn.Value.GravityScale = 1.0f;
            }
        }
        public static void IdleCredits()
        {
           if (JailBreak.jailbreak.Config.ShopCoins.toggleIdleCredits)
           {
                int amount = JailBreak.jailbreak.Config.ShopCoins.amountIdleCreditsVip;
                JailBreak.jailbreak.AddTimer(JailBreak.jailbreak.Config.ShopCoins.everyXSeconds, () =>
                {
                    if (Utilities.GetPlayers().Count < JailBreak.jailbreak.Config.ShopCoins.minPlayers) return;

                    foreach (var player in Utilities.GetPlayers())
                    {
                        ulong steamid = player.SteamID;
                        if (Utilities.GetPlayers().Count <= JailBreak.jailbreak.Config.ShopCoins.minPlayers)
                        {
                            if (JailBreak.jailbreak.Config.ShopCoins.toggleIdleCreditsVip && Player.playerHasFlag(player, "@css/reservation"))
                            {
                                Task.Run(() => DataBase.Database.AddCredits(steamid, amount));
                                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Proslo je 10 minuta i dobio si " + amount + " kredita";
                                player.PrintToChat($"\u200B{message}");
                            }
                            else
                            {
                                Task.Run(() => DataBase.Database.AddCredits(steamid, amount));
                                string message = ChatColors.Green + "[JailShop] " + ChatColors.Default + "Proslo je 10 minuta i dobio si " + amount + " kredita";
                                player.PrintToChat($"\u200B{message}");
                            }
                        }
                    }
                }, TimerFlags.REPEAT);
            }
            return;
        }
        public static void SetupJailShop()
        {

        }
    }
}
