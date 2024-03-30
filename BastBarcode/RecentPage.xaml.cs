using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class RecentPage : Page
    {
        public RecentPage()
        {
            this.InitializeComponent();
            loader();
        }

        private async void loader()
        {
            await ProductDtls.Load();
            List_Main.ItemsSource = ProductDtls.Store;
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            ProductDtls dtl = (btn.DataContext as ProductDtls);
            ProductDtls.Instance = dtl;
            ProductDtls.InstaceChanged = true;
            MainApp.tabView.SelectedIndex = 0;
        }

        private async void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            ProductDtls dtl = (btn.DataContext as ProductDtls);
            ProductDtls.Store.Remove(dtl);
            await ProductDtls.Save();
        }

        private async void Btn_AboutUs(object sender, RoutedEventArgs e)
        {
            ContentDialog c = new AboutUs();
            await c.ShowAsync();
        }
    }
}
