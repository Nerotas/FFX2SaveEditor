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
    public partial class CalmLands : Window
    {
        public uint OpenAirCredits
        {
            get { return uint.Parse(tbxOpenAirCredits.Text); }
            set { tbxOpenAirCredits.Text = value.ToString(); }
        }
        public uint OpenAirPoints
        {
            get { return uint.Parse(tbxOpenAirPoints.Text); }
            set { tbxOpenAirPoints.Text = value.ToString(); }
        }
        public uint ArgentCredits
        {
            get { return uint.Parse(tbxArgentCredits.Text); }
            set { tbxArgentCredits.Text = value.ToString(); }
        }
        public uint ArgentPoints
        {
            get { return uint.Parse(tbxArgentPoints.Text); }
            set { tbxArgentPoints.Text = value.ToString(); }
        }
        public uint MarraigePoints
        {
            get { return uint.Parse(tbxMarraigePoints.Text); }
            set { tbxMarraigePoints.Text = value.ToString(); }
        }
        public uint CreditsCh5
        {
            get { return uint.Parse(tbxCreditsCh5.Text); }
            set { tbxCreditsCh5.Text = value.ToString(); }
        }
        public uint Credits2Ch5
        {
            get { return uint.Parse(tbxCredits2Ch5.Text); }
            set { tbxCredits2Ch5.Text = value.ToString(); }
        }
        public uint HoverRides
        {
            get { return uint.Parse(tbxHoverRides.Text); }
            set { tbxHoverRides.Text = value.ToString(); }
        }

        public CalmLands()
        {
            InitializeComponent();
        }

        private void textbox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var tbx = (TextBox)sender;
            if (string.IsNullOrEmpty(tbx.Text)) return;

            var qty = int.Parse(tbx.Text);
            if (e.Delta < 0 && qty > 0)
                qty-=1000;
            else if (e.Delta > 0 && qty < 1000000000)
                qty+=1000;

            if (qty > 999999999)
                qty = 999999999;

            // Clamp real caps for specific fields
            if (tbx == tbxOpenAirPoints || tbx == tbxArgentPoints)
            {
                if (qty < 0) qty = 0;
                if (qty > 400) qty = 400; // researched max NPC-only publicity points
            }
            else if (tbx == tbxMarraigePoints)
            {
                if (qty < 0) qty = 0;
                if (qty > 255) qty = 255; // matchmaking is a byte-backed counter
            }

            tbx.Text = qty.ToString();
            // Keep leader status in sync when scrolling on points
            if (tbx == tbxOpenAirPoints || tbx == tbxArgentPoints)
            {
                UpdateLeaderStatus();
                UpdatePublicityLevels();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize leader label
            UpdateLeaderStatus();
            UpdatePublicityLevels();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            bool oversized = false;

            if (!string.IsNullOrWhiteSpace(e.Text) && !string.IsNullOrWhiteSpace(textBox.Text))
                oversized = textBox.Text.Length + e.Text.Length - textBox.SelectionLength > 9;

            e.Handled = oversized || Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private uint ParseUint(TextBox tbx)
        {
            if (tbx == null) return 0;
            if (uint.TryParse(tbx.Text, out var val)) return val;
            return 0;
        }

        private void SetUint(TextBox tbx, uint value)
        {
            tbx.Text = value.ToString();
        }

        private void UpdateLeaderStatus()
        {
            if (txtLeaderStatus == null) return;
            var openAir = ParseUint(tbxOpenAirPoints);
            var argent = ParseUint(tbxArgentPoints);
            if (openAir == argent)
            {
                txtLeaderStatus.Text = "Leader: Tie";
            }
            else if (openAir > argent)
            {
                txtLeaderStatus.Text = $"Leader: Open Air (+{openAir - argent})";
            }
            else
            {
                txtLeaderStatus.Text = $"Leader: Argent (+{argent - openAir})";
            }
        }

        private void Points_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Clamp to researched max for publicity fields
            var oa = ParseUint(tbxOpenAirPoints);
            var ar = ParseUint(tbxArgentPoints);
            if (oa > 400) tbxOpenAirPoints.Text = "400";
            if (ar > 400) tbxArgentPoints.Text = "400";
            UpdateLeaderStatus();
            UpdatePublicityLevels();
        }

        private void Marriage_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Clamp marriage points to real max (255)
            var val = ParseUint(tbxMarraigePoints);
            if (val > 255)
            {
                tbxMarraigePoints.Text = "255";
            }
        }

        private int ComputeLevel(uint points)
        {
            // Publicity Level mapping (0..5):
            // 0-99 => 0, 100-199 => 1, 200-299 => 2, 300-399 => 3, 400+ => 5 (MAX)
            if (points >= 400) return 5;
            var level = (int)(points / 100);
            if (level < 0) level = 0;
            if (level > 5) level = 5;
            return level;
        }

        private void UpdatePublicityLevels()
        {
            if (txtOpenAirLevel == null || txtArgentLevel == null) return;
            var openAir = ParseUint(tbxOpenAirPoints);
            var argent = ParseUint(tbxArgentPoints);
            var oaLevel = ComputeLevel(openAir);
            var arLevel = ComputeLevel(argent);
            var oaNext = oaLevel >= 5 ? (uint)400 : (uint)((oaLevel + 1) * 100);
            var arNext = arLevel >= 5 ? (uint)400 : (uint)((arLevel + 1) * 100);
            txtOpenAirLevel.Text = oaLevel >= 5 ? "Open Air Level: 5 (MAX)" : $"Open Air Level: {oaLevel} (next @ {oaNext})";
            txtArgentLevel.Text = arLevel >= 5 ? "Argent Level: 5 (MAX)" : $"Argent Level: {arLevel} (next @ {arNext})";
        }

        private void btnOpenAirWins_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, 400);
            SetUint(tbxArgentPoints, 0);
            UpdateLeaderStatus();
            UpdatePublicityLevels();
        }

        private void btnArgentWins_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, 0);
            SetUint(tbxArgentPoints, 400);
            UpdateLeaderStatus();
            UpdatePublicityLevels();
        }

        private void btnTie_Click(object sender, RoutedEventArgs e)
        {
            var openAir = ParseUint(tbxOpenAirPoints);
            var argent = ParseUint(tbxArgentPoints);
            var target = Math.Min(400u, Math.Max(openAir, argent));
            SetUint(tbxOpenAirPoints, target);
            SetUint(tbxArgentPoints, target);
            UpdateLeaderStatus();
            UpdatePublicityLevels();
        }

        private void btnMaxBoth_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, 400);
            SetUint(tbxArgentPoints, 400);
            UpdateLeaderStatus();
            UpdatePublicityLevels();
        }

        private void btnZeroBoth_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, 0);
            SetUint(tbxArgentPoints, 0);
            UpdateLeaderStatus();
        }

        private void btnMarriageMax_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxMarraigePoints, 255);
        }

        private void btnMarriageZero_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxMarraigePoints, 0);
        }
    }
}
