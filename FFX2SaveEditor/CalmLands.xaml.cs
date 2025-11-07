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

            tbx.Text = qty.ToString();
            // Keep leader status in sync when scrolling on points
            if (tbx == tbxOpenAirPoints || tbx == tbxArgentPoints)
                UpdateLeaderStatus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize leader label
            UpdateLeaderStatus();
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
            UpdateLeaderStatus();
        }

        private void btnOpenAirWins_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, uint.MaxValue);
            SetUint(tbxArgentPoints, 0);
            UpdateLeaderStatus();
        }

        private void btnArgentWins_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, 0);
            SetUint(tbxArgentPoints, uint.MaxValue);
            UpdateLeaderStatus();
        }

        private void btnTie_Click(object sender, RoutedEventArgs e)
        {
            var openAir = ParseUint(tbxOpenAirPoints);
            var argent = ParseUint(tbxArgentPoints);
            var target = Math.Max(openAir, argent);
            SetUint(tbxOpenAirPoints, target);
            SetUint(tbxArgentPoints, target);
            UpdateLeaderStatus();
        }

        private void btnMaxBoth_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, uint.MaxValue);
            SetUint(tbxArgentPoints, uint.MaxValue);
            UpdateLeaderStatus();
        }

        private void btnZeroBoth_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxOpenAirPoints, 0);
            SetUint(tbxArgentPoints, 0);
            UpdateLeaderStatus();
        }

        private void btnMarriageMax_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxMarraigePoints, uint.MaxValue);
        }

        private void btnMarriageZero_Click(object sender, RoutedEventArgs e)
        {
            SetUint(tbxMarraigePoints, 0);
        }
    }
}
