using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace practice6
{
    public sealed partial class LobbyMenu : Page
    {
        public LobbyMenu()
        {
            this.InitializeComponent();
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
        }

        private void ButtonOnHoverSound(object sender, PointerRoutedEventArgs e)
        {
            ButtonSound.Play();
        }

        private void ButtonOnUnhoverSound(object sender, PointerRoutedEventArgs e)
        {
            ButtonSound.Stop();
        }

        private void HostButtonClicked(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(Battlefield));
        }
    }
}
