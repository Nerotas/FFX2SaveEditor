using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FFX2SaveEditor
{
 public partial class BlueBullets : Window
 {
 private readonly List<Ability> _abilities;
 public BlueBullets(List<Ability> abilities)
 {
 InitializeComponent();
 _abilities = abilities;

 foreach (var ability in _abilities)
 {
 var cb = new CheckBox
 {
 Content = ability.Name,
 IsChecked = ability.Mastered,
 Tag = ability,
 Margin = new Thickness(4)
 };
 SkillsPanel.Children.Add(cb);
 }
 }

 private void Ok_Click(object sender, RoutedEventArgs e)
 {
 foreach (var child in SkillsPanel.Children)
 {
 if (child is CheckBox cb && cb.Tag is Ability ability)
 {
 ability.Ap = cb.IsChecked == true ? ability.MaxAp : (ushort)0;
 }
 }
 DialogResult = true;
 }

 private void MasterAll_Click(object sender, RoutedEventArgs e)
 {
  foreach (var child in SkillsPanel.Children)
  {
   if (child is CheckBox cb && cb.Tag is Ability ability)
   {
	cb.IsChecked = true;
	ability.Ap = ability.MaxAp;
   }
  }
 }
 }
}
