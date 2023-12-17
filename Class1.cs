using System;
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
using NativeUI;
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

//uses lemon and SHVDN version 2, also added windows forms



namespace LemonUI.Menu1
{

    public class Basics : Script
    {

        public static bool CanPlayerSuperJump { get; set; }
        public static bool canPlayerFastRun { get; set; }

        private static readonly ObjectPool DemoPool = new ObjectPool();
        private static readonly NativeMenu DemoMenu = new NativeMenu(bqnnerText: "Essential Menu", name: "Made ~b~By Anonik v1.30", description: "Made ~b~By Anonik v1.30");




        //start of Top Level Items, at top of menu, not part of submenu
        private static readonly NativeMenu SelfMenu = new NativeMenu("Self Options", "Self Options", "");  //The first submenu
        private static readonly NativeMenu VehicleMenu = new NativeMenu("Vehicle Options", "Vehicle Options", "");  //The first submenu

        private static readonly NativeItem ClearWeather = new NativeItem("Clear Weather", "");
        private static readonly NativeItem CarGuy = new NativeItem("Change to Car Guy Model", "");

        //start of Submenu
        private static readonly NativeMenu DemoSubMenuPed = new NativeMenu("Peds", "Ped Gangs", "");  //The first submenu
        private static readonly NativeItem Ballas = new NativeItem("Ballas and Family", "Description"); //Item 1 of submenu
        private static readonly NativeItem LostMCGang = new NativeItem("Import Export", "Description"); //Item 2 of submenu

        private static readonly NativeMenu ModelMenuPed = new NativeMenu("Model Changer", "Model Changer","");
        private static readonly NativeItem Franklins = new NativeItem("Franklin Model", "");

        //Selfmenu

        private static readonly NativeCheckboxItem godmode1 = new NativeCheckboxItem("~b~God Mode.", "Description", false); // Enabled by Default with a Description
        private static readonly NativeItem maxhealth = new NativeItem("Max Health");
        private static readonly NativeCheckboxItem neverwanted = new NativeCheckboxItem("~b~Never Wanted", false);
        private static readonly NativeItem giveallwep = new NativeItem("Give All Weapons");
        private static readonly NativeItem givemaxammo = new NativeItem("Give Max Ammo");
        private static readonly NativeCheckboxItem infiniteammo = new NativeCheckboxItem("~r~Infinite Ammo", false);
        private static readonly NativeCheckboxItem superjump = new NativeCheckboxItem("Super Jump", false);
        private static readonly NativeCheckboxItem invisible = new NativeCheckboxItem("Invisible", false);
        private static readonly NativeCheckboxItem fastrun = new NativeCheckboxItem("Fast Run", false);

        //VehicleMenu

        private static readonly NativeItem fixvehicle = new NativeItem("Fix Car Health");
        private static readonly NativeCheckboxItem cargodmod = new NativeCheckboxItem("Invincible Car", false);
        private static readonly NativeCheckboxItem cardrivein = new NativeCheckboxItem("Car Autopilot","Autopilot", false);
        private static readonly NativeMenu Vehspw = new NativeMenu("Vehicle Spawn", "Vehicle spawner", "");  //The first submenu





        public Basics()
        {


            //==============================================STYLE===============================================================

            //SELF MENU
            SelfMenu.BannerText.Color = Color.Brown;
            SelfMenu.NameFont = Font.Monospace;
            SelfMenu.Banner.Color = Color.Black;
            SelfMenu.BannerText.Font = Font.Pricedown;
            SelfMenu.DescriptionFont = Font.Monospace;
            maxhealth.TitleFont = Font.Monospace;
            godmode1.TitleFont = Font.Monospace;
            neverwanted.TitleFont = Font.Monospace;
            giveallwep.TitleFont = Font.Monospace;
            givemaxammo.TitleFont = Font.Monospace;
            infiniteammo.TitleFont = Font.Monospace;
            superjump.TitleFont = Font.Monospace;
            invisible.TitleFont = Font.Monospace;  
            fastrun.TitleFont = Font.Monospace;

            //VEHICLE MENU
            VehicleMenu.BannerText.Color = Color.Brown;
            VehicleMenu.NameFont = Font.Monospace;
            VehicleMenu.Banner.Color = Color.Black;
            VehicleMenu.BannerText.Font = Font.Pricedown;
            VehicleMenu.DescriptionFont = Font.Monospace;
            fixvehicle.TitleFont = Font.Monospace;
            cargodmod.TitleFont = Font.Monospace;
            cardrivein.TitleFont = Font.Monospace;
            cardrivein.AltTitleFont = Font.Monospace;   

            //MAIN MENU
            DemoMenu.Banner.Color = Color.Black;
            DemoMenu.NameFont = Font.Monospace;
            DemoMenu.DescriptionFont = Font.Monospace;
            DemoMenu.BannerText.Font = Font.Pricedown;


            DemoSubMenuPed.Banner.Color = Color.Black;
            DemoSubMenuPed.BannerText.Color = Color.Brown;
            
            //MODEL CHANGER
            Franklins.TitleFont = Font.Monospace;



            //========================================= END STYLE===============================================================



            //How you order items below doesn't matter
            DemoPool.Add(DemoMenu); // The pool is container for your menus, add the menu
            DemoPool.Add(SelfMenu);
            DemoPool.Add(VehicleMenu);
            DemoPool.Add(Vehspw);
            DemoPool.Add(DemoSubMenuPed); // add first submenu
             DemoPool.Add(ModelMenuPed);//Model Changer
            

            //Main Menu Categories
            DemoMenu.AddSubMenu(SelfMenu);//godmode
            DemoMenu.AddSubMenu(VehicleMenu);
            DemoMenu.AddSubMenu(Vehspw);

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

            //Vehicle Menu
            VehicleMenu.Add(fixvehicle);
            VehicleMenu.Add(cargodmod);
            VehicleMenu.Add(cardrivein);

     


            //standalone items to add to menu
            DemoMenu.Add(ClearWeather);
            DemoMenu.Add(CarGuy);

            //Items for SubMenu Ped
            DemoMenu.AddSubMenu(DemoSubMenuPed); //the submenu
            DemoSubMenuPed.Add(Ballas); // item 1
            DemoSubMenuPed.Add(LostMCGang); // item 2

            DemoMenu.AddSubMenu(ModelMenuPed);
            ModelMenuPed.Add(Franklins);

            //Misc Items, these are buttons/standalone (menu items without sub menus)
            CarGuy.Activated += SetModelCarGuy;
            ClearWeather.Activated += SetWeatherClear;

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

            //Item Att Vehicle
            fixvehicle.Activated += SetFixCar;
            cargodmod.Activated += SetCarGodMod;
            cardrivein.Activated += SetCarDriveIn;


            //

            Tick += Basics_Tick;
            KeyDown += Basics_KeyDown; //line added to replace code in Basics_Tick
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

        private void CreateVehicleSpawnerMenu()
        {
            foreach (VehicleHash vehicleHash in Enum.GetValues(typeof(VehicleHash)))
            {
                NativeItem itemSpawnVehicle = new NativeItem(vehicleHash.ToString(), $"Spawns a {vehicleHash} right in front of you!");
                itemSpawnVehicle.Activated += (sender, args) =>
                {
                    Ped character = Game.Player.Character;

                    Model vehicleModel = new Model(vehicleHash);
                    vehicleModel.Request();

                    Vehicle vehicle = World.CreateVehicle(vehicleModel, character.Position + character.ForwardVector * 3.0f, character.Heading + 90.0f);

                    vehicleModel.MarkAsNoLongerNeeded();

                    Notification.Show($"Vehicle: {vehicleHash} has been spawned!");
                };
                Vehspw.Add(itemSpawnVehicle);
            }
        }






        private void SetWeatherClear(object sender, EventArgs e)
        { 
            World.Weather = Weather.Clear;
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
        }


        

        private void Basics_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F6) //put whatever key binding you want here
            {
                DemoMenu.Visible = true;
                //GTA.UI.Notification.Show("Welcome",true);
                GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Call911,"Anonik","Welcome","Essential Menu 1.30", true,true);

 
            }
        }
    }
}
