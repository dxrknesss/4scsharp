using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
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

namespace practice6
{

    public sealed partial class Battlefield : Page
    {
        SeaField _player = new SeaField();
        SeaField _enemy = new SeaField();
        byte _selectedShip = 0;
        bool _hasShipSelected = false;
        Windows.Foundation.Point _takeShipStartPoint;

        public Battlefield()
        {
            this.InitializeComponent();
            InitializeFields();

            TakeShip.PointerPressed += ClickTakeShip;
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

        void ClickEnemyField(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            byte row = Convert.ToByte(b.GetValue(Grid.RowProperty));
            byte column = Convert.ToByte(b.GetValue(Grid.ColumnProperty));
            _enemy[row, column] = 's';

            DrawShipOnField(b, true);
        }

        void ShipCloneClick(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);
            Image im = sender as Image;
            if (point.Properties.IsRightButtonPressed)
            {
                im.RenderTransformOrigin = new Windows.Foundation.Point { X = 0.5, Y = 0.5 };
                try
                {
                    RotateTransform transform = (RotateTransform)im.RenderTransform;
                    transform.Angle += 90;
                } catch (InvalidCastException exception)
                {
                    im.RenderTransform = new RotateTransform() { Angle = 90 };
                }
            }
            else if (point.Properties.IsLeftButtonPressed)
            {
                var ttv = PlayerField.TransformToVisual(Window.Current.Content);
                Windows.Foundation.Point playerFieldPosition = ttv.TransformPoint(new Windows.Foundation.Point(0, 0));

                Button b = (PlayerField.Children[0] as Button);
                Windows.Foundation.Point pos = point.Position;
                double margin = b.Margin.Top, leftMargin = PlayerField.ActualOffset.X, topMargin = Canvas.GetTop(PlayerRelPanel);
                double buttonWidth = PlayerField.Width / SeaField._xDim - margin, buttonHeight = PlayerField.Height / SeaField._yDim - margin;
                if ((pos.X >= playerFieldPosition.X + margin && pos.X <= playerFieldPosition.X + PlayerField.Width - margin) &&
                    (pos.Y >= playerFieldPosition.Y + margin && pos.Y <= playerFieldPosition.Y + PlayerField.Height - margin))
                {
                    byte row, col;

                    col = Convert.ToByte(Math.Ceiling((pos.X - leftMargin) / (margin + buttonWidth)) - 1);
                    row = Convert.ToByte(Math.Ceiling((pos.Y - topMargin) / (margin + buttonHeight)) - 1);
                    Button button = (Button)this.FindName($"PlayerButton{row}:{col}");
                    _player[row, col] = 's';
                    try

                    {
                        RotateTransform transform = (RotateTransform)im.RenderTransform;
                        double angle = transform.Angle;
                        DrawShipOnField(button, angle % 180 == 0, angle);
                    }
                    catch (InvalidCastException exception)
                    {
                        DrawShipOnField(button, false); // TODO: use rotation angle as input to this function
                    }

                    ShipClone.PointerMoved -= TakeShipPointerMoved;
                    ShipClone.PointerPressed -= ShipCloneClick;
                    ShipClone.Visibility = Visibility.Collapsed;
                }
            }
        }

        void ClickTakeShip(object sender, PointerRoutedEventArgs e)
        {
            ShipClone.PointerMoved += TakeShipPointerMoved;
            ShipClone.PointerPressed += ShipCloneClick;

            var ttv = TakeShip.TransformToVisual(Window.Current.Content);
            Windows.Foundation.Point screenCords = ttv.TransformPoint(new Windows.Foundation.Point(0, 0));
            Canvas.SetLeft(ShipClone, screenCords.X);
            Canvas.SetTop(ShipClone, screenCords.Y);

            ShipClone.Visibility = Visibility.Visible;
        }

        void TakeShipPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(this).Position;
            Canvas.SetLeft(ShipClone, position.X - ShipClone.Width / 2);
            Canvas.SetTop(ShipClone, position.Y - ShipClone.Height / 2);
        }

        void DrawShipOnField(Button drawPlace, bool isVertical, double rotation = 0)
        {
            Uri uri = new Uri("ms-appx:///Assets/Textures/ship.png"); // reference file inside the app's package
            BitmapImage bitmap = new BitmapImage { UriSource = uri };
            Image image = new Image { Source = bitmap };
            if (!isVertical) 
            {
                // TODO: add horizontal ship placement
                image.Rotation = (float)rotation;
            }

            drawPlace.Content = image;
        }
    }
}
