﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.Math;
using GTA.NaturalMotion;
using LemonUI;
using System.Media;
using System.IO;
using System.Threading;
using System.Reflection;
using LemonUI.Menus;
using Font = GTA.UI.Font;
using LemonUI.Elements;
using LemonUI.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Collections;
using GTA.UI;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Runtime.ConstrainedExecution;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//uses lemon and SHVDN version 2, also added windows forms



namespace LemonUI.Menu1
{

    public class Basics : Script
    {

        public static bool CanPlayerSuperJump { get; set; }
        public static bool canPlayerFastRun { get; set; }

        //MoneyDrop Story Mode
        bool moneyDrop40kOn;
        bool moneyDrop1MilOn;
        bool moneyDrop10MilOn;
        bool moneyDrop15MilOn;
        bool moneyDrop20MilOn;
        bool moneyDrop100MilOn;
        
     



        private static readonly ObjectPool DemoPool = new ObjectPool();
        private static readonly NativeMenu DemoMenu = new NativeMenu(bannerText: "Essential Menu", name: "Made By Anonik v1,30", description: "Made By Anonik v1,30");




        //start of Top Level Items, at top of menu, not part of submenu
        private static readonly NativeMenu SelfMenu = new NativeMenu("Self Options", "Self Options", "");  //The first submenu
        private static readonly NativeMenu VehicleMenu = new NativeMenu("Vehicle Options", "Vehicle Options", "");  //The first submenu
        private static readonly NativeMenu TeleportMenu = new NativeMenu("Teleport Options", "Teleporting Options");
        private static readonly NativeMenu WeatherMenu = new NativeMenu("Weather Options", "Weather Options");//Weather Menu
        private static readonly NativeMenu BetaMenu = new NativeMenu("[Beta Menu ]", "Beta Options");//Weather Menu


        private static readonly NativeItem CarGuy = new NativeItem("Change to Car Guy Model", "");



        //start of Submenu
        private static readonly NativeMenu DemoSubMenuPed = new NativeMenu("Peds", "Ped Gangs", "");  //The first submenu
        private static readonly NativeItem Ballas = new NativeItem("Ballas and Family", "Description"); //Item 1 of submenu
        private static readonly NativeItem LostMCGang = new NativeItem("Import Export", "Description"); //Item 2 of submenu

        private static readonly NativeMenu ModelMenuPed = new NativeMenu("Model Changer", "Model Changer","");
        private static readonly NativeItem Franklins = new NativeItem("Franklin Model", "");


        //BetaMenu
        private static readonly NativeCheckboxItem drunkmode = new NativeCheckboxItem("~b~Drunk Mode", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem nocollmode = new NativeCheckboxItem("~b~No Collision", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem chaosmode = new NativeCheckboxItem("~r~Chaos Mode", "Description", false); // Enabled by Default with a Description



        //Selfmenu


        private static readonly NativeCheckboxItem godmode1 = new NativeCheckboxItem("~b~God Mode", "Description", false); // Enabled by Default with a Description
        private static readonly NativeItem maxhealth = new NativeItem("Max Health");
        private static readonly NativeCheckboxItem neverwanted = new NativeCheckboxItem("~b~Never Wanted", false);
        private static readonly NativeItem giveallwep = new NativeItem("Give All Weapons");
        private static readonly NativeItem givemaxammo = new NativeItem("Give Max Ammo");
        private static readonly NativeCheckboxItem infiniteammo = new NativeCheckboxItem("~r~Infinite Ammo", false);
        private static readonly NativeCheckboxItem superjump = new NativeCheckboxItem("Super Jump", false);
        private static readonly NativeCheckboxItem invisible = new NativeCheckboxItem("Invisible", false);
        private static readonly NativeCheckboxItem fastrun = new NativeCheckboxItem("Fast Run", false);
        private static readonly NativeMenu MoneyMenu = new NativeMenu("Money Drop", "Money Drop", "");
        private static readonly NativeCheckboxItem money40k = new NativeCheckboxItem("Money Drop 40k", false);
        private static readonly NativeCheckboxItem money1milion = new NativeCheckboxItem("Money Drop 1Mil", false);
        private static readonly NativeCheckboxItem money10milion = new NativeCheckboxItem("Money Drop 10Mil", false);
        private static readonly NativeCheckboxItem money15milion = new NativeCheckboxItem("Money Drop 15Mil", false);
        private static readonly NativeCheckboxItem money20milion = new NativeCheckboxItem("Money Drop 20Mil", false);
        private static readonly NativeCheckboxItem money100milion = new NativeCheckboxItem("Money Drop 100Mil", false);

        //VehicleMenu

        private static readonly NativeItem fixvehicle = new NativeItem("Fix Car Health");
        private static readonly NativeCheckboxItem cargodmod = new NativeCheckboxItem("Invincible Car", false);
        private static readonly NativeCheckboxItem cardrivein = new NativeCheckboxItem("Car Autopilot","Autopilot", false);
        private static readonly NativeItem vehiclespawn = new NativeItem("Spawn Input Vehicle","Type a input name vehicle");
        private static readonly NativeItem vehiclepimp = new NativeItem("Max Car Stats");


        //TeleportMenu
        private static readonly NativeItem teltoway = new NativeItem("Teleport To Waypoint");
        private static readonly NativeItem teltochiliad = new NativeItem("Teleport to Mont Chilliad");
        private static readonly NativeItem teltopub1 = new NativeItem("Teleport to Tequilala");
        private static readonly NativeItem teltopub2 = new NativeItem("Teleport to Bahama");
        private static readonly NativeItem teltobank = new NativeItem("Teleport to Bank");
        private static readonly NativeItem teltobell = new NativeItem("Teleport to Cluckin Bell");
        private static readonly NativeItem teltofib = new NativeItem("Teleport to FIB");
        private static readonly NativeItem teltofloyd = new NativeItem("Teleport to Floyd House");
        private static readonly NativeItem teltolife = new NativeItem("Teleport to Life Invader");
        private static readonly NativeItem teltolester = new NativeItem("Teleport to Lester House");
        private static readonly NativeItem teltooneil = new NativeItem("Teleport to Oneil Farm");
        private static readonly NativeItem teltosolomon = new NativeItem("Teleport to Solomon Office");
        private static readonly NativeMenu Northmenu = new NativeMenu("N-Y Loader", "North Yankton Loader");
        private static readonly NativeItem loadnorth = new NativeItem("Load North Yankton");
        private static readonly NativeItem unloadnorth = new NativeItem("Unload North Yankton");

        //WeatherMenu
        private static readonly NativeItem ClearWeather = new NativeItem("Clear", "");
        private static readonly NativeItem ClearingWeather = new NativeItem("Clearing", "");
        private static readonly NativeItem SunnyWeather = new NativeItem("ExtraSunny", "");
        private static readonly NativeItem CloudWeather = new NativeItem("Clouds", "");
        private static readonly NativeItem SmogWeather = new NativeItem("Smog", "");
        private static readonly NativeItem FoggyWeather = new NativeItem("Foggy", "");
        private static readonly NativeItem NeutralWeather = new NativeItem("Neutral", "");
        private static readonly NativeItem HalloweenWeather = new NativeItem("Halloween", "");
        private static readonly NativeItem BlizzardWeather = new NativeItem("Blizzard", "");
        private static readonly NativeItem RainingWeather = new NativeItem("Raining", "");
        private static readonly NativeItem OvercastWeather = new NativeItem("Overcast", "");
        private static readonly NativeItem ThunderWeather = new NativeItem("Thunderstorm", "");
        private static readonly NativeItem ChristmasWeather = new NativeItem("Christmas", "");
        private static readonly NativeItem SnowingWeather = new NativeItem("Snowing", "");
        private static readonly NativeItem SnowlightWeather = new NativeItem("Snowligt", "");
        private static readonly NativeCheckboxItem XmasWeather = new NativeCheckboxItem("Xmas Snow","Persistent Snow", false);

        public Basics()
        {


            //==============================================STYLE===============================================================

            //SELF MENU
            //menu.BannerColor = System.Drawing.Color.FromArgb(180, 20, 20);
            SelfMenu.BannerText.Color = Color.Brown;
            SelfMenu.NameFont = Font.RockstarTag;
            SelfMenu.Banner.Color = Color.FromArgb(180, 20, 20);//Purple;
            SelfMenu.BannerText.Font = Font.Pricedown;
            SelfMenu.DescriptionFont = Font.RockstarTag;
            maxhealth.TitleFont = Font.RockstarTag;
            godmode1.TitleFont = Font.RockstarTag;
            neverwanted.TitleFont = Font.RockstarTag;
            giveallwep.TitleFont = Font.RockstarTag;
            givemaxammo.TitleFont = Font.RockstarTag;
            infiniteammo.TitleFont = Font.RockstarTag;
            superjump.TitleFont = Font.RockstarTag;
            invisible.TitleFont = Font.RockstarTag;  
            fastrun.TitleFont = Font.RockstarTag;

            //MONEY MENU
            MoneyMenu.BannerText.Color = Color.Brown;
            MoneyMenu.NameFont = Font.RockstarTag;
            MoneyMenu.Banner.Color = Color.Black;
            MoneyMenu.BannerText.Font = Font.Pricedown;
            MoneyMenu.DescriptionFont  = Font.RockstarTag;
            money40k.TitleFont = Font.RockstarTag;
            money1milion.TitleFont = Font.RockstarTag;
            money10milion.TitleFont = Font.RockstarTag;
            money15milion.TitleFont = Font.RockstarTag;
            money20milion.TitleFont = Font.RockstarTag;
            money100milion.TitleFont = Font.RockstarTag;

            //VEHICLE MENU
            VehicleMenu.BannerText.Color = Color.Brown;
            VehicleMenu.NameFont = Font.RockstarTag;
            VehicleMenu.Banner.Color = Color.Black;
            VehicleMenu.BannerText.Font = Font.Pricedown;
            VehicleMenu.DescriptionFont = Font.RockstarTag;
            fixvehicle.TitleFont = Font.RockstarTag;
            cargodmod.TitleFont = Font.RockstarTag;
            cardrivein.TitleFont = Font.RockstarTag;
            cardrivein.AltTitleFont = Font.RockstarTag;   
            vehiclespawn.TitleFont = Font.RockstarTag;
            vehiclepimp.TitleFont = Font.RockstarTag;

            //TELEPORT MENU
            TeleportMenu.BannerText.Color = Color.Brown;
            TeleportMenu.NameFont = Font.RockstarTag;
            TeleportMenu.Banner.Color = Color.Black;
            TeleportMenu.BannerText.Font = Font.Pricedown;
            TeleportMenu.DescriptionFont = Font.RockstarTag;
            teltoway.TitleFont = Font.RockstarTag;
            teltochiliad.TitleFont = Font.RockstarTag;
            teltopub1.TitleFont = Font.RockstarTag;
            teltofib.TitleFont = Font.RockstarTag;
            teltobell.TitleFont = Font.RockstarTag; 
            teltobank.TitleFont = Font.RockstarTag;
            teltopub2.TitleFont = Font.RockstarTag;
            teltosolomon.TitleFont = Font.RockstarTag;    
            teltooneil.TitleFont = Font.RockstarTag;  
            teltolife.TitleFont = Font.RockstarTag;
            teltolester.TitleFont = Font.RockstarTag;
            teltofloyd.TitleFont = Font.RockstarTag;

            //WEATHER MENU


          

            //NORTH MENU
            Northmenu.BannerText.Color = Color.Brown;
            Northmenu.Banner.Color = Color.Black;
            Northmenu.NameFont = Font.RockstarTag; 
            Northmenu.BannerText.Font = Font.Pricedown;
            Northmenu.DescriptionFont = Font.RockstarTag;
            loadnorth.TitleFont = Font.RockstarTag;
            unloadnorth.TitleFont = Font.RockstarTag;

            //MAIN MENU
            DemoMenu.Banner.Color = Color.Purple;
            DemoMenu.NameFont = Font.RockstarTag;
            DemoMenu.DescriptionFont = Font.RockstarTag;
            DemoMenu.BannerText.Font = Font.Pricedown;
            //DemoMenu.SetBanner("~HUD_COLOUR_GOLD~MOD MENU"); // Colore oro usando codici GTA


            DemoSubMenuPed.Banner.Color = Color.Purple;
            DemoSubMenuPed.BannerText.Color = Color.Brown;
            
            //MODEL CHANGER
            Franklins.TitleFont = Font.RockstarTag;



            //========================================= END STYLE===============================================================







        //How you order items below doesn't matter
            DemoPool.Add(DemoMenu); // The pool is container for your menus, add the menu
            DemoPool.Add(SelfMenu);
            DemoPool.Add(VehicleMenu);
            DemoPool.Add(TeleportMenu);
            DemoPool.Add(WeatherMenu);
            DemoPool.Add(BetaMenu);
      



            //DemoPool.Add(DemoSubMenuPed); // add first submenu
             DemoPool.Add(ModelMenuPed);//Model Changer
            DemoPool.Add(Northmenu);//NorthMenu
            DemoPool.Add(MoneyMenu);//MoneyDropMenu
            

            //Main Menu Categories
            DemoMenu.AddSubMenu(SelfMenu);
            DemoMenu.AddSubMenu(VehicleMenu);
            DemoMenu.AddSubMenu(TeleportMenu);
            DemoMenu.AddSubMenu(WeatherMenu);
            DemoMenu.AddSubMenu(BetaMenu);

            //Submenu Categories

            //Teleport SubMenu
            TeleportMenu.AddSubMenu(Northmenu);
            Northmenu.Add(loadnorth);
            Northmenu.Add(unloadnorth);


            //Beta Menu
            //BetaMenu.Add
            BetaMenu.Add(drunkmode);
            BetaMenu.Add(nocollmode);
            BetaMenu.Add(chaosmode);

            //Self Menu
            SelfMenu.Add(godmode1);
            SelfMenu.Add(maxhealth);
            SelfMenu.Add(neverwanted);
            SelfMenu.Add(giveallwep);
            SelfMenu.Add(givemaxammo);
            SelfMenu.Add(infiniteammo);
            SelfMenu.Add(superjump);
            SelfMenu.Add(invisible);
            SelfMenu.Add(fastrun);
            SelfMenu.AddSubMenu(MoneyMenu);
            MoneyMenu.Add(money40k);
            MoneyMenu.Add(money1milion);
            MoneyMenu.Add(money10milion);
            MoneyMenu.Add(money15milion);
            MoneyMenu.Add(money20milion);
            MoneyMenu.Add(money100milion);

            //Vehicle Menu
            VehicleMenu.Add(fixvehicle);
            VehicleMenu.Add(cargodmod);
            VehicleMenu.Add(cardrivein);
            VehicleMenu.Add(vehiclespawn);
            VehicleMenu.Add(vehiclepimp);
            //VehicleMenu.Add(itemSpawnVehicle);

            

            //Teleport Menu
            TeleportMenu.Add(teltoway);
            TeleportMenu.Add(teltochiliad);
            TeleportMenu.Add(teltopub1);
            TeleportMenu.Add(teltopub2);
            TeleportMenu.Add(teltobank);
            TeleportMenu.Add(teltobell);
            TeleportMenu.Add(teltofib);
            TeleportMenu.Add(teltofloyd);
            TeleportMenu.Add(teltolife);
            TeleportMenu.Add(teltolester);
            TeleportMenu.Add(teltooneil);
            TeleportMenu.Add(teltosolomon);

            //WeatherMenu
            WeatherMenu.Add(ClearWeather);
            WeatherMenu.Add(ClearingWeather);
            WeatherMenu.Add(SunnyWeather);
            WeatherMenu.Add(CloudWeather);
            WeatherMenu.Add(SmogWeather);
            WeatherMenu.Add(FoggyWeather);
            WeatherMenu.Add(NeutralWeather);
            WeatherMenu.Add(HalloweenWeather);
            WeatherMenu.Add(BlizzardWeather);
            WeatherMenu.Add(RainingWeather);
            WeatherMenu.Add(OvercastWeather);
            WeatherMenu.Add(ThunderWeather);
            WeatherMenu.Add(ChristmasWeather);
            WeatherMenu.Add(SnowingWeather);
            WeatherMenu.Add(SnowlightWeather);
            WeatherMenu.Add(XmasWeather);


            //standalone items to add to menu

            //DemoMenu.Add(CarGuy);

            //Items for SubMenu Ped
            //DemoMenu.AddSubMenu(DemoSubMenuPed); //the submenu
            //DemoSubMenuPed.Add(Ballas); // item 1
            //DemoSubMenuPed.Add(LostMCGang); // item 2

            DemoMenu.AddSubMenu(ModelMenuPed);
            ModelMenuPed.Add(Franklins);

            //Misc Items, these are buttons/standalone (menu items without sub menus)
            CarGuy.Activated += SetModelCarGuy;

            //Item activation Beta
            drunkmode.Activated += SetDrunk;
            nocollmode.Activated += NoColl;
            chaosmode.Activated += SetChaos;

            //Item activation Self
            Ballas.Activated += SpawnBallas;
            LostMCGang.Activated += SpawnLostMC;
            Franklins.Activated += SpawnFrank;
            godmode1.Activated += SetGodMode;
            maxhealth.Activated += SetMaxHealth;
            neverwanted.Activated += SetNeverWanted;
            giveallwep.Activated += SetGiveAllWep;
            givemaxammo.Activated += SetMaxAmmo;
            infiniteammo.Activated += SetInfAmmo;
            superjump.Activated += SetSuperJump;
            invisible.Activated += SetInvisible;
            fastrun.Activated += SetFastrun;

            //Item Att MoneyMenu SELFMENU
            money40k.Activated += SetMoneyDrop40k;
            money1milion.Activated += SetMoneyDrop1Mil;
            money10milion.Activated += SetMoneyDrop10Mil;
            money15milion.Activated += SetMoneyDrop15Mil;
            money20milion.Activated += SetMoneyDrop20Mil;
            money100milion.Activated += SetMoneyDrop100Mil;

            //Item Att Vehicle
            fixvehicle.Activated += SetFixCar;
            cargodmod.Activated += SetCarGodMod;
            cardrivein.Activated += SetCarDriveIn;
            vehiclespawn.Activated += SetCarSpawn;
            vehiclepimp.Activated += SetCarMax;



            //Item Att Teleport
            teltoway.Activated += SetTelWay;
            teltochiliad.Activated += SetTelChilliad;
            teltopub1.Activated += SetTequilala;
            teltopub2.Activated += SetBahama;
            teltobank.Activated += SetBank;
            teltobell.Activated += SetBell;
            teltofib.Activated += SetFib;
            teltofloyd.Activated += SetFloydHouse;
            teltolife.Activated += SetLifeInv;
            teltolester.Activated += SetLester;
            teltosolomon.Activated += SetSolomon;
            teltooneil.Activated += SetOneil;

            //Item Att WEATHERMENU
            ClearWeather.Activated += SetWeatherClear;
            ClearingWeather.Activated += SetWeatherClearing;
            SunnyWeather.Activated += SetWeatherSunny;
            CloudWeather.Activated += SetWeatherClouds;
            SmogWeather.Activated += SetWeatherSmog;
            FoggyWeather.Activated += SetWeatherFoggy;
            NeutralWeather.Activated += SetWeatherNeutral;
            HalloweenWeather.Activated += SetWeatherHalloween;
            BlizzardWeather.Activated += SetWeatherBlizzard;
            RainingWeather.Activated += SetWeatherRaining;
            OvercastWeather.Activated += SetWeatherOvercast;
            ThunderWeather.Activated += SetWeatherThunder;
            ChristmasWeather.Activated += SetWeatherChristmas;
            SnowingWeather.Activated += SetWeatherSnowing;
            SnowlightWeather.Activated += SetWeatherSnowlight;
            XmasWeather.Activated += SetWeatherXmas;

            //Item North Menu - TELEPORTMENU
            loadnorth.Activated += SetNorthLoad;
            unloadnorth.Activated += SetNorthUnload;
                
            //

            Tick += Basics_Tick;
            KeyDown += Basics_KeyDown; //line added to replace code in Basics_Tick
        }



  

        private void SetDrunk(object sender, EventArgs e)
        {
            if (drunkmode.Checked == true)
            {
                //(Function.Call(Hash.SET_PED_IS_DRUNK));
                Function.Call(Hash.REQUEST_ANIM_SET, "move_m@drunk@verydrunk");
                Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character.Handle, "move_m@drunk@verydrunk", 0x3E800000);
                
                //Function.Call(Hash.SET_ENTITY_COMPLETELY_DISABLE_COLLISION, Game.Player.Character);
            }

            if (drunkmode.Checked == false)
            {
                Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Game.Player.Character);
               // Function.Call(Hash.SET_ENTITY_COLLISION, Game.Player.Character);
               
            }

        }


        private void SetChaos(object sender, EventArgs e)
        {
            if (chaosmode.Checked == true)
            {
                Ped player = Game.Player.Character;

                //foreach (Ped ped in World.GetAllPeds())
                foreach (Ped ped in World.GetNearbyPeds(player, 500f)) // solo pedoni vicini
                {
                    if (ped == null || !ped.Exists() || ped.IsDead || ped == player)
                        continue;

                    // Salta i poliziotti o pedoni già in combattimento
                    if (ped.RelationshipGroup == Game.GenerateHash("COP") || ped.IsInCombat)
                        continue;

                    // Imposta la relazione a ostile
                    ped.RelationshipGroup = Game.GenerateHash("HATE_GROUP");
                    Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.GenerateHash("HATE_GROUP"), Game.Player.Character.RelationshipGroup);
                    Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.Player.Character.RelationshipGroup, Game.GenerateHash("HATE_GROUP"));

                    if (ped.IsInVehicle() && ped == ped.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver))
                    {
                        // Il pedone è il guidatore di un veicolo
                        Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD,
                                ped,
                                ped.CurrentVehicle,
                                player.Position.X,
                                player.Position.Y,
                                player.Position.Z,
                                60f,  // velocità
                                1,    // driving mode
                                ped.CurrentVehicle.Model.Hash,
                                1074528293, //786603 driving style: aggressive //1074528293 follia
                                10f,  // stopping range
                                5f    // straight line distance tolerance 
                          );
                    }


                    ped.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
                    

                    // Ordina al pedone di attaccare il giocatore
                    ped.Task.FightAgainst(player);
                }

                /*Ped player = Game.Player.Character;
                Vector3 playerPos = player.Position;

                foreach (Ped ped in World.GetNearbyPeds(player, 100f)) // solo pedoni vicini
                {
                    if (ped == null || !ped.Exists() || ped.IsDead || ped == player || convertedPeds.Contains(ped))
                        continue;

                    // Evita poliziotti e pedoni già in combattimento
                    if (ped.RelationshipGroup == Game.GenerateHash("COP") || ped.IsInCombat)
                        continue;

                    // Imposta relazione ostile
                    ped.RelationshipGroup = hostileGroupHash;
                    Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, hostileGroupHash, player.RelationshipGroup);
                    Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, player.RelationshipGroup, hostileGroupHash);

                    // Dai un'arma se non ne ha
                    if (!ped.IsArmed())
                    {
                        ped.Weapons.Give(WeaponHash.Pistol, 100, true, true);
                    }

                    // Attacca il giocatore
                    ped.Task.FightAgainst(player);
                    convertedPeds.Add(ped);
                }*/
            }

            if (chaosmode.Checked == false)
            {
                Ped player = Game.Player.Character;

                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped == null || !ped.Exists() || ped.IsDead || ped == player)
                        continue;

                    // Ripristina il gruppo relazionale dei pedoni (puoi usare "CIVMALE" o "CIVFEMALE")
                    ped.RelationshipGroup = Game.GenerateHash("CIVMALE");

                    // Ferma eventuali task aggressivi
                    ped.Task.ClearAll();
                }

                // Ripristina le relazioni tra i gruppi
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 0, Game.GenerateHash("HATE_GROUP"), Game.Player.Character.RelationshipGroup);
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 0, Game.Player.Character.RelationshipGroup, Game.GenerateHash("HATE_GROUP"));
            }
        }
        

        private void NoColl(object sender, EventArgs e)
        {
            if (nocollmode.Checked == true)
            {
                Game.Player.Character.IsCollisionEnabled = false;
                
            }

            if (nocollmode.Checked == false)
            {
                Game.Player.Character.IsCollisionEnabled = true;
            }
        }

        //godmode

        private void SetGodMode(object sender, EventArgs e)
        {
            if (godmode1.Checked == true)
            {
                Game.Player.Character.IsInvincible = true;
                
            }

            if (godmode1.Checked == false)
            {
                Game.Player.Character.IsInvincible = false;
            }
          
        }

        private void SetMaxHealth(object sender, EventArgs e)
        {
            Game.Player.Character.Health = 400;
        }

        private void SetNeverWanted(object sender, EventArgs e)
        {
            if (neverwanted.Checked == true)
            {
                Game.Player.WantedLevel = 0;
                Game.MaxWantedLevel = 0;

            }

            if (neverwanted.Checked == false)
            {
                 
                Game.MaxWantedLevel = 5;
            }

        }

        private void SetGiveAllWep(object sender, EventArgs e)
        {
            WeaponHash[] allWeaponHashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
            for (int i = 0; i < allWeaponHashes.Length; i++)
            {
                Game.Player.Character.Weapons.Give(allWeaponHashes[i], 9999, true, true);
            }
        }

        private void SetMaxAmmo(object sender, EventArgs e)
        {
            Ped PlayerONE = Game.Player.Character;
            Weapon currentweapon = PlayerONE.Weapons.Current;
            currentweapon.Ammo = currentweapon.MaxAmmo;
        }

        private void SetInfAmmo(object sender, EventArgs e)
        {
            if (infiniteammo.Checked == true)
            {
                Game.Player.Character.Weapons.Current.InfiniteAmmo = true;
                Game.Player.Character.Weapons.Current.InfiniteAmmoClip = true;

            }

            if (infiniteammo.Checked == false)
            {

                Game.Player.Character.Weapons.Current.InfiniteAmmo = false;
                Game.Player.Character.Weapons.Current.InfiniteAmmoClip = false;
                Ped PlayerONE = Game.Player.Character;
                Weapon currentweapon = PlayerONE.Weapons.Current;
                currentweapon.Ammo = currentweapon.MaxAmmo;
            }
        }

        private void SetSuperJump(object sender, EventArgs e)
        {
            if (superjump.Checked == true)
            {
                CanPlayerSuperJump = !CanPlayerSuperJump;
                //Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Game.Player);
            }

            if (superjump.Checked == false)
            {
                CanPlayerSuperJump = false;
            }
        }

        private void SetInvisible(object sender, EventArgs e)
        {
            if (invisible.Checked == true)
            {
                Game.Player.Character.IsVisible = false;
            }

            if (invisible.Checked == false)
            {
                Game.Player.Character.IsVisible = true;
            }
        }

        private void SetFastrun(object sender, EventArgs e)
        {
            if (fastrun.Checked == true)
            {
                canPlayerFastRun = !canPlayerFastRun;
            }

            if (fastrun.Checked == false)
            {
                canPlayerFastRun = false;
                Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Game.Player, 1.0f);
            }
        }

        private void SetMoneyDrop40k(object sender, EventArgs e)
        {
            if (money40k.Checked == true)
            {
                moneyDrop40kOn = !moneyDrop40kOn;
            }

            if (money40k.Checked == false)
            {
                moneyDrop40kOn = false;
            }
        }

        private void SetMoneyDrop1Mil(object sender, EventArgs e)
        {
            if (money1milion.Checked == true)
            {
                moneyDrop1MilOn = !moneyDrop1MilOn;
            }

            if (money1milion.Checked == false)
            {
                moneyDrop1MilOn = false;
            }
        }

        private void SetMoneyDrop10Mil(object sender, EventArgs e)
        {
            if (money10milion.Checked == true)
            {
                moneyDrop10MilOn = !moneyDrop10MilOn;
            }

            if (money10milion.Checked == false)
            {
                moneyDrop10MilOn = false;
            }
        }

        private void SetMoneyDrop15Mil(object sender, EventArgs e)
        {
            if (money15milion.Checked == true)
            {
                moneyDrop15MilOn = !moneyDrop15MilOn;
            }

            if (money15milion.Checked == false)
            {
                moneyDrop15MilOn = false;
            }
        }

        private void SetMoneyDrop20Mil(object sender, EventArgs e)
        {
            if (money20milion.Checked == true)
            {
                moneyDrop20MilOn = !moneyDrop20MilOn;
            }

            if (money20milion.Checked == false)
            {
                moneyDrop20MilOn = false;
            }
        }

        private void SetMoneyDrop100Mil(object sender, EventArgs e)
        {
            if (money100milion.Checked == true)
            {
                moneyDrop100MilOn = !moneyDrop100MilOn;
            }

            if (money100milion.Checked == false)
            {
                moneyDrop100MilOn = false;
            }
        }


        private void SetFixCar(object sender, EventArgs e)
        {
            Function.Call(Hash.SET_VEHICLE_FIXED, Game.Player.Character.CurrentVehicle);
            Function.Call(Hash.FIX_VEHICLE_WINDOW, Game.Player.Character.CurrentVehicle);
            Function.Call(Hash.SET_VEHICLE_DEFORMATION_FIXED, Game.Player.Character.CurrentVehicle);
            Function.Call(Hash.SET_VEHICLE_TYRE_FIXED, Game.Player.Character.CurrentVehicle);
        }

        private void SetCarGodMod(object sender, EventArgs e)
        {
            if (cargodmod.Checked == true)
            {
                Ped player2 = Game.Player.Character;

                if (!player2.IsInVehicle()) return;
                if (Game.Player.LastVehicle == null || !Game.Player.LastVehicle.Exists()) return;

                player2.CurrentVehicle.IsInvincible = true;
                player2.LastVehicle.IsInvincible = true;
            }

            if (cargodmod.Checked == false)
            {
                Ped player2 = Game.Player.Character;

                if (!player2.IsInVehicle()) return;
                if (Game.Player.LastVehicle == null || !Game.Player.LastVehicle.Exists()) return;

                player2.CurrentVehicle.IsInvincible = false;
                player2.LastVehicle.IsInvincible = false;
            }
        }

        private void SetCarDriveIn(object sender, EventArgs e)
        {
            if (cardrivein.Checked == true)
            {
                Ped player2 = Game.Player.Character;

                if (!player2.IsInVehicle()) return;
                if (Game.Player.LastVehicle == null || !Game.Player.LastVehicle.Exists()) return;
                
                Vehicle veh = Game.Player.Character.CurrentVehicle;
                Game.Player.Character.Task.DriveTo(veh, World.WaypointPosition, 0f, 200,DrivingStyle.IgnoreLights);
                //player2.CurrentVehicle.IsEngineRunning = false;
            }

            if (cardrivein.Checked == false)
            {
                Ped player2 = Game.Player.Character;

                if (!player2.IsInVehicle()) return;
                if (Game.Player.LastVehicle == null || !Game.Player.LastVehicle.Exists()) return;
                Game.Player.Character.Task.ClearAll();
            }
        }

      
        private void SetCarSpawn(object sender, EventArgs e)
        {
            Ped gamePed = Game.Player.Character;
            string modelName = Game.GetUserInput();
            Model model = new Model(modelName);
            model.Request();

            if (model.IsInCdImage && model.IsValid)
            {
                Vehicle v = World.CreateVehicle(model, gamePed.Position, gamePed.Heading);
                v.PlaceOnGround();
                gamePed.Task.WarpIntoVehicle(v, VehicleSeat.Driver);
            }

        }

        private void SetCarMax(object sender, EventArgs e)
        {

            if(Game.Player.Character.CurrentVehicle == null || !Game.Player.Character.CurrentVehicle.Exists()) return;


            Vehicle veh = Game.Player.Character.CurrentVehicle;
            veh.IsPersistent = true;
   
            veh.CanTiresBurst = false;
            veh.IsStolen = false;
            veh.DirtLevel = 0f;

            Game.Player.Character.CurrentVehicle.Mods.CustomPrimaryColor = Color.Gold;
            
            GTA.Native.Function.Call(Hash.SET_VEHICLE_MOD, veh, 1, 1, false);
            Function.Call(Hash.SET_VEHICLE_MOD,veh, 0, 0, false);
            Function.Call(Hash.SET_VEHICLE_MOD,veh, 11, 3, false);
            Function.Call(Hash.SET_VEHICLE_MOD,veh, 22, false);
            Function.Call(Hash.SET_VEHICLE_MAX_SPEED, veh, 200.0f);

        }

        private void SetTelWay(object sender, EventArgs e)
        {
            Player player = Game.Player;


            var markerPosition = World.WaypointPosition;
            var groundHeight = World.GetGroundHeight(markerPosition);

            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = markerPosition + (Vector3.WorldDown * 200.5f);
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = markerPosition + (Vector3.WorldDown * 200.5f);
            }





            Vector3 ToGround(Vector3 position)
            {
                position.Z = World.GetGroundHeight(new Vector2(position.X, position.Y));
                return new Vector3(position.X, position.Y, position.Z);
            }

            Vector3 GetWaypointCoords()
            {
                Vector3 pos = Function.Call<Vector3>(Hash.GET_BLIP_COORDS, Function.Call<Blip>(Hash.GET_FIRST_BLIP_INFO_ID, 8));

                if (Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE) && pos != null || pos != new Vector3(0, 0, 0))
                {
                    Vector3 WayPos = ToGround(pos);
                    if (WayPos.Z == 0 || WayPos.Z == 1)
                    {
                        WayPos = World.GetNextPositionOnStreet(WayPos);
                    
                    }
                    return WayPos;
                }
                else
                {
                 
                }
                return Game.Player.Character.Position;
            }

            Game.Player.Character.Position = GetWaypointCoords();
        }


        

        private void SetTelChilliad (object  sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(451.2820f, 5572.9897f, 796.6793f);
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(451.2820f, 5572.9897f, 796.6793f);
            }
        }

        private void SetBank(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(255.851f, 217.030f, 101.683f);
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(255.851f, 217.030f, 101.683f);
            }
        }

        private void SetTequilala(object sender, EventArgs e)
        {
            //IAA Office X: 117.220 Y: -620.938 Z: 206.047
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                //player.Character.Position = new Vector3(117.220f,-620.938f,06.047f);
                player.Character.Position = new Vector3(-556.5089111328125f, 286.318115234375f, 81.1763f);
                Function.Call(Hash.DISABLE_INTERIOR, Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -556.5089111328125, 286.318115234375, 81.1763), false);
                Function.Call(Hash.CAP_INTERIOR, Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -556.5089111328125, 286.318115234375, 81.1763), false);
                Function.Call(Hash.REQUEST_IPL, "v_rockclub");
                Function.Call(Hash.IS_DOOR_CLOSED,false, 993120320, -565.1712f, 276.6259f, 83.28626f, false, 0.0f, 0.0f, 0.0f);// front door not working
                Function.Call(Hash.IS_DOOR_CLOSED, false, -561.2866f, 293.5044f, 87.77851f, false, 0.0f, 0.0f, 0.0f);// back door not working

            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(255.851f, 217.030f, 101.683f);
            }
        }

        private void SetBahama(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-1388.0013427734375f, -618.419677734375f, 30.819599151611328f);
                Function.Call(Hash.DISABLE_INTERIOR, Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1388.0013427734375, -618.419677734375, 30.819599151611328), false);
                Function.Call(Hash.REQUEST_IPL, "v_bahama");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-1388.0013427734375f, -618.419677734375f, 30.819599151611328f);

            }
        }

        private void SetBell(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-72.68752f, 6253.72656f, 31.08991f);
                Function.Call(Hash.REQUEST_IPL, "CS1_02_cf_onmission1");
                Function.Call(Hash.REQUEST_IPL, "CS1_02_cf_onmission2");
                Function.Call(Hash.REQUEST_IPL, "CS1_02_cf_onmission3");
                Function.Call(Hash.REQUEST_IPL, "CS1_02_cf_onmission4");
                Function.Call(Hash.REMOVE_IPL, "CS1_02_cf_offmission");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-72.68752f, 6253.72656f, 31.08991f);

            }
        }

        private void SetFib(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(110.4f, -744.2f, 45.7f);
                Function.Call(Hash.REQUEST_IPL, "FIBlobby");
                Function.Call(Hash.REMOVE_IPL, "FIBlobbyfake");
               // Function.Call(Hash._DOOR_CONTROL, -1517873911, 106.3793f, -742.6982f, 46.51962f, false, 0.0f, 0.0f, 0.0f);
               // Function.Call(Hash._DOOR_CONTROL, -90456267, 105.7607f, -746.646f, 46.18266f, false, 0.0f, 0.0f, 0.0f);
            }

          
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(110.4f, -744.2f, 45.7f);

            }
        }

        private void SetFloydHouse(object sender, EventArgs e)
        {
            
                Player player = Game.Player;
                if (!player.Character.IsInVehicle())
                {
                    player.Character.Position = new Vector3(-1149.709f, -1521.088f, 10.78267f);
                    Function.Call(Hash.REMOVE_IPL, "vb_30_crimetape");
                    //Function.Call(Hash._DOOR_CONTROL, -607040053, -1149.709f, -1521.088f, 10.78267f, false, 0.0f, 0.0f, 0.0f);
                }
                else
                {
                    Vehicle v = player.Character.CurrentVehicle;
                    v.Position = new Vector3(-1149.709f, -1521.088f, 10.78267f);


                }
            
        }

        private void SetLifeInv(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-1047.9f, -233.0f, 39.0f);
                Function.Call(Hash.REQUEST_IPL, "facelobby");  // lifeinvader
                Function.Call(Hash.REMOVE_IPL, "facelobbyfake");
                //Function.Call(Hash._DOOR_CONTROL, -340230128, -1042.518, -240.6915, 38.11796, true, 0.0f, 0.0f, -1.0f);
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-1047.9f, -233.0f, 39.0f);

            }
        }

        private void SetLester(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(1274.933837890625f, -1714.7255859375f, 53.77149963378906f);
                Function.Call(Hash.DISABLE_INTERIOR, Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1274.933837890625, -1714.7255859375, 53.77149963378906), false);
                Function.Call(Hash.REQUEST_IPL, "v_lesters");
                //Function.Call(Hash._DOOR_CONTROL, 1145337974, 1273.816f, -1720.697f, 54.92143f, false, 0.0f, 0.0f, 0.0f);
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(1274.933837890625f, -1714.7255859375f, 53.77149963378906f);

            }
        }

        private void SetOneil(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(2441.2f, 4968.5f, 51.7f);
                Function.Call(Hash.REMOVE_IPL, "farm_burnt");
                Function.Call(Hash.REMOVE_IPL, "farm_burnt_lod");
                Function.Call(Hash.REMOVE_IPL, "farm_burnt_props");
                Function.Call(Hash.REMOVE_IPL, "farmint_cap");
                Function.Call(Hash.REMOVE_IPL, "farmint_cap_lod");
                Function.Call(Hash.REQUEST_IPL, "farm");
                Function.Call(Hash.REQUEST_IPL, "farmint");
                Function.Call(Hash.REQUEST_IPL, "farm_lod");
                Function.Call(Hash.REQUEST_IPL, "farm_props");

            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(2441.2f, 4968.5f, 51.7f);

            }
        }

        private void SetSolomon(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-1005.663208f, -478.3460998535156f, 49.0265f);
                Function.Call(Hash.DISABLE_INTERIOR, Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1005.663208f, -478.3460998535156f, 49.0265f), false);
                Function.Call(Hash.REQUEST_IPL, "v_58_sol_office");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-1005.663208f, -478.3460998535156f, 49.0265f);

            }
        }

        private void SetNorthLoad(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                
                                  
                Function.Call(Hash.REQUEST_IPL, "plg_01");
                Function.Call(Hash.REQUEST_IPL, "prologue01");
                Function.Call(Hash.REQUEST_IPL, "prologue01_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01c");
                Function.Call(Hash.REQUEST_IPL, "prologue01c_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01d");
                Function.Call(Hash.REQUEST_IPL, "prologue01d_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01f");
                Function.Call(Hash.REQUEST_IPL, "prologue01f_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01g");
                Function.Call(Hash.REQUEST_IPL, "prologue01h");
                Function.Call(Hash.REQUEST_IPL, "prologue01h_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01i");
                Function.Call(Hash.REQUEST_IPL, "prologue01i_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01j");
                Function.Call(Hash.REQUEST_IPL, "prologue01j_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01k");
                Function.Call(Hash.REQUEST_IPL, "prologue01k_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01z");
                Function.Call(Hash.REQUEST_IPL, "prologue01z_lod");
                Function.Call(Hash.REQUEST_IPL, "plg_02");
                Function.Call(Hash.REQUEST_IPL, "prologue02");
                Function.Call(Hash.REQUEST_IPL, "prologue02_lod");
                Function.Call(Hash.REQUEST_IPL, "plg_03");
                Function.Call(Hash.REQUEST_IPL, "prologue03");
                Function.Call(Hash.REQUEST_IPL, "prologue03_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue03b");
                Function.Call(Hash.REQUEST_IPL, "prologue03b_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue03_grv_dug");
                Function.Call(Hash.REQUEST_IPL, "prologue03_grv_dug_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue03b");
                Function.Call(Hash.REQUEST_IPL, "prologue0_grv_torch");
                Function.Call(Hash.REQUEST_IPL, "plg_04");
                Function.Call(Hash.REQUEST_IPL, "prologue04");
                Function.Call(Hash.REQUEST_IPL, "prologue04_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue04b");
                Function.Call(Hash.REQUEST_IPL, "prologue04b_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue04_cover");
                Function.Call(Hash.REQUEST_IPL, "des_potree_end");
                Function.Call(Hash.REQUEST_IPL, "des_potree_start");
                Function.Call(Hash.REQUEST_IPL, "des_potree_start_lod");
                Function.Call(Hash.REQUEST_IPL, "plg_05");
                Function.Call(Hash.REQUEST_IPL, "prologue05");
                Function.Call(Hash.REQUEST_IPL, "prologue05_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue05b");
                Function.Call(Hash.REQUEST_IPL, "prologue05b_lod");
                Function.Call(Hash.REQUEST_IPL, "plg_06");
                Function.Call(Hash.REQUEST_IPL, "prologue06");
                Function.Call(Hash.REQUEST_IPL, "prologue06_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue06b");
                Function.Call(Hash.REQUEST_IPL, "prologue06b_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue06_int");
                Function.Call(Hash.REQUEST_IPL, "prologue06_int_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue06_pannel");
                Function.Call(Hash.REQUEST_IPL, "prologue06_pannel_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue_m2_door");
                Function.Call(Hash.REQUEST_IPL, "prologue_m2_door_lod");
                Function.Call(Hash.REQUEST_IPL, "plg_occl_00");
                Function.Call(Hash.REQUEST_IPL, "prologue_occl");
                Function.Call(Hash.REQUEST_IPL, "plg_rd");
                Function.Call(Hash.REQUEST_IPL, "prologuerd");
                Function.Call(Hash.REQUEST_IPL, "prologuerdb");
                Function.Call(Hash.REQUEST_IPL, "prologuerd_lod");
        {
            // Carica IPL di North Yankton
            /*Function.Call(Hash.REQUEST_IPL, "prologue01");
            Function.Call(Hash.REQUEST_IPL, "prologue01_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01c");
            Function.Call(Hash.REQUEST_IPL, "prologue01c_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01d");
            Function.Call(Hash.REQUEST_IPL, "prologue01d_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01e");
            Function.Call(Hash.REQUEST_IPL, "prologue01e_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01f");
            Function.Call(Hash.REQUEST_IPL, "prologue01f_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01g");
            Function.Call(Hash.REQUEST_IPL, "prologue01g_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01h");
            Function.Call(Hash.REQUEST_IPL, "prologue01h_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01i");
            Function.Call(Hash.REQUEST_IPL, "prologue01i_lod");
            Function.Call(Hash.REQUEST_IPL, "prologue01j");
            Function.Call(Hash.REQUEST_IPL, "prologue01j_lod");*/

                    Vector3 yanktonCoords = new Vector3(3217.0f, -4834.0f, 111.0f); // posizione centrale
                    Game.Player.Character.Position = yanktonCoords;
                    //player.Character.Position = new Vector3(3360.19f, -4849.67f, 111.8f);

                }


    }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(3360.19f, -4849.67f, 111.8f);

            }
        }

        private void SetNorthUnload(object sender, EventArgs e)
        {
            Function.Call(Hash.REMOVE_IPL, "plg_01");
            Function.Call(Hash.REMOVE_IPL, "prologue01");
            Function.Call(Hash.REMOVE_IPL, "prologue01_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01c");
            Function.Call(Hash.REMOVE_IPL, "prologue01c_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01d");
            Function.Call(Hash.REMOVE_IPL, "prologue01d_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01f");
            Function.Call(Hash.REMOVE_IPL, "prologue01f_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01g");
            Function.Call(Hash.REMOVE_IPL, "prologue01h");
            Function.Call(Hash.REMOVE_IPL, "prologue01h_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01i");
            Function.Call(Hash.REMOVE_IPL, "prologue01i_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01j");
            Function.Call(Hash.REMOVE_IPL, "prologue01j_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01k");
            Function.Call(Hash.REMOVE_IPL, "prologue01k_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue01z");
            Function.Call(Hash.REMOVE_IPL, "prologue01z_lod");
            Function.Call(Hash.REMOVE_IPL, "plg_02");
            Function.Call(Hash.REMOVE_IPL, "prologue02");
            Function.Call(Hash.REMOVE_IPL, "prologue02_lod");
            Function.Call(Hash.REMOVE_IPL, "plg_03");
            Function.Call(Hash.REMOVE_IPL, "prologue03");
            Function.Call(Hash.REMOVE_IPL, "prologue03_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue03b");
            Function.Call(Hash.REMOVE_IPL, "prologue03b_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue03_grv_dug");
            Function.Call(Hash.REMOVE_IPL, "prologue03_grv_dug_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue03b");
            Function.Call(Hash.REMOVE_IPL, "prologue0_grv_torch");
            Function.Call(Hash.REMOVE_IPL, "plg_04");
            Function.Call(Hash.REMOVE_IPL, "prologue04");
            Function.Call(Hash.REMOVE_IPL, "prologue04_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue04b");
            Function.Call(Hash.REMOVE_IPL, "prologue04b_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue04_cover");
            Function.Call(Hash.REMOVE_IPL, "des_potree_end");
            Function.Call(Hash.REMOVE_IPL, "des_potree_start");
            Function.Call(Hash.REMOVE_IPL, "des_potree_start_lod");
            Function.Call(Hash.REMOVE_IPL, "plg_05");
            Function.Call(Hash.REMOVE_IPL, "prologue05");
            Function.Call(Hash.REMOVE_IPL, "prologue05_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue05b");
            Function.Call(Hash.REMOVE_IPL, "prologue05b_lod");
            Function.Call(Hash.REMOVE_IPL, "plg_06");
            Function.Call(Hash.REMOVE_IPL, "prologue06");
            Function.Call(Hash.REMOVE_IPL, "prologue06_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue06b");
            Function.Call(Hash.REMOVE_IPL, "prologue06b_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue06_int");
            Function.Call(Hash.REMOVE_IPL, "prologue06_int_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue06_pannel");
            Function.Call(Hash.REMOVE_IPL, "prologue06_pannel_lod");
            Function.Call(Hash.REMOVE_IPL, "prologue_m2_door");
            Function.Call(Hash.REMOVE_IPL, "prologue_m2_door_lod");
            Function.Call(Hash.REMOVE_IPL, "plg_occl_00");
            Function.Call(Hash.REMOVE_IPL, "prologue_occl");
            Function.Call(Hash.REMOVE_IPL, "plg_rd");
            Function.Call(Hash.REMOVE_IPL, "prologuerd");
            Function.Call(Hash.REMOVE_IPL, "prologuerdb");
            Function.Call(Hash.REMOVE_IPL, "prologuerd_lod");

        }

   


 


        private void SetWeatherClear(object sender, EventArgs e)
        { 
            World.Weather = Weather.Clear;
        }
        private void SetWeatherClearing(object sender, EventArgs e)
        {
            World.Weather = Weather.Clearing;
        }
        private void SetWeatherSunny(object sender, EventArgs e)
        {
            World.Weather = Weather.ExtraSunny;
        }
        private void SetWeatherClouds(object sender, EventArgs e)
        {
            World.Weather = Weather.Clouds;
        }
        private void SetWeatherSmog(object sender, EventArgs e)
        {
            World.Weather = Weather.Smog;
        }
        private void SetWeatherFoggy(object sender, EventArgs e)
        {
            World.Weather = Weather.Foggy;
        }
        private void SetWeatherNeutral(object sender, EventArgs e)
        {
            World.Weather = Weather.Neutral;
        }
        private void SetWeatherHalloween(object sender, EventArgs e)
        {
            World.Weather = Weather.Halloween;
        }
        private void SetWeatherBlizzard(object sender, EventArgs e)
        {
            World.Weather = Weather.Blizzard;
        }
        private void SetWeatherRaining(object sender, EventArgs e)
        {
            World.Weather = Weather.Raining;
        }
        private void SetWeatherOvercast(object sender, EventArgs e)
        {
            World.Weather = Weather.Overcast;
        }
        private void SetWeatherThunder(object sender, EventArgs e)
        {
            World.Weather = Weather.ThunderStorm;
        }
        private void SetWeatherChristmas(object sender, EventArgs e)
        {
            World.Weather = Weather.Christmas;
        }
        private void SetWeatherSnowing(object sender, EventArgs e)
        {
            World.Weather = Weather.Snowing;
        }
        private void SetWeatherSnowlight(object sender, EventArgs e)
        {
            World.Weather = Weather.Snowlight;
        }

        private void SetWeatherXmas(object sender, EventArgs e)
        {
            if (XmasWeather.Checked == true)
            {
                Function.Call(Hash.REQUEST_IPL, "snow_maps");
                Function.Call(Hash.REQUEST_IPL, "snow_grassmap");
                Function.Call(Hash.REQUEST_IPL, "prologue01");
                Function.Call(Hash.REQUEST_IPL, "prologue01_lod");
                Function.Call(Hash.REQUEST_IPL, "prologue01c");
                Function.Call(Hash.REQUEST_IPL, "prologue01c_lod");
                Function.Call(Hash.SET_WEATHER_TYPE_OVERTIME_PERSIST, "XMAS");
                Function.Call(Hash.SET_WEATHER_TYPE_NOW_PERSIST, "XMAS");
                Function.Call(Hash.SET_WEATHER_TYPE_NOW, "XMAS");
                Function.Call((Hash)0xAEEDAD1420C65CC0, true); // SET_FORCE_PED_FOOTSTEPS_TRACKS
                Function.Call((Hash)0x4CC7F0FEA5283FE0, true); // SET_FORCE_VEHICLE_TRAILS

            }

            if (XmasWeather.Checked == false)
            {
                Function.Call(Hash.REMOVE_IPL, "snow_maps");
                Function.Call(Hash.REMOVE_IPL, "snow_grassmap");
                Function.Call(Hash.REMOVE_IPL, "prologue01");
                Function.Call(Hash.REMOVE_IPL, "prologue01_lod");
                Function.Call(Hash.REMOVE_IPL, "prologue01c");
                Function.Call(Hash.REMOVE_IPL, "prologue01c_lod");
                Function.Call(Hash.SET_WEATHER_TYPE_OVERTIME_PERSIST, "CLEAR");
                Function.Call(Hash.SET_WEATHER_TYPE_NOW_PERSIST, "CLEAR");
                Function.Call(Hash.SET_WEATHER_TYPE_NOW, "CLEAR");
                Function.Call((Hash)0xAEEDAD1420C65CC0, false);
                Function.Call((Hash)0x4CC7F0FEA5283FE0, false);

            }
        }






        private void SetModelCarGuy(object sender, EventArgs e)
        { Game.Player.ChangeModel(PedHash.Car3Guy2); }

        private void SpawnLostMC(object sender, EventArgs e)
        { Ped PedLosHair = World.CreatePed(PedHash.Lost01GFY, Game.Player.Character.GetOffsetPosition(new Vector3(1, 5, 0)));
        }

        private void SpawnBallas(object sender, EventArgs e)
        { Ped PedFamily = World.CreatePed(PedHash.Families01GFY, Game.Player.Character.GetOffsetPosition(new Vector3(1, 3, 0))); }

        private void SpawnVagos(object sender, EventArgs e)
        { var VALead = World.CreatePed(PedHash.Vagos01GFY, Game.Player.Character.GetOffsetPosition(new Vector3(1, 2, 0))); }

        private void SpawnFrank(object sender, EventArgs e)
        {
            Ped gamePed = Game.Player.Character;
            Game.Player.ChangeModel(PedHash.Franklin);
        }

        private void Basics_Tick(object sender, EventArgs e)
        {
            
            DemoPool.Process();

            if (CanPlayerSuperJump)
            {
                Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Game.Player);
            }

            if (canPlayerFastRun)
            {
                Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Game.Player, 1.49f);

                if (Game.Player.Character.IsJumping)
                {
                    Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Game.Player, true, 0, 0, 10, 0, 0, 0, true, true, true, true, false, true);
                }

            }


            if (moneyDrop40kOn)
            {
                Vector3 pos = Game.Player.Character.Position;
                var hash = Function.Call<int>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
                var model = new Model(0x113FD533); // prop_money_bag_01
                model.Request(1000);
                Function.Call(Hash.CREATE_AMBIENT_PICKUP, hash, pos.X, pos.Y, pos.Z, 0, 40000, 0x113FD533, false, true);
                model.MarkAsNoLongerNeeded();
            }

            if (moneyDrop1MilOn)
            {
                Vector3 pos = Game.Player.Character.Position;
                var hash = Function.Call<int>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
                var model = new Model(0x113FD533); // prop_money_bag_01
                model.Request(1000);
                Function.Call(Hash.CREATE_AMBIENT_PICKUP, hash, pos.X, pos.Y, pos.Z, 0, 1000000, 0x113FD533, false, true);
                model.MarkAsNoLongerNeeded();
            }


            if (moneyDrop10MilOn)
            {
                Vector3 pos = Game.Player.Character.Position;
                var hash = Function.Call<int>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
                var model = new Model(0x113FD533); // prop_money_bag_01
                model.Request(1000);
                Function.Call(Hash.CREATE_AMBIENT_PICKUP, hash, pos.X, pos.Y, pos.Z, 0, 10000000, 0x113FD533, false, true);
                model.MarkAsNoLongerNeeded();
            }

            if (moneyDrop15MilOn)
            {
                Vector3 pos = Game.Player.Character.Position;
                var hash = Function.Call<int>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
                var model = new Model(0x113FD533); // prop_money_bag_01
                model.Request(1000);
                Function.Call(Hash.CREATE_AMBIENT_PICKUP, hash, pos.X, pos.Y, pos.Z, 0, 15000000, 0x113FD533, false, true);
                model.MarkAsNoLongerNeeded();
            }

            if (moneyDrop20MilOn)
            {
                Vector3 pos = Game.Player.Character.Position;
                var hash = Function.Call<int>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
                var model = new Model(0x113FD533); // prop_money_bag_01
                model.Request(1000);
                Function.Call(Hash.CREATE_AMBIENT_PICKUP, hash, pos.X, pos.Y, pos.Z, 0, 20000000, 0x113FD533, false, true);
                model.MarkAsNoLongerNeeded();
            }

            if (moneyDrop100MilOn)
            {
                Vector3 pos = Game.Player.Character.Position;
                var hash = Function.Call<int>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
                var model = new Model(0x113FD533); // prop_money_bag_01
                model.Request(1000);
                Function.Call(Hash.CREATE_AMBIENT_PICKUP, hash, pos.X, pos.Y, pos.Z, 0, 100000000, 0x113FD533, false, true);
                model.MarkAsNoLongerNeeded();
            }

        }

       




        private void Basics_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F6) //put whatever key binding you want here
            {
                DemoMenu.Visible = !DemoMenu.Visible;
                //DemoMenu.Visible = true;
 
                //GTA.UI.Notification.Show("Welcome",true);
                GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Call911,"Anonik","Welcome","Essential Menu 1.30", true,true);
               

 
            }
        }
    }
}
