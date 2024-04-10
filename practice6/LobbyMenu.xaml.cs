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

        private void JoinButtonClicked(object sender, RoutedEventArgs e)
        {
            Visibility vis = Visibility.Visible, invis = Visibility.Collapsed;
            ChangeButtonVisibility(invis);

            MessageGrid.Visibility = vis;
            JoinGameButton.Visibility = vis;
            JoinGameRelativePanel.Visibility = vis;
        }

        private async void HostGame(object sender, RoutedEventArgs e)
        {
            if (NetworkManager.Client == null)
            {
                NetworkManager.SetClient(5555);
            }

            (Window.Current.Content as Frame).Navigate(typeof(Battlefield));
            if (await NetworkManager.ReceivePacketAsync(NetworkManager.PacketType.PT_HELLO))
            {
                GameInfo.PlayerCount = 2;
                GameInfo.CurrentGameState = GameInfo.GameState.PLACE_SHIPS;
            }
            
        }

        private void JoinGame(object sender, RoutedEventArgs e)
        {
            if (InputIPJoin.Text.Length != 0 && InputLobbyNameJoin.Text.Length != 0)
            {
                return;
            }

            if (InputIPJoin.Text.Length != 0)
            {
                NetworkManager.SetClient(InputIPJoin.Text, 5555);
            }
            else if (InputLobbyNameJoin.Text.Length != 0) // todo: implement
            {
                NetworkManager.SetClient(NetworkManager.BroadcastAddress, 5555);
            }

            NetworkManager.SendHello(true); // todo: refactor
            GameInfo.CurrentGameType = GameInfo.GameType.MULTI;
            GameInfo.PlayerCount = 2;
            (Window.Current.Content as Frame).Navigate(typeof(Battlefield));
            GameInfo.CurrentGameState = GameInfo.GameState.PLACE_SHIPS;
        }

        void ChangeButtonVisibility(Visibility visibility)
        {
            HostButton.Visibility = visibility;
            JoinButton.Visibility = visibility;
            BackButton.Visibility = visibility;
        }
    }
}
