using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace practice6
{

    public sealed partial class Battlefield : Page
    {
        SeaField _player = new SeaField();
        SeaField _enemy = new SeaField();

        public Battlefield()
        {
            this.InitializeComponent();
        }

        private void ClickEnemyField(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            byte row = Convert.ToByte(b.GetValue(Grid.RowProperty));
            byte column = Convert.ToByte(b.GetValue(Grid.ColumnProperty));
            _enemy[row, column] = 's';

            DrawShip(b, true);
        }

        void DrawShip(Button sender, bool isVertical)
        {
            Uri uri = new Uri("ms-appx:///Assets/Textures/ship.png"); // reference file inside the app's package
            BitmapImage bitmap = new BitmapImage { UriSource = uri };
            Image image = new Image { Source = bitmap };
            if (!isVertical) 
            {
                // TODO: add horizontal ship placement
            }

            sender.Content = image;
        }
    }
}
