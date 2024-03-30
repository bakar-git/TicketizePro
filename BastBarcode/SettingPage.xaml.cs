using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BastBarcode
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        ApplicationDataContainer localSettings;
        public SettingPage()
        {
            this.InitializeComponent();
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Loaded += SettingPage_Loaded;
            switch (localSettings.Values["Theme"] as string)
            {
                case "light":
                    RadioBtn_light.IsChecked = true;
                    break;
                case "dark":
                    RadioBtn_dark.IsChecked = true;
                    break;
                case "system":
                    RadioBtn_system.IsChecked = true;
                    break;
            }
            RadioBtn_light.Checked += RadioBtn_theme_Checked;
            RadioBtn_dark.Checked += RadioBtn_theme_Checked;
            RadioBtn_system.Checked += RadioBtn_theme_Checked;
        }

        private void SettingPage_Loaded(object sender, RoutedEventArgs e)
        {
            if ((localSettings.Values["OneBtnForSaveAndPrint"] as string) == "true") RadioBtn_OneBtn.IsChecked = true;
            else RadioBtn_TwoBtn.IsChecked = true;
            if ((localSettings.Values["ShowConfirmButton"] as string) == "true") ChkBx_ConfrmBtn.IsChecked = true;
            else ChkBx_ConfrmBtn.IsChecked = false;
        }

        private void RadioButton_OneBtn_Checked(object sender, RoutedEventArgs e)
        {
            localSettings.Values["OneBtnForSaveAndPrint"] = "true";
        }
        private void RadioButton_TwoBtn_Checked(object sender, RoutedEventArgs e)
        {
            localSettings.Values["OneBtnForSaveAndPrint"] = "false";
        }

        private void ChkBx_ConfrmBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ChkBx_ConfrmBtn.IsChecked) localSettings.Values["ShowConfirmButton"] = "true";
            else localSettings.Values["ShowConfirmButton"] = "false";
        }
        private async void Btn_AboutUs(object sender, RoutedEventArgs e)
        {
            ContentDialog c = new AboutUs();
            await c.ShowAsync();
        }

        private async void RadioBtn_theme_Checked(object sender, RoutedEventArgs e)
        {
            if((bool)RadioBtn_light.IsChecked) localSettings.Values["Theme"] = "light";
            else if((bool)RadioBtn_dark.IsChecked) localSettings.Values["Theme"] = "dark";
            else if((bool)RadioBtn_system.IsChecked) localSettings.Values["Theme"] = "system";
            await CoreApplication.RequestRestartAsync("");
        }
    }
}
