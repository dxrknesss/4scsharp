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
using System.Collections.Generic;
using Windows.UI.Input.Inking;
using System.IO;
using System.Linq;

namespace practice6
{

    public sealed partial class Battlefield : Page
    {
        SeaField _player = new SeaField();
        SeaField _enemy = new SeaField();
        byte _shortShipsLeft = 4, _mediumShipsLeft = 2, _longShipsLeft = 2;
        byte _selectedShip = 0;
        bool _hasShipSelected = false;
        BitmapSource _longShip1 = new BitmapImage(new Uri("ms-appx:///Assets/Textures/longship1.png")),
                _longShip2 = new BitmapImage(new Uri("ms-appx:///Assets/Textures/longship2.png")),
                _longShip3 = new BitmapImage(new Uri("ms-appx:///Assets/Textures/longship3.png"));


        public Battlefield()
        {
            this.InitializeComponent();
            InitializeFields();
            InitializeAnnounce();
            GameInfo.GameStateChange += ChangeAnnounceText;
        }

        void InitializeFields()
        {
            Button p, e;
            SolidColorBrush gray = new SolidColorBrush(Windows.UI.Colors.Gray);
            Thickness margin2 = new Thickness(2, 2, 2, 2);
            StringBuilder sb = new StringBuilder();
            double buttonHeight = EnemyField.Height / SeaField._yDim - margin2.Top * 2;
            double buttonWidth = EnemyField.Width / SeaField._xDim - margin2.Left * 2;
            for (int i = 0; i < SeaField._xDim; i++)
            {
                for (int j = 0; j < SeaField._yDim; j++)
                {
                    e = new Button
                    {
                        Background = gray,
                        Margin = margin2,
                        Name = sb.Append("EnemyButton").Append(i).Append(':').Append(j).ToString(),
                        Width = buttonWidth,
                        Height = buttonHeight,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    e.Click += ClickEnemyField;
                    Grid.SetRow(e, i);
                    Grid.SetColumn(e, j);
                    EnemyField.Children.Add(e);
                    sb.Clear();

                    // TODO: refactor
                    p = new Button
                    {
                        Background = gray,
                        Margin = margin2,
                        Name = sb.Append("PlayerButton").Append(i).Append(':').Append(j).ToString(),
                        Width = buttonWidth,
                        Height = buttonHeight,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    Grid.SetRow(p, i);
                    Grid.SetColumn(p, j);
                    PlayerField.Children.Add(p);
                    sb.Clear();
                }
            }
        }

        void InitializeAnnounce()
        {
            var gameType = GameInfo.CurrentGameType;
            if (gameType == GameInfo.GameType.MULTI)
            {
                if (GameInfo.PlayerCount == 1)
                {
                    LeftAnnounce.Text = GameInfo.AnnounceTexts[GameInfo.GameState.WAIT_FOR_PLAYER];
                }
                else
                {

                }
            }
            else
            {
                LeftAnnounce.Text = GameInfo.AnnounceTexts[GameInfo.GameState.PLACE_SHIPS];
            }
        }

        void ChangeAnnounceText(GameInfo.GameState state)
        {
            LeftAnnounce.Text = GameInfo.AnnounceTexts[state];
            Bindings.Update();
        }

        void ClickEnemyField(object sender, RoutedEventArgs e) // TODO: change
        {
            Button b = sender as Button;
            byte row = Convert.ToByte(b.GetValue(Grid.RowProperty));
            byte column = Convert.ToByte(b.GetValue(Grid.ColumnProperty));
            _enemy[row, column] = 's';

            // DrawShipOnField(b, 180);
        }

        void ShipCloneClick(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);
            Image ship = sender as Image;
            if (point.Properties.IsRightButtonPressed)
            {
                ship.RenderTransformOrigin = new Windows.Foundation.Point { X = 0.5, Y = 0.5 };
                var transform = (RotateTransform)ship.RenderTransform;
                transform.Angle += 90;
            }
            else if (point.Properties.IsLeftButtonPressed)
            {
                var playerFieldPosition = FindCords(PlayerField);

                Windows.Foundation.Point pos = point.Position;
                double margin = (PlayerField.Children[0] as Button).Margin.Top, leftMargin = PlayerRelPanel.ActualOffset.X, topMargin = PlayerRelPanel.ActualOffset.Y;
                double buttonWidth = PlayerField.Width / SeaField._xDim - margin, buttonHeight = PlayerField.Height / SeaField._yDim - margin;
                if (pos.X >= playerFieldPosition.X + margin && pos.X <= playerFieldPosition.X + PlayerField.Width - margin &&
                    pos.Y >= playerFieldPosition.Y + margin && pos.Y <= playerFieldPosition.Y + PlayerField.Height - margin)
                {
                    byte row, col;

                    col = Convert.ToByte(Math.Ceiling((pos.X - leftMargin) / (margin + buttonWidth)) - 1);
                    row = Convert.ToByte(Math.Ceiling((pos.Y - topMargin) / (margin + buttonHeight)) - 1);

                    Point initPoint = new Point(row, col);
                    List<Point> points = 
                        _selectedShip == 1 ? new List<Point>() { initPoint } : 
                    FindPointsForCurrentRotation(ship, initPoint).ToList();
                    var pointArr = points.ToArray();

                    if (!_player.IsValidPoint(pointArr))
                    {
                        return;
                    }

                    _player[pointArr] = 's';

                    double angle = ((RotateTransform)ship.RenderTransform).Angle;
                    DrawShipOnField(points, angle, ship);

                    ship.PointerMoved -= TakeShipPointerMoved;
                    ship.PointerPressed -= ShipCloneClick;
                    ship.Visibility = Visibility.Collapsed;
                    _hasShipSelected = false;
                    _selectedShip = 0;
                    if (_shortShipsLeft == 0 && _mediumShipsLeft == 0 && _longShipsLeft == 0)
                    {
                        GameInfo.CurrentGameState = GameInfo.GameState.ATTACK; // fix for two players, choose by random
                    }
                }
            }
        }

        Point[] FindPointsForCurrentRotation(Image ship, Point initPoint)
        {
            try
            {
                RotateTransform transform = (RotateTransform)ship.RenderTransform;
                int direction = ((int)transform.Angle % 360) / 90; // 0 is up, 1 is right, 2 is down, 3 is left

                Point[] points;
                if (_selectedShip == 2)
                {
                    points = new Point[2];
                    points[0] = initPoint;
                    points[1] = GetPointFromAnotherAndDirection(initPoint, direction);
                } 
                else if (_selectedShip == 3)
                {
                    points = new Point[3];
                    points[0] = initPoint;
                    points[1] = GetPointFromAnotherAndDirection(initPoint, direction);
                    points[2] = GetPointFromAnotherAndDirection(initPoint, direction, true);
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

                return points;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                throw exception;
            }
        }

        Point GetPointFromAnotherAndDirection(Point p, int direction, bool multByMinusOne = false)
        {
            sbyte x = (sbyte)p.X, y = (sbyte)p.Y;
            sbyte add = (sbyte)((direction / 2 > 0) ? -1 : 1);
            if (multByMinusOne)
            {
                add *= -1;
            }

            if (direction % 2 == 1)
            {
                x += add;
            } 
            else
            {
                y += add;
            }

            return new Point((byte)x, (byte)y);
        }

        void ClickTakeShip(object sender, PointerRoutedEventArgs e)
        {
            if (_hasShipSelected)
            {
                return;
            }

            Image shipImage = null, senderImage = (Image)sender;
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
                shipImage.RenderTransform = new RotateTransform();
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
            var ship = (Image)sender;
            Canvas.SetLeft(ship, position.X - ship.Width / 2);
            Canvas.SetTop(ship, position.Y - ship.Height / 2);
        }

        void DrawShipOnField(List<Point> drawPlaces, double rotation = 0, Image source = null)
        {
            if (drawPlaces.Count < 1)
            {
                return;
            }

            List<Button> buttons = new List<Button>();
            foreach(Point p in drawPlaces)
            {
                buttons.Add((Button)this.FindName($"PlayerButton{p.X}:{p.Y}"));
            }
            
            double measurement = buttons[0].Height - 5;    

            for (int i = 0; i< buttons.Count; i++)
            {
                Image image = new Image
                {
                    Width = measurement,
                    Height = measurement,
                    RenderTransform = new RotateTransform { Angle = rotation },
                    RenderTransformOrigin = new Windows.Foundation.Point { X = 0.5, Y = 0.5 },
                };
                switch(i)
                {
                    case 0:
                        image.Source = _selectedShip == 1 ? ShipClone.Source : _longShip1;
                        break;
                    case 1:
                        image.Source = _selectedShip == 2 ? _longShip3 : _longShip2;
                        break;
                    case 2:
                        image.Source = _longShip3;
                        break;
                }
                
                buttons[i].Content = image;
            }
        }
    }
}
