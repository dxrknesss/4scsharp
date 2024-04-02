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
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using Windows.UI.Input.Inking;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;
using System.Drawing;

namespace practice6
{

    public sealed partial class Battlefield : Page
    {
        SeaField _player = new SeaField();
        SeaField _enemy = new SeaField();
        byte _shortShipsLeft = 4, _mediumShipsLeft = 2, _longShipsLeft = 2;
        byte _selectedShip = 0;
        bool _hasShipSelected = false;
        BitmapImage _longShip1 = new BitmapImage(new Uri("ms-appx:///Assets/Textures/longship1.png")),
                _longShip2 = new BitmapImage(new Uri("ms-appx:///Assets/Textures/longship2.png")),
                _longShip3 = new BitmapImage(new Uri("ms-appx:///Assets/Textures/longship3.png")),
                _explosion = new BitmapImage(new Uri("ms-appx:///Assets/Textures/explosion.png"));
        Random _rnd = new Random();


        public Battlefield()
        {
            this.InitializeComponent();
            InitializeFields();
            InitializeAnnounce();
            if (GameInfo.CurrentGameType == GameInfo.GameType.SINGLE)
            {
                InitializeEnemyField();
            }
            InitializeElements();
            GameInfo.GameStateChange += ChangeAnnounceText;
            GameInfo.GameStateChange += BotAttack;
            
        }

        void PlaySoundWhenCollectionChanges(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            WinSound.Play();
        }

        void InitializeFields()
        {
            StringBuilder sb = new StringBuilder();
            SolidColorBrush gray = new SolidColorBrush(Windows.UI.Colors.Gray);
            Thickness margin2 = new Thickness(2, 2, 2, 2);
            double buttonHeight = EnemyField.Height / SeaField._yDim - margin2.Top * 2;
            double buttonWidth = EnemyField.Width / SeaField._xDim - margin2.Left * 2;

            for (int i = 0; i < SeaField._yDim; i++)
            {
                EnemyField.ColumnDefinitions.Add(new ColumnDefinition());
                PlayerField.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < SeaField._xDim; i++)
            {
                EnemyField.RowDefinitions.Add(new RowDefinition());
                PlayerField.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < SeaField._yDim; j++)
                {
                    EnemyField.Children.Add(CreateButton(i, j, gray, ref sb, margin2, buttonHeight, buttonWidth, true));
                    PlayerField.Children.Add(CreateButton(i, j, gray, ref sb, margin2, buttonHeight, buttonWidth, false));
                }
            }
        }

        Button CreateButton(int row, int col, SolidColorBrush backgroundColor,
            ref StringBuilder name, Thickness margin, double buttonHeight, double buttonWidth, bool isEnemyField)
        {
            name.Append(isEnemyField ? "EnemyButton" : "PlayerButton");
            name.Append(row).Append(':').Append(col);

            Button button = new Button
            {
                Background = backgroundColor,
                Margin = margin,
                Name = name.ToString(),
                Width = buttonWidth,
                Height = buttonHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Grid.SetRow(button, row);
            Grid.SetColumn(button, col);

            if (isEnemyField)
            {
                button.Click += ClickEnemyField;
            }
            name.Clear();

            return button;
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
                else // TODO: add functionality
                {

                }
            }
            else
            {
                LeftAnnounce.Text = GameInfo.AnnounceTexts[GameInfo.GameState.PLACE_SHIPS];
            }
        }

        void ChangeAnnounceText()
        {
            LeftAnnounce.Text = GameInfo.AnnounceTexts[GameInfo.CurrentGameState];
            Bindings.Update();
        }

        async void BotAttack()
        {
            if (GameInfo.CurrentGameState == GameInfo.GameState.WAIT)
            {
                await Task.Delay(1000);
                Point point = new Point((byte)_rnd.Next(0, SeaField._xDim), (byte)_rnd.Next(0, SeaField._yDim));
                while (_player[point.X, point.Y] == 'x')
                {
                    point = new Point((byte)_rnd.Next(0, SeaField._xDim), (byte)_rnd.Next(0, SeaField._yDim));
                }

                if (_player[point.X, point.Y] == 's')
                {
                    _player.ShipPoints.Remove(point);
                }
                DrawHit(point, false);
                _player[point.X, point.Y] = 'x';
                GameInfo.CurrentGameState = GameInfo.GameState.ATTACK;
            }
        }

        void InitializeEnemyField()
        {
            byte enemyShips = _shortShipsLeft, enemyMidShips = _mediumShipsLeft, enemyLongShips = _longShipsLeft;

            Random rnd = new Random();

            for (byte ships = 0; ships < enemyShips; ships++)
            {
                DrawShipsOnEnemyFieldSingleplayer(ref rnd);
            }

            for (byte ships = 0; ships < enemyMidShips; ships++)
            {
                DrawShipsOnEnemyFieldSingleplayer(ref rnd, 2);
            }

            for (byte ships = 0; ships < enemyLongShips; ships++) // todo: refactor
            {
                DrawShipsOnEnemyFieldSingleplayer(ref rnd, 3);
            }
            _enemy.ShipPoints.CollectionChanged += CheckWinLoseCondition;
        }

        void InitializeElements()
        {
            double windowHeight = (Window.Current.Content as Frame).ActualHeight, 
                windowWidth = (Window.Current.Content as Frame).ActualWidth;
            var banner = WinLoseBanner;
            Canvas.SetTop(banner, (windowHeight - banner.Height) / 2);
            Canvas.SetLeft(banner, (windowWidth - banner.Width) / 2);
        }

        private void CheckWinLoseCondition(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var collection = sender as ObservableCollection<Point>;
                if (collection.Count == 0)
                {
                    WinLoseBanner.Visibility = Visibility.Visible;
                    if (collection.Equals(_player.ShipPoints)) // fix for multiplayer
                    {
                        LoseSound.Play();
                        WinLoseText.Text = "You lose!";
                        _enemy.ShipPoints.CollectionChanged -= CheckWinLoseCondition;
                    }
                    else if (collection.Equals(_enemy.ShipPoints))
                    {
                        WinSound.Play();
                        WinLoseText.Text = "You win!";
                        _player.ShipPoints.CollectionChanged -= CheckWinLoseCondition;
                    }
                }
            }
        }

        void DrawShipsOnEnemyFieldSingleplayer(ref Random rnd, byte shipSelected = 1)
        {
            Point initPoint = new Point((byte)rnd.Next(0, SeaField._xDim), (byte)rnd.Next(0, SeaField._yDim));
            Point[] pointArr = new Point[] { initPoint };
            double angle = rnd.Next(0, 4) * 90;
            if (shipSelected > 1) // TODO: refactor
            {
                pointArr = new Point[0];
                while (!_enemy.IsValidPoint(pointArr))
                {
                    initPoint = new Point((byte)rnd.Next(0, SeaField._xDim), (byte)rnd.Next(0, SeaField._yDim));
                    angle = rnd.Next(0, 4) * 90;
                    pointArr = FindPointsForCurrentRotation(angle, initPoint, shipSelected);
                }
            }
            else
            {
                while (!_enemy.IsValidPoint(pointArr))
                {
                    pointArr[0] = new Point((byte)rnd.Next(0, SeaField._xDim), (byte)rnd.Next(0, SeaField._yDim));
                }
            }
            
            _enemy[pointArr] = 's';
            foreach (Point p in pointArr)
            {
                _enemy.ShipPoints.Add(p);
            }
            DrawShipOnField(pointArr.ToList(), angle, null, true, shipSelected);
        }

        void ClickEnemyField(object sender, RoutedEventArgs e) // TODO: change
        {
            if (GameInfo.CurrentGameState == GameInfo.GameState.ATTACK)
            {
                Button b = sender as Button;
                byte row = Convert.ToByte(b.GetValue(Grid.RowProperty));
                byte col = Convert.ToByte(b.GetValue(Grid.ColumnProperty));

                if (_enemy[row, col] == 's')
                {
                    (FindButtonByName($"EnemyButton{row}:{col}").Content as Image).Visibility = Visibility.Visible;
                    _enemy.ShipPoints.Remove(new Point(row, col));
                }
                else if (_enemy[row, col] == 'x')
                {
                    return;
                }
                _enemy[row, col] = 'x';
                DrawHit(new Point(row, col), true);
                GameInfo.CurrentGameState = GameInfo.GameState.WAIT;
            }
        }

        void ShipCloneClick(object sender, PointerRoutedEventArgs e)
        {
            if (GameInfo.CurrentGameState != GameInfo.GameState.PLACE_SHIPS)
            {
                return;
            }

            var point = e.GetCurrentPoint(this);
            Image ship = sender as Image;
            var transform = (RotateTransform)ship.RenderTransform;
            if (point.Properties.IsRightButtonPressed)
            {
                ship.RenderTransformOrigin = new Windows.Foundation.Point { X = 0.5, Y = 0.5 };
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
                    FindPointsForCurrentRotation(transform.Angle, initPoint, _selectedShip).ToList();
                    var pointArr = points.ToArray();

                    if (!_player.IsValidPoint(pointArr))
                    {
                        return;
                    }

                    _player[pointArr] = 's';
                    foreach(Point p in pointArr)
                    {
                        _player.ShipPoints.Add(p);
                    }

                    double angle = ((RotateTransform)ship.RenderTransform).Angle;
                    DrawShipOnField(points, angle, ship.Source, false, _selectedShip);

                    ship.PointerMoved -= TakeShipPointerMoved;
                    ship.PointerPressed -= ShipCloneClick;
                    ship.Visibility = Visibility.Collapsed;
                    _hasShipSelected = false;
                    _selectedShip = 0;
                    if (_shortShipsLeft == 0 && _mediumShipsLeft == 0 && _longShipsLeft == 0)
                    {
                        _player.ShipPoints.CollectionChanged += CheckWinLoseCondition;
                        _player.ShipPoints.CollectionChanged += PlaySoundWhenCollectionChanges;
                        GameInfo.CurrentGameState = GameInfo.GameState.ATTACK; // fix for two players, choose by random
                    }
                }
            }
        }

        Point[] FindPointsForCurrentRotation(double rotation, Point initPoint, byte shipSelected)
        {
            try
            {
                int direction = ((int)rotation % 360) / 90; // 0 is right, 1 is down, 2 is left, 3 is up

                if (shipSelected < 2 || shipSelected > 3)
                {
                    throw new ArgumentOutOfRangeException();
                }
                Point[] points = new Point[shipSelected];
                points[0] = initPoint;
                points[1] = GetPointFromAnotherAndDirection(initPoint, direction);
                if (shipSelected == 3)
                {
                    points[2] = GetPointFromAnotherAndDirection(initPoint, direction, true);
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
            ref byte shipsLeft = ref _shortShipsLeft;
            string senderName = senderImage.Name;

            switch (senderName)
            {
                case "TakeShip":
                    shipsLeft = ref _shortShipsLeft;
                    shipImage = ShipClone;
                    _selectedShip = 1;
                    break;
                case "TakeMediumShip":
                    shipImage = MediumShipClone;
                    shipsLeft = ref _mediumShipsLeft;
                    _selectedShip = 2;
                    break;
                case "TakeLongShip":
                    shipImage = LongShipClone;
                    shipsLeft = ref _longShipsLeft;
                    _selectedShip = 3;
                    break;
                default:
                    return;
            }

            if (shipsLeft < 1)
            {
                _selectedShip = 0;
                return;
            }
            shipsLeft--;

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

        void DrawHit(Point where, bool isEnemyField)
        {
            StringBuilder buttonName = new StringBuilder();
            buttonName.Append(isEnemyField ? "EnemyButton" : "PlayerButton");
            buttonName.Append(where.X).Append(':').Append(where.Y);
            Button b = FindButtonByName(buttonName.ToString());

            double measurement = b.Height - 10;
            Image image = new Image
            {
                Width = measurement,
                Height = measurement,
                Source = _explosion, // TODO: add source
            };
            var currentContent = b.Content as Image;
            Grid grid = new Grid();
            b.Content = grid;
            if (currentContent != null)
            {
                grid.Children.Add(currentContent);
            }
            grid.Children.Add(image);
        }

        void DrawShipOnField(List<Point> drawPlaces, double rotation = 0, ImageSource source = null, bool isEnemyField = false, byte selectedShip = 1)
        {
            if (drawPlaces.Count < 1)
            {
                return;
            }

            List<Button> buttons = new List<Button>();
            StringBuilder buttonName = new StringBuilder();
            
            foreach (Point p in drawPlaces)
            {
                buttonName.Append(isEnemyField ? "EnemyButton" : "PlayerButton");
                buttonName.Append(p.X).Append(':').Append(p.Y);
                buttons.Add(FindButtonByName(buttonName.ToString()));
                buttonName.Clear();
            }

            double measurement = buttons[0].Height - 5;
            for (int i = 0; i < buttons.Count; i++)
            {
                Image image = new Image
                {
                    Width = measurement,
                    Height = measurement,
                    RenderTransform = new RotateTransform { Angle = rotation },
                    RenderTransformOrigin = new Windows.Foundation.Point { X = 0.5, Y = 0.5 },
                    Source = source,
                    Visibility = isEnemyField ? Visibility.Collapsed : Visibility.Visible,
                };

                switch (i)
                {
                    case 0:
                        image.Source = buttons.Count > 1 ? _longShip1 : source ?? ShipClone.Source;
                        break;
                    case 1:
                        image.Source = selectedShip == 2 ? _longShip3 : _longShip2;
                        break;
                    case 2:
                        image.Source = _longShip3;
                        break;
                }

                buttons[i].Content = image;
            }
        }

        Button FindButtonByName(string buttonName) 
        {
            return (Button)this.FindName(buttonName);
        }

        void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            // (Window.Current.Content as Frame).Navigate(typeof(MainPage)); 
        }
    }
}
