using System;
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
using Windows.UI.Xaml.Automation;
using System.Text;
using Windows.ApplicationModel.ConversationalAgent;
using System.ComponentModel;

namespace practice6
{

    public sealed partial class Battlefield : Page
    {
        SeaField _player = new SeaField();
        SeaField _enemy = new SeaField();
        byte _shortShipsLeft = 4, _mediumShipsLeft = 2, _longShipsLeft = 2;
        byte _selectedShip = 0;
        bool _hasShipSelected = false;


        public Battlefield()
        {   
            this.InitializeComponent();
            InitializeFields();            
        }

        void InitializeFields()
        {
            Button p, e;
            SolidColorBrush gray = new SolidColorBrush(Windows.UI.Colors.Gray);
            Thickness margin2 = new Thickness(2, 2, 2, 2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < SeaField._xDim; i++)
            {
                for (int j = 0; j < SeaField._yDim; j++)
                {
                    e = new Button { Background = gray, Margin = margin2, Name = sb.Append("EnemyButton").Append(i).Append(':').Append(j).ToString() };
                    e.Click += ClickEnemyField;
                    Grid.SetRow(e, i);
                    Grid.SetColumn(e, j);
                    EnemyField.Children.Add(e);
                    sb.Clear();

                    // TODO: refactor
                    p = new Button { Background = gray, Margin = margin2, Name = sb.Append("PlayerButton").Append(i).Append(':').Append(j).ToString() };
                    Grid.SetRow(p, i);
                    Grid.SetColumn(p, j);
                    PlayerField.Children.Add(p);
                    sb.Clear();
                }
            }
        }

        void ClickEnemyField(object sender, RoutedEventArgs e) // TODO: change
        {
            Button b = sender as Button;
            byte row = Convert.ToByte(b.GetValue(Grid.RowProperty));
            byte column = Convert.ToByte(b.GetValue(Grid.ColumnProperty));
            _enemy[row, column] = 's';

            DrawShipOnField(b, 180);
        }

        void ShipCloneClick(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);
            Image ship = sender as Image;
            if (point.Properties.IsRightButtonPressed)
            {
                ship.RenderTransformOrigin = new Windows.Foundation.Point { X = 0.5, Y = 0.5 };
                try
                {
                    RotateTransform transform = (RotateTransform)ship.RenderTransform;
                    transform.Angle += 90;
                }
                catch (InvalidCastException exception)
                {
                    ship.RenderTransform = new RotateTransform() { Angle = 90 };
                }
            }
            else if (point.Properties.IsLeftButtonPressed)
            {
                var playerFieldPosition = FindCords(PlayerField);

                Button b = (PlayerField.Children[0] as Button);
                Windows.Foundation.Point pos = point.Position;
                var container = VisualTreeHelper.GetParent(PlayerField) as UIElement;
                double margin = b.Margin.Top, leftMargin = PlayerRelPanel.ActualOffset.X, topMargin = PlayerRelPanel.ActualOffset.Y;
                double buttonWidth = PlayerField.Width / SeaField._xDim - margin, buttonHeight = PlayerField.Height / SeaField._yDim - margin;
                if ((pos.X >= playerFieldPosition.X + margin && pos.X <= playerFieldPosition.X + PlayerField.Width - margin) &&
                    (pos.Y >= playerFieldPosition.Y + margin && pos.Y <= playerFieldPosition.Y + PlayerField.Height - margin))
                {
                    byte row, col;

                    col = Convert.ToByte(Math.Ceiling((pos.X - leftMargin) / (margin + buttonWidth)) - 1);
                    row = Convert.ToByte(Math.Ceiling((pos.Y - topMargin) / (margin + buttonHeight)) - 1);
                    if (_player[row, col] == 's')
                    {
                        return;
                    }

                    Button button = (Button)this.FindName($"PlayerButton{row}:{col}");
                    _player[row, col] = 's';

                    try
                    {
                        RotateTransform transform = (RotateTransform)ship.RenderTransform;
                        double angle = transform.Angle;
                        DrawShipOnField(button, angle);
                    }
                    catch (InvalidCastException exception)
                    {
                        DrawShipOnField(button); // TODO: use rotation angle as input to this function
                    }

                    ship.PointerMoved -= TakeShipPointerMoved;
                    ship.PointerPressed -= ShipCloneClick;
                    ship.Visibility = Visibility.Collapsed;
                    ship.RenderTransform = new RotateTransform();
                    _hasShipSelected = false;
                    _selectedShip = 0;
                }
            }
        }

        void ClickTakeShip(object sender, PointerRoutedEventArgs e)
        {
            if (_hasShipSelected)
            {
                return;
            }

            Image shipImage = null, senderImage = sender as Image;
            byte shipsLeft;
            string senderName = senderImage.Name;
            if (senderName.Equals("TakeShip"))
            {
                shipsLeft = _shortShipsLeft;
            }
            else if (senderName.Equals("TakeMediumShip"))
            {
                shipsLeft = _mediumShipsLeft;
            }
            else if (senderName.Equals("TakeLongShip"))
            {
                shipsLeft = _longShipsLeft;
            }
            else
            {
                return;
            }

            if (shipsLeft > 0)
            {
                switch (senderName)
                {
                    case "TakeShip":
                        shipImage = ShipClone;
                        _selectedShip = 1;
                        _shortShipsLeft--;
                        break;
                    case "TakeMediumShip":
                        shipImage = MediumShipClone;
                        _selectedShip = 2;
                        _mediumShipsLeft--;
                        break;
                    case "TakeLongShip":
                        shipImage = LongShipClone;
                        _selectedShip = 3;
                        _longShipsLeft--;
                        break;
                    default:
                        return;
                }
                shipImage.PointerMoved += TakeShipPointerMoved;
                shipImage.PointerPressed += ShipCloneClick;

                var screenCords = FindCords(senderImage);

                Canvas.SetLeft(shipImage, screenCords.X);
                Canvas.SetTop(shipImage, screenCords.Y);

                _hasShipSelected = true;
                shipImage.Visibility = Visibility.Visible;
                Bindings.Update();
            }
        }

        Windows.Foundation.Point FindCords(UIElement sender)
        {
            var ttv = sender.TransformToVisual(Window.Current.Content);
            return ttv.TransformPoint(new Windows.Foundation.Point(0, 0));
        }

        void TakeShipPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(this).Position;
            var ship = sender as Image;
            Canvas.SetLeft(ship, position.X - ship.Width / 2);
            Canvas.SetTop(ship, position.Y - ship.Height / 2);
        }

        void DrawShipOnField(Button drawPlace, double rotation = 0)
        {
            // TODO: draw different ships
            Uri uri = new Uri("ms-appx:///Assets/Textures/ship.png"); // reference file inside the app's package
            BitmapImage bitmap = new BitmapImage { UriSource = uri };
            Image image = new Image { Source = bitmap, Width = 64, Height = 64, Rotation = (float)rotation, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };

            drawPlace.Content = image;
        }
    }
}
