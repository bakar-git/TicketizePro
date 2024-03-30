using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BastBarcode
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrintingPage : Page
    {
        public ProductDtls Dtls = new ProductDtls();
        ApplicationDataContainer localSettings;
        public PrintingPage()
        {
            this.InitializeComponent();
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Loaded += PrintingPage_Loaded;
            
        }

        private void PrintingPage_Loaded(object sender, RoutedEventArgs e)
        {
            if ((localSettings.Values["OneBtnForSaveAndPrint"] as string) == "true")
            {
                Grid_TwoBtn1.Visibility = Visibility.Collapsed;
                Grid_TwoBtn2.Visibility = Visibility.Collapsed;
                Btn_Print1.Visibility = Visibility.Visible;
                Btn_Print2.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_TwoBtn1.Visibility = Visibility.Visible;
                Grid_TwoBtn2.Visibility = Visibility.Visible;
                Btn_Print1.Visibility = Visibility.Collapsed;
                Btn_Print2.Visibility = Visibility.Collapsed;
            }
            if ((localSettings.Values["ShowConfirmButton"] as string) == "true") Btn_Next.Visibility = Visibility.Visible;
            else Btn_Next.Visibility = Visibility.Collapsed;
            if (ProductDtls.InstaceChanged)
            {
                TxtBx_BusinessName.Text = ProductDtls.Instance.BusinessName;
                TxtBx_PName.Text = ProductDtls.Instance.ProductName;
                TxtBx_PColor.Text = ProductDtls.Instance.ProductColor;
                TxtBx_PNewPrice.Text = ProductDtls.Instance.ProductNewPrice;
                TxtBx_POldPrice.Text = ProductDtls.Instance.ProductOldPrice;
                TxtBx_PSize.Text = ProductDtls.Instance.ProductSize;
                ProductDtls.InstaceChanged = false;
            }
        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            Bdr_Form.Visibility = Visibility.Collapsed;
            Bdr_FormConfirm.Visibility = Visibility.Visible;
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Bdr_Form.Visibility = Visibility.Visible;
            Bdr_FormConfirm.Visibility = Visibility.Collapsed;
        }

        private async void Btn_PrintSave_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            if(string.IsNullOrEmpty(TxtBx_PName.Text) || string.IsNullOrEmpty(TxtBx_PColor.Text) || string.IsNullOrEmpty(TxtBx_BusinessName.Text) || string.IsNullOrEmpty(TxtBx_PSize.Text) || string.IsNullOrEmpty(TxtBx_PNewPrice.Text))
            {
                MessageDialog x = new MessageDialog("Please fill necessary fields");
                x.Title = "Error";
                await x.ShowAsync();
            }
            else
            {
                await Print();
                await Save();
            }
            btn.IsEnabled=true;
        }

        private async void Btn_Print_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            if (string.IsNullOrEmpty(TxtBx_PName.Text) || string.IsNullOrEmpty(TxtBx_PColor.Text) || string.IsNullOrEmpty(TxtBx_BusinessName.Text) || string.IsNullOrEmpty(TxtBx_PSize.Text) || string.IsNullOrEmpty(TxtBx_PNewPrice.Text))
            {
                MessageDialog x = new MessageDialog("Please fill necessary fields");
                x.Title = "Error";
                await x.ShowAsync();
            }
            else
            {
                await Print();
            }
            btn.IsEnabled = true;
        }
        private async void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            if (string.IsNullOrEmpty(TxtBx_PName.Text) || string.IsNullOrEmpty(TxtBx_PColor.Text) || string.IsNullOrEmpty(TxtBx_BusinessName.Text) || string.IsNullOrEmpty(TxtBx_PSize.Text) || string.IsNullOrEmpty(TxtBx_PNewPrice.Text))
            {
                MessageDialog x = new MessageDialog("Please fill necessary fields");
                x.Title = "Error";
                await x.ShowAsync();
            }
            else
            {
                await Save();
            }
            btn.IsEnabled = true;
        }

        private async Task Print()
        {
            Windows.Storage.StorageFolder tmpFldr = Windows.Storage.ApplicationData.Current.TemporaryFolder;
            Windows.Storage.StorageFile prntFle = await tmpFldr.CreateFileAsync("print.html", Windows.Storage.CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(prntFle, await PrintTemplate.Make(Dtls));
            await Windows.System.Launcher.LaunchFileAsync(prntFle);
        }

        private async Task Save()
        {
            foreach (var item in Dtls.ProductSizeList)
            {
                ProductDtls tostore = new ProductDtls() { 
                    BusinessName = Dtls.BusinessName,
                    ProductName = Dtls.ProductName,
                    ProductColor = Dtls.ProductColor,
                    ProductSize = item,
                    ProductNewPrice = Dtls.ProductNewPrice,
                    ProductOldPrice = Dtls.ProductOldPrice,
                    PrintingTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                };
                ProductDtls.Store.Add(tostore);
            }
            await ProductDtls.Save();
        }
        private async void Btn_AboutUs(object sender, RoutedEventArgs e)
        {
            ContentDialog c = new AboutUs();
            await c.ShowAsync();
        }

    }
}
