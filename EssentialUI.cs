using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Font = GTA.UI.Font;

// ============================================================================
//  EssentialUI: libreria grafica dei menu (sostituisce LemonUI).
//
//  Ha gli stessi nomi di LemonUI (NativeMenu, NativeItem, NativeCheckboxItem,
//  NativeListItem<T>, NativeSliderItem, ObjectPool) e gli stessi eventi
//  (Activated, CheckboxChanged, ItemChanged, ValueChanged), cosi' il codice dei
//  menu in Basics.cs resta quasi uguale. Il disegno e' fatto da zero con
//  DRAW_RECT e TextElement: temi, animazioni del banner, globo, font scelti dal
//  menu "Style", salvati in scripts\essentialui.ini.
//
//  Comandi: Su/Giu (o NumPad 8/2), Sinistra/Destra (o NumPad 4/6),
//           Invio (o NumPad 5), Backspace (o NumPad 0).
//
//  NON compilato da me: se Visual Studio segnala errori, mandameli.
// ============================================================================
namespace EssentialUI
{
    // ------------------------------------------------------------ eventi
    public class ItemChangedEventArgs<T> : EventArgs
    {
        public int Index { get; internal set; }
        public T Object { get; internal set; }
    }

    public enum ItemKind { Button, Checkbox, List, Slider, Submenu }

    // ------------------------------------------------------------ voci
    public class NativeItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AltTitle { get; set; }
        public bool Enabled { get; set; }

        public event EventHandler Activated;

        public NativeItem(string title) : this(title, "", "") { }
        public NativeItem(string title, string description) : this(title, description, "") { }
        public NativeItem(string title, string description, string altTitle)
        {
            Title = title ?? "";
            Description = description ?? "";
            AltTitle = altTitle ?? "";
            Enabled = true;
        }

        internal virtual ItemKind Kind { get { return ItemKind.Button; } }
        internal virtual string ValueText { get { return null; } }
        internal virtual bool IsOn { get { return false; } }
        internal virtual float Fraction { get { return 0f; } }
        internal virtual Color BarColor { get { return Color.Empty; } }
        internal virtual void Left() { }
        internal virtual void Right() { }
        internal virtual void Activate() { OnActivated(); }

        protected void OnActivated()
        {
            EventHandler handler = Activated;
            if (handler == null)
                return;
            try
            {
                handler(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // un errore in una funzione non deve fermare tutto lo script
                Notification.Show("Errore: " + ex.Message);
            }
        }
    }

    public class NativeCheckboxItem : NativeItem
    {
        bool isChecked;
        public event EventHandler CheckboxChanged;

        public NativeCheckboxItem(string title) : base(title) { }
        public NativeCheckboxItem(string title, bool check) : base(title) { isChecked = check; }
        public NativeCheckboxItem(string title, string description) : base(title, description) { }
        public NativeCheckboxItem(string title, string description, bool check) : base(title, description) { isChecked = check; }

        public bool Checked
        {
            get { return isChecked; }
            set
            {
                if (isChecked == value)
                    return;
                isChecked = value;
                EventHandler handler = CheckboxChanged;
                if (handler != null)
                {
                    try { handler(this, EventArgs.Empty); }
                    catch (Exception ex) { Notification.Show("Errore: " + ex.Message); }
                }
            }
        }

        internal override ItemKind Kind { get { return ItemKind.Checkbox; } }
        internal override bool IsOn { get { return isChecked; } }

        internal override void Activate()
        {
            Checked = !Checked;   // prima cambia il valore, poi avvisa (come LemonUI)
            OnActivated();
        }
    }

    public class NativeListItem<T> : NativeItem
    {
        public List<T> Items { get; private set; }
        int index;
        public event EventHandler<ItemChangedEventArgs<T>> ItemChanged;

        public NativeListItem(string title, params T[] items) : base(title)
        {
            Items = new List<T>(items ?? new T[0]);
        }

        public NativeListItem(string title, string description, params T[] items) : base(title, description)
        {
            Items = new List<T>(items ?? new T[0]);
        }

        public int SelectedIndex
        {
            get { return index; }
            set { index = (Items.Count == 0) ? 0 : Math.Max(0, Math.Min(Items.Count - 1, value)); }
        }

        public T SelectedItem
        {
            get { return Items.Count == 0 ? default(T) : Items[index]; }
        }

        internal override ItemKind Kind { get { return ItemKind.List; } }
        internal override string ValueText
        {
            get
            {
                if (Items.Count == 0)
                    return "";
                T v = Items[index];
                return v == null ? "" : v.ToString();
            }
        }

        void Change(int delta)
        {
            if (Items.Count == 0)
                return;

            index = (index + delta + Items.Count) % Items.Count;

            EventHandler<ItemChangedEventArgs<T>> handler = ItemChanged;
            if (handler != null)
            {
                ItemChangedEventArgs<T> args = new ItemChangedEventArgs<T>();
                args.Index = index;
                args.Object = Items[index];
                try { handler(this, args); }
                catch (Exception ex) { Notification.Show("Errore: " + ex.Message); }
            }
        }

        internal override void Left() { Change(-1); }
        internal override void Right() { Change(1); }
    }

    public class NativeSliderItem : NativeItem
    {
        int value;
        public int Maximum { get; set; }
        public Color SliderColor { get; set; }
        public event EventHandler ValueChanged;

        public NativeSliderItem(string title, string description, int maximum, int startValue) : base(title, description)
        {
            Maximum = Math.Max(1, maximum);
            value = Math.Max(0, Math.Min(Maximum, startValue));
            SliderColor = Color.Empty;
        }

        public int Value
        {
            get { return value; }
            set
            {
                int v = Math.Max(0, Math.Min(Maximum, value));
                if (v == this.value)
                    return;
                this.value = v;
                EventHandler handler = ValueChanged;
                if (handler != null)
                {
                    try { handler(this, EventArgs.Empty); }
                    catch (Exception ex) { Notification.Show("Errore: " + ex.Message); }
                }
            }
        }

        internal override ItemKind Kind { get { return ItemKind.Slider; } }
        internal override float Fraction { get { return Maximum > 0 ? value / (float)Maximum : 0f; } }
        internal override Color BarColor { get { return SliderColor; } }
        internal override void Left() { Value = value - 1; }
        internal override void Right() { Value = value + 1; }
        internal override void Activate() { OnActivated(); }
    }

    public class NativeSubmenuItem : NativeItem
    {
        public NativeMenu Menu { get; private set; }

        public NativeSubmenuItem(NativeMenu menu) : base(menu.Subtitle, menu.Description)
        {
            Menu = menu;
        }

        internal override ItemKind Kind { get { return ItemKind.Submenu; } }

        internal override void Activate()
        {
            UiHost.OpenSub(Menu);
            OnActivated();
        }
    }

    // ------------------------------------------------------------ menu
    public class NativeMenu
    {
        public string Title { get; set; }         // titolo nel banner
        public string Subtitle { get; set; }      // nome mostrato anche come voce nel menu padre
        public string Description { get; set; }
        public List<NativeItem> Items { get; private set; }

        internal int Selected;
        internal int Scroll;

        public NativeMenu(string bannerText, string name) : this(bannerText, name, "") { }

        public NativeMenu(string bannerText, string name, string description)
        {
            Title = bannerText ?? "";
            Subtitle = name ?? "";
            Description = description ?? "";
            Items = new List<NativeItem>();
        }

        public int SelectedIndex
        {
            get { return Selected; }
            set { Selected = Math.Max(0, Math.Min(Math.Max(0, Items.Count - 1), value)); }
        }

        public NativeItem SelectedItem
        {
            get { return (Items.Count == 0) ? null : Items[Math.Max(0, Math.Min(Items.Count - 1, Selected))]; }
        }

        public void Add(NativeItem item)
        {
            if (item != null)
                Items.Add(item);
        }

        public void Remove(NativeItem item)
        {
            Items.Remove(item);
            ClampSelection();
        }

        public void Clear()
        {
            Items.Clear();
            Selected = 0;
            Scroll = 0;
        }

        public NativeSubmenuItem AddSubMenu(NativeMenu menu)
        {
            NativeSubmenuItem item = new NativeSubmenuItem(menu);
            Items.Add(item);
            return item;
        }

        public bool Visible
        {
            get { return UiHost.IsTop(this); }
            set
            {
                if (value)
                    UiHost.Show(this);
                else
                    UiHost.Hide(this);
            }
        }

        internal void ClampSelection()
        {
            int count = Items.Count;
            if (count == 0)
            {
                Selected = 0;
                Scroll = 0;
                return;
            }
            Selected = Math.Max(0, Math.Min(count - 1, Selected));
            Scroll = Math.Max(0, Math.Min(Math.Max(0, count - UiHost.MaxVisible), Scroll));
            if (Selected < Scroll)
                Scroll = Selected;
            if (Selected >= Scroll + UiHost.MaxVisible)
                Scroll = Selected - UiHost.MaxVisible + 1;
        }
    }

    public class ObjectPool
    {
        readonly List<NativeMenu> menus = new List<NativeMenu>();

        public void Add(NativeMenu menu)
        {
            if (menu != null && !menus.Contains(menu))
                menus.Add(menu);
        }

        public bool AreAnyVisible { get { return UiHost.IsOpen; } }

        public void HideAll() { UiHost.HideAll(); }

        // da chiamare a ogni Tick
        public void Process() { UiHost.Process(); }
    }

    // ------------------------------------------------------------ temi
    public class UiTheme
    {
        public string Name;
        public Color BannerA, BannerB, Accent, Row, Sel, SelText, Text, Panel;
        public bool Rainbow; // tema dinamico: banner, accento e selezione cambiano colore
    }

    // ------------------------------------------------------------ impostazioni e menu Style
    public static class UiStyle
    {
        // font di gioco selezionabili: ID, nome, scala "giusta" per le voci e per il titolo
        internal static readonly int[] FontIds = { 4, 0, 7, 1, 2 };
        internal static readonly string[] FontNames = { "Chalet Comprime", "Chalet London", "Pricedown", "House Script", "Monospace" };

        internal static float ItemFontScale(int id)
        {
            switch (id)
            {
                case 4: return 0.5f;
                case 0: return 0.4f;
                case 7: return 0.4f;
                case 1: return 0.6f;
                default: return 0.38f;
            }
        }

        internal static float TitleFontScale(int id)
        {
            switch (id)
            {
                case 7: return 0.95f;
                case 1: return 1.0f;
                case 4: return 0.9f;
                default: return 0.7f;
            }
        }

        internal static readonly UiTheme[] Themes = new UiTheme[]
        {
            new UiTheme { Name = "Neon Glass",
                BannerA = Color.FromArgb(15, 40, 60), BannerB = Color.FromArgb(24, 120, 150),
                Accent = Color.FromArgb(61, 224, 255), Row = Color.FromArgb(205, 8, 14, 20),
                Sel = Color.FromArgb(235, 18, 96, 120), SelText = Color.White,
                Text = Color.FromArgb(230, 244, 250), Panel = Color.FromArgb(230, 6, 10, 15) },
            new UiTheme { Name = "Classic",
                BannerA = Color.FromArgb(18, 80, 170), BannerB = Color.FromArgb(40, 140, 255),
                Accent = Color.FromArgb(70, 150, 255), Row = Color.FromArgb(185, 0, 0, 0),
                Sel = Color.FromArgb(245, 255, 255, 255), SelText = Color.Black,
                Text = Color.White, Panel = Color.FromArgb(225, 0, 0, 0) },
            new UiTheme { Name = "Red",
                BannerA = Color.FromArgb(70, 6, 10), BannerB = Color.FromArgb(200, 24, 34),
                Accent = Color.FromArgb(224, 36, 47), Row = Color.FromArgb(215, 11, 11, 12),
                Sel = Color.FromArgb(245, 224, 36, 47), SelText = Color.White,
                Text = Color.White, Panel = Color.FromArgb(235, 11, 11, 12) },
            new UiTheme { Name = "Amber Terminal",
                BannerA = Color.FromArgb(50, 34, 0), BannerB = Color.FromArgb(150, 100, 0),
                Accent = Color.FromArgb(255, 176, 0), Row = Color.FromArgb(220, 7, 8, 7),
                Sel = Color.FromArgb(245, 255, 176, 0), SelText = Color.Black,
                Text = Color.FromArgb(255, 190, 40), Panel = Color.FromArgb(235, 7, 8, 7) },
            new UiTheme { Name = "Purple Haze",
                BannerA = Color.FromArgb(35, 12, 70), BannerB = Color.FromArgb(120, 56, 200),
                Accent = Color.FromArgb(190, 130, 255), Row = Color.FromArgb(210, 12, 8, 24),
                Sel = Color.FromArgb(240, 120, 60, 200), SelText = Color.White,
                Text = Color.FromArgb(240, 230, 255), Panel = Color.FromArgb(235, 10, 6, 20) },
            new UiTheme { Name = "Gold Black",
                BannerA = Color.FromArgb(30, 24, 8), BannerB = Color.FromArgb(130, 100, 24),
                Accent = Color.FromArgb(230, 185, 60), Row = Color.FromArgb(215, 10, 10, 10),
                Sel = Color.FromArgb(245, 230, 185, 60), SelText = Color.Black,
                Text = Color.FromArgb(245, 238, 215), Panel = Color.FromArgb(235, 8, 8, 8) },
            new UiTheme { Name = "Rainbow", Rainbow = true,
                BannerA = Color.FromArgb(20, 20, 30), BannerB = Color.FromArgb(60, 60, 90),
                Accent = Color.White, Row = Color.FromArgb(210, 8, 10, 16),
                Sel = Color.FromArgb(240, 40, 40, 40), SelText = Color.White,
                Text = Color.White, Panel = Color.FromArgb(230, 6, 8, 12) },
            new UiTheme { Name = "Sky Blue",
                BannerA = Color.FromArgb(70, 112, 245), BannerB = Color.FromArgb(100, 142, 255),
                Accent = Color.FromArgb(0, 84, 255), Row = Color.FromArgb(255, 60, 60, 62),
                Sel = Color.FromArgb(255, 80, 150, 210), SelText = Color.White,
                Text = Color.White, Panel = Color.FromArgb(255, 60, 60, 62) },
            new UiTheme { Name = "Ocean",
                BannerA = Color.FromArgb(6, 40, 70), BannerB = Color.FromArgb(10, 110, 140),
                Accent = Color.FromArgb(80, 220, 230), Row = Color.FromArgb(205, 6, 18, 28),
                Sel = Color.FromArgb(235, 10, 100, 130), SelText = Color.White,
                Text = Color.FromArgb(225, 245, 250), Panel = Color.FromArgb(230, 5, 14, 22) },
            new UiTheme { Name = "Sunset",
                BannerA = Color.FromArgb(90, 20, 60), BannerB = Color.FromArgb(230, 100, 40),
                Accent = Color.FromArgb(255, 160, 80), Row = Color.FromArgb(210, 20, 10, 18),
                Sel = Color.FromArgb(240, 200, 70, 50), SelText = Color.White,
                Text = Color.FromArgb(255, 238, 225), Panel = Color.FromArgb(235, 16, 8, 14) },
            new UiTheme { Name = "Mint",
                BannerA = Color.FromArgb(10, 60, 45), BannerB = Color.FromArgb(30, 160, 110),
                Accent = Color.FromArgb(110, 240, 180), Row = Color.FromArgb(205, 6, 20, 16),
                Sel = Color.FromArgb(235, 25, 130, 95), SelText = Color.White,
                Text = Color.FromArgb(225, 250, 240), Panel = Color.FromArgb(230, 5, 16, 12) },
            new UiTheme { Name = "Mono",
                BannerA = Color.FromArgb(30, 30, 32), BannerB = Color.FromArgb(90, 90, 95),
                Accent = Color.FromArgb(200, 200, 205), Row = Color.FromArgb(215, 14, 14, 15),
                Sel = Color.FromArgb(245, 235, 235, 238), SelText = Color.Black,
                Text = Color.FromArgb(235, 235, 238), Panel = Color.FromArgb(235, 10, 10, 11) },
            new UiTheme { Name = "Azure",
                BannerA = Color.FromArgb(26, 86, 176), BannerB = Color.FromArgb(50, 120, 214),
                Accent = Color.FromArgb(120, 185, 255), Row = Color.FromArgb(255, 58, 58, 58),
                Sel = Color.FromArgb(255, 236, 236, 236), SelText = Color.Black,
                Text = Color.White, Panel = Color.FromArgb(255, 10, 10, 10) },
        };

        // ---- impostazioni correnti (valori iniziali = preset Default) ----
        internal static int ThemeIdx = 0;
        internal static int BannerAnim = 0;      // 0 Pulse, 1 Static, 2 Rainbow, 3 Shimmer, 4 Scanline, 5 Heartbeat, 6 Globe Spin, 7 Globe Shine
        internal static int OpenAnim = 0;        // 0 Slide, 1 Drop, 2 Fade, 3 Cascade
        internal static int WidthIdx = 1;        // 0 Narrow, 1 Normal, 2 Wide
        internal static int PosIdx = 0;          // 0 Left, 1 Right
        internal static int SizeIdx = 1;         // 0 Small, 1 Normal, 2 Large
        internal static int QualityIdx = 1;      // qualita' del globo
        internal static int ItemFontIdx = 0;
        internal static int TitleFontIdx = 4;
        internal static int SubFontIdx = 0;
        internal static bool SlideSel = true;
        internal static bool SelPulse = false;
        internal static bool Nudge = true;
        internal static bool ValArrows = true;
        internal static bool Lines = false;
        internal static bool SubInBanner = false;
        internal static bool Bold = true;
        internal static bool UpperTitle = true;

        internal static UiTheme Theme { get { return Themes[Math.Max(0, Math.Min(Themes.Length - 1, ThemeIdx))]; } }
        internal static float MenuW { get { return WidthIdx == 0 ? 180f : (WidthIdx == 1 ? 210f : 260f); } }
        internal static bool PosRight { get { return PosIdx == 1; } }
        internal static float SizeMul { get { return SizeIdx == 0 ? 0.9f : (SizeIdx == 1 ? 1f : 1.15f); } }
        internal static float GlobeSpacing { get { return QualityIdx == 0 ? 0.055f : (QualityIdx == 1 ? 0.034f : 0.024f); } }

        static readonly string SettingsPath = Path.Combine("scripts", "essentialui.ini");

        static UiStyle()
        {
            Load();
        }

        // ---- salvataggio / caricamento ----
        internal static void Save()
        {
            try
            {
                string[] lines =
                {
                    "Theme=" + ThemeIdx, "BannerAnim=" + BannerAnim, "OpenAnim=" + OpenAnim, "Width=" + WidthIdx,
                    "Position=" + PosIdx, "TextSize=" + SizeIdx, "GlobeQuality=" + QualityIdx,
                    "ItemFont=" + ItemFontIdx, "TitleFont=" + TitleFontIdx, "SubtitleFont=" + SubFontIdx,
                    "SlideSel=" + (SlideSel ? 1 : 0), "SelPulse=" + (SelPulse ? 1 : 0), "Nudge=" + (Nudge ? 1 : 0),
                    "ValArrows=" + (ValArrows ? 1 : 0), "Lines=" + (Lines ? 1 : 0), "SubInBanner=" + (SubInBanner ? 1 : 0),
                    "Bold=" + (Bold ? 1 : 0), "UpperTitle=" + (UpperTitle ? 1 : 0),
                };
                File.WriteAllLines(SettingsPath, lines);
            }
            catch (Exception)
            {
                // se il file non e' scrivibile, le impostazioni valgono solo per questa sessione
            }
        }

        static void Load()
        {
            try
            {
                if (!File.Exists(SettingsPath))
                    return;

                foreach (string line in File.ReadAllLines(SettingsPath))
                {
                    int eq = line.IndexOf('=');
                    if (eq <= 0)
                        continue;

                    string key = line.Substring(0, eq).Trim();
                    int v;
                    if (!int.TryParse(line.Substring(eq + 1).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
                        continue;

                    switch (key)
                    {
                        case "Theme": ThemeIdx = Clamp(v, 0, Themes.Length - 1); break;
                        case "BannerAnim": BannerAnim = Clamp(v, 0, 7); break;
                        case "OpenAnim": OpenAnim = Clamp(v, 0, 3); break;
                        case "Width": WidthIdx = Clamp(v, 0, 2); break;
                        case "Position": PosIdx = Clamp(v, 0, 1); break;
                        case "TextSize": SizeIdx = Clamp(v, 0, 2); break;
                        case "GlobeQuality": QualityIdx = Clamp(v, 0, 2); break;
                        case "ItemFont": ItemFontIdx = Clamp(v, 0, FontIds.Length - 1); break;
                        case "TitleFont": TitleFontIdx = Clamp(v, 0, FontIds.Length - 1); break;
                        case "SubtitleFont": SubFontIdx = Clamp(v, 0, FontIds.Length - 1); break;
                        case "SlideSel": SlideSel = v != 0; break;
                        case "SelPulse": SelPulse = v != 0; break;
                        case "Nudge": Nudge = v != 0; break;
                        case "ValArrows": ValArrows = v != 0; break;
                        case "Lines": Lines = v != 0; break;
                        case "SubInBanner": SubInBanner = v != 0; break;
                        case "Bold": Bold = v != 0; break;
                        case "UpperTitle": UpperTitle = v != 0; break;
                    }
                }
            }
            catch (Exception)
            {
                // file rovinato: restano i valori predefiniti
            }
        }

        static int Clamp(int v, int min, int max)
        {
            return Math.Max(min, Math.Min(max, v));
        }

        // ---- controlli del menu Style (servono ai preset per aggiornare i valori mostrati) ----
        static NativeListItem<string> listPreset, listTheme, listBanner, listOpen, listQuality, listWidth, listPos;
        static NativeListItem<string> listItemFont, listTitleFont, listSubFont, listSize;
        static NativeCheckboxItem togSlide, togPulseSel, togNudge, togValArrows, togLines, togSubBanner, togBold, togUpper;

        // indici font: 0 Chalet Comprime, 1 Chalet London, 2 Pricedown, 3 House Script, 4 Monospace
        static void ApplyPreset(int p)
        {
            if (p == 1)         // Sky: banner azzurro, titolo Pricedown e sottotitolo House Script dentro il banner
                SetLook(7, 0, 0, 2, 3, false, false, true, true);
            else if (p == 2)    // Classic: righe nere, selezione bianca, banner fermo
                SetLook(1, 1, 2, 0, 0, true, false, false, false);
            else if (p == 3)    // Globe: banner blu con globo stampato e fascia di luce
                SetLook(12, 7, 3, 3, 0, false, false, false, false);
            else                // Default
                SetLook(0, 0, 0, 4, 0, true, true, false, false);
            Save();
        }

        static void SetLook(int themeIdx, int banner, int openA, int titleFont, int subFont, bool upper, bool bold,
                            bool lines, bool subBanner)
        {
            ThemeIdx = themeIdx;
            BannerAnim = banner;
            OpenAnim = openA;
            TitleFontIdx = titleFont;
            SubFontIdx = subFont;
            UpperTitle = upper;
            Bold = bold;
            Lines = lines;
            SubInBanner = subBanner;
            ItemFontIdx = 0;
            SizeIdx = 1;
            WidthIdx = 1;

            listTheme.SelectedIndex = themeIdx;
            listBanner.SelectedIndex = banner;
            listOpen.SelectedIndex = openA;
            listTitleFont.SelectedIndex = titleFont;
            listSubFont.SelectedIndex = subFont;
            listItemFont.SelectedIndex = 0;
            listSize.SelectedIndex = 1;
            listWidth.SelectedIndex = 1;
            togUpper.Checked = upper;
            togBold.Checked = bold;
            togLines.Checked = lines;
            togSubBanner.Checked = subBanner;
        }

        // Crea il menu "Style" (con il sottomenu "Fonts") e lo aggiunge al pool. Si collega con DemoMenu.AddSubMenu(...)
        public static NativeMenu CreateMenus(ObjectPool pool)
        {
            string[] themeNames = new string[Themes.Length];
            for (int i = 0; i < Themes.Length; i++)
                themeNames[i] = Themes[i].Name;

            // ---- Fonts ----
            listItemFont = new NativeListItem<string>("Item Font", "Font delle voci", FontNames);
            listTitleFont = new NativeListItem<string>("Title Font", "Font del titolo nel banner", FontNames);
            listSubFont = new NativeListItem<string>("Subtitle Font", "Font del sottotitolo", FontNames);
            listSize = new NativeListItem<string>("Text Size", "Dimensione delle scritte", new string[] { "Small", "Normal", "Large" });
            togBold = new NativeCheckboxItem("Bold Text", "Scritte piu' marcate", Bold);
            togUpper = new NativeCheckboxItem("Uppercase Title", "Titolo tutto maiuscolo", UpperTitle);

            listItemFont.SelectedIndex = ItemFontIdx;
            listTitleFont.SelectedIndex = TitleFontIdx;
            listSubFont.SelectedIndex = SubFontIdx;
            listSize.SelectedIndex = SizeIdx;

            listItemFont.ItemChanged += (s, e) => { ItemFontIdx = e.Index; Save(); };
            listTitleFont.ItemChanged += (s, e) => { TitleFontIdx = e.Index; Save(); };
            listSubFont.ItemChanged += (s, e) => { SubFontIdx = e.Index; Save(); };
            listSize.ItemChanged += (s, e) => { SizeIdx = e.Index; Save(); };
            togBold.CheckboxChanged += (s, e) => { Bold = togBold.Checked; Save(); };
            togUpper.CheckboxChanged += (s, e) => { UpperTitle = togUpper.Checked; Save(); };

            NativeMenu fonts = new NativeMenu("Fonts", "Fonts", "Font, dimensione e grassetto");
            fonts.Add(listItemFont);
            fonts.Add(listTitleFont);
            fonts.Add(listSubFont);
            fonts.Add(listSize);
            fonts.Add(togBold);
            fonts.Add(togUpper);

            // ---- Style ----
            listPreset = new NativeListItem<string>("Preset", "Imposta colori, font e animazioni in un colpo solo",
                new string[] { "Default", "Sky", "Classic", "Globe" });
            listTheme = new NativeListItem<string>("Theme", "Cambia i colori del menu", themeNames);
            listBanner = new NativeListItem<string>("Banner Animation", "Come si anima il banner",
                new string[] { "Pulse", "Static", "Rainbow", "Shimmer", "Scanline", "Heartbeat", "Globe Spin", "Globe Shine" });
            listOpen = new NativeListItem<string>("Open Animation", "Come appare il menu",
                new string[] { "Slide", "Drop", "Fade", "Cascade" });
            listQuality = new NativeListItem<string>("Globe Quality", "Dettaglio del globo (High usa piu' risorse)",
                new string[] { "Low", "Normal", "High" });
            togSlide = new NativeCheckboxItem("Sliding Selection", "La barra scivola da una riga all'altra", SlideSel);
            togPulseSel = new NativeCheckboxItem("Selection Pulse", "La barra selezionata pulsa", SelPulse);
            togNudge = new NativeCheckboxItem("Text Nudge", "La voce selezionata scatta a destra", Nudge);
            togValArrows = new NativeCheckboxItem("Arrows On Selected", "Frecce dei valori solo sulla riga selezionata", ValArrows);
            togLines = new NativeCheckboxItem("Accent Lines", "Linea colorata sotto il banner", Lines);
            togSubBanner = new NativeCheckboxItem("Subtitle In Banner", "Titolo e sottotitolo dentro il banner", SubInBanner);
            listWidth = new NativeListItem<string>("Width", "Larghezza del menu", new string[] { "Narrow", "Normal", "Wide" });
            listPos = new NativeListItem<string>("Position", "Lato dello schermo", new string[] { "Left", "Right" });

            listTheme.SelectedIndex = ThemeIdx;
            listBanner.SelectedIndex = BannerAnim;
            listOpen.SelectedIndex = OpenAnim;
            listQuality.SelectedIndex = QualityIdx;
            listWidth.SelectedIndex = WidthIdx;
            listPos.SelectedIndex = PosIdx;

            listPreset.ItemChanged += (s, e) => ApplyPreset(e.Index);
            listTheme.ItemChanged += (s, e) => { ThemeIdx = e.Index; Save(); };
            listBanner.ItemChanged += (s, e) => { BannerAnim = e.Index; Save(); };
            listOpen.ItemChanged += (s, e) => { OpenAnim = e.Index; Save(); };
            listQuality.ItemChanged += (s, e) => { QualityIdx = e.Index; UiHost.RebuildGlobe(); Save(); };
            togSlide.CheckboxChanged += (s, e) => { SlideSel = togSlide.Checked; Save(); };
            togPulseSel.CheckboxChanged += (s, e) => { SelPulse = togPulseSel.Checked; Save(); };
            togNudge.CheckboxChanged += (s, e) => { Nudge = togNudge.Checked; Save(); };
            togValArrows.CheckboxChanged += (s, e) => { ValArrows = togValArrows.Checked; Save(); };
            togLines.CheckboxChanged += (s, e) => { Lines = togLines.Checked; Save(); };
            togSubBanner.CheckboxChanged += (s, e) => { SubInBanner = togSubBanner.Checked; Save(); };
            listWidth.ItemChanged += (s, e) => { WidthIdx = e.Index; Save(); };
            listPos.ItemChanged += (s, e) => { PosIdx = e.Index; Save(); };

            NativeMenu style = new NativeMenu("Style", "Style", "Temi, animazioni e font");
            style.Add(listPreset);
            style.Add(listTheme);
            style.Add(listBanner);
            style.Add(listOpen);
            style.Add(listQuality);
            style.Add(togSlide);
            style.Add(togPulseSel);
            style.Add(togNudge);
            style.Add(togValArrows);
            style.Add(togLines);
            style.Add(togSubBanner);
            style.Add(listWidth);
            style.Add(listPos);
            style.AddSubMenu(fonts);

            pool.Add(style);
            pool.Add(fonts);
            return style;
        }
    }

    // ------------------------------------------------------------ gestione input + disegno
    public static class UiHost
    {
        // ---- costanti di layout (pixel virtuali 1280x720) ----
        internal const int MaxVisible = 10;
        const float MenuMargin = 40f;
        const float MenuY = 40f;
        const float BannerHNormal = 54f;
        const float BannerHTall = 78f;
        const float SubHNormal = 22f;
        const float RowH = 28f;
        const float FootH = 26f;
        const float TextOffsetY = 3f;    // se il testo e' troppo alto/basso nella riga, cambia questo
        const float BoldOffset = 0.8f;   // grassetto finto: testo disegnato due volte con questo scarto
        static readonly Font ArrowFont = Font.Monospace;

        // ---- stato ----
        static readonly List<NativeMenu> stack = new List<NativeMenu>();
        static bool open;
        static float anim;      // 0 chiuso .. 1 aperto
        static float selY;      // posizione animata della barra di selezione (in righe)
        static int lastProcess;

        static NativeMenu Top { get { return stack.Count == 0 ? null : stack[stack.Count - 1]; } }

        internal static bool IsOpen { get { return open && stack.Count > 0; } }
        internal static bool IsTop(NativeMenu m) { return open && Top == m; }

        internal static void Show(NativeMenu m)
        {
            if (m == null)
                return;
            if (open && Top == m)
                return;

            stack.Clear();
            stack.Add(m);
            m.ClampSelection();
            selY = m.Selected - m.Scroll;
            open = true;
        }

        internal static void Hide(NativeMenu m)
        {
            if (stack.Contains(m))
                open = false;
        }

        internal static void HideAll()
        {
            open = false;
        }

        internal static void OpenSub(NativeMenu m)
        {
            stack.Add(m);
            m.ClampSelection();
            selY = m.Selected - m.Scroll;
        }

        static void Back()
        {
            if (stack.Count > 1)
            {
                stack.RemoveAt(stack.Count - 1);
                NativeMenu m = Top;
                m.ClampSelection();
                selY = m.Selected - m.Scroll;
            }
            else
            {
                open = false;
            }
            Sound("BACK");
        }

        static void Sound(string name)
        {
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, name, "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        // ------------------------------------------------------------ input (tasti letti ogni frame)
        class KeyState
        {
            public bool Down;
            public int Next;
        }

        static readonly Dictionary<Keys, KeyState> keyStates = new Dictionary<Keys, KeyState>();
        static readonly Keys[] navKeys =
        {
            Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter, Keys.Back,
            Keys.NumPad8, Keys.NumPad2, Keys.NumPad4, Keys.NumPad6, Keys.NumPad5, Keys.NumPad0
        };

        static KeyState St(Keys k)
        {
            KeyState s;
            if (!keyStates.TryGetValue(k, out s))
            {
                s = new KeyState();
                keyStates[k] = s;
            }
            return s;
        }

        // Dopo una pausa (per esempio una finestra di testo aperta) i tasti ancora premuti non devono contare come nuovi
        static void SwallowKeys(int now)
        {
            foreach (Keys k in navKeys)
            {
                KeyState s = St(k);
                s.Down = Game.IsKeyPressed(k);
                s.Next = now + 380;
            }
        }

        // true quando il tasto e' stato appena premuto (e, se repeat, anche mentre resta premuto)
        static bool Fire(Keys k, bool repeat, int now)
        {
            bool down = Game.IsKeyPressed(k);
            KeyState s = St(k);

            if (down && !s.Down)
            {
                s.Down = true;
                s.Next = now + 380;
                return true;
            }
            if (down && repeat && now >= s.Next)
            {
                s.Next = now + 70;
                return true;
            }
            if (!down)
                s.Down = false;
            return false;
        }

        static void HandleInput(int now)
        {
            NativeMenu m = Top;
            if (m == null)
                return;

            m.ClampSelection();
            int count = m.Items.Count;

            // le due varianti (frecce e tastierino) si leggono sempre tutte e due, per tenere aggiornato lo stato dei tasti
            bool up = Fire(Keys.Up, true, now) | Fire(Keys.NumPad8, true, now);
            bool down = Fire(Keys.Down, true, now) | Fire(Keys.NumPad2, true, now);
            bool left = Fire(Keys.Left, true, now) | Fire(Keys.NumPad4, true, now);
            bool right = Fire(Keys.Right, true, now) | Fire(Keys.NumPad6, true, now);
            bool enter = Fire(Keys.Enter, false, now) | Fire(Keys.NumPad5, false, now);
            bool back = Fire(Keys.Back, false, now) | Fire(Keys.NumPad0, false, now);

            if (count > 0 && (up || down))
            {
                m.Selected = (m.Selected + (up ? -1 : 1) + count) % count;
                m.ClampSelection();
                Sound("NAV_UP_DOWN");
            }

            if (count > 0 && (left || right))
            {
                NativeItem it = m.Items[m.Selected];
                if (left)
                    it.Left();
                else
                    it.Right();
                if (it.Kind == ItemKind.List || it.Kind == ItemKind.Slider)
                    Sound("NAV_LEFT_RIGHT");
            }

            if (count > 0 && enter)
            {
                m.Items[m.Selected].Activate();
                Sound("SELECT");
            }

            if (back)
                Back();
        }

        // ------------------------------------------------------------ utilita' di disegno
        static Color FromHsv(float h, float s, float v)
        {
            h = h % 360f;
            if (h < 0f)
                h += 360f;

            float c = v * s;
            float x = c * (1f - Math.Abs((h / 60f) % 2f - 1f));
            float m = v - c;
            float r = 0f, g = 0f, b = 0f;

            if (h < 60f) { r = c; g = x; }
            else if (h < 120f) { r = x; g = c; }
            else if (h < 180f) { g = c; b = x; }
            else if (h < 240f) { g = x; b = c; }
            else if (h < 300f) { r = x; b = c; }
            else { r = c; b = x; }

            return Color.FromArgb((int)((r + m) * 255f), (int)((g + m) * 255f), (int)((b + m) * 255f));
        }

        static Color Fade(Color c, float k)
        {
            return Color.FromArgb((int)(c.A * k), c.R, c.G, c.B);
        }

        // Miscela due colori (l'alpha resta quello del primo)
        static Color Mix(Color a, Color b, float t)
        {
            return Color.FromArgb(
                a.A,
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        static float Clamp01(float v)
        {
            return Math.Max(0f, Math.Min(1f, v));
        }

        // Maiuscolo che non rovina i codici colore del gioco (~r~, ~b~, ...)
        static string UpperKeepCodes(string s)
        {
            if (string.IsNullOrEmpty(s) || s.IndexOf('~') < 0)
                return s == null ? "" : s.ToUpper();

            char[] chars = s.ToCharArray();
            bool inCode = false;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '~')
                    inCode = !inCode;
                else if (!inCode)
                    chars[i] = char.ToUpper(chars[i]);
            }
            return new string(chars);
        }

        // Rettangolo in pixel virtuali (1280x720), angolo in alto a sinistra
        static void Rect(float x, float y, float w, float h, Color c)
        {
            if (c.A == 0 || w <= 0f || h <= 0f)
                return;
            Function.Call(Hash.DRAW_RECT,
                (x + w / 2f) / 1280f, (y + h / 2f) / 720f, w / 1280f, h / 720f,
                (int)c.R, (int)c.G, (int)c.B, (int)c.A);
        }

        static void Text(string s, float x, float y, float scale, Color c, Font font, Alignment al)
        {
            if (string.IsNullOrEmpty(s))
                return;
            new TextElement(s, new PointF(x, y), scale, c, font, al).Draw();
            if (UiStyle.Bold)
                new TextElement(s, new PointF(x + BoldOffset, y), scale, c, font, al).Draw();
        }

        // Titolo del banner: con contorno scuro
        static void TitleText(string s, float x, float y, float scale, Color c, Font font)
        {
            if (string.IsNullOrEmpty(s))
                return;
            TextElement t = new TextElement(s, new PointF(x, y), scale, c, font, Alignment.Center);
            t.Outline = true;
            t.Draw();
        }

        // ------------------------------------------------------------ globo "stampato" (Globe Shine)
        static float[] gx, gy;
        static int gCount;
        static bool globeBuilt;

        internal static void RebuildGlobe()
        {
            BuildGlobePoints();
        }

        static void BuildGlobePoints()
        {
            float spacing = UiStyle.GlobeSpacing;
            List<float> lx = new List<float>(), ly = new List<float>();

            float tilt = 20f * (float)Math.PI / 180f;   // inclinazione: si vede un po' di piu' l'emisfero nord
            float ct = (float)Math.Cos(tilt);
            float st = (float)Math.Sin(tilt);
            const float twoPi = 6.2831853f;

            // sfera unitaria -> schermo (x destra, y su, z verso chi guarda)
            Func<Vector3, Vector3> project = p => new Vector3(p.X, p.Y * ct - p.Z * st, p.Y * st + p.Z * ct);

            // lengthFactor: 1 per i cerchi massimi, cos(lat) per i paralleli (hanno raggio minore)
            Action<Func<float, Vector3>, float, bool> addCurve = (f, lengthFactor, limb) =>
            {
                int steps = Math.Max(12, (int)(twoPi * lengthFactor / spacing));
                for (int i = 0; i < steps; i++)
                {
                    Vector3 a = project(f(twoPi * i / steps));
                    if (!limb && a.Z <= 0.03f)
                        continue;   // solo il lato davanti

                    lx.Add(a.X);
                    ly.Add(-a.Y);   // y dello schermo va verso il basso
                }
            };

            // 6 meridiani
            for (int k = 0; k < 6; k++)
            {
                float lon = (float)Math.PI * k / 6f;
                float sl = (float)Math.Sin(lon);
                float cl = (float)Math.Cos(lon);
                addCurve(tt => new Vector3((float)Math.Cos(tt) * sl, (float)Math.Sin(tt), (float)Math.Cos(tt) * cl), 1f, false);
            }

            // 5 paralleli
            float[] lats = { -60f, -30f, 0f, 30f, 60f };
            for (int k = 0; k < lats.Length; k++)
            {
                float phi = lats[k] * (float)Math.PI / 180f;
                float cp = (float)Math.Cos(phi);
                float sp = (float)Math.Sin(phi);
                addCurve(tt => new Vector3(cp * (float)Math.Sin(tt), sp, cp * (float)Math.Cos(tt)), cp, false);
            }

            // contorno della sfera
            addCurve(tt => new Vector3((float)Math.Cos(tt), (float)Math.Sin(tt), 0f), 1f, true);

            gx = lx.ToArray();
            gy = ly.ToArray();
            gCount = gx.Length;
            globeBuilt = true;
        }

        static void DrawGlobeShine(float x, float y, float w, float h, Color bannerColor, float k, float time)
        {
            if (!globeBuilt)
                BuildGlobePoints();

            // il globo e' piu' grande del banner e sporge in alto a destra, come stampato sullo sfondo
            float r = h * 1.05f;
            float cx = x + w * 0.80f;
            float cy = y + h * 0.28f;

            Color baseCol = Mix(Color.FromArgb(255, bannerColor), Color.White, 0.30f);

            // ogni punto e' un quadratino grande quanto la distanza fra i punti: le linee risultano continue
            float dot = Math.Max(1.1f, UiStyle.GlobeSpacing * r * 1.05f);

            // posizione della fascia di luce: parte prima del bordo sinistro e finisce oltre il destro
            float period = 3.4f;
            float bandPos = ((time % period) / period) * 2.0f - 0.5f;

            // riflesso morbido sul fondo del banner (strisce molto trasparenti)
            for (int j = 0; j < 12; j++)
            {
                float bx = x + bandPos * w + (j - 6) * 4f;
                float left = Math.Max(x, bx);
                float right = Math.Min(x + w, bx + 4f);
                float a = 26f * (1f - Math.Abs(j - 5.5f) / 6f);
                Rect(left, y, right - left, h, Fade(Color.FromArgb((int)a, 255, 255, 255), k));
            }

            for (int i = 0; i < gCount; i++)
            {
                float px = cx + gx[i] * r;
                float py = cy + gy[i] * r;
                if (px < x || px > x + w || py < y || py > y + h)
                    continue;   // fuori dal banner

                // distanza dalla fascia (leggermente inclinata) -> 0..1
                float u = (px - x) / w + (py - y) / h * 0.30f;
                float d = Math.Abs(u - bandPos);
                float f = d < 0.2f ? 1f - d / 0.2f : 0f;
                f = f * f;

                Color c = Mix(baseCol, Color.White, f);
                int a2 = (int)(40f + 190f * f);   // fade: le linee si accendono e tornano tenui
                Rect(px - dot / 2f, py - dot / 2f, dot, dot, Fade(Color.FromArgb(a2, c.R, c.G, c.B), k));
            }
        }

        // Globo "a puntini" che ruota (Globe Spin)
        static void DrawGlobeSpin(float cx, float cy, float r, float time, Color dotColor, Color orbitColor, float k)
        {
            const int rings = 8;      // paralleli
            const int perRing = 18;   // punti per parallelo
            float rot = time * 0.9f;  // velocita' di rotazione (radianti/secondo)

            for (int i = 1; i < rings; i++)
            {
                float lat = (float)(-Math.PI / 2.0 + Math.PI * i / rings);
                float cl = (float)Math.Cos(lat);
                float sl = (float)Math.Sin(lat);

                for (int j = 0; j < perRing; j++)
                {
                    float lon = (float)(2.0 * Math.PI * j / perRing) + rot;
                    float px = cl * (float)Math.Sin(lon);
                    float pz = cl * (float)Math.Cos(lon);          // >0: lato davanti

                    float depth = (pz + 1f) * 0.5f;                // 0 dietro .. 1 davanti
                    int a = (int)(25f + 125f * depth);             // tenue per non coprire il titolo
                    float size = 1.1f + 1.2f * depth;
                    Rect(cx + px * r - size / 2f, cy - sl * r - size / 2f, size, size,
                        Fade(Color.FromArgb(a, dotColor.R, dotColor.G, dotColor.B), k));
                }
            }

            // contorno della sfera
            for (int j = 0; j < 44; j++)
            {
                float ang = (float)(2.0 * Math.PI * j / 44);
                Rect(cx + (float)Math.Cos(ang) * r - 0.6f, cy + (float)Math.Sin(ang) * r - 0.6f, 1.2f, 1.2f,
                    Fade(Color.FromArgb(60, dotColor.R, dotColor.G, dotColor.B), k));
            }

            // satellite in orbita inclinata
            float oa = time * 1.7f;
            float ox = cx + (float)Math.Cos(oa) * r * 1.35f;
            float oy = cy + (float)Math.Sin(oa) * r * 0.30f - (float)Math.Cos(oa) * r * 0.22f;
            float osz = ((float)Math.Sin(oa) > 0f) ? 3.2f : 2.0f;   // piu' grande quando passa davanti
            Rect(ox - osz / 2f, oy - osz / 2f, osz, osz, Fade(Color.FromArgb(230, orbitColor.R, orbitColor.G, orbitColor.B), k));
        }

        // ------------------------------------------------------------ ciclo principale (chiamato da ObjectPool.Process)
        internal static void Process()
        {
            float dt = Game.LastFrameTime;
            int now = Environment.TickCount;

            // se lo script e' stato fermo (finestra di testo, caricamenti) ignora i tasti ancora premuti
            if (now - lastProcess > 250)
                SwallowKeys(now);
            lastProcess = now;

            if (open && stack.Count > 0)
                HandleInput(now);

            anim += (open ? 1f : -1f) * dt * 6f;
            anim = Clamp01(anim);
            if (anim <= 0f)
            {
                if (!open)
                    stack.Clear();
                return;
            }

            NativeMenu m = Top;
            if (m == null)
                return;

            // Mentre il menu e' aperto spegni i comandi che userebbero le frecce (telefono ecc.)
            if (open)
            {
                int[] blocked = { 27, 172, 173, 174, 175, 176, 177 };
                for (int i = 0; i < blocked.Length; i++)
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, blocked[i], true);
            }

            Draw(m, dt);
        }

        static void Draw(NativeMenu m, float dt)
        {
            m.ClampSelection();
            int count = m.Items.Count;
            int visible = Math.Min(count, MaxVisible);

            UiTheme theme = UiStyle.Theme;
            float menuW = UiStyle.MenuW;
            float sizeMul = UiStyle.SizeMul;
            int bannerAnim = UiStyle.BannerAnim;
            int openAnim = UiStyle.OpenAnim;
            bool subInBanner = UiStyle.SubInBanner;

            float time = Game.GameTime / 1000f;
            float hue = (time / 6f * 360f) % 360f;

            // font e scale correnti
            Font itemFont = (Font)UiStyle.FontIds[UiStyle.ItemFontIdx];
            Font titleFont = (Font)UiStyle.FontIds[UiStyle.TitleFontIdx];
            Font subFont = (Font)UiStyle.FontIds[UiStyle.SubFontIdx];
            float itemScale = UiStyle.ItemFontScale(UiStyle.FontIds[UiStyle.ItemFontIdx]) * sizeMul;
            float titleScale = UiStyle.TitleFontScale(UiStyle.FontIds[UiStyle.TitleFontIdx]);
            float smallScale = 0.4f * sizeMul;

            // ---- colori del tema (il tema Rainbow li ricalcola ogni frame) ----
            Color bannerA = theme.BannerA;
            Color bannerB = theme.BannerB;
            Color accent = theme.Accent;
            Color sel = theme.Sel;
            if (theme.Rainbow)
            {
                bannerA = FromHsv(hue, 0.9f, 0.35f);
                bannerB = FromHsv(hue + 40f, 0.9f, 0.7f);
                accent = FromHsv(hue + 180f, 0.6f, 1f);
                sel = Color.FromArgb(240, FromHsv(hue, 0.85f, 0.75f));
            }

            float wave = 0.5f + 0.5f * (float)Math.Sin(time * 2.4f);
            if (UiStyle.SelPulse)
                sel = Mix(sel, Color.White, 0.22f * wave);

            // ---- animazione di apertura/chiusura ----
            float inv = 1f - anim;
            float ease = 1f - inv * inv * inv;
            bool posRight = UiStyle.PosRight;
            float x = posRight ? 1280f - MenuMargin - menuW : MenuMargin;
            float y = MenuY;
            if (openAnim == 0)
                x += (posRight ? 1f : -1f) * (1f - ease) * 30f;   // scorre di lato
            else if (openAnim == 1)
                y -= (1f - ease) * 30f;                           // scende dall'alto

            // la barra di selezione insegue la riga scelta
            float target = m.Selected - m.Scroll;
            if (UiStyle.SlideSel)
                selY += (target - selY) * Math.Min(1f, dt * 22f);
            else
                selY = target;

            // ---- altezza banner e striscia sottotitolo ----
            float bannerH = subInBanner ? BannerHTall : BannerHNormal;
            float subH = subInBanner ? 0f : SubHNormal;

            // ---- banner ----
            Color bannerColor;
            if (bannerAnim == 0)
                bannerColor = Mix(bannerA, bannerB, wave);                          // Pulse
            else if (bannerAnim == 2)
                bannerColor = Color.FromArgb(bannerA.A, FromHsv(hue, 0.9f, 0.55f)); // Rainbow
            else if (bannerAnim == 5)
            {
                // Heartbeat: due battiti ravvicinati e poi una pausa
                float b1 = (float)Math.Pow(Math.Max(0.0, Math.Sin(time * 5.0)), 8.0);
                float b2 = 0.6f * (float)Math.Pow(Math.Max(0.0, Math.Sin(time * 5.0 - 1.0)), 8.0);
                bannerColor = Mix(bannerA, bannerB, Clamp01(b1 + b2));
            }
            else
                bannerColor = Mix(bannerA, bannerB, 0.35f);                         // Static, Shimmer, Scanline, Globe
            Rect(x, y, menuW, bannerH, Fade(Color.FromArgb(255, bannerColor), ease));

            if (bannerAnim == 3)
            {
                // Shimmer: una fascia chiara attraversa il banner (16 strisce sottili con trasparenza decrescente)
                float period = 2.4f;
                float pos = ((time % period) / period) * (menuW + 120f) - 60f;
                for (int k = 0; k < 16; k++)
                {
                    float sx = pos + (k - 8) * 3f;
                    float left = Math.Max(0f, sx);
                    float right = Math.Min(menuW, sx + 3f);
                    float a = 70f * (1f - Math.Abs(k - 7.5f) / 8f);
                    Rect(x + left, y, right - left, bannerH, Fade(Color.FromArgb((int)a, 255, 255, 255), ease));
                }
            }
            else if (bannerAnim == 4)
            {
                // Scanline: una linea chiara scende lungo il banner
                float ly = y + ((time % 1.8f) / 1.8f) * (bannerH - 2f);
                Rect(x, ly, menuW, 2f, Fade(Color.FromArgb(110, 255, 255, 255), ease));
            }
            else if (bannerAnim == 7)
            {
                // Globe Shine: globo "stampato" (linee sottili) e una fascia di luce che le accende passando
                DrawGlobeShine(x, y, menuW, bannerH, bannerColor, ease, time);
            }
            else if (bannerAnim == 6)
            {
                // Globe Spin: globo che ruota dietro al titolo, con un satellite in orbita
                DrawGlobeSpin(x + menuW / 2f, y + bannerH / 2f, bannerH / 2f - 4f, time, Mix(accent, Color.White, 0.5f), accent, ease);
            }

            if (UiStyle.Lines)
                Rect(x, y + bannerH - 3f, menuW, 3f, Fade(accent, ease));

            // titolo (e sottotitolo se e' dentro il banner)
            string title = UiStyle.UpperTitle ? UpperKeepCodes(m.Title) : m.Title;
            if (subInBanner)
            {
                TitleText(title, x + menuW / 2f, y + 4f, titleScale, Fade(Color.White, ease), titleFont);
                Text(m.Subtitle, x + menuW / 2f, y + bannerH - 27f, UiStyle.ItemFontScale(UiStyle.FontIds[UiStyle.SubFontIdx]) * 0.9f * sizeMul,
                    Fade(Color.White, ease), subFont, Alignment.Center);
            }
            else
            {
                TitleText(title, x + menuW / 2f, y + bannerH / 2f - 21f * titleScale, titleScale, Fade(Color.White, ease), titleFont);
            }

            // ---- striscia sottotitolo + contatore ----
            float subY = y + bannerH;
            if (!subInBanner)
            {
                Rect(x, subY, menuW, subH, Fade(theme.Panel, ease));
                Text(m.Subtitle, x + 10f, subY + 3f, UiStyle.ItemFontScale(UiStyle.FontIds[UiStyle.SubFontIdx]) * 0.8f * sizeMul,
                    Fade(accent, ease), subFont, Alignment.Left);
                if (count > 0)
                    Text((m.Selected + 1) + " / " + count, x + menuW - 10f, subY + 3f, smallScale, Fade(theme.Text, ease), itemFont, Alignment.Right);
            }

            // ---- righe ----
            float rowsTop = subY + subH;
            bool cascade = (openAnim == 3);

            for (int i = 0; i < visible; i++)
            {
                float rk = cascade ? Clamp01(anim * (visible + 2) - i) : ease;
                Rect(x, rowsTop + i * RowH, menuW, RowH, Fade(theme.Row, rk));
            }

            // barra di selezione che scivola
            if (visible > 0)
            {
                int barRow = Math.Max(0, Math.Min(visible - 1, (int)Math.Round(selY)));
                float barK = cascade ? Clamp01(anim * (visible + 2) - barRow) : ease;
                Rect(x, rowsTop + selY * RowH, menuW, RowH, Fade(sel, barK));
            }

            for (int i = 0; i < visible; i++)
            {
                int idx = m.Scroll + i;
                NativeItem it = m.Items[idx];
                float rowY = rowsTop + i * RowH;
                float rk = cascade ? Clamp01(anim * (visible + 2) - i) : ease;
                if (!it.Enabled)
                    rk *= 0.45f;

                // il testo diventa "da selezionato" quando la barra gli e' sopra
                float dist = Math.Abs(i - selY);
                bool onBar = dist < 0.5f;
                Color fg = Fade(onBar ? theme.SelText : theme.Text, rk);
                float textY = rowY + TextOffsetY;
                float nudge = UiStyle.Nudge ? 5f * (1f - Math.Min(1f, dist)) : 0f;

                Text(it.Title, x + 10f + nudge, textY, itemScale, fg, itemFont, Alignment.Left);

                switch (it.Kind)
                {
                    case ItemKind.Submenu:
                        Text(">>>", x + menuW - 10f, textY, smallScale, fg, ArrowFont, Alignment.Right);
                        break;

                    case ItemKind.List:
                        {
                            string val = it.ValueText ?? "";
                            if (!UiStyle.ValArrows || onBar)
                                val = "< " + val + " >";
                            Text(val, x + menuW - 10f, textY, smallScale, fg, ArrowFont, Alignment.Right);
                            break;
                        }

                    case ItemKind.Checkbox:
                        {
                            float bx = x + menuW - 10f - 16f;
                            float by = rowY + (RowH - 16f) / 2f;
                            Rect(bx, by, 16f, 16f, fg);
                            Rect(bx + 2f, by + 2f, 12f, 12f, Fade(onBar ? sel : theme.Row, rk));
                            if (it.IsOn)
                                Rect(bx + 4f, by + 4f, 8f, 8f, Fade(accent, rk));
                            break;
                        }

                    case ItemKind.Slider:
                        {
                            float bw = 56f, bh = 5f;
                            float bx = x + menuW - 10f - bw;
                            float by = rowY + (RowH - bh) / 2f;
                            Color barCol = it.BarColor.A == 0 ? accent : it.BarColor;
                            Rect(bx, by, bw, bh, Fade(Color.FromArgb(90, fg.R, fg.G, fg.B), rk));
                            Rect(bx, by, bw * it.Fraction, bh, Fade(barCol, rk));
                            break;
                        }

                    default:
                        if (!string.IsNullOrEmpty(it.AltTitle))
                            Text(it.AltTitle, x + menuW - 10f, textY, smallScale, fg, itemFont, Alignment.Right);
                        break;
                }
            }

            // ---- descrizione in basso (con il contatore a destra se il sottotitolo e' nel banner) ----
            if (count > 0)
            {
                NativeItem current = m.Items[m.Selected];
                string desc = current.Description ?? "";
                if (desc.Length > 0 || subInBanner)
                {
                    float footY = rowsTop + visible * RowH + 4f;
                    Rect(x, footY, menuW, 2f, Fade(accent, ease));
                    Rect(x, footY + 2f, menuW, FootH, Fade(theme.Panel, ease));
                    Text(desc, x + 10f, footY + 2f + TextOffsetY - 1f,
                        UiStyle.ItemFontScale(UiStyle.FontIds[UiStyle.ItemFontIdx]) * 0.85f * sizeMul, Fade(theme.Text, ease), itemFont, Alignment.Left);
                    if (subInBanner)
                        Text((m.Selected + 1) + " / " + count, x + menuW - 10f, footY + 2f + TextOffsetY - 1f, smallScale,
                            Fade(accent, ease), itemFont, Alignment.Right);
                }
            }
        }
    }
}
