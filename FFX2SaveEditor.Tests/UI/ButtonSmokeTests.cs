using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Controls;

namespace FFX2SaveEditor.Tests.UI
{
    [TestClass]
    public class ButtonSmokeTests
    {
        private void RunInSta(Action action)
        {
            Exception ex = null;
            var t = new Thread(() =>
            {
                try { action(); }
                catch (Exception e) { ex = e; }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            if (ex != null) throw ex;
        }

        [TestMethod]
        public void AllMainButtons_Click_DoesNotThrow()
        {
            RunInSta(() =>
            {
                // Ensure an Application and required resources exist for XAML StaticResource lookups
                if (Application.Current == null)
                {
                    new Application();
                }
                if (Application.Current.Resources["Ffx2Button"] == null)
                {
                    Application.Current.Resources["Ffx2Button"] = new Style(typeof(Button));
                }

                var window = new FFX2SaveEditor.MainWindow();
                // Don't show the window; we only need to invoke the handlers
                string[] buttonNames = new[]
                {
                    "btnLoad","btnSave","btnConvertToPC",
                    "btnItems","btnStoryCompletion","btnEquip","btnGarmentGrid",
                    "btnAccessories","btnAbilities","btnDresspheres",
                    "btnMiniGames","btnSidequests","btnConfig"
                };

                foreach (var name in buttonNames)
                {
                    var obj = window.FindName(name) as Button;
                    Assert.IsNotNull(obj, $"Button '{name}' not found in MainWindow.");
                    obj.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            });
        }
    }
}
