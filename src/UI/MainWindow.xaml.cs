using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;

namespace UI
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        private const double CANVAS_SCALE = 10.0;

        private List<Shape> list = new List<Shape>();
        private Dictionary<string, RayTracer.AObject> objects =new Dictionary<string, RayTracer.AObject>();
        private Shape selected;

        private static int rectangleCount = 0;
        private static int ellipseCount = 0;
        private Random rnd = new Random();

        private TextBox previousTextBox;
        private string previousString;

        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DropDownButton_Checked(object sender, RoutedEventArgs e)
        {

        }
        #region Click Events
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(MenuItem))
            {
                MenuItem menuItem = (MenuItem)sender;
                if (menuItem.Name == "block")
                {
                    CreateBlock();
                }
                else if (menuItem.Name == "sphere")
                {
                    CreateEllipse();
                }
            }
        }
        private void SelectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(MenuItem))
            {
                MenuItem menuItem = (MenuItem)sender;
                foreach (Shape shape in list)
                {
                    if (shape.Name == (string)menuItem.Header)
                    {
                        SelectItem(shape);
                        break;
                    }
                }
            }
        }
        #endregion


        #region Objects
        private void CreateBlock()
        {
            DragCanvas canvas = (DragCanvas)this.FindName("canvas");
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
            rect.Width = 100;
            rect.Height = 50;
            rect.Name = "Rectangle" + rectangleCount;
            
            RayTracer.Block block = new RayTracer.Block(0, 0, 0, (int)rect.Width, (int)rect.Height, 40);
            objects[rect.Name] = block;

            Canvas.SetLeft(rect, 0);
            Canvas.SetTop(rect, 0);
            canvas.Children.Add(rect);
            list.Add(rect);
            AddAndSelectObject(rect);
            rectangleCount++;
        }

        private void CreateEllipse()
        {
            DragCanvas canvas = (DragCanvas)this.FindName("canvas");
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
            float diameter = 50;
            ellipse.Width = diameter;
            ellipse.Height = diameter;
            ellipse.Name = "Ellipse" + ellipseCount;

            RayTracer.Sphere sphere = new RayTracer.Sphere(-1,new RayTracer.Point(0,0,0), diameter);
            objects[ellipse.Name] = sphere;

            Canvas.SetLeft(ellipse, 0);
            Canvas.SetTop(ellipse, 0);
            canvas.Children.Add(ellipse);

            list.Add(ellipse);
            AddAndSelectObject(ellipse);
            ellipseCount++;
        }

        private void AddAndSelectObject(Shape shape)
        {
            ContextMenu contextMenu = (ContextMenu)this.FindName("selectContextMenu");
            MenuItem menuItem = new MenuItem();
            menuItem.Header = shape.Name;
            menuItem.Click += SelectMenuItem_Click;
            contextMenu.Items.Add(menuItem);
            SelectItem(shape);
        }

        public void SelectItem(Shape shape)
        {
            if (selected == null)
            {
                selected = shape;
            }
            // Remove previous selected indication
            selected.StrokeDashArray = null;
            
            SetObjectNameLabel(shape.Name);
            // Show selection
            shape.StrokeDashArray = new DoubleCollection(new double[] { 4.0, 4.0 });

            ((DragCanvas)this.FindName("canvas")).BringToFront(shape);

            FillValues(shape);

            this.selected = shape;
        }

        public void FillValues(Shape shape)
        {
            if (shape.GetType() == typeof(Rectangle))
            {   // TODO change axis
                Rectangle rectangle = (Rectangle)shape;
                TextBox positionX, positionY, positionZ, width, height;
                positionX = (TextBox)this.FindName("rectanglePositionX");
                positionY = (TextBox)this.FindName("rectanglePositionY");
                positionZ = (TextBox)this.FindName("rectanglePositionZ");
                width = (TextBox)this.FindName("rectangleWidth");
                height = (TextBox)this.FindName("rectangleHeight");


                positionX.Text = (Canvas.GetLeft(rectangle) / CANVAS_SCALE).ToString();
                positionY.Text = (Canvas.GetTop(rectangle) / CANVAS_SCALE).ToString();
                width.Text = (rectangle.Width / CANVAS_SCALE).ToString();
                height.Text = (rectangle.Height / CANVAS_SCALE).ToString();
            } else if (shape.GetType() == typeof(Ellipse))
            {
                Ellipse ellipse = (Ellipse)shape;
                TextBox positionX, positionY, positionZ, width, height;
                positionX = (TextBox)this.FindName("rectanglePositionX");
                positionY = (TextBox)this.FindName("rectanglePositionY");
                positionZ = (TextBox)this.FindName("rectanglePositionZ");
                width = (TextBox)this.FindName("rectangleWidth");
                height = (TextBox)this.FindName("rectangleHeight");


                positionX.Text = (Canvas.GetLeft(ellipse) / CANVAS_SCALE).ToString();
                positionY.Text = (Canvas.GetTop(ellipse) / CANVAS_SCALE).ToString();
                width.Text = (ellipse.Width / CANVAS_SCALE).ToString();
                height.Text = (ellipse.Height / CANVAS_SCALE).ToString();
            }
            FillProperties(shape);
        }

        private void FillProperties(Shape shape)
        {
            
        }

        private void SetObjectNameLabel(string name)
        {
            Label objectName = (Label)this.FindName("objectName");
            objectName.Content = name;
        }


        #endregion



        #region TextBoxValidation
        private void DoubleNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {   // Only positive
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);

            if (!e.Handled)
            {
                if (previousTextBox != null)
                {
                    this.previousString = previousTextBox.Text;
                }
            }
        }

        private void IntNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            if (!e.Handled)
            {
                if (previousTextBox != null)
                {
                    this.previousString = previousTextBox.Text;
                }
            }
        }

        private void PositionTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (this.previousTextBox == currentTextBox)
            {
                try
                {
                    Double.Parse(currentTextBox.Text, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    if (!(currentTextBox.Text == "-" || currentTextBox.Text == ""))
                    {
                        currentTextBox.Text = previousString;
                    }
                }
            }
            this.previousTextBox = (TextBox)sender;
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (this.previousTextBox == currentTextBox)
            {
                try
                {
                    Double.Parse(currentTextBox.Text, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    if (!(currentTextBox.Text == "-" || currentTextBox.Text == ""))
                    {
                        currentTextBox.Text = previousString;
                        e.Handled = true;
                    }
                }
            }
            this.previousTextBox = (TextBox)sender;
        }

        #endregion
    }
}
