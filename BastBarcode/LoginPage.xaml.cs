using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BastBarcode
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string Name = TxtBx_UserName.Text;
            string Password = PwdBx_UserPwd.Password;
            if (Name == "admin" && Password == "admin") this.Frame.Navigate(typeof(MainApp), null);
            else InfoBar_Error.IsOpen = true;
        }

        private async void Btn_AboutUs(object sender, RoutedEventArgs e)
        {
            ContentDialog c = new AboutUs();
            await c.ShowAsync();
        }
    }
}