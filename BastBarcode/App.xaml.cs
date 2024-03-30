using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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

namespace BastBarcode
{
    sealed partial class App : Application
    {
        public App()
        {
            // {{ Settings
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("OneBtnForSaveAndPrint")) localSettings.Values["OneBtnForSaveAndPrint"] = "true";
            if (!localSettings.Values.ContainsKey("ShowConfirmButton")) localSettings.Values["ShowConfirmButton"] = "true";
            // {{ Theme Settings
            if (!localSettings.Values.ContainsKey("Theme")) localSettings.Values["Theme"] = "system";
            switch (localSettings.Values["Theme"] as string)
            {
                case "light":
                    this.RequestedTheme = ApplicationTheme.Light; // set theme to light
                    break;
                case "dark": 
                    this.RequestedTheme = ApplicationTheme.Dark; // set theme to dark
                    break;
            }
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // {{ Go to this page
                    rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
