using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FFX2SaveEditor
{
    public partial class MultiBlueBullets : Window
    {
        private readonly Dictionary<PartyMember, List<Ability>> _perCharAbilities;

        public MultiBlueBullets(Dictionary<PartyMember, List<Ability>> perCharAbilities)
        {
            InitializeComponent();
            _perCharAbilities = perCharAbilities;

            BuildPanel(PartyMember.Yuna, YunaPanel);
            BuildPanel(PartyMember.Rikku, RikkuPanel);
            BuildPanel(PartyMember.Paine, PainePanel);
        }

        private void BuildPanel(PartyMember member, StackPanel panel)
        {
            panel.Children.Clear();
            if (!_perCharAbilities.TryGetValue(member, out var list) || list == null)
                return;

            foreach (var ability in list)
            {
                var cb = new CheckBox
                {
                    Content = ability.Name,
                    IsChecked = ability.Mastered,
                    Tag = ability,
                    Margin = new Thickness(4)
                };
                panel.Children.Add(cb);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Traverse each panel and update AP to mastered/unmastered
            ApplyPanel(YunaPanel);
            ApplyPanel(RikkuPanel);
            ApplyPanel(PainePanel);
            DialogResult = true;
        }

        private void MasterAll_Click(object sender, RoutedEventArgs e)
        {
            SetAllPanels(true);
        }

        private void UnmasterAll_Click(object sender, RoutedEventArgs e)
        {
            SetAllPanels(false);
        }

        private void SetAllPanels(bool mastered)
        {
            SetPanel(YunaPanel, mastered);
            SetPanel(RikkuPanel, mastered);
            SetPanel(PainePanel, mastered);
        }

        private static void SetPanel(Panel panel, bool mastered)
        {
            foreach (var child in panel.Children)
            {
                if (child is CheckBox cb && cb.Tag is Ability ability)
                {
                    cb.IsChecked = mastered;
                    ability.Ap = mastered ? ability.MaxAp : (ushort)0;
                }
            }
        }

        private static void ApplyPanel(Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is CheckBox cb && cb.Tag is Ability ability)
                {
                    ability.Ap = cb.IsChecked == true ? ability.MaxAp : (ushort)0;
                }
            }
        }
    }
}
