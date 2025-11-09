using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FFX2SaveEditor
{
    /// <summary>
    /// Interaction logic for CalmLands.xaml
    /// </summary>
    public partial class MiniGame : Window
    {
        public MiniGame()
        {
            InitializeComponent();
        }

        public MiniGameMode Mode { get; set; }

        // Calm Lands
        public uint OpenAirCredits { get { return ParseUInt(tbxOpenAirCredits.Text); } set { tbxOpenAirCredits.Text = value.ToString(); } }
        public uint OpenAirPoints { get { return ParseUInt(tbxOpenAirPoints.Text); } set { tbxOpenAirPoints.Text = value.ToString(); } }
        public uint ArgentCredits { get { return ParseUInt(tbxArgentCredits.Text); } set { tbxArgentCredits.Text = value.ToString(); } }
        public uint ArgentPoints { get { return ParseUInt(tbxArgentPoints.Text); } set { tbxArgentPoints.Text = value.ToString(); } }
        public uint MarraigePoints { get { return ParseUInt(tbxMarraigePoints.Text); } set { tbxMarraigePoints.Text = value.ToString(); } }
        public uint CreditsCh5 { get { return ParseUInt(tbxCreditsCh5.Text); } set { tbxCreditsCh5.Text = value.ToString(); } }
        public uint Credits2Ch5 { get { return ParseUInt(tbxCredits2Ch5.Text); } set { tbxCredits2Ch5.Text = value.ToString(); } }
        public uint HoverRides { get { return ParseUInt(tbxHoverRides.Text); } set { tbxHoverRides.Text = value.ToString(); } }

        // Thunder Plains
    // Thunder Plains tower calibration uses a score (0-30). Treat checked as a full calibration score of 30
    public byte Tower1Calibrated { get { return (bool)(chkT1.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT1.IsChecked = value > 25; } }
    public byte Tower2Calibrated { get { return (bool)(chkT2.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT2.IsChecked = value > 25; } }
    public byte Tower3Calibrated { get { return (bool)(chkT3.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT3.IsChecked = value > 25; } }
    public byte Tower4Calibrated { get { return (bool)(chkT4.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT4.IsChecked = value > 25; } }
    public byte Tower5Calibrated { get { return (bool)(chkT5.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT5.IsChecked = value > 25; } }
    public byte Tower6Calibrated { get { return (bool)(chkT6.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT6.IsChecked = value > 25; } }
    public byte Tower7Calibrated { get { return (bool)(chkT7.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT7.IsChecked = value > 25; } }
    public byte Tower8Calibrated { get { return (bool)(chkT8.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT8.IsChecked = value > 25; } }
    public byte Tower9Calibrated { get { return (bool)(chkT9.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT9.IsChecked = value > 25; } }
    public byte Tower10Calibrated { get { return (bool)(chkT10.IsChecked ?? false) ? (byte)30 : (byte)0; } set { chkT10.IsChecked = value > 25; } }
        public byte Tower1Attempts { get { return ParseByte(tbxT1A.Text); } set { tbxT1A.Text = value.ToString(); } }
        public byte Tower2Attempts { get { return ParseByte(tbxT2A.Text); } set { tbxT2A.Text = value.ToString(); } }
        public byte Tower3Attempts { get { return ParseByte(tbxT3A.Text); } set { tbxT3A.Text = value.ToString(); } }
        public byte Tower4Attempts { get { return ParseByte(tbxT4A.Text); } set { tbxT4A.Text = value.ToString(); } }
        public byte Tower5Attempts { get { return ParseByte(tbxT5A.Text); } set { tbxT5A.Text = value.ToString(); } }
        public byte Tower6Attempts { get { return ParseByte(tbxT6A.Text); } set { tbxT6A.Text = value.ToString(); } }
        public byte Tower7Attempts { get { return ParseByte(tbxT7A.Text); } set { tbxT7A.Text = value.ToString(); } }
        public byte Tower8Attempts { get { return ParseByte(tbxT8A.Text); } set { tbxT8A.Text = value.ToString(); } }
        public byte Tower9Attempts { get { return ParseByte(tbxT9A.Text); } set { tbxT9A.Text = value.ToString(); } }
        public byte Tower10Attempts { get { return ParseByte(tbxT10A.Text); } set { tbxT10A.Text = value.ToString(); } }

        // Bikanel
        public uint SuccessfulDigs { get { return ParseUInt(tbxSuccessfulDigs.Text); } set { tbxSuccessfulDigs.Text = value.ToString(); } }
        public uint FailedDigs { get { return ParseUInt(tbxFailedDigs.Text); } set { tbxFailedDigs.Text = value.ToString(); } }
    // Besaid
    public uint GunnerPoints { get { return ParseUInt(tbxGunnerPoints.Text); } set { tbxGunnerPoints.Text = value.ToString(); UpdateGunnerTier(); } }
        // Chocobo
        public byte[] ChocoboSuccesses
        {
            get { return new byte[] { ParseByte(tbxChoco1.Text), ParseByte(tbxChoco2.Text), ParseByte(tbxChoco3.Text), ParseByte(tbxChoco4.Text), ParseByte(tbxChoco5.Text) }; }
            set { if (value?.Length ==5){ tbxChoco1.Text = value[0].ToString(); tbxChoco2.Text = value[1].ToString(); tbxChoco3.Text = value[2].ToString(); tbxChoco4.Text = value[3].ToString(); tbxChoco5.Text = value[4].ToString(); } }
        }
        public uint PahsanaGreens { get { return ParseUInt(tbxPahsana.Text); } set { tbxPahsana.Text = value.ToString(); } }
        public uint MimettGreens { get { return ParseUInt(tbxMimett.Text); } set { tbxMimett.Text = value.ToString(); } }
        public uint SylkisGreens { get { return ParseUInt(tbxSylkis.Text); } set { tbxSylkis.Text = value.ToString(); } }
        public uint GysahlGreens { get { return ParseUInt(tbxGysahl.Text); } set { tbxGysahl.Text = value.ToString(); } }
        // Gagazet
        public uint KimahriSelfEsteemCh2 { get { return ParseUInt(tbxKimCh2.Text); } set { tbxKimCh2.Text = value.ToString(); } }
        public uint KimahriSelfEsteem { get { return ParseUInt(tbxKim.Text); } set { tbxKim.Text = value.ToString(); } }
        // Misc
        public byte Faction { get { return ParseByte(tbxFaction.Text); } set { tbxFaction.Text = value.ToString(); } }
        public uint Encounters { get { return ParseUInt(tbxEncounters.Text); } set { tbxEncounters.Text = value.ToString(); } }
        public float OakaDebt { get { return ParseFloat(tbxOakaDebt.Text); } set { tbxOakaDebt.Text = value.ToString("n2"); } }

        private uint ParseUInt(string s){ uint.TryParse(s, out var v); return v; }
        private byte ParseByte(string s){ byte.TryParse(s, out var v); return v; }
        private float ParseFloat(string s){ float.TryParse(s, out var v); return v; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool all = Mode == MiniGameMode.All;
            grpCalm.Visibility = (Mode == MiniGameMode.CalmLands || all) ? Visibility.Visible : Visibility.Collapsed;
            grpThunder.Visibility = (Mode == MiniGameMode.ThunderPlains || all) ? Visibility.Visible : Visibility.Collapsed;
            grpBikanel.Visibility = (Mode == MiniGameMode.Bikanel || all) ? Visibility.Visible : Visibility.Collapsed;
            grpBesaid.Visibility = (Mode == MiniGameMode.Besaid || all) ? Visibility.Visible : Visibility.Collapsed;
            grpChocobo.Visibility = (Mode == MiniGameMode.Chocobo || all) ? Visibility.Visible : Visibility.Collapsed;
            grpGagazet.Visibility = (Mode == MiniGameMode.Gagazet || all) ? Visibility.Visible : Visibility.Collapsed;
            grpMisc.Visibility = (Mode == MiniGameMode.Misc || all) ? Visibility.Visible : Visibility.Collapsed;

            // Initialize computed publicity levels if Calm Lands is visible
            if (grpCalm.Visibility == Visibility.Visible)
                UpdateCalmPublicityLevels();

            // Initialize Gunner tier if Besaid is visible
            if (grpBesaid.Visibility == Visibility.Visible)
                UpdateGunnerTier();
        }

        private void textbox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var tbx = (TextBox)sender;
            if (string.IsNullOrEmpty(tbx.Text)) return;
            var qty = long.Parse(tbx.Text);
            if (e.Delta <0 && qty >0) qty -=1000; else if (e.Delta >0 && qty <1000000000) qty +=1000;
            if (qty <0) qty =0; if (qty >999999999) qty =999999999;
            tbx.Text = qty.ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e){ DialogResult = false; }
        private void btnOK_Click(object sender, RoutedEventArgs e){ DialogResult = true; }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            bool oversized = false;

            if (!string.IsNullOrWhiteSpace(e.Text) && !string.IsNullOrWhiteSpace(textBox.Text))
                oversized = textBox.Text.Length + e.Text.Length - textBox.SelectionLength >9;

            e.Handled = oversized || Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void PreviewFloatHandler(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9.]") || (e.Text == "." && ((TextBox)sender).Text.Contains("."));
        }

        private int ComputeLevel(uint points)
        {
            // Publicity Level mapping (0..5) consistent with CalmLands:
            // 0-99 => 0, 100-199 => 1, 200-299 => 2, 300-399 => 3, 400+ => 5
            if (points >= 400) return 5;
            var level = (int)(points / 100);
            if (level < 0) level = 0;
            if (level > 5) level = 5;
            return level;
        }

        private void UpdateCalmPublicityLevels()
        {
            try
            {
                if (txtOpenAirLevel2 == null || txtArgentLevel2 == null) return;
                uint oa = ParseUInt(tbxOpenAirPoints.Text);
                uint ar = ParseUInt(tbxArgentPoints.Text);
                // Clamp to researched publicity cap (400)
                if (oa > 400) { tbxOpenAirPoints.Text = "400"; oa = 400; }
                if (ar > 400) { tbxArgentPoints.Text = "400"; ar = 400; }
                var oaLevel = ComputeLevel(oa);
                var arLevel = ComputeLevel(ar);
                var oaNext = oaLevel >= 5 ? (uint)400 : (uint)((oaLevel + 1) * 100);
                var arNext = arLevel >= 5 ? (uint)400 : (uint)((arLevel + 1) * 100);
                txtOpenAirLevel2.Text = oaLevel >= 5 ? "Level: 5 (MAX)" : $"Level: {oaLevel} (next @ {oaNext})";
                txtArgentLevel2.Text = arLevel >= 5 ? "Level: 5 (MAX)" : $"Level: {arLevel} (next @ {arNext})";
            }
            catch { /* ignore parse errors */ }
        }

        private void CalmPoints_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCalmPublicityLevels();
        }

        private void Marriage_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Clamp marriage/matchmaking points to byte max (255)
            try
            {
                uint m = ParseUInt(tbxMarraigePoints.Text);
                if (m > 255) tbxMarraigePoints.Text = "255";
            }
            catch { }
        }

        // Gunner's Gauntlet thresholds and UI updater
        private static readonly int[] GunnerThresholds = new[] { 500, 750, 900, 1000, 1150, 1300, 1400, 2000, 2800 };
        private static readonly string[] GunnerRewards = new[] {
            "Enigma Plate (GG)",
            "Power Wrist",
            "Silver Bracer",
            "Titanium Bangle",
            "Mortal Coil (GG)",
            "Beaded Brooch",
            "Diamond Gloves",
            "Faerie Earrings",
            "Adamantite"
        };

        private void GunnerPoints_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGunnerTier();
        }

        private void UpdateGunnerTier()
        {
            try
            {
                if (txtGunnerTier == null) return;
                var score = ParseUInt(tbxGunnerPoints.Text);
                int tier = 0;
                while (tier < GunnerThresholds.Length && score >= GunnerThresholds[tier]) tier++;

                if (tier >= GunnerThresholds.Length)
                {
                    txtGunnerTier.Text = "Reward Tier: MAX (all rewards reached)";
                }
                else if (tier == 0)
                {
                    txtGunnerTier.Text = $"Reward Tier: —  •  Next at: {GunnerThresholds[0]} ({GunnerRewards[0]})";
                }
                else
                {
                    var lastIdx = tier - 1;
                    var nextAt = GunnerThresholds[tier];
                    txtGunnerTier.Text = $"Reward Tier: Lv{tier}  •  Last: {GunnerRewards[lastIdx]}  •  Next at: {nextAt} ({GunnerRewards[tier]})";
                }
            }
            catch { }
        }
    }

    public enum MiniGameMode { CalmLands, ThunderPlains, Bikanel, Besaid, Chocobo, Gagazet, Misc, All }
}
