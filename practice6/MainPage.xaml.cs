using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace practice6
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Task.Run(() => GameInfo.GetInstance());
            SetWindowSize();
            MPButton.Click += OnMPClick;
        }

        void SetWindowSize()
        {
            Size size = new Size(1600, 900);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Window.Current.CoreWindow.SizeChanged += (s, e) =>
            {
                ApplicationView.GetForCurrentView().TryResizeView(size);
            };
        }

        private void OnMPClick(object sender, RoutedEventArgs e)
        {
            GameInfo.CurrentGameType = GameInfo.GameType.MULTI;
            (Window.Current.Content as Frame).Navigate(typeof(LobbyMenu));
        }

        private void OnSPClick(object sender, RoutedEventArgs e)
        {
            GameInfo.CurrentGameType = GameInfo.GameType.SINGLE;
            GameInfo.CurrentGameState = GameInfo.GameState.PLACE_SHIPS;
            (Window.Current.Content as Frame).Navigate(typeof(Battlefield));
        }

        private void ButtonOnHoverSound(object sender, PointerRoutedEventArgs e)
        {
            ButtonSound.Play();
        }

        private void ButtonOnUnhoverSound(object sender, PointerRoutedEventArgs e)
        {
            ButtonSound.Stop();
        }
    }
}
