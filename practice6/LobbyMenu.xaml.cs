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
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading.Tasks;

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
            Visibility vis = Visibility.Visible, invis = Visibility.Collapsed;
            ChangeButtonVisibility(invis);

            GameInfo.CurrentGameType = GameInfo.GameType.MULTI;
            GameInfo.PlayerCount = 1;
            

            MessageGrid.Visibility = vis;
            HostGameRelativePanel.Visibility = vis;
            HostGameButton.Visibility = vis;
        }

        async private void JoinButtonClicked(object sender, RoutedEventArgs e)
        {
            Visibility vis = Visibility.Visible, invis = Visibility.Collapsed;
            ChangeButtonVisibility(invis);

            MessageGrid.Visibility = vis;
            HostGameButton.Visibility = vis;
            JoinGameRelativePanel.Visibility = vis;

            NetworkManager.Client = new UdpClient(5555);
            NetworkManager.Client.EnableBroadcast = true;

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                while (true)
                {
                    var result = await NetworkManager.Client.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);
                }
            });
        }

        private void HostGame(object sender, RoutedEventArgs e) 
        {
            if (NetworkManager.Client == null)
            {
                NetworkManager.SetClient(5555);
            }
            NetworkManager.ReceiveHello();

            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            //{
            //    while (true)
            //    {
            //        var result = await NetworkManager.client.ReceiveAsync();
            //        string message = Encoding.UTF8.GetString(result.Buffer);
            //    }
            //});
        }

        private void JoinGame(object sender, RoutedEventArgs e) 
        { 
            if (InputIPJoin.Text != null && InputLobbyNameJoin.Text != null)
            {
                return;
            }

            if (InputIPJoin.Text != null)
            {
                NetworkManager.SetClient(InputIPJoin.Text, 5555);
            }
            else if (InputLobbyNameJoin.Text != null) // todo: implement
            {
                NetworkManager.SetClient("192.168.194.255", 5555);
            }
            NetworkManager.SendHello();

            GameInfo.CurrentGameType = GameInfo.GameType.MULTI;
            GameInfo.PlayerCount = 2;
        }

        void ChangeButtonVisibility(Visibility visibility)
        {
            HostButton.Visibility = visibility;
            JoinButton.Visibility = visibility;
            BackButton.Visibility = visibility;
        }
    }
}
