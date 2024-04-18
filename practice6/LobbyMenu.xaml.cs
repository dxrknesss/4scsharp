using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace practice6
{
    public sealed partial class LobbyMenu : Page
    {
        public LobbyMenu()
        {
            this.InitializeComponent();
            InitializeComponents();
        }

        void InitializeComponents()
        {
            double windowHeight = Window.Current.Bounds.Height,
                windowWidth = Window.Current.Bounds.Width;
            ButtonGrid.Width = windowWidth / 2;
            ButtonGrid.Height = windowHeight / 2;

            double gridWidth = ButtonGrid.Width;
            double gridHeightBy4 = ButtonGrid.Height / 4;
            HostButton.Width = gridWidth / 1.5;
            HostButton.Height = gridHeightBy4;

            JoinButton.Width = gridWidth / 1.5;
            JoinButton.Height = gridHeightBy4;

            BackButton.Width = gridWidth / 2;
            BackButton.Height = gridHeightBy4 / 1.5;

            JoinGameButton.Width = JoinGameGrid.Width / 2;
            JoinGameButton.Height = JoinGameGrid.Height / 7;
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
            NetworkManager.CurrentPeerType = NetworkManager.PeerType.HOST;

            NetworkManager.SetClient(5555);

            (Window.Current.Content as Frame).Navigate(typeof(Battlefield));
        }

        private void JoinButtonClicked(object sender, RoutedEventArgs e)
        {
            Visibility vis = Visibility.Visible, invis = Visibility.Collapsed;
            ChangeButtonVisibility(invis);

            NetworkManager.CurrentPeerType = NetworkManager.PeerType.CLIENT;

            JoinGameGrid.Visibility = vis;
            JoinGameButton.Visibility = vis;
            JoinGameRelativePanel.Visibility = vis;
        }

        private void JoinGame(object sender, RoutedEventArgs e)
        {
            if (InputIPJoin.Text.Length != 0)
            {
                NetworkManager.SetClient(InputIPJoin.Text, 5555);
            }

            if (NetworkManager.SendDataSync(
                new byte[] { (byte)NetworkManager.PacketType.PT_HELLO },
                NetworkManager.PacketType.PT_ACK,
                NetworkManager.RemoteConnectionPoint))
            {
                NetworkManager.ConnectionEstablished = true;
                (Window.Current.Content as Frame).Navigate(typeof(Battlefield));
            }
        }

        void ChangeButtonVisibility(Visibility visibility)
        {
            HostButton.Visibility = visibility;
            JoinButton.Visibility = visibility;
            BackButton.Visibility = visibility;
        }
    }
}
