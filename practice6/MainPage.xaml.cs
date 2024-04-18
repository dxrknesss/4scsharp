using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace practice6
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Task.Run(() => GameInfo.GetInstance());
            InitializeComponents();
        }

        void InitializeComponents()
        {
            Size size = new Size(1000, 1000);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Window.Current.CoreWindow.SizeChanged += (s, e) =>
            {
                ApplicationView.GetForCurrentView().TryResizeView(size);
            };

            double windowHeight = Window.Current.Bounds.Height,
                windowWidth = Window.Current.Bounds.Width;
            ButtonGrid.Width = windowWidth / 2;
            ButtonGrid.Height = windowHeight / 2;

            SPButton.Height = ButtonGrid.Height / 4;
            SPButton.Width = ButtonGrid.Width / 1.5;

            MPButton.Height = ButtonGrid.Height / 4;
            MPButton.Width = ButtonGrid.Width / 1.5;
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
