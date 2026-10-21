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
using LemonUI;
using LemonUI.Tools;
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
using static LemonUI.Menu1.Basics;
using LemonUI.Scaleform;



//uses lemon and SHVDN version 2, also added windows forms



namespace LemonUI.Menu1
{

    public class Basics : Script
    {



        bool expowpon;
        bool expoml;
        bool moneygun;

        //CAYO PERICO
        bool cayoOn;
        bool cayoMpLoaded;

        //Meteor Shower
        bool meteorShowerOn;
        int meteorTimer1 = 0;
        int meteorTimer2 = 0;
        readonly Random meteorRnd = new Random();
        readonly List<Prop> rockArray = new List<Prop>();
        readonly List<Model> rockModels = new List<Model>();
        static readonly string[] rockModelNames =
        {
           "prop_asteroid_01", "prop_test_boulder_01", "prop_test_boulder_02",
            "prop_test_boulder_03", "prop_test_boulder_04"
         };
        const int AsteroidHash = unchecked((int)0xDF9841D7); // prop_asteroid_01
        const int DecalBangConcrete = 1010;                  // DecalType.Bang_concrete_bang



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
        private static readonly NativeMenu DemoMenu = new NativeMenu(bannerText: "Essential Menu", name: "Made By Anonik v1336 ~r~BETA", description: "Made By Anonik v1336 ~r~BETA");

        // Aggiungi i menu per lo Skin Changer
        private static readonly NativeMenu SkinChangerMenu = new NativeMenu("Skin Changer", "Change Player Model");
        private static readonly NativeMenu ProtagonistMenu = new NativeMenu("Protagonists", "Main Characters");
        private static readonly NativeMenu GangMenu = new NativeMenu("Gangs", "Gang Members");
        private static readonly NativeMenu CivMaleMenu = new NativeMenu("Male Civilians", "Male Civilians");
        private static readonly NativeMenu CivFemaleMenu = new NativeMenu("Female Civilians", "Female Civilians");
        private static readonly NativeMenu CopMenu = new NativeMenu("Police", "Law Enforcement");
        private static readonly NativeMenu EmergencyMenu = new NativeMenu("Emergency", "Medical/Fire");
        private static readonly NativeMenu MiscMenu = new NativeMenu("Miscellaneous", "Other Models");
        private static readonly NativeMenu AnimalMenu = new NativeMenu("Animals", "Animal Models");
        private static readonly NativeMenu StoryMenu = new NativeMenu("Story Characters", "Story Mode Characters");

 

        //start of Top Level Items, at top of menu, not part of submenu
        private static readonly NativeMenu SelfMenu = new NativeMenu("Self Options", "Self Options", "");  //The first submenu
        private static readonly NativeMenu VehicleMenu = new NativeMenu("Vehicle Options", "Vehicle Options", "");  //The first submenu
        private static readonly NativeMenu WeaponMenu = new NativeMenu("Weapon Options", "Weapon Options", "");  //The first submenu
        private static readonly NativeMenu TeleportMenu = new NativeMenu("Teleport Options", "Teleporting Options");
        private static readonly NativeMenu WeatherMenu = new NativeMenu("Weather Options", "Weather Options");//Weather Menu
        private static readonly NativeMenu VisionMenu = new NativeMenu("Vision Menu", "Vision Options");//Vision Menu
        private static readonly NativeMenu BetaMenu = new NativeMenu("[Beta Menu ]", "Beta Options");//Beta Menu
        private static readonly NativeMenu AnonikMenu = new NativeMenu("Anonik Menu", "Anonik Options");//Anonik Menu


        private static readonly NativeItem CarGuy = new NativeItem("Change to Car Guy Model", "");



        //start of Submenu
        private static readonly NativeMenu DemoSubMenuPed = new NativeMenu("Peds", "Ped Gangs", "");  //The first submenu
        private static readonly NativeItem Ballas = new NativeItem("Ballas and Family", "Description"); //Item 1 of submenu
        private static readonly NativeItem LostMCGang = new NativeItem("Import Export", "Description"); //Item 2 of submenu

       //private static readonly NativeMenu ModelMenuPed = new NativeMenu("Model Changer", "Model Changer","");
        //private static readonly NativeItem Franklins = new NativeItem("Franklin Model", "");


        //BetaMenu
        private static readonly NativeCheckboxItem drunkmode = new NativeCheckboxItem("~b~Drunk Mode", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem nocollmode = new NativeCheckboxItem("~b~No Collision", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem chaosmode = new NativeCheckboxItem("~r~Chaos Mode", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem blackoutmode = new NativeCheckboxItem("Blackout Mode", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem moongravmode = new NativeCheckboxItem("Moon Gravity Mode", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem meteorshowermode = new NativeCheckboxItem("Meteor Shower", "Description", false);




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
        // Skin Changer Menu

        //WeaponMenu
        private static readonly NativeCheckboxItem expoweap = new NativeCheckboxItem("Explosive Ammo", "Description", false); // Enabled by Default with a Description
        private static readonly NativeCheckboxItem expomelee = new NativeCheckboxItem("Explosive Melee", "Description", false);
        private static readonly NativeCheckboxItem moneygunop = new NativeCheckboxItem("Money Gun", "Description", false);

        //VehicleMenu

        private static readonly NativeItem fixvehicle = new NativeItem("Fix Car Health");
        private static readonly NativeCheckboxItem cargodmod = new NativeCheckboxItem("Invincible Car", false);
        private static readonly NativeCheckboxItem cardrivein = new NativeCheckboxItem("Car Autopilot","Autopilot", false);
        private static readonly NativeItem vehiclespawn = new NativeItem("Spawn Input Vehicle","Type a input name vehicle");
        private static readonly NativeItem vehiclepimp = new NativeItem("Max Car Stats");
        private static readonly NativeMenu colorMenu = new NativeMenu("Colori Veicolo", "Scegli un colore");
        private static readonly NativeItem colorMenuItem = new NativeItem("Change Color");
        private static readonly NativeMenu upgradeMenu = new NativeMenu("Upgrades", "Vehicle Tuning");
        private static readonly NativeItem fullUpgradeItem = new NativeItem("🔧 Max Upgrade");
        private static readonly NativeListItem<string> engineItem = new NativeListItem<string>("🔋 Motore", new[] { "Stock", "Level 1", "Level 2", "Level 3" });
        private static readonly NativeListItem<string> brakesItem = new NativeListItem<string>("🛑 Freni", new[] { "Stock", "Street", "Sport", "Race" });
        private static readonly NativeListItem<string> transmissionItem = new NativeListItem<string>("⚙️ Cambio", new[] { "Stock", "Street", "Sport", "Race" });
        private static readonly NativeListItem<string> suspensionItem = new NativeListItem<string>("🌀 Sospensioni", new[] { "Stock", "Lowered", "Street", "Sport", "Competition" });
        private static readonly NativeCheckboxItem turboItem = new NativeCheckboxItem("🚀 Turbo", false);




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
        private static readonly NativeItem teltoyatch = new NativeItem("Teleport to Yatch");
        private static readonly NativeItem teltotorture = new NativeItem("Teleport to Torture Room");
        private static readonly NativeItem teltoiaaoffice = new NativeItem("Teleport to IAA Office");
        private static readonly NativeItem teltohumlabentrance = new NativeItem("Teleport to Humane Labs Entrance");
        private static readonly NativeItem teltohumlabtunnel = new NativeItem("Teleport to Humane Labs Tunnel");
        private static readonly NativeItem teltoblainecountybank = new NativeItem("Teleport to Blaine County Bank");
        private static readonly NativeItem teltohospital = new NativeItem("Teleport to Hospital");
        private static readonly NativeItem teltomorgue = new NativeItem("Teleport to Coroner Morgue");

        //North Yankton
        private static readonly NativeMenu Northmenu = new NativeMenu("N-Y Loader", "North Yankton Loader");
        private static readonly NativeItem loadnorth = new NativeItem("Load North Yankton");
        private static readonly NativeItem unloadnorth = new NativeItem("Unload North Yankton");
        //End North Yankton

        //Online Teleport Menu
        private static readonly NativeMenu OnlineTeleportMenu = new NativeMenu("Online Teleport", "Online Teleport Locations");

        //Cayo Perico Submenu
        private static readonly NativeMenu CayoMenu = new NativeMenu("Cayo Perico Menu", "Cayo Perico Teleport Locations");
        private static readonly NativeCheckboxItem cayoloadmode = new NativeCheckboxItem("Load Cayo Perico", "To Exit First Unteleport and after uncheck", false);
        private static readonly NativeItem loadteltocayoperico = new NativeItem("Teleport Cayo Perico");
        private static readonly NativeItem unloadteltocayoperico = new NativeItem("Unteleport Cayo Perico");
        // End Cayo Perico Submenu
        
        
        //Lab Teleport
        private static readonly NativeItem teltomethfarm = new NativeItem("Teleport to Meth Farm");
        private static readonly NativeItem teltoweedlab = new NativeItem("Teleport to Weed Lab");
        private static readonly NativeItem teltococainelab = new NativeItem("Teleport to Cocaine Lab");
        private static readonly NativeItem teltocounterfeitlab = new NativeItem("Teleport to Counterfeit Lab");
        private static readonly NativeItem teltodocforgeryofficelab = new NativeItem("Teleport to Document Forgery Office");
        //End Lab Teleport

        //Club House Teleport
        private static readonly NativeItem teltoclub1 = new NativeItem("Teleport to Club House 1");
        private static readonly NativeItem teltoclub2 = new NativeItem("Teleport to Club House 2");
        //End Club House Teleport

        //Stores Teleport
        private static readonly NativeItem teltojewelstore = new NativeItem("Teleport to Jewel Store");
        private static readonly NativeItem teltomaxrendastorre = new NativeItem("Teleport to Max Renda Store");
        private static readonly NativeItem teltostadium = new NativeItem("Teleport to Stadium");
        private static readonly NativeItem teltouniondep = new NativeItem("Teleport to Union Depository");
        //End Stores Teleport

        //ONLINE APPARTMENT SUB MENU ====================================================================

        // MODERN HOUSE
        private static readonly NativeMenu OnlineHouseMenu = new NativeMenu("Online House Menu", "Online House");
        private static readonly NativeMenu ModernAppMenu = new NativeMenu("Modern Appartment","Modern Appartment");
        private static readonly NativeItem teltomodernhouse1 = new NativeItem("Teleport to Modernhouse1");
        private static readonly NativeItem teltomodernhouse2 = new NativeItem("Teleport to Modernhouse2");
        private static readonly NativeItem teltovibrant1 = new NativeItem("Vibrant 1 Apartment");
        private static readonly NativeItem teltovibrant2 = new NativeItem("Vibrant 2 Apartment");
        private static readonly NativeItem teltovibrant3 = new NativeItem("Vibrant 3 Apartment");
        private static readonly NativeItem teltosharp1 = new NativeItem("Sharp 1 Apartment");
        private static readonly NativeItem teltosharp2 = new NativeItem("Sharp 2 Apartment");
        private static readonly NativeItem teltosharp3 = new NativeItem("Sharp 3 Apartment");
        private static readonly NativeItem teltomonochrome1 = new NativeItem("Monochrome 1 Apartment");
        private static readonly NativeItem teltomonochrome2 = new NativeItem("Monochrome 2 Apartment");
        private static readonly NativeItem teltomonochrome3 = new NativeItem("Monochrome 3 Apartment");
        private static readonly NativeItem teltoseductive1 = new NativeItem("Seductive 1 Apartment");
        private static readonly NativeItem teltoseductive2 = new NativeItem("Seductive 2 Apartment");
        private static readonly NativeItem teltoseductive3 = new NativeItem("Seductive 3 Apartment");
        private static readonly NativeItem teltoregal1 = new NativeItem("Regal 1 Apartment");
        private static readonly NativeItem teltoregal2 = new NativeItem("Regal 2 Apartment");
        private static readonly NativeItem teltoregal3 = new NativeItem("Regal 3 Apartment");
        private static readonly NativeItem teltoaqua1 = new NativeItem("Aqua 1 Apartment");
        private static readonly NativeItem teltoaqua2 = new NativeItem("Aqua 2 Apartment");
        private static readonly NativeItem teltoaqua3 = new NativeItem("Aqua 3 Apartment");

        //END MODERN HOUSE

        //ARCADIUS INTERIOR
        private static readonly NativeMenu ArcadiusAppMenu = new NativeMenu("Arcadius Menu", "Arcadius Interiors");
        private static readonly NativeItem telexecrich = new NativeItem("Executive Rich");
        private static readonly NativeItem telexeccool = new NativeItem("Executive Cool");
        private static readonly NativeItem telexeccontrast = new NativeItem("Executive Contrast");
        private static readonly NativeItem teloldspicewarm = new NativeItem("Old Spice Warm");
        private static readonly NativeItem teloldspiceclassical = new NativeItem("Old Spice Classical");
        private static readonly NativeItem teloldspicevintage = new NativeItem("Old Spice Vintage");
        private static readonly NativeItem telpowerbrokerice = new NativeItem("Power Broker Ice");
        private static readonly NativeItem telpowerbrokerconservative = new NativeItem("Power Broker Conservative");
        private static readonly NativeItem telpowerbrokerpolished = new NativeItem("Power Broker Polished");
        private static readonly NativeItem telarcgarage1 = new NativeItem("Garage 1");
        private static readonly NativeItem telarcgarage2 = new NativeItem("Garage 2");
        private static readonly NativeItem telarcgarage3 = new NativeItem("Garage 3");
        private static readonly NativeItem telarcmodshop = new NativeItem("Mod Shop");

        //END ARCADIUS INTERIOR




        //ONLINE APPARTMENT END SUB MENU ================================================================

        //End Online Teleport Menu

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

        //VisionMenu
        private static readonly NativeCheckboxItem ThermalVision = new NativeCheckboxItem("Termal Vision", "",false);
        private static readonly NativeCheckboxItem NightVision = new NativeCheckboxItem("Night Vision", "", false);
        private static readonly NativeCheckboxItem MatrixVision = new NativeCheckboxItem("MatrixVision", "", false);
        private static readonly NativeCheckboxItem Ragevision = new NativeCheckboxItem("Rage Vision", "Description", false); // Enabled by Default with a Description

        //Vehicle Spawner Menu
        private static readonly NativeMenu VehicleSpawnerMenu = new NativeMenu("Vehicle Spawner", "Spawn Veicoli");
        private Dictionary<string, List<string>> vehicleCategories = new Dictionary<string, List<string>>();
        private string iniPath = Path.Combine("scripts", "vehicles.ini");

        //Model Spawner Menu
        private static readonly NativeMenu SkinSpawnerMenu = new NativeMenu("Skin Changer", "Change Skin");
        private Dictionary<string, List<string>> pedCategories = new Dictionary<string, List<string>>();
        private string iniPPath = Path.Combine("scripts", "pedlist.ini");

        //Bodyguard Menu
        private static readonly NativeMenu BodyGuardMenu = new NativeMenu("Bodyguard Menu", "Bodyguard Menu");
        private Dictionary<string, List<string>> bodyCategories = new Dictionary<string, List<string>>();
        private string iniPPPath = Path.Combine("scripts", "bguard.ini");
        private static readonly NativeItem RemoveBody = new NativeItem("Remove Bodyguard", "Remove all Bodyguard"); //Item 1 of submenu
        private static readonly NativeCheckboxItem BodyGodMode = new NativeCheckboxItem("Bodygard Invincible", "Bodyguard GodMode", false);
        private List<Ped> bodyguards = new List<Ped>();

        //Anonik Menu
        private static readonly NativeItem FunAnonikFunc = new NativeItem("Delete Anonik");

        public Basics()
        {

            // Aggiungi i nuovi menu al pool
            DemoPool.Add(SkinChangerMenu);
            DemoPool.Add(ProtagonistMenu);
            DemoPool.Add(GangMenu);
            DemoPool.Add(CivMaleMenu);
            DemoPool.Add(CivFemaleMenu);
            DemoPool.Add(CopMenu);
            DemoPool.Add(EmergencyMenu);
            DemoPool.Add(MiscMenu);
            DemoPool.Add(AnimalMenu);
            DemoPool.Add(StoryMenu);

            // Aggiungi SkinChangerMenu al menu principale
            //DemoMenu.AddSubMenu(SkinChangerMenu);

            // Configura la gerarchia dei menu per lo Skin Changer
            SkinChangerMenu.AddSubMenu(ProtagonistMenu);
            SkinChangerMenu.AddSubMenu(GangMenu);
            SkinChangerMenu.AddSubMenu(CivMaleMenu);
            SkinChangerMenu.AddSubMenu(CivFemaleMenu);
            SkinChangerMenu.AddSubMenu(CopMenu);
            SkinChangerMenu.AddSubMenu(EmergencyMenu);
            SkinChangerMenu.AddSubMenu(MiscMenu);
            SkinChangerMenu.AddSubMenu(AnimalMenu);
            SkinChangerMenu.AddSubMenu(StoryMenu);

            // Configura lo stile per i nuovi menu
            SkinChangerMenu.Banner.Color = Color.Purple;
            SkinChangerMenu.BannerText.Font = Font.Pricedown;
            SkinChangerMenu.NameFont = Font.Monospace;

            // Popola i menu con i modelli
            SetupSkinChanger();


            //==============================================STYLE===============================================================

            


            //SELF MENU
            //menu.BannerColor = System.Drawing.Color.FromArgb(180, 20, 20);
            SelfMenu.BannerText.Color = Color.Brown;
            SelfMenu.NameFont = Font.Monospace;
            SelfMenu.Banner.Color = Color.FromArgb(180, 20, 20);//Purple;
            SelfMenu.BannerText.Font = Font.Monospace;
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

            //MONEY MENU
            MoneyMenu.BannerText.Color = Color.Brown;
            MoneyMenu.NameFont = Font.Monospace;
            MoneyMenu.Banner.Color = Color.Black;
            MoneyMenu.BannerText.Font = Font.Pricedown;
            MoneyMenu.DescriptionFont  = Font.Monospace;
            money40k.TitleFont = Font.Monospace;
            money1milion.TitleFont = Font.Monospace;
            money10milion.TitleFont = Font.Monospace;
            money15milion.TitleFont = Font.Monospace;
            money20milion.TitleFont = Font.Monospace;
            money100milion.TitleFont = Font.Monospace;

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
            vehiclespawn.TitleFont = Font.Monospace;
            vehiclepimp.TitleFont = Font.Monospace;

            //TELEPORT MENU
            TeleportMenu.BannerText.Color = Color.Brown;
            TeleportMenu.NameFont = Font.Monospace;
            TeleportMenu.Banner.Color = Color.Black;
            TeleportMenu.BannerText.Font = Font.Pricedown;
            TeleportMenu.DescriptionFont = Font.Monospace;
            teltoway.TitleFont = Font.Monospace;
            teltochiliad.TitleFont = Font.Monospace;
            teltopub1.TitleFont = Font.Monospace;
            teltofib.TitleFont = Font.Monospace;
            teltobell.TitleFont = Font.Monospace; 
            teltobank.TitleFont = Font.Monospace;
            teltopub2.TitleFont = Font.Monospace;
            teltosolomon.TitleFont = Font.Monospace;    
            teltooneil.TitleFont = Font.Monospace;  
            teltolife.TitleFont = Font.Monospace;
            teltolester.TitleFont = Font.Monospace;
            teltofloyd.TitleFont = Font.Monospace;
            teltoyatch.TitleFont = Font.Monospace;
            teltotorture.TitleFont = Font.Monospace;
            teltoiaaoffice.TitleFont = Font.Monospace;
            teltohumlabentrance.TitleFont = Font.Monospace;
            teltohumlabtunnel.TitleFont = Font.Monospace;
            teltoblainecountybank.TitleFont = Font.Monospace;
            teltohospital.TitleFont = Font.Monospace;
            teltomorgue.TitleFont = Font.Monospace;
            teltojewelstore.TitleFont = Font.Monospace;
            teltomaxrendastorre.TitleFont = Font.Monospace;
            teltostadium.TitleFont = Font.Monospace;
            teltouniondep.TitleFont = Font.Monospace;



            //NORTH MENU
            Northmenu.BannerText.Color = Color.Brown;
            Northmenu.Banner.Color = Color.Black;
            Northmenu.NameFont = Font.Monospace; 
            Northmenu.BannerText.Font = Font.Pricedown;
            Northmenu.DescriptionFont = Font.Monospace;
            loadnorth.TitleFont = Font.Monospace;
            unloadnorth.TitleFont = Font.Monospace;

            //ONLINE TELEPORT MENU
            OnlineTeleportMenu.BannerText.Color = Color.Brown;
            OnlineTeleportMenu.Banner.Color = Color.Black;
            OnlineTeleportMenu.NameFont = Font.Monospace;
            OnlineTeleportMenu.BannerText.Font = Font.Pricedown;
            OnlineTeleportMenu.DescriptionFont = Font.Monospace;
            teltomethfarm.TitleFont = Font.Monospace;
            teltoweedlab.TitleFont = Font.Monospace;
            teltococainelab.TitleFont = Font.Monospace;
            teltocounterfeitlab.TitleFont = Font.Monospace;
            teltodocforgeryofficelab.TitleFont = Font.Monospace;
            teltoclub1.TitleFont = Font.Monospace;
            teltoclub2.TitleFont = Font.Monospace;


            //CAYOSUBMENU
            CayoMenu.BannerText.Color = Color.Brown;
            CayoMenu.Banner.Color = Color.Black;
            CayoMenu.NameFont = Font.Monospace;
            CayoMenu.BannerText.Font = Font.Pricedown;
            CayoMenu.DescriptionFont = Font.Monospace;
            cayoloadmode.TitleFont = Font.Monospace;
            loadteltocayoperico.TitleFont = Font.Monospace;
            unloadteltocayoperico.TitleFont = Font.Monospace;

            //OnlineHouseSUBMENU
            OnlineHouseMenu.BannerText.Color = Color.Brown;
            OnlineHouseMenu.Banner.Color = Color.Black;
            OnlineHouseMenu.NameFont = Font.Monospace;
            OnlineHouseMenu.BannerText.Font = Font.Pricedown;
            OnlineHouseMenu.DescriptionFont = Font.Monospace;
            ModernAppMenu.BannerText.Color = Color.Brown;
            ModernAppMenu.Banner.Color = Color.Black;
            ModernAppMenu.NameFont = Font.Monospace;
            ModernAppMenu.BannerText.Font = Font.Pricedown;
            ModernAppMenu.DescriptionFont = Font.Monospace;
            ArcadiusAppMenu.BannerText.Color = Color.Brown;
            ArcadiusAppMenu.Banner.Color = Color.Black;
            ArcadiusAppMenu.NameFont = Font.Monospace;
            ArcadiusAppMenu.BannerText.Font = Font.Pricedown;
            ArcadiusAppMenu.DescriptionFont = Font.Monospace;

            //HOUSE
            teltomodernhouse1.TitleFont = Font.Monospace;
            teltomodernhouse2.TitleFont = Font.Monospace;
            teltovibrant1.TitleFont = Font.Monospace; teltovibrant2.TitleFont = Font.Monospace; teltovibrant3.TitleFont = Font.Monospace;
            teltosharp1.TitleFont = Font.Monospace; teltosharp2.TitleFont = Font.Monospace; teltosharp3.TitleFont = Font.Monospace;
            teltomonochrome1.TitleFont = Font.Monospace; teltomonochrome2.TitleFont = Font.Monospace; teltomonochrome3.TitleFont = Font.Monospace;
            teltoseductive1.TitleFont = Font.Monospace; teltoseductive2.TitleFont = Font.Monospace; teltoseductive3.TitleFont = Font.Monospace;
            teltoregal1.TitleFont = Font.Monospace; teltoregal2.TitleFont = Font.Monospace; teltoregal3.TitleFont = Font.Monospace;
            teltoaqua1.TitleFont = Font.Monospace; teltoaqua2.TitleFont = Font.Monospace; teltoaqua3.TitleFont = Font.Monospace;

            //ARCADIUS
            telexecrich.TitleFont = Font.Monospace;
            telexeccool.TitleFont = Font.Monospace;
            telexeccontrast.TitleFont = Font.Monospace;
            teloldspicewarm.TitleFont = Font.Monospace;
            teloldspiceclassical.TitleFont = Font.Monospace;
            teloldspicevintage.TitleFont = Font.Monospace;
            telpowerbrokerice.TitleFont = Font.Monospace;
            telpowerbrokerconservative.TitleFont = Font.Monospace;
            telpowerbrokerpolished.TitleFont = Font.Monospace;
            telarcgarage1.TitleFont = Font.Monospace;
            telarcgarage2.TitleFont = Font.Monospace;
            telarcgarage3.TitleFont = Font.Monospace;
            telarcmodshop.TitleFont = Font.Monospace;


            //MAIN MENU
            DemoMenu.Banner.Color = Color.Purple;
            DemoMenu.NameFont = Font.Monospace;
            DemoMenu.DescriptionFont = Font.Monospace;
            DemoMenu.BannerText.Font = Font.Pricedown;
            //DemoMenu.SetBanner("~HUD_COLOUR_GOLD~MOD MENU"); // Colore oro usando codici GTA


            DemoSubMenuPed.Banner.Color = Color.Purple;
            DemoSubMenuPed.BannerText.Color = Color.Brown;

            VehicleSpawnerMenu.Banner.Color = Color.Purple;
            VehicleSpawnerMenu.NameFont = Font.Monospace;
            VehicleSpawnerMenu.BannerText.Font = Font.Pricedown;
            VehicleSpawnerMenu.DescriptionFont = Font.Monospace;

            colorMenu.Banner.Color = Color.Purple;
            colorMenu.NameFont = Font.Monospace;


            //ANONIK MENU
            AnonikMenu.Banner.Color = Color.Black;


            //MODEL CHANGER
            //Franklins.TitleFont = Font.Monospace;



            //========================================= END STYLE===============================================================







            //How you order items below doesn't matter
            DemoPool.Add(DemoMenu); // The pool is container for your menus, add the menu
            DemoPool.Add(SelfMenu);
            DemoPool.Add(WeaponMenu);
            DemoPool.Add(VehicleMenu);
            DemoPool.Add(TeleportMenu);
            DemoPool.Add(WeatherMenu);
            DemoPool.Add(VisionMenu);
            DemoPool.Add(BetaMenu);
            DemoPool.Add(AnonikMenu);





            //DemoPool.Add(DemoSubMenuPed); // add first submenu
            //DemoPool.Add(ModelMenuPed);//Model Changer
            DemoPool.Add(Northmenu);//NorthMenu
            DemoPool.Add(MoneyMenu);//MoneyDropMenu
            DemoPool.Add(OnlineTeleportMenu);//Online Teleport Menu
            DemoPool.Add(CayoMenu);//Cayo Perico Menu
            DemoPool.Add(OnlineHouseMenu);//House menu
            DemoPool.Add(ModernAppMenu);//Modern House
            DemoPool.Add(ArcadiusAppMenu);//Arcadius Interior


            //Main Menu Categories
            DemoMenu.AddSubMenu(SelfMenu);
            DemoMenu.AddSubMenu(WeaponMenu);
            DemoMenu.AddSubMenu(VehicleMenu);
            DemoMenu.AddSubMenu(TeleportMenu);
            DemoMenu.AddSubMenu(WeatherMenu);
            DemoMenu.AddSubMenu(VisionMenu);
            SetupBodyChanger();
            DemoMenu.AddSubMenu(BetaMenu);
            DemoMenu.AddSubMenu(AnonikMenu);

            //Submenu Categories

            //Teleport SubMenu

            //North Yankton SubMenu
            TeleportMenu.AddSubMenu(Northmenu);
            Northmenu.Add(loadnorth);
            Northmenu.Add(unloadnorth);
            //End North Yankton SubMenu

            //Online Teleport SubMenu
            TeleportMenu.AddSubMenu(OnlineTeleportMenu);
            OnlineTeleportMenu.AddSubMenu(CayoMenu);
            OnlineTeleportMenu.AddSubMenu(OnlineHouseMenu);
            CayoMenu.Add(cayoloadmode);
            CayoMenu.Add(loadteltocayoperico);
            CayoMenu.Add(unloadteltocayoperico);
            OnlineHouseMenu.AddSubMenu(ModernAppMenu);
            OnlineHouseMenu.AddSubMenu(ArcadiusAppMenu);
            //End Online Teleport SubMenu

            //Online Teleporrt Item
            OnlineTeleportMenu.Add(teltomethfarm);
            OnlineTeleportMenu.Add(teltoweedlab);
            OnlineTeleportMenu.Add(teltococainelab);
            OnlineTeleportMenu.Add(teltocounterfeitlab);
            OnlineTeleportMenu.Add(teltodocforgeryofficelab);
            OnlineTeleportMenu.Add(teltoclub1);
            OnlineTeleportMenu.Add(teltoclub2);

            //OnlineHouse Item
            ModernAppMenu.Add(teltomodernhouse1);
            ModernAppMenu.Add(teltomodernhouse2);
            ModernAppMenu.Add(teltovibrant1); ModernAppMenu.Add(teltovibrant2); ModernAppMenu.Add(teltovibrant3);
            ModernAppMenu.Add(teltosharp1); ModernAppMenu.Add(teltosharp2); ModernAppMenu.Add(teltosharp3);
            ModernAppMenu.Add(teltomonochrome1); ModernAppMenu.Add(teltomonochrome2); ModernAppMenu.Add(teltomonochrome3);
            ModernAppMenu.Add(teltoseductive1); ModernAppMenu.Add(teltoseductive2); ModernAppMenu.Add(teltoseductive3);
            ModernAppMenu.Add(teltoregal1); ModernAppMenu.Add(teltoregal2); ModernAppMenu.Add(teltoregal3);
            ModernAppMenu.Add(teltoaqua1); ModernAppMenu.Add(teltoaqua2); ModernAppMenu.Add(teltoaqua3);
            //End Onlinehouse Item

            //Arcadius Item
            ArcadiusAppMenu.Add(telexecrich);
            ArcadiusAppMenu.Add(telexeccool);
            ArcadiusAppMenu.Add(telexeccontrast);
            ArcadiusAppMenu.Add(teloldspicewarm);
            ArcadiusAppMenu.Add(teloldspiceclassical);
            ArcadiusAppMenu.Add(teloldspicevintage);
            ArcadiusAppMenu.Add(telpowerbrokerice);
            ArcadiusAppMenu.Add(telpowerbrokerconservative);
            ArcadiusAppMenu.Add(telpowerbrokerpolished);
            ArcadiusAppMenu.Add(telarcgarage1);
            ArcadiusAppMenu.Add(telarcgarage2);
            ArcadiusAppMenu.Add(telarcgarage3);
            ArcadiusAppMenu.Add(telarcmodshop);
            //End Arcadius Item


            //End Teleport SubMenu



            //Beta Menu
            //BetaMenu.Add
            BetaMenu.Add(drunkmode);
            BetaMenu.Add(nocollmode);
            BetaMenu.Add(chaosmode);
            BetaMenu.Add(blackoutmode);
            BetaMenu.Add(moongravmode);
            BetaMenu.Add(meteorshowermode);


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
            SetupPedChanger();
            SelfMenu.AddSubMenu(MoneyMenu);
            MoneyMenu.Add(money40k);
            MoneyMenu.Add(money1milion);
            MoneyMenu.Add(money10milion);
            MoneyMenu.Add(money15milion);
            MoneyMenu.Add(money20milion);
            MoneyMenu.Add(money100milion);

            //Weapon Menu
            WeaponMenu.Add(expoweap);
            WeaponMenu.Add(expomelee);
            WeaponMenu.Add(moneygunop);

            //Vehicle Menu
            VehicleMenu.Add(fixvehicle);
            VehicleMenu.Add(cargodmod);
            VehicleMenu.Add(cardrivein);
            VehicleMenu.Add(vehiclespawn);
            VehicleMenu.Add(vehiclepimp);
            //VehicleMenu.Add(colorMenuItem);
            DemoPool.Add(colorMenu); // Aggiungi al pool
            VehicleMenu.Add(colorMenuItem);

            upgradeMenu.Add(fullUpgradeItem);
            upgradeMenu.Add(engineItem);
            upgradeMenu.Add(brakesItem);
            upgradeMenu.Add(transmissionItem);
            upgradeMenu.Add(suspensionItem);
            upgradeMenu.Add(turboItem);
            VehicleMenu.AddSubMenu(upgradeMenu);
            DemoPool.Add(upgradeMenu);

            SetupVehicleSpawner();
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
            TeleportMenu.Add(teltoyatch);
            TeleportMenu.Add(teltotorture);
            TeleportMenu.Add(teltoiaaoffice);
            TeleportMenu.Add(teltohumlabentrance);
            TeleportMenu.Add(teltohumlabtunnel);
            TeleportMenu.Add(teltoblainecountybank);
            TeleportMenu.Add(teltohospital);
            TeleportMenu.Add(teltomorgue);
            TeleportMenu.Add(teltojewelstore);
            TeleportMenu.Add(teltomaxrendastorre);
            TeleportMenu.Add(teltostadium);
            TeleportMenu.Add(teltouniondep);

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

            //Vision Menu
            VisionMenu.Add(ThermalVision);
            VisionMenu.Add(NightVision);
            VisionMenu.Add(MatrixVision);
            VisionMenu.Add(Ragevision);

            //Bodyguard Menu
            BodyGuardMenu.Add(BodyGodMode);
            BodyGuardMenu.Add(RemoveBody);

            //Anonik Menu
            AnonikMenu.Add(FunAnonikFunc);

            // Aggiungi i menu al pool



            //standalone items to add to menu

            //DemoMenu.Add(CarGuy);

            //Items for SubMenu Ped
            //DemoMenu.AddSubMenu(DemoSubMenuPed); //the submenu
            //DemoSubMenuPed.Add(Ballas); // item 1
            //DemoSubMenuPed.Add(LostMCGang); // item 2

            //DemoMenu.AddSubMenu(ModelMenuPed);
            //ModelMenuPed.Add(Franklins);

            //Misc Items, these are buttons/standalone (menu items without sub menus)
            CarGuy.Activated += SetModelCarGuy;

            //Item activation Beta
            drunkmode.Activated += SetDrunk;
            nocollmode.Activated += NoColl;
            chaosmode.Activated += SetChaos;
            blackoutmode.Activated += SetBlackout;
            moongravmode.Activated += SetMoonGrav;
            meteorshowermode.Activated += SetMeteorShower;


            //Item activation Self
            Ballas.Activated += SpawnBallas;
            LostMCGang.Activated += SpawnLostMC;
            //Franklins.Activated += SpawnFrank;
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
            colorMenuItem.Activated += SetVehicleColor;

            fullUpgradeItem.Activated += (s, e) =>
            {
                ApplyVehicleMod(11, 3); // Motore
                ApplyVehicleMod(12, 3); // Freni
                ApplyVehicleMod(13, 3); // Trasmissione
                ApplyVehicleMod(15, 4); // Sospensioni
                SetTurbo(true);
                Notification.Show("~g~Upgrade completo installato!");
            };

            engineItem.ItemChanged += (s, e) => ApplyVehicleMod(11, e.Index); // Motore
            brakesItem.ItemChanged += (s, e) => ApplyVehicleMod(12, e.Index); // Freni
            transmissionItem.ItemChanged += (s, e) => ApplyVehicleMod(13, e.Index); // Cambio
            suspensionItem.ItemChanged += (s, e) => ApplyVehicleMod(15, e.Index); // Sospensioni
            turboItem.CheckboxChanged += (s, e) => SetTurbo(turboItem.Checked);


            //Item Att WEAPONMENU
            expoweap.Activated += SetWeapExpo;
            expomelee.Activated += SetMeleeExpo;
            moneygunop.Activated += SetMoneyGun;


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
            teltoyatch.Activated += SetYatch;
            teltotorture.Activated += SetTorture;
            teltoiaaoffice.Activated += SetIaaOffice;
            teltohumlabentrance.Activated += SetHumLabEntrance;
            teltohumlabtunnel.Activated += SetHumLabTunnel;
            teltoblainecountybank.Activated += SetBlaineBank;
            teltohospital.Activated += SetHospital;
            teltomorgue.Activated += SetMorgue;
            teltojewelstore.Activated += SetJewelStore;
            teltomaxrendastorre.Activated += SetMaxRendaStore;
            teltostadium.Activated += SetStadium;
            teltouniondep.Activated += SetUnionDepo;


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

            //Item Att VISIONMENU
            ThermalVision.Activated += SetThermalVision;
            NightVision.Activated += SetNightVision;
            MatrixVision.Activated += SetMatrixVision;
            Ragevision.Activated += SetRageVision;

            //Item North Menu - TELEPORTMENU
            loadnorth.Activated += SetNorthLoad;
            unloadnorth.Activated += SetNorthUnload;

            //Item Online Teleport Menu
            teltomethfarm.Activated += SetMethFarm;
            teltoweedlab.Activated += SetWeedLab;
            teltococainelab.Activated += SetCocaineLab;
            teltocounterfeitlab.Activated += SetCounterfeitLab;
            teltodocforgeryofficelab.Activated += SetForgeryOffice;
            teltoclub1.Activated += SetClubhouse1;
            teltoclub2.Activated += SetClubhouse2;

            //Item Cayo Perico Menu
            cayoloadmode.Activated += SetCayoLoad;
            loadteltocayoperico.Activated += SetCayoPerico;
            unloadteltocayoperico.Activated += SetCayoPericoUnload;

            //Item Modern Appartmen
            teltomodernhouse1.Activated += SetModernApp1;
            teltomodernhouse2.Activated += SetModernApp2;
            teltovibrant1.Activated += SetVibrant1; teltovibrant2.Activated += SetVibrant2; teltovibrant3.Activated += SetVibrant3;
            teltosharp1.Activated += SetSharp1; teltosharp2.Activated += SetSharp2; teltosharp3.Activated += SetSharp3;
            teltomonochrome1.Activated += SetMonochrome1; teltomonochrome2.Activated += SetMonochrome2; teltomonochrome3.Activated += SetMonochrome3;
            teltoseductive1.Activated += SetSeductive1; teltoseductive2.Activated += SetSeductive2; teltoseductive3.Activated += SetSeductive3;
            teltoregal1.Activated += SetRegal1; teltoregal2.Activated += SetRegal2; teltoregal3.Activated += SetRegal3;
            teltoaqua1.Activated += SetAqua1; teltoaqua2.Activated += SetAqua2; teltoaqua3.Activated += SetAqua3;

            //Item Arcadius
            telexecrich.Activated += SetExecRich;
            telexeccool.Activated += SetExecCool;
            telexeccontrast.Activated += SetExecContrast;
            teloldspicewarm.Activated += SetOldSpiceWarm;
            teloldspiceclassical.Activated += SetOldSpiceClassical;
            teloldspicevintage.Activated += SetOldSpiceVintage;
            telpowerbrokerice.Activated += SetPowerBrokerIce;
            telpowerbrokerconservative.Activated += SetPowerBrokerConservative;
            telpowerbrokerpolished.Activated += SetPowerBrokerPolished;
            telarcgarage1.Activated += SetArcadiusGarage1;
            telarcgarage2.Activated += SetArcadiusGarage2;
            telarcgarage3.Activated += SetArcadiusGarage3;
            telarcmodshop.Activated += SetArcadiusModShop;

            //Item Bodyguard Menu
            BodyGodMode.Activated += SetBodyGodMode;
            RemoveBody.Activated += SetRemoveBody;

            //Item Anonik Menu
            FunAnonikFunc.Activated += FunAnonik;


            //

            Tick += Basics_Tick;
            KeyDown += Basics_KeyDown; //line added to replace code in Basics_Tick




        }





        private void SetupVehicleSpawner()
        {
            if (!File.Exists(iniPath))
            {
                Notification.Show("~r~vehicles.ini non trovato!");
                return;
            }

            string currentCategory = null;
            foreach (var line in File.ReadAllLines(iniPath))
            {
                string trimmed = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";"))
                    continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentCategory = trimmed.Substring(1, trimmed.Length - 2).Trim();
                    if (!vehicleCategories.ContainsKey(currentCategory))
                        vehicleCategories[currentCategory] = new List<string>();
                }
                else if (currentCategory != null)
                {
                    vehicleCategories[currentCategory].Add(trimmed.ToLower());
                }
            }

            foreach (var category in vehicleCategories)
            {
                NativeMenu catMenu = new NativeMenu(category.Key, $"Veicoli {category.Key}");
                foreach (var modelName in category.Value)
                {
                    var item = new NativeItem(modelName.ToUpper());
                    item.Activated += (s, a) => SpawnVehicle(modelName);
                    catMenu.Add(item);
                }

                VehicleSpawnerMenu.AddSubMenu(catMenu);
                DemoPool.Add(catMenu);
                catMenu.Banner.Color = Color.Purple;
                catMenu.NameFont = Font.Monospace;
                VehicleSpawnerMenu.Banner.Color = Color.Purple;
                VehicleSpawnerMenu.NameFont = Font.Monospace;
            }

            VehicleMenu.AddSubMenu(VehicleSpawnerMenu);
            DemoPool.Add(VehicleSpawnerMenu);
        }

        private void SpawnVehicle(string modelName)
        {
            Ped ped = Game.Player.Character;
            Model model = new Model(modelName);

            if (!model.IsInCdImage || !model.IsValid)
            {
                Notification.Show($"~r~Modello non valido: ~w~{modelName}");
                return;
            }

            model.Request(500);
            while (!model.IsLoaded) Script.Wait(100);

            Vector3 spawnPos = ped.Position + ped.ForwardVector * 5f;
            Vehicle veh = World.CreateVehicle(model, spawnPos);
            ped.SetIntoVehicle(veh, VehicleSeat.Driver);

            Notification.Show($"~g~Spawned: ~w~{modelName.ToUpper()}");
            model.MarkAsNoLongerNeeded();
        }

        private void ApplyVehicleMod(int modType, int index)
        {
            Vehicle veh = Game.Player.Character.CurrentVehicle;
            if (veh == null || !veh.Exists()) return;
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, veh, modType, index, false);
        }

        private void SetTurbo(bool enabled)
        {
            Vehicle veh = Game.Player.Character.CurrentVehicle;
            if (veh == null || !veh.Exists()) return;
            Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, 18, enabled); // 18 = Turbo
        }

       



        //Pedlist menu

        private void SetupPedChanger()
        {
            if (!File.Exists(iniPPath))
            {
                Notification.Show("~r~pedlist.ini non trovato!");
                return;
            }

            string currentCategory1 = null;
            foreach (var line in File.ReadAllLines(iniPPath))
            {
                string trimmed = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";"))
                    continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentCategory1 = trimmed.Substring(1, trimmed.Length - 2).Trim();
                    if (!pedCategories.ContainsKey(currentCategory1))
                        pedCategories[currentCategory1] = new List<string>();
                }
                else if (currentCategory1 != null)
                {
                    string display = trimmed;
                    string model = trimmed;

                    if (trimmed.Contains("="))
                    {
                        var parts = trimmed.Split(new[] { '=' }, 2);
                        display = parts[0].Trim();
                        model = parts[1].Trim().ToLower();
                    }

                    pedCategories[currentCategory1].Add($"{display}|{model}");
                }
            }

            foreach (var category in pedCategories)
            {
                NativeMenu cat1Menu = new NativeMenu(category.Key, $"Skin {category.Key}");
                foreach (var entry in category.Value)
                {
                    string visibleName = entry;
                    string modelName = entry;

                    if (entry.Contains("|"))
                    {
                        var parts = entry.Split('|');
                        visibleName = parts[0];
                        modelName = parts[1];
                    }

                    var item = new NativeItem(visibleName);
                    item.Activated += (s, a) => SpawnPed(modelName);
                    cat1Menu.Add(item);
                }

                SkinSpawnerMenu.AddSubMenu(cat1Menu);
                DemoPool.Add(cat1Menu);
                cat1Menu.Banner.Color = Color.Purple;
                cat1Menu.NameFont = Font.Monospace;
                SkinSpawnerMenu.Banner.Color = Color.Purple;
                SkinSpawnerMenu.NameFont = Font.Monospace;
            }

            SelfMenu.AddSubMenu(SkinSpawnerMenu);
            DemoPool.Add(SkinSpawnerMenu);
        }


        /* private void SpawnPed(string modelName)
         {
             Ped ped = Game.Player.Character;
             Model model = new Model(modelName);

             if (!model.IsInCdImage || !model.IsValid)
             {
                 Notification.Show($"~r~Modello non valido: ~w~{modelName}");
                 return;
             }

             model.Request(500);
             while (!model.IsLoaded) Script.Wait(500);

             if (ped.IsDead)
             {
                 Game.Player.ChangeModel(new Model("player_zero")); // Imposta il modello di default (o uno di base)
                 return;
             }

             Game.Player.ChangeModel(model);
             Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character);
             //Function.Call(Hash.SET_PED_RANDOM_COMPONENT_VARIATION, Game.Player.Character,0);
             model.MarkAsNoLongerNeeded();
             GC.Collect();
             Script.Wait(500);

             /*Vector3 spawnPos = ped.Position + ped.ForwardVector * 5f;
             Vehicle veh = World.CreateVehicle(model, spawnPos);
             ped.SetIntoVehicle(veh, VehicleSeat.Driver);*/

        /*     Notification.Show($"~g~Changed: ~w~{modelName.ToUpper()}");
             //model.MarkAsNoLongerNeeded();
             Script.Wait(500);
         }*/

        private void SpawnPed(string modelName)
        {
            Ped ped = Game.Player.Character;
            Model model = new Model(modelName);

            if (!model.IsInCdImage || !model.IsValid)
            {
                Notification.Show($"~r~Modello non valido: ~w~{modelName}");
                return;
            }

            model.Request(500);
            while (!model.IsLoaded) Script.Wait(100);

            // Se il pedone è morto, eseguiamo il respawn e poi cambiamo il modello
            if (ped.IsDead)
            {
                // Forza il respawn del pedone
                RespawnPed();

                // Attendi un po' di tempo per permettere al respawn di completarsi
                Script.Wait(3000); // Prova con 3 secondi, ma regola questo valore se necessario

                // Cambia il modello solo dopo che il pedone è stato "resuscitato"
                Game.Player.ChangeModel(model);
                Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character);
                Notification.Show($"~g~Changed: ~w~{modelName.ToUpper()}");

                model.MarkAsNoLongerNeeded();
            }
            else
            {
                // Cambia il modello solo se il pedone non è morto
                if (ped.Model.Hash != model.Hash)
                {
                    Game.Player.ChangeModel(model);
                    Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character);
                    Notification.Show($"~g~Changed: ~w~{modelName.ToUpper()}");

                    model.MarkAsNoLongerNeeded();
                }
            }

            Script.Wait(500);
        }

        // Funzione per forzare il respawn del pedone
        private void RespawnPed()
        {
            Ped ped = Game.Player.Character;

            // Rimuovi il pedone attuale (questo "resuscita" il pedone)
            ped.Delete(); // Elimina il pedone morto

            // Attendi qualche frame per fare in modo che il pedone venga effettivamente rimosso
            Script.Wait(500);

            // Ora respawna il pedone con il modello di base (modello di default)
            Game.Player.ChangeModel(new Model("player_zero")); // Cambia con il modello che preferisci
            Notification.Show("Respawn completato!");
        }


        //EndPedlist menu


        //bodymenu
        private void SetupBodyChanger()
        {
            if (!File.Exists(iniPPPath))
            {
                Notification.Show("~r~bguard.ini non trovato!");
                return;
            }

            string currentCategory2 = null;
            foreach (var line in File.ReadAllLines(iniPPPath))
            {
                string trimmed = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";"))
                    continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentCategory2 = trimmed.Substring(1, trimmed.Length - 2).Trim();
                    if (!bodyCategories.ContainsKey(currentCategory2))
                        bodyCategories[currentCategory2] = new List<string>();
                }
                else if (currentCategory2 != null)
                {
                    string display = trimmed;
                    string model = trimmed;

                    if (trimmed.Contains("="))
                    {
                        var parts = trimmed.Split(new[] { '=' }, 2);
                        display = parts[0].Trim();
                        model = parts[1].Trim().ToLower();
                    }

                    bodyCategories[currentCategory2].Add($"{display}|{model}");
                }
            }

            foreach (var category in bodyCategories)
            {
                NativeMenu cat2Menu = new NativeMenu(category.Key, $"Bodyguard {category.Key}");
                foreach (var entry in category.Value)
                {
                    string visibleName = entry;
                    string modelName = entry;

                    if (entry.Contains("|"))
                    {
                        var parts = entry.Split('|');
                        visibleName = parts[0];
                        modelName = parts[1];
                    }

                    var item = new NativeItem(visibleName);
                    item.Activated += (s, a) => SpawnBody(modelName);
                    cat2Menu.Add(item);
                }

                BodyGuardMenu.AddSubMenu(cat2Menu);
                DemoPool.Add(cat2Menu);
                cat2Menu.Banner.Color = Color.Purple;
                cat2Menu.NameFont = Font.Monospace;
                BodyGuardMenu.Banner.Color = Color.Purple;
                BodyGuardMenu.NameFont = Font.Monospace;
                
            }

            DemoMenu.AddSubMenu(BodyGuardMenu);
            DemoPool.Add(BodyGuardMenu);
        }


        private void SpawnBody(string modelName)
        {
            Ped ped = Game.Player.Character;
            Model model = new Model(modelName);

            if (!model.IsInCdImage || !model.IsValid)
            {
                Notification.Show($"~r~Modello non valido: ~w~{modelName}");
                return;
            }

            model.Request(500);
            while (!model.IsLoaded) Script.Wait(100);

            // Se il pedone è morto, eseguiamo il respawn e poi cambiamo il modello

                // Cambia il modello solo se il pedone non è morto
                if (ped.Model.Hash != model.Hash)
                {
                /*Game.Player.ChangeModel(model);
                // Imposta il pedone come bodyguard
                Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character);*/

                Ped player = Game.Player.Character;

                Vector3 loc = player.Position + (player.ForwardVector * 5);

                Ped bodyguard = World.CreatePed(model, loc);

                bodyguard.Weapons.Give(WeaponHash.Pistol50, 9999, true, true);

                bodyguard.Armor = 100;

                PedGroup ped1 = player.PedGroup;

                Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, ped1);

                Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100);

                Function.Call(Hash.SET_PED_ACCURACY, bodyguard, 100);

                bodyguard.Task.FightAgainstHatedTargets(50000);
                bodyguards.Add(bodyguard);

                Notification.Show($"~g~Spawned: ~w~{modelName.ToUpper()}");

                    model.MarkAsNoLongerNeeded();
                }


            Script.Wait(500);
        }

        private void SetRemoveBody(object sender, EventArgs e)
        {
            Ped player = Game.Player.Character;
            if (player.PedGroup != null)
            {
                Vector3 loc = player.Position + (player.ForwardVector * 5);
                PedGroup ped = player.PedGroup;
                Function.Call(Hash.SET_ENTITY_INVINCIBLE, ped, false);
                Function.Call(Hash.REMOVE_GROUP, ped);

                foreach (Ped p in bodyguards)
                {
                    if (p.Exists()) p.Delete();
                }
                bodyguards.Clear();

                Notification.Show("~g~Bodyguard rimosso!");
            }
            else
            {
                Notification.Show("~r~Nessun Bodyguard da rimuovere!");
            }
        }

   

        private void SetBodyGodMode(object sender, EventArgs e)
        {
            bool enable = BodyGodMode.Checked;

            foreach (Ped bodyguard in bodyguards)
            {
                if (bodyguard.Exists())
                {
                    bodyguard.IsInvincible = enable;
                    Function.Call(Hash.SET_ENTITY_PROOFS, bodyguard, enable, enable, enable, enable, enable, enable, enable, enable); // opzionale per protezione completa
                }
            }

            Notification.Show($"~g~God Mode Bodyguard: {(enable ? "ON" : "OFF")}");
        }




        //EndPedlist menu






        private void SetupSkinChanger()
        {

            // Protagonisti
            AddModelToMenu(ProtagonistMenu, "Michael", PedHash.Michael);
            AddModelToMenu(ProtagonistMenu, "Franklin", PedHash.Franklin);
            AddModelToMenu(ProtagonistMenu, "Trevor", PedHash.Trevor);
            AddModelToMenu(ProtagonistMenu, "Lester", PedHash.LesterCrest2);
            AddModelToMenu(ProtagonistMenu, "Lamar", PedHash.LamarDavis);
            AddModelToMenu(ProtagonistMenu, "Jimmy", PedHash.JimmyBoston);
            AddModelToMenu(ProtagonistMenu, "Amanda", PedHash.AmandaTownley);

            // Gang
            AddModelToMenu(GangMenu, "Ballas", PedHash.Ballas01GFY);
            AddModelToMenu(GangMenu, "Families", PedHash.Families01GFY);
            AddModelToMenu(GangMenu, "Vagos", PedHash.Vagos01GFY);
            //AddModelToMenu(GangMenu, "Marabunta", PedHash.Marabunta01);
            AddModelToMenu(GangMenu, "Lost MC", PedHash.Lost01GFY);
            AddModelToMenu(GangMenu, "Armenian Boss", PedHash.ArmBoss01GMM);
            AddModelToMenu(GangMenu, "Korean Boss", PedHash.KorBoss01GMM);

            // Civili Uomini
            AddModelToMenu(CivMaleMenu, "Businessman 1", PedHash.Business01AFY);
            AddModelToMenu(CivMaleMenu, "Businessman 2", PedHash.Business02AFY);
            AddModelToMenu(CivMaleMenu, "Cyclist", PedHash.Cyclist01);
            //AddModelToMenu(CivMaleMenu, "Construction Worker", PedHash.Construct01);
            //AddModelToMenu(CivMaleMenu, "Fisherman", PedHash.Fisherman01AMM);
            AddModelToMenu(CivMaleMenu, "Golfer", PedHash.Golfer01AMY);
            AddModelToMenu(CivMaleMenu, "Hiker", PedHash.Hiker01AMY);

            // Civili Donne
            AddModelToMenu(CivFemaleMenu, "Businesswoman", PedHash.Business02AFY);
            AddModelToMenu(CivFemaleMenu, "Female Agent", PedHash.FemaleAgent);
            //AddModelToMenu(CivFemaleMenu, "GenHot Girl", PedHash.GenHot01AFY);
            AddModelToMenu(CivFemaleMenu, "Tourist", PedHash.Tourist01AFY);
            AddModelToMenu(CivFemaleMenu, "Yoga Girl", PedHash.Yoga01AFY);
            AddModelToMenu(CivFemaleMenu, "Beach Girl", PedHash.Beach01AFM);
            AddModelToMenu(CivFemaleMenu, "Epsilon Girl", PedHash.Epsilon01AFY);

            // Polizia
            AddModelToMenu(CopMenu, "LSPD Officer", PedHash.Cop01SFY);
            AddModelToMenu(CopMenu, "Sheriff", PedHash.Sheriff01SFY);
            AddModelToMenu(CopMenu, "SWAT", PedHash.Swat01SMY);
            AddModelToMenu(CopMenu, "FIB Agent", PedHash.FibSec01);
            AddModelToMenu(CopMenu, "Army Soldier", PedHash.Marine01SMY);
            AddModelToMenu(CopMenu, "Undercover Cop", PedHash.CiaSec01SMM);

            // Emergenza
            AddModelToMenu(EmergencyMenu, "Paramedic", PedHash.Paramedic01SMM);
            AddModelToMenu(EmergencyMenu, "Fireman", PedHash.Fireman01SMY);
            //AddModelToMenu(EmergencyMenu, "Doctor", PedHash.Medic01SMM);
            AddModelToMenu(EmergencyMenu, "Nurse", PedHash.MrsPhillips);

            // Varie
            AddModelToMenu(MiscMenu, "Clown", PedHash.Clown01SMY);
            AddModelToMenu(MiscMenu, "Zombie", PedHash.Zombie01);
            //AddModelToMenu(MiscMenu, "Santa", PedHash.Christmas01);
            //AddModelToMenu(MiscMenu, "Boxer", PedHash.Boxer01SMY);
            //AddModelToMenu(MiscMenu, "DJ", PedHash.Djblamryon);
            AddModelToMenu(MiscMenu, "Pilot", PedHash.Pilot01SMM);
            AddModelToMenu(MiscMenu, "Car Guy", PedHash.Car3Guy2);

            // Animali
            AddModelToMenu(AnimalMenu, "Mountain Lion", PedHash.MountainLion);
            //AddModelToMenu(AnimalMenu, "Cat", PedHash.Cat01);
            AddModelToMenu(AnimalMenu, "Coyote", PedHash.Coyote);
            AddModelToMenu(AnimalMenu, "Deer", PedHash.Deer);
            AddModelToMenu(AnimalMenu, "Dog (Rottweiler)", PedHash.Rottweiler);
            AddModelToMenu(AnimalMenu, "Dog (Retriever)", PedHash.Retriever);
            AddModelToMenu(AnimalMenu, "Dog (Husky)", PedHash.Husky);

            // Personaggi della storia
            //AddModelToMenu(StoryMenu, "Devon Weston", PedHash.Devon);
            AddModelToMenu(StoryMenu, "Solomon Richards", PedHash.Solomon);
            AddModelToMenu(StoryMenu, "Tao Cheng", PedHash.TaoCheng);
            AddModelToMenu(StoryMenu, "Steve Haines", PedHash.SteveHains);
            AddModelToMenu(StoryMenu, "Dave Norton", PedHash.DaveNorton);
            AddModelToMenu(StoryMenu, "Floyd Hebert", PedHash.Floyd);
            AddModelToMenu(StoryMenu, "Jay Norris", PedHash.JayNorris);
        }








        private void AddModelToMenu(NativeMenu menu, string name, PedHash model)
        {
            var item = new NativeItem(name);
            item.Activated += (sender, args) =>
            
            {
                if (Game.Player.Character.IsInVehicle())
                {
                    Notification.Show("~r~Exit the vehicle before changing model");
                    return;
                }

                try
                {

                    // Aggiungi effetto visivo prima del cambio modello
                    // Esempio di effetto particellare

                    // Effetto visivo pre-cambio
                    /*
                     * SwitchShortMichaelIn" / "SwitchShortMichaelOut" (effetto di transizione dei personaggi)

                        "DeathFailOut" (effetto di dissolvenza)

                        "CamPushInNeutral" (effetto zoom)

                        "FocusIn" / "FocusOut" (effetto di messa a fuoco)
                     */
                    Function.Call((Hash)0x2206BF9A37B7F724, "CamPushInNeutral", 0, true); // _START_SCREEN_EFFECT
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "FocusIn", "HintCamSounds", true);
                    // Then request the model
                    Game.Player.ChangeModel(model);
                    Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character);


                    Notification.Show($"~g~Changed to: ~w~{name}");

                    // Pulizia dopo 1 secondo
                    Script.Wait(1000);
                    Function.Call((Hash)0x068E835A1D0DC0E3, "CamPushInNeutral"); // _STOP_SCREEN_EFFECT
                    Function.Call((Hash)0x068E835A1D0DC0E3, "DeathFailOut"); // _STOP_SCREEN_EFFECT
                }
                catch
                {
                    Notification.Show($"~r~Failed to load model: ~w~{name}");
                }
            };
            menu.Add(item);
        }



        private void FunAnonik(object sender, EventArgs e)
        {
            Notification.Show("Uffa funziono");
            Game.PlayerBlip.Sprite = BlipSprite.PoliceStation;
            Game.PlayerBlip.Color = BlipColor.Green;    
            Game.Player.ChangeModel(new Model("s_m_y_swat_01"));
            Game.Player.Character.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
            Game.Player.WantedLevel = 0;
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player.Character, true);
            Function.Call(Hash.ANIMPOSTFX_PLAY, "FocusIn", 0, true);
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


        private void SetBlackout(object sender, EventArgs e)
        {
            if (blackoutmode.Checked == true)
            {
                World.Blackout = true;
            }
            if (blackoutmode.Checked == false)
            {
               World.Blackout = false;
            }
        }

        private void SetMoonGrav(object sender, EventArgs e)
        {
            if (moongravmode.Checked == true)
            {
                Function.Call(Hash.SET_GRAVITY_LEVEL, 2); // Imposta la gravità a livello 1 (gravità ridotta)
            }
            if (moongravmode.Checked == false)
            {
                Function.Call(Hash.SET_GRAVITY_LEVEL, 0); // Imposta la gravità a livello 0 (gravità originale)
                
            }
        }



        private void SetMeteorShower(object sender, EventArgs e)
        {
            meteorShowerOn = meteorshowermode.Checked;
            meteorTimer1 = 0;
            meteorTimer2 = 0;

            if (meteorShowerOn)
            {
                if (rockModels.Count == 0)
                {
                    foreach (string name in rockModelNames)
                        rockModels.Add(new Model(name));
                }
                foreach (Model m in rockModels)
                    m.Request();

                Function.Call(Hash.CLEAR_WEATHER_TYPE_PERSIST);
                World.TransitionToWeather(Weather.ThunderStorm, 3f);
            }
            else
            {
                foreach (Model m in rockModels)
                {
                    if (m.IsLoaded)
                        m.MarkAsNoLongerNeeded();
                }
                foreach (Prop r in rockArray)
                {
                    if (r.Exists())
                        r.MarkAsNoLongerNeeded();
                }
                rockArray.Clear();

                World.TransitionToWeather(Weather.ExtraSunny, 3f);
            }
        }

        private void DoMeteorShowerTick()
        {
            if (!meteorShowerOn)
                return;

            Ped myPed = Game.Player.Character;
            if (myPed == null || !myPed.Exists())
                return;

            // Spawn di un nuovo meteorite
            if (Game.GameTime > meteorTimer1)
            {
                float radius = (float)(22.0 + meteorRnd.NextDouble() * (78.0 - 22.0));
                double angle = meteorRnd.Next(0, 360) * Math.PI / 180.0;

                Vector3 myPos = myPed.Position;
                Vector3 pos = new Vector3(
                    myPos.X + radius * (float)Math.Cos(angle),
                    myPos.Y + radius * (float)Math.Sin(angle),
                    myPos.Z + 65.0f);

                Model model = rockModels[meteorRnd.Next(rockModels.Count)];

                if (model.IsLoaded)
                {
                    Prop rock = World.CreateProp(model, pos, false, false);

                    /*if (rock.Exists())
                    {
                        rock.Heading = (float)(meteorRnd.NextDouble() * 360.0);
                        rock.IsPositionFrozen = false;
                        Function.Call(Hash.SET_ENTITY_LOD_DIST, rock.Handle, 1000000);
                        rock.IsPersistent = true;
                        rock.ApplyForce(new Vector3(35.0f, 0f, -57.0f));
                        rockArray.Add(rock);
                    }*/

                    if (rock != null && rock.Exists())
                    {
                        rock.Heading = (float)(meteorRnd.NextDouble() * 360.0);
                        rock.IsPositionFrozen = false;
                        Function.Call(Hash.SET_ENTITY_LOD_DIST, rock.Handle, 1000000);
                        rock.IsPersistent = true;
                        rock.ApplyForce(new Vector3(35.0f, 0f, -57.0f));
                        rockArray.Add(rock);
                    }
                }

                meteorTimer1 = Game.GameTime + 840;
            }

            // Logica per ogni meteorite
            foreach (Prop r in rockArray)
            {
                if (!r.Exists())
                    continue;

                if (r.Speed > 0.55f)
                {
                    RaycastResult rayToGround = World.Raycast(r.Position, Vector3.WorldDown, 1.4f, IntersectFlags.Map, r);
                    bool isAsteroid = r.Model.Hash == AsteroidHash;

                    if (r.IsInWater)
                    {
                        TriggerPtfx("scr_oddjobtraffickingair", "scr_ojdg4_water_exp", r, isAsteroid ? 1.2f : 0.8f);
                    }
                    else if (!rayToGround.DidHit)
                    {
                        r.ApplyForce(new Vector3(0.2f, 0f, -2.0f));
                        TriggerPtfx("scr_agencyheistb", "scr_agency3b_heli_expl", r, isAsteroid ? 1.2f : 0.8f);
                    }
                    else
                    {
                        Vector3 hit = rayToGround.HitPosition;
                        float decalSize = isAsteroid ? 6.0f : 3.0f;

                        Function.Call(Hash.ADD_DECAL, DecalBangConcrete,
                            hit.X, hit.Y, hit.Z,
                            0f, 0f, -1f,
                            0f, 1f, 0f,
                            decalSize, decalSize,
                            0.1f, 0.3f, 0.6f, 0.5f,
                            -1.0f, false, false, false);

                        World.AddExplosion(hit, (ExplosionType)14, 0.0f, 0.017f, null, true, false);
                        Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Crate_Land", r.Handle, "FBI_05_SOUNDS", false, 0);
                    }
                }
            }

            // Pulizia periodica
            if (Game.GameTime > meteorTimer2)
            {
                foreach (Prop r in rockArray)
                {
                    if (r.Exists())
                        r.MarkAsNoLongerNeeded();
                }
                rockArray.Clear();
                meteorTimer2 = Game.GameTime + 10500;
            }
        }

        private void TriggerPtfx(string asset, string effect, Entity entity, float scale)
        {
            Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, asset);
            if (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, asset))
                return;

            Function.Call(Hash.USE_PARTICLE_FX_ASSET, asset);
            Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, effect, entity.Handle,
                0f, 0f, 0f,
                0f, 0f, 0f,
                scale, false, false, false);
        }


        private void SetRageVision(object sender, EventArgs e)
        {
            if (Ragevision.Checked == true)
            {
                Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "REDMIST_blend");
                Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 1.0f);
            }
            if (Ragevision.Checked == false)
            {
                Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
                Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 0.0f);
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

        private void SetWeapExpo(object sender, EventArgs e)
        {
            if (expoweap.Checked == true)
            {
               expowpon = !expowpon;
                //Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Game.Player.Character);
                //WeaponHash weapon = Game.Player.Character.Weapons.Current.Hash;
                //Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, weapon, true);
                //Vector3 coords = Game.Player.Character.GetPositionOffset(new Vector3(0f, 5f, 0f));
                //Function.Call(Hash.ADD_EXPLOSION, coords.X, coords.Y, coords.Z, 2, 1.0f, true, false, 0.0f);
            }
            if (expoweap.Checked == false)
            {
                expowpon = false;
                //aggiungi codice per disabilitare munizioni esplosive
            }
        }

        private void SetMeleeExpo(object sender, EventArgs e)
        {
            if (expomelee.Checked == true)
            {
                expoml = !expoml;
            }
            if (expomelee.Checked == false)
            {
                expoml = false;
                //aggiungi codice per disabilitare munizioni esplosive
            }
        }

        private void SetMoneyGun(object sender, EventArgs e)
        {
            if (moneygunop.Checked == true)
            {
                moneygun = !moneygun;

            }
            if (moneygunop.Checked == false)
            {
                moneygun = false;
                //aggiungi codice per disabilitare munizioni esplosive
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

        private void SetVehicleColor(object sender, EventArgs e)
        {
            if (!Game.Player.Character.IsInVehicle())
            {
                Notification.Show("~r~Non sei in un veicolo.");
                return;
            }

            Vehicle veh = Game.Player.Character.CurrentVehicle;
            colorMenu.Clear();

            Dictionary<string, VehicleColor> colors = new Dictionary<string, VehicleColor>
    {
        { "Red", VehicleColor.MetallicTorinoRed },
        { "Greem", VehicleColor.MetallicDarkGreen },
        { "Blue", VehicleColor.MetallicBlue },
        { "Black", VehicleColor.MetallicBlack },
        { "White", VehicleColor.MetallicWhite }
    };

            foreach (KeyValuePair<string, VehicleColor> kv in colors)
            {
                NativeItem item = new NativeItem(kv.Key);
                item.Activated += (s2, a2) =>
                {
                    Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);
                    Function.Call(Hash.SET_VEHICLE_COLOURS, veh, (int)kv.Value, (int)kv.Value);
                    Notification.Show($"~g~Colore impostato: ~w~{kv.Key}");
                };
                colorMenu.Add(item);
            }

            // Aggiunge il pulsante Indietro
            NativeItem backItem = new NativeItem("Go Back");
            backItem.Activated += (s2, a2) =>
            {
                colorMenu.Visible = false;
                VehicleMenu.Visible = true;
            };
            colorMenu.Add(backItem);

            if (VehicleMenu.Visible)
                VehicleMenu.Visible = false;

            colorMenu.Visible = true;
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


        private void SetTorture(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(147.170f, -2201.804f, 4.688f);
                Function.Call(Hash.REQUEST_IPL, "v_torture");
                Function.Call(Hash.REQUEST_IPL, "v_torture_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(147.170f, -2201.804f, 4.688f);
            }
        }

        private void SetIaaOffice(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(117.220f, -620.938f, 206.047f);
                Function.Call(Hash.REQUEST_IPL, "v_IAA");
                Function.Call(Hash.REQUEST_IPL, "v_IAA_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(117.220f, -620.938f, 206.047f);
            }
        }

        private void SetHumLabTunnel(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(3525.495f, 3705.301f, 20.992f);
                Function.Call(Hash.REQUEST_IPL, "v_humlab");
                Function.Call(Hash.REQUEST_IPL, "v_humlab_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(3525.495f, 3705.301f, 20.992f);
            }
        }

        private void SetHumLabEntrance(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(3619.749f, 2742.740f, 28.690f);
                Function.Call(Hash.REQUEST_IPL, "v_humlab");
                Function.Call(Hash.REQUEST_IPL, "v_humlab_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(3619.749f, 2742.740f, 28.690f);
            }
        }

        private void SetBlaineBank(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-109.299f, 6464.035f, 31.627f);
                Function.Call(Hash.REQUEST_IPL, "hei_bank_heist");
                Function.Call(Hash.REQUEST_IPL, "hei_bank_heist_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-109.299f, 6464.035f, 31.627f);
            }
        }

        
        private void SetHospital(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                //player.Character.Position = new Vector3(306.0f, -1439.0f, 46.5f);
                player.Character.Position = new Vector3(356.822f, -590.151f, 43.315f);
                //Function.Call(Hash.REQUEST_IPL, "rc12b_fixed");
                //Function.Call(Hash.REQUEST_IPL, "rc12b_destroyed");
                Function.Call(Hash.REQUEST_IPL, "rc12b_default");
                Function.Call(Hash.REQUEST_IPL, "rc12b_hospitalinterior");
                Function.Call(Hash.REQUEST_IPL, "rc12b_hospitalinterior_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(356.822f, -590.151f, 43.315f);
            }
        }

        private void SetMorgue(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(243.351f, -1376.014f, 39.534f);
                Function.Call(Hash.REQUEST_IPL, "coronertrash");
                Function.Call(Hash.REQUEST_IPL, "coronertrash_lod");
                Function.Call(Hash.REQUEST_IPL, "coroner_int_on");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(243.351f, -1376.014f, 39.534f);
            }
        }

        //START CAYO PERICO FUNCTION

        private void SetCayoLoad(object sender, EventArgs e)
        {
            cayoOn = cayoloadmode.Checked;

            if (cayoOn && !cayoMpLoaded)
            {
                Function.Call((Hash)0x888C3502DBBEEF5); // ON_ENTER_MP, una sola volta
                cayoMpLoaded = true;
            }

            if (!cayoOn)
            {
                Function.Call(Hash.SET_SCENARIO_GROUP_ENABLED, "Heist_Island_Peds", false);
                Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", false);
                Function.Call((Hash)0xF74B1FFA4A15FBEA, 0);
            }
        }

        private void DoCayoTick()
        {
            if (!cayoOn)
                return;

            Ped myPed = Game.Player.Character;

            if (myPed.Position.DistanceTo2D(new Vector3(5031.428f, -5150.907f, 0f)) < 2000f)
            {
                Function.Call(Hash.SET_SCENARIO_GROUP_ENABLED, "Heist_Island_Peds", true);
                Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", true); // island hopper
                Function.Call((Hash)0xF74B1FFA4A15FBEA, 1);                   // path nodes

                if (Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, myPed.Handle) == 0)
                {
                    Function.Call(Hash.SET_RADAR_AS_EXTERIOR_THIS_FRAME);
                    Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, unchecked((int)0xc0a90510), 4700.0f, -5145.0f, 0, 0);
                }
            }
            else
            {
                Function.Call(Hash.SET_SCENARIO_GROUP_ENABLED, "Heist_Island_Peds", false);
                Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", false);
                Function.Call((Hash)0xF74B1FFA4A15FBEA, 0);
            }
        }

        private void SetCayoPerico(object sender, EventArgs e)
        {
            if (!cayoOn)
            {
                Notification.Show("~r~Attiva prima 'Load Cayo Perico'");
                return;
            }

            Vector3 pos = new Vector3(4840.57f, -5174.43f, 2.0f);

            Function.Call(Hash.NEW_LOAD_SCENE_START_SPHERE, pos.X, pos.Y, pos.Z, 300f, 0);
            Script.Wait(3000);
            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, pos.X, pos.Y, pos.Z);

            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
                player.Character.Position = pos;
            else
                player.Character.CurrentVehicle.Position = pos;

            Function.Call(Hash.NEW_LOAD_SCENE_STOP);
        }

     


        private void SetCayoPericoUnload(object sender, EventArgs e)
        {
            cayoOn = false;
            Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", false); // SET_ISLAND_HOPPER_ENABLED
            Function.Call((Hash)0x5E1460624D194A38, false);                // minimappa isola
            Function.Call((Hash)0xF74B1FFA4A15FBEA, false);                // nodi AI isola (a memoria, la meno sicura)
            Function.Call(Hash.SET_AUDIO_FLAG, "PlayerOnDLCHeist4Island", false);
            Function.Call((Hash)0x9BAE5AD2508DF078, false);                // SET_INSTANCE_PRIORITY_MODE(1)
            Function.Call((Hash)0x0888C3502DBBEEF5, false);                      // ON_ENTER_MP

            Game.Player.Character.Position = new Vector3(-1037.0f, -2738.0f, 20.169f);
        }



        //END CAYO PERICO FUNCTION


        private void SetMethFarm(object sender, EventArgs e)
        {
            // Carica i dati MP, come per Cayo (una sola volta)
            if (!cayoMpLoaded)
            {
                Function.Call((Hash)0x888C3502DBBEEF5); // ON_ENTER_MP
                cayoMpLoaded = true;
                Script.Wait(1000);
            }

            // Nomi a memoria: non so quale dei ware0X sia il meth, quindi li chiedo tutti
            string[] bikerIpls =
            {
                 "bkr_biker_interior_placement_interior_1_biker_dlc_int_ware01_milo", //Meth Lab
                 "bkr_biker_interior_placement_interior_2_biker_dlc_int_ware02_milo", 
                 "bkr_biker_interior_placement_interior_3_biker_dlc_int_ware03_milo",
                 "bkr_biker_interior_placement_interior_4_biker_dlc_int_ware04_milo",
                 "bkr_biker_interior_placement_interior_5_biker_dlc_int_ware05_milo",
                 "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo",   // clubhouse 1
                 "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo"   // clubhouse 2
            };
            foreach (string ipl in bikerIpls)
                Function.Call(Hash.REQUEST_IPL, ipl);

            Script.Wait(1500);

            Vector3 pos = new Vector3(1009.5f, -3196.6f, -38.99f);

            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, pos.X, pos.Y, pos.Z);
            //Notification.Show("Interior ID: " + interior);

            if (interior == 0)
            {
                Notification.Show("~r~Interno non trovato, teleport annullato");
                return;
            }

            Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior);
            Function.Call(Hash.DISABLE_INTERIOR, interior, false);
            Function.Call(Hash.CAP_INTERIOR, interior, false);

            int waited = 0;
            while (!Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && waited < 5000)
            {
                Script.Wait(100);
                waited += 100;
            }

            // Entity set del meth lab (nomi a memoria)
            string[] methSets =
            {
                 //"meth_lab_basic",
                 "meth_lab_upgrade",
                 "meth_lab_production",
                 "meth_lab_security_high",
                 "meth_lab_setup"
            };

            string report = "";
            foreach (string set in methSets)
            {
                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, set);
                report += set + ": " + Function.Call<bool>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, interior, set) + "\n";
            }

            Function.Call(Hash.REFRESH_INTERIOR, interior);
            Script.Wait(1000);
            //Notification.Show(report);

            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, pos.X, pos.Y, pos.Z);
            Script.Wait(500);

            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
                player.Character.Position = pos;
            else
                player.Character.CurrentVehicle.Position = pos;
        }





        private void LoadBikerBusiness(Vector3 pos, string[] sets)
        {
            if (!cayoMpLoaded)
            {
                Function.Call((Hash)0x888C3502DBBEEF5); // ON_ENTER_MP
                cayoMpLoaded = true;
                Script.Wait(1000);
            }

            string[] bikerIpls =
            {
        "bkr_biker_interior_placement_interior_1_biker_dlc_int_ware01_milo",
        "bkr_biker_interior_placement_interior_2_biker_dlc_int_ware02_milo",
        "bkr_biker_interior_placement_interior_3_biker_dlc_int_ware03_milo",
        "bkr_biker_interior_placement_interior_4_biker_dlc_int_ware04_milo",
        "bkr_biker_interior_placement_interior_5_biker_dlc_int_ware05_milo"
    };
            foreach (string ipl in bikerIpls)
                Function.Call(Hash.REQUEST_IPL, ipl);

            Script.Wait(1500);

            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, pos.X, pos.Y, pos.Z);
            //Notification.Show("Interior ID: " + interior);

            if (interior == 0)
            {
                Notification.Show("~r~Interno non trovato, teleport annullato");
                return;
            }

            Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior);
            Function.Call(Hash.DISABLE_INTERIOR, interior, false);
            Function.Call(Hash.CAP_INTERIOR, interior, false);

            int waited = 0;
            while (!Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && waited < 5000)
            {
                Script.Wait(100);
                waited += 100;
            }

            string report = "";
            foreach (string set in sets)
            {
                Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, set);
                report += set + ": " + Function.Call<bool>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, interior, set) + "\n";
            }

            Function.Call(Hash.REFRESH_INTERIOR, interior);
            Script.Wait(1000);
            //Notification.Show(report);

            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, pos.X, pos.Y, pos.Z);
            Script.Wait(500);

            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
                player.Character.Position = pos;
            else
                player.Character.CurrentVehicle.Position = pos;
        }

        private void SetWeedLab(object sender, EventArgs e)
        {
            LoadBikerBusiness(new Vector3(1051.491f, -3196.536f, -39.14f), new string[]
            {
        "weed_standard_equip", "weed_upgrade_equip", "weed_security_upgrade",
        "weed_drying", "weed_chairs", "weed_production",
        "weed_growtha_stage3", "weed_growthb_stage3", "weed_growthc_stage3",
        "weed_growthd_stage3", "weed_growthe_stage3", "weed_growthf_stage3",
        "weed_growthg_stage3", "weed_growthh_stage3", "weed_growthi_stage3",
        "weed_hosea", "weed_hoseb", "weed_hosec", "weed_hosed", "weed_hosee",
        "weed_hosef", "weed_hoseg", "weed_hoseh", "weed_hosei"
            });
        }

        private void SetCocaineLab(object sender, EventArgs e)
        {
            LoadBikerBusiness(new Vector3(1093.6f, -3196.6f, -38.99f), new string[]
            {
        "set_up", "equipment_basic", "equipment_upgrade", "security_high",
        "production_basic", "production_upgrade", "table_equipment",
        "table_equipment_upgrade", "coke_press_upgrade", "furniture_upgrade",
        "special_chairs", "coke_cut_01", "coke_cut_02", "coke_cut_03",
        "coke_cut_04", "coke_cut_05"
            });
        }

        private void SetCounterfeitLab(object sender, EventArgs e)
        {
            LoadBikerBusiness(new Vector3(1121.897f, -3195.338f, -40.4f), new string[]
            {
        "counterfeit_setup", "counterfeit_standard_equip", "counterfeit_upgrade_equip",
        "counterfeit_security", "money_cutter", "special_chairs",
        "dryera_on", "dryerb_on", "dryerc_on", "dryerd_on",
        "counterfeit_cashpile_a", "counterfeit_cashpile_b",
        "counterfeit_cashpile_c", "counterfeit_cashpile_d"
            });
        }

        private void SetForgeryOffice(object sender, EventArgs e)
        {
            LoadBikerBusiness(new Vector3(1165.0f, -3196.6f, -39.01f), new string[]
            {
        /*"interior_basic",*/ "interior_upgrade",
        "set_up", /*"equipment_basic",*/ "equipment_upgrade",
        "production", /*"security_low",*/ "security_high",
        "clutter", "chair01", "chair02", "chair03",
        "chair04", "chair05", "chair06", "chair07"
            });
        }


        private void SetClubhouse1(object sender, EventArgs e)
        {
            LoadBikerBusiness(new Vector3(1107.04f, -3157.399f, -37.519f), new string[]
            {
        "walls_01", "furnishings_01", "decorative_01",
        "mural_01", "gun_locker", "mod_booth",
        "lower_walls_default", "upper_walls_default"
            });
        }

        private void SetClubhouse2(object sender, EventArgs e)
        {
            LoadBikerBusiness(new Vector3(998.4809f, -3164.711f, -38.907f), new string[]
            {
        "walls_01", "furnishings_01", "decorative_01",
        "mural_01", "gun_locker", "mod_booth",
        "lower_walls_default", "upper_walls_default"
            });
        }




        private void SetJewelStore(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-631.0f, -236.0f, 38.2f);
                Function.Call(Hash.REQUEST_IPL, "post_hiest_unload");
                Function.Call(Hash.REQUEST_IPL, "post_hiest_unload_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-631.0f, -236.0f, 38.2f);
            }
        }

        private void SetMaxRendaStore(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-585.8247f, -282.72f, 35.45475f);
                Function.Call(Hash.REQUEST_IPL, "refit_unload");
                Function.Call(Hash.REQUEST_IPL, "refit_unload_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-585.8247f, -282.72f, 35.45475f);
            }
        }

        private void SetStadium(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-250.0f, -2010.0f, 30.0f);
                Function.Call(Hash.REQUEST_IPL, "sp1_10_real_interior");
                Function.Call(Hash.REQUEST_IPL, "sp1_10_real_interior_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-250.0f, -2010.0f, 30.0f);
            }
        }


        private void SetUnionDepo(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(2.6968f, -667.0166f, 16.13061f);
                Function.Call(Hash.REQUEST_IPL, "FINBANK");
                Function.Call(Hash.REQUEST_IPL, "FINBANK_lod");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(2.6968f, -667.0166f, 16.13061f);
            }
        }

        private void SetModernApp1(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-786.8663f, 315.7642f, 217.6385f);
                Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_01_a");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-786.8663f, 315.7642f, 217.6385f);
            }
        }

        private void SetModernApp2(object sender, EventArgs e)
        {
            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = new Vector3(-786.9563f, 315.6229f, 187.9136f);
                Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_01_c");
            }
            else
            {
                Vehicle v = player.Character.CurrentVehicle;
                v.Position = new Vector3(-786.9563f, 315.6229f, 187.9136f);
            }
        }



        private static readonly string[] apartmentIpls =
        {
        "apa_v_mp_h_03_a", "apa_v_mp_h_03_b", "apa_v_mp_h_03_c",
        "apa_v_mp_h_04_a", "apa_v_mp_h_04_b", "apa_v_mp_h_04_c",
        "apa_v_mp_h_05_a", "apa_v_mp_h_05_b", "apa_v_mp_h_05_c",
        "apa_v_mp_h_06_a", "apa_v_mp_h_06_b", "apa_v_mp_h_06_c",
        "apa_v_mp_h_07_a", "apa_v_mp_h_07_b", "apa_v_mp_h_07_c",
        "apa_v_mp_h_08_a", "apa_v_mp_h_08_b", "apa_v_mp_h_08_c"
        };

        private void LoadApartment(string ipl, Vector3 pos)
        {
            if (!cayoMpLoaded)
            {
                Function.Call((Hash)0x888C3502DBBEEF5); // ON_ENTER_MP
                cayoMpLoaded = true;
                Script.Wait(1000);
            }

            // Toglie gli altri stili, per non avere due arredamenti sovrapposti
            foreach (string other in apartmentIpls)
            {
                if (other != ipl)
                    Function.Call(Hash.REMOVE_IPL, other);
            }

            Function.Call(Hash.REQUEST_IPL, ipl);
            Script.Wait(1500);

            Notification.Show(ipl + " attivo: " + Function.Call<bool>(Hash.IS_IPL_ACTIVE, ipl));

            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, pos.X, pos.Y, pos.Z);
            if (interior != 0)
            {
                Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior);
                Function.Call(Hash.DISABLE_INTERIOR, interior, false);
                Function.Call(Hash.CAP_INTERIOR, interior, false);

                int waited = 0;
                while (!Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && waited < 5000)
                {
                    Script.Wait(100);
                    waited += 100;
                }
            }

            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, pos.X, pos.Y, pos.Z);
            Script.Wait(500);

            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
                player.Character.Position = pos;
            else
                player.Character.CurrentVehicle.Position = pos;
        }


        private void SetVibrant1(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_03_a", new Vector3(-786.6245f, 315.6175f, 217.6385f)); }
        private void SetVibrant2(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_03_c", new Vector3(-786.9584f, 315.7974f, 187.9135f)); }
        private void SetVibrant3(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_03_b", new Vector3(-774.0223f, 342.1718f, 196.6863f)); }

        private void SetSharp1(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_04_a", new Vector3(-787.0902f, 315.7039f, 217.6384f)); }
        private void SetSharp2(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_04_c", new Vector3(-787.0155f, 315.7071f, 187.9135f)); }
        private void SetSharp3(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_04_b", new Vector3(-773.8976f, 342.1525f, 196.6863f)); }

        private void SetMonochrome1(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_05_a", new Vector3(-786.9887f, 315.7393f, 217.6386f)); }
        private void SetMonochrome2(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_05_c", new Vector3(-786.8809f, 315.6634f, 187.9136f)); }
        private void SetMonochrome3(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_05_b", new Vector3(-774.0675f, 342.0773f, 196.6864f)); }

        private void SetSeductive1(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_06_a", new Vector3(-787.1423f, 315.6943f, 217.6384f)); }
        private void SetSeductive2(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_06_c", new Vector3(-787.0961f, 315.815f, 187.9135f)); }
        private void SetSeductive3(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_06_b", new Vector3(-773.9552f, 341.9892f, 196.6862f)); }

        private void SetRegal1(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_07_a", new Vector3(-787.029f, 315.7113f, 217.6385f)); }
        private void SetRegal2(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_07_c", new Vector3(-787.0574f, 315.6567f, 187.9135f)); }
        private void SetRegal3(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_07_b", new Vector3(-774.0109f, 342.0965f, 196.6863f)); }

        private void SetAqua1(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_08_a", new Vector3(-786.9469f, 315.5655f, 217.6383f)); }
        private void SetAqua2(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_08_c", new Vector3(-786.9756f, 315.723f, 187.9134f)); }
        private void SetAqua3(object sender, EventArgs e) { LoadApartment("apa_v_mp_h_08_b", new Vector3(-774.0349f, 342.0296f, 196.6862f)); }


        private static readonly string[] arcadiusOfficeIpls =
        {
        "ex_dt1_02_office_01a", "ex_dt1_02_office_01b", "ex_dt1_02_office_01c",
        "ex_dt1_02_office_02a", "ex_dt1_02_office_02b", "ex_dt1_02_office_02c",
        "ex_dt1_02_office_03a", "ex_dt1_02_office_03b", "ex_dt1_02_office_03c"
        };

        private void LoadArcadius(string ipl, Vector3 pos, bool isOffice)
        {
            if (!cayoMpLoaded)
            {
                Function.Call((Hash)0x888C3502DBBEEF5); // ON_ENTER_MP
                cayoMpLoaded = true;
                Script.Wait(1000);
            }

            // Solo gli uffici sono stili alternativi: garage e mod shop sono indipendenti
            if (isOffice)
            {
                foreach (string other in arcadiusOfficeIpls)
                {
                    if (other != ipl)
                        Function.Call(Hash.REMOVE_IPL, other);
                }
            }

            Function.Call(Hash.REQUEST_IPL, ipl);
            Script.Wait(1500);

            Notification.Show(ipl + " attivo: " + Function.Call<bool>(Hash.IS_IPL_ACTIVE, ipl));

            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, pos.X, pos.Y, pos.Z);
            if (interior != 0)
            {
                Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior);
                Function.Call(Hash.DISABLE_INTERIOR, interior, false);
                Function.Call(Hash.CAP_INTERIOR, interior, false);

                int waited = 0;
                while (!Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && waited < 5000)
                {
                    Script.Wait(100);
                    waited += 100;
                }
            }

            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, pos.X, pos.Y, pos.Z);
            Script.Wait(500);

            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
                player.Character.Position = pos;
            else
                player.Character.CurrentVehicle.Position = pos;
        }

        private void SetExecRich(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_02b", new Vector3(-141.1987f, -620.913f, 168.8205f), true); }
        private void SetExecCool(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_02c", new Vector3(-141.5429f, -620.9524f, 168.8204f), true); }
        private void SetExecContrast(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_02a", new Vector3(-141.2896f, -620.9618f, 168.8204f), true); }
        private void SetOldSpiceWarm(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_01a", new Vector3(-141.4966f, -620.8292f, 168.8204f), true); }
        private void SetOldSpiceClassical(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_01b", new Vector3(-141.3997f, -620.9006f, 168.8204f), true); }
        private void SetOldSpiceVintage(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_01c", new Vector3(-141.5361f, -620.9186f, 168.8204f), true); }
        private void SetPowerBrokerIce(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_03a", new Vector3(-141.392f, -621.0451f, 168.8204f), true); }
        private void SetPowerBrokerConservative(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_03b", new Vector3(-141.1945f, -620.8729f, 168.8204f), true); }
        private void SetPowerBrokerPolished(object sender, EventArgs e) { LoadArcadius("ex_dt1_02_office_03c", new Vector3(-141.4924f, -621.0035f, 168.8205f), true); }

        private void SetArcadiusGarage1(object sender, EventArgs e) { LoadArcadius("imp_dt1_02_cargarage_a", new Vector3(-191.0133f, -579.1428f, 135.0f), false); }
        private void SetArcadiusGarage2(object sender, EventArgs e) { LoadArcadius("imp_dt1_02_cargarage_b", new Vector3(-117.4989f, -568.1132f, 135.0f), false); }
        private void SetArcadiusGarage3(object sender, EventArgs e) { LoadArcadius("imp_dt1_02_cargarage_c", new Vector3(-136.078f, -630.1852f, 135.0f), false); }
        private void SetArcadiusModShop(object sender, EventArgs e) { LoadArcadius("imp_dt1_02_modgarage", new Vector3(-146.6166f, -596.6301f, 166.0f), false); }



        private void SetYatch(object sender, EventArgs e)
        {
            Function.Call(Hash.REQUEST_IPL, "smboat");
            Function.Call(Hash.REQUEST_IPL, "smboat_lod");

            Script.Wait(1500);

            Vector3 pos = new Vector3(-2045.8f, -1031.2f, 11.9f);
            Vector3 p = Game.Player.Character.Position;
            //Notification.Show($"X: {p.X:F2}  Y: {p.Y:F2}  Z: {p.Z:F2}");

            Player player = Game.Player;
            if (!player.Character.IsInVehicle())
            {
                player.Character.Position = pos;
            }
            else
            {
                player.Character.CurrentVehicle.Position = pos;
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

        private void SetThermalVision(object sender, EventArgs e)
        {
            if (ThermalVision.Checked == true)
            {
                Game.IsThermalVisionActive = true;
            }
            
            if (ThermalVision.Checked == false)
            {
                Game.IsThermalVisionActive = false;
            }
        }

        private void SetNightVision(object sender, EventArgs e)
        {
            if (NightVision.Checked == true)
            {
                Game.IsNightVisionActive = true;
            }

            if (NightVision.Checked == false)
            {
                Game.IsNightVisionActive = false;
            }
        }

        private void SetMatrixVision(object sender, EventArgs e)
        {

            if (MatrixVision.Checked == true)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "FocusIn", "HintCamSounds", true);
                Function.Call(Hash.SET_TIME_SCALE, 0.3f);
                Notification.Show("~b~Matrix Mode ~g~Attivata");
            }

            if (MatrixVision.Checked == false)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "FocusIn", "HintCamSounds", false);
                Function.Call(Hash.SET_TIME_SCALE, 1.0f);
                Notification.Show("~b~Matrix Mode ~r~Disabled");
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

            DoMeteorShowerTick();


            DoCayoTick();

            if (Game.Player.Character.IsDead)
            {
                RespawnPed();
                Game.Player.Character.Position = new Vector3(0, 0, 0);
                Game.Player.Character.Health = 200;
                Game.Player.Character.Armor = 100;
            }





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

            if (expowpon)
            {
                 Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Game.Player, true);
                //GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Call911, "Anoniaaak", "Welcome", "Essential Menu 1.3.32 BETA", true, true);
            }

            if (expoml)
            {
                Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, Game.Player, true);
            }

            if (moneygun)
            {
                Game.Player.Character.Weapons.Current.InfiniteAmmo = true;
                Game.Player.Character.Weapons.Current.InfiniteAmmoClip = true;
                Ped ped = Game.Player.Character;
                if (Function.Call<bool>(Hash.IS_PED_SHOOTING, ped.Handle))
                {
                    OutputArgument arg = new OutputArgument();
                    Function.Call(Hash.GET_PED_LAST_WEAPON_IMPACT_COORD, ped.Handle, arg);
                    GTA.Math.Vector3 result = arg.GetResult<GTA.Math.Vector3>();
                    if (result != GTA.Math.Vector3.Zero)
                    {
                        Model model = new Model(0x113FD533);
                        if (!model.IsLoaded)
                            model.Request(1000);
                        int hash = Function.Call<int>(Hash.GET_HASH_KEY, "PICKUP_MONEY_CASE");
                        Function.Call(Hash.CREATE_AMBIENT_PICKUP, hash, result.X, result.Y, result.Z, 0, 1000000, model.Hash, 0, 1);
                    }
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

            ScriptSettings.Load("scripts\\esconfig.ini");
            GTA.ScriptSettings configkey;
            Keys OpenMenu;

            configkey = ScriptSettings.Load("scripts\\esconfig.ini");
            OpenMenu = configkey.GetValue<Keys>("Options", "OpenMenu", Keys.F6); //The F6 key will be set my default, but the user can change the key

            if (e.KeyCode == OpenMenu) //f6 //put whatever key binding you want here (e.KeyCode == Keys.F6)
            {
                DemoMenu.Visible = !DemoMenu.Visible;
                //DemoMenu.Visible = true;

                //GTA.UI.Notification.Show("Welcome",true);
                GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Call911,"Anonik","Welcome","Essential Menu 1.336 BETA", true,true);
                //ShowMissionPassedMessage("ESSENTIAL MENU", "Versione BETA");
                // Crea e mostra un messaggio grande stile "Mission Passed" con titolo e sottotitolo
                //bigMessage = new BigMessage("ESSENTIAL MENU", "VERSIONE BETA", MessageType.MissionPassedOldGen);
               // bigMessageTimer = Environment.TickCount;





            }
        }
    }
}
