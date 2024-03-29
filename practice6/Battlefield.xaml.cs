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
            //TakeShip.PointerMoved += TakeShipPointerMoved;
            //TakeShip.PointerReleased += TakeShipPointerReleased;
        }

        void InitializeFields()
        {
            Button p, e;
            SolidColorBrush gray = new SolidColorBrush(Windows.UI.Colors.Gray);
            Thickness margin2 = new Thickness(2, 2, 2, 2);
            for (int i = 0; i < SeaField._xDim; i++)
            {
                for (int j = 0; j < SeaField._yDim; j++)
                {
                    e = new Button { Background = gray, Margin = margin2 };
                    e.Click += ClickEnemyField;
                    Grid.SetRow(e, i);
                    Grid.SetColumn(e, j);
                    EnemyField.Children.Add(e);

                    p = new Button { Background = gray, Margin = margin2 };
                    p.Click += ClickPlayerField;
                    Grid.SetRow(p, i);
                    Grid.SetColumn(p, j);
                    PlayerField.Children.Add(p);
                }
            }
        }

        void ClickEnemyField(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            byte row = Convert.ToByte(b.GetValue(Grid.RowProperty));
            byte column = Convert.ToByte(b.GetValue(Grid.ColumnProperty));
            _enemy[row, column] = 's';

            DrawShip(b, true);
        }

        void ShipCloneClick(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(sender as UIElement);
            Image im = sender as Image;
            if (point.Properties.IsRightButtonPressed)
            {
                im.RenderTransformOrigin = new Windows.Foundation.Point { X = 0.5, Y = 0.5 };
                try
                {
                    var transform = (RotateTransform)im.RenderTransform;
                    transform.Angle += 90;
                } catch(InvalidCastException exception)
                {
                    im.RenderTransform = new RotateTransform() { Angle = 90 };
                }
                //if (ShipClone.Rotation == 90) // TODO: refactor
                //{
                //    ShipClone.Rotation = 0;
                //}
                //else
                //{
                //    ShipClone.Rotation = 90;
                //}
            }
        }

        void ClickTakeShip(object sender, PointerRoutedEventArgs e)
        {
            //if (_hasShipSelected)
            //{
            //    _hasShipSelected = false;
            //    _selectedShip = 0;
            //    return;
            //}
            //TakeShip.CapturePointer(e.Pointer);
            //_hasShipSelected = true;
            //_selectedShip = 1;

            //_takeShipStartPoint = e.GetCurrentPoint(this).Position;
            //switch (((Image)sender).Name)
            //{
            //    case "TakeShip":

            //        break;
            //    default:
            //        throw new ArgumentException("Invalid ship");
            //}
            ShipClone.PointerMoved += TakeShipPointerMoved;
            ShipClone.PointerPressed += ShipCloneClick;

            var ttv = TakeShip.TransformToVisual(Window.Current.Content);
            Windows.Foundation.Point screenCords = ttv.TransformPoint(new Windows.Foundation.Point(0,0));
            Canvas.SetLeft(ShipClone, screenCords.X);
            Canvas.SetTop(ShipClone, screenCords.Y);

            ShipClone.Visibility = Visibility.Visible;
        }

        //void HideDragInfo(object sender, DragEventArgs e)
        //{
        //    e.DragUIOverride.IsCaptionVisible = false;
        //    e.DragUIOverride.IsGlyphVisible = false;
        //}

        void TakeShipPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //if (_hasShipSelected)
            //{
            var position = e.GetCurrentPoint(this).Position;
            Canvas.SetLeft(ShipClone, position.X - ShipClone.Width / 2);
            Canvas.SetTop(ShipClone, position.Y - ShipClone.Height / 2);
            //}
        }

        //void TakeShipPointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    (sender as UIElement).ReleasePointerCapture(e.Pointer);
        //    _hasShipSelected = false;
        //    _selectedShip = 0;
        //}

        void ClickPlayerField(object sender, RoutedEventArgs e)
        {
            if (!_hasShipSelected) // if player has not selected any ships
            {
                return;
            }
            // TODO: implement functionality
        }

        //void ButtonDrop(object sender, DragEventArgs e)
        //{
        //    DrawShip(sender as Button, true);
        //}

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
