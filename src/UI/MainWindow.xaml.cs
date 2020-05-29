﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Globalization;

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
        private Dictionary<string, RayTracer.AObject> objects = new Dictionary<string, RayTracer.AObject>();
        private Shape selected;

        private View currentView = View.FRONT;
        private static int rectangleCount = 0;
        private static int ellipseCount = 0;
        private Random rnd = new Random();

        private TextBox previousTextBox;
        private string previousString;

        #endregion
        public MainWindow()
        {
            InitializeComponent();

            TextBox positionZ, depth;
            positionZ = (TextBox)this.FindName("positionZ");
            positionZ.IsEnabled = false;
            depth = (TextBox)this.FindName("rectangleDepth");
            depth.IsEnabled = false;
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

        

        private void viewButtonFront_Click(object sender, RoutedEventArgs e)
        {
            if (currentView != View.FRONT)
            {
                EnableAllTextBoxes();
                TextBox positionZ, depth;
                positionZ = (TextBox)this.FindName("positionZ");
                positionZ.IsEnabled = false;
                depth = (TextBox)this.FindName("rectangleDepth");
                depth.IsEnabled = false;
                currentView = View.FRONT;
                RepositionShapes();
            }
        }
        private void viewButtonTop_Click(object sender, RoutedEventArgs e)
        {
            if (currentView != View.TOP)
            {
                EnableAllTextBoxes();
                TextBox positionY, height;
                positionY = (TextBox)this.FindName("positionY");
                positionY.IsEnabled = false;
                height = (TextBox)this.FindName("rectangleHeight");
                height.IsEnabled = false;
                currentView = View.TOP;
                RepositionShapes();
            }
        }

        private void viewButtonRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentView != View.RIGHT)
            {
                EnableAllTextBoxes();
                TextBox positionX, width;
                positionX = (TextBox)this.FindName("positionX");
                positionX.IsEnabled = false;
                width = (TextBox)this.FindName("rectangleWidth");
                width.IsEnabled = false;
                currentView = View.RIGHT;
                RepositionShapes();
            }
        }

        private void EnableAllTextBoxes()
        {
            TextBox positionX, positionY, positionZ, width, height, depth;
            positionX = (TextBox)this.FindName("positionX");
            positionY = (TextBox)this.FindName("positionY");
            positionZ = (TextBox)this.FindName("positionZ");
            width = (TextBox)this.FindName("rectangleWidth");
            height = (TextBox)this.FindName("rectangleHeight");
            depth = (TextBox)this.FindName("rectangleDepth");

            positionX.IsEnabled = true;
            positionY.IsEnabled = true;
            positionZ.IsEnabled = true;
            width.IsEnabled = true;
            height.IsEnabled = true;
            depth.IsEnabled = true;
        }

        #endregion


        #region Objects
        private void CreateBlock()
        {
            DragCanvas canvas = (DragCanvas)this.FindName("canvas");
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            int r = rnd.Next(256);
            int g = rnd.Next(256);
            int b = rnd.Next(256);
            rect.Fill = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            rect.Width = 100;
            rect.Height = 50;
            rect.Name = "Rectangle" + rectangleCount;
            RayTracer.Block block;
            if (currentView == View.FRONT)
            {
                block = new RayTracer.Block(0, 0, 0, (int)rect.Width, (int)rect.Height, 5);
            }
            else if (currentView == View.TOP)
            {
                block = new RayTracer.Block(0, 0, 0, (int)rect.Width, 5, (int)rect.Height);

            }
            else
            {
                block = new RayTracer.Block(0, 0, 0, 5, (int)rect.Height, (int)rect.Width);
            }
            block.SetColor((float)(r / 256.0), (float)(g / 256.0), (float)(b / 256.0));
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
            int r = rnd.Next(256);
            int g = rnd.Next(256);
            int b = rnd.Next(256);
            ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            float diameter = 50;
            ellipse.Width = diameter;
            ellipse.Height = diameter;
            ellipse.Name = "Ellipse" + ellipseCount;

            RayTracer.Sphere sphere = new RayTracer.Sphere(-1, new RayTracer.Point(2.5f, 2.5f, 2.5f), diameter);
            sphere.SetColor((float)(r / 256.0), (float)(g / 256.0), (float)(b / 256.0));
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
            FillValues(shape, false, true);
        }

        public void SelectItem(Shape shape)
        {
            if (selected == shape)
            {
                return;
            }
            if (selected != null)
            {
                selected.StrokeDashArray = null;
            }
            this.selected = shape;

            SetObjectNameLabel(shape.Name);
            // Show selection
            shape.StrokeDashArray = new DoubleCollection(new double[] { 4.0, 4.0 });

            ((DragCanvas)this.FindName("canvas")).BringToFront(shape);

            WrapPanel rw = (WrapPanel)this.FindName("rectangleWrapper");
            WrapPanel sw = (WrapPanel)this.FindName("sphereWrapper");
            if (shape.GetType() == typeof(Rectangle))
            {
                rw.Visibility = Visibility.Visible;
                sw.Visibility = Visibility.Hidden;
            }
            else if (shape.GetType() == typeof(Ellipse))
            {
                rw.Visibility = Visibility.Hidden;
                sw.Visibility = Visibility.Visible;
            }
            FillValues(shape);
        }

        public void FillValues(Shape shape, bool dropped = false, bool first = false)
        {
            if (!first) {
                if (selected == shape && !dropped)
                {
                    return;
                }
            }

            TextBox positionX, positionY, positionZ;
            positionX = (TextBox)this.FindName("positionX");
            positionY = (TextBox)this.FindName("positionY");
            positionZ = (TextBox)this.FindName("positionZ");

            
            if (shape.GetType() == typeof(Rectangle))
            {   
                Rectangle rectangle = (Rectangle)shape;
                TextBox width, height, depth;

                width = (TextBox)this.FindName("rectangleWidth");
                height = (TextBox)this.FindName("rectangleHeight");
                depth = (TextBox)this.FindName("rectangleDepth");

                UpdatePositionValues(shape);
                RayTracer.Block block = (RayTracer.Block)objects[rectangle.Name];
                
                positionX.Text = block.a.X.ToString();
                positionY.Text = block.a.Y.ToString();
                positionZ.Text = block.a.Z.ToString();
                width.Text = block.getWidth().ToString();
                height.Text = block.getHeight().ToString();
                depth.Text = block.getDepth().ToString();
            }
            else if (shape.GetType() == typeof(Ellipse))
            {
                Ellipse ellipse = (Ellipse)shape;
                TextBox diameter;

                diameter = (TextBox)this.FindName("diameter");

                UpdatePositionValues(shape);

                RayTracer.Sphere sphere = (RayTracer.Sphere)objects[ellipse.Name];                
                positionX.Text = sphere.xPos.ToString();
                positionY.Text = sphere.yPos.ToString();
                positionZ.Text = sphere.zPos.ToString();
                diameter.Text = sphere.diameter.ToString();
            }
            FillProperties(shape);
        }

        private void UpdatePositionValues(Shape shape)
        {
            if (currentView == View.FRONT)
            {
                if (shape.GetType() == typeof(Rectangle))
                {
                    Rectangle rectangle = (Rectangle)shape;
                    RayTracer.Block block = (RayTracer.Block)objects[rectangle.Name];
                    double x = GetX(rectangle);
                    double y = GetY(rectangle);
                    block.SetPosition((float)x, (float)y, block.a.Z, (float)(rectangle.Width / CANVAS_SCALE), (float)(rectangle.Height / CANVAS_SCALE), block.getDepth());

                }
                else if (shape.GetType() == typeof(Ellipse))
                {
                    Ellipse ellipse = (Ellipse)shape;
                    RayTracer.Sphere sphere = (RayTracer.Sphere)objects[ellipse.Name];
                    double x = (Canvas.GetLeft(ellipse) + (ellipse.Width / 2)) / CANVAS_SCALE;
                    double y = (Canvas.GetTop(ellipse) + (ellipse.Height / 2)) / CANVAS_SCALE;
                    sphere.xPos = (float)x;
                    sphere.yPos = (float)y;
                    sphere.diameter = (float)(ellipse.Width / CANVAS_SCALE);
                }
            }
            else if (currentView == View.TOP)
            {
                if (shape.GetType() == typeof(Rectangle))
                {
                    Rectangle rectangle = (Rectangle)shape;
                    RayTracer.Block block = (RayTracer.Block)objects[rectangle.Name];
                    double x = GetX(rectangle);
                    double z = GetY(rectangle);
                    block.SetPosition((float)x, block.a.Y, (float)z, (float)(rectangle.Width / CANVAS_SCALE), block.getHeight(), (float)(rectangle.Height / CANVAS_SCALE));
                }
                else if (shape.GetType() == typeof(Ellipse))
                {
                    Ellipse ellipse = (Ellipse)shape;
                    RayTracer.Sphere sphere = (RayTracer.Sphere)objects[ellipse.Name];
                    double x = (Canvas.GetLeft(ellipse) + (ellipse.Width / 2)) / CANVAS_SCALE;
                    double z = (Canvas.GetTop(ellipse) + (ellipse.Height / 2)) / CANVAS_SCALE;

                    sphere.xPos = (float)x;
                    sphere.zPos = (float)z;
                    sphere.diameter = (float)(ellipse.Width / CANVAS_SCALE);
                }
            }
            else if (currentView == View.RIGHT)
            {
                if (shape.GetType() == typeof(Rectangle))
                {
                    Rectangle rectangle = (Rectangle)shape;
                    RayTracer.Block block = (RayTracer.Block)objects[rectangle.Name];
                    double z = GetX(rectangle);
                    double y = GetY(rectangle);
                    block.SetPosition(block.a.X, (float)y, (float)z, block.getWidth(), (float)(rectangle.Height / CANVAS_SCALE), (float)(rectangle.Width / CANVAS_SCALE));
                }
                else if (shape.GetType() == typeof(Ellipse))
                {
                    Ellipse ellipse = (Ellipse)shape;
                    RayTracer.Sphere sphere = (RayTracer.Sphere)objects[ellipse.Name];
                    double z = (Canvas.GetLeft(ellipse) + (ellipse.Width / 2)) / CANVAS_SCALE;
                    double y = (Canvas.GetTop(ellipse) + (ellipse.Height / 2)) / CANVAS_SCALE;
                    sphere.zPos = (float)z;
                    sphere.yPos = (float)y;
                    sphere.diameter = (float)(ellipse.Width / CANVAS_SCALE);
                }
            }
        }

        private void FillProperties(Shape shape)
        {
            TextBox r, g, b, reflection, diffusion, transparency;
            r = (TextBox)this.FindName("ColorR");
            g = (TextBox)this.FindName("ColorG");
            b = (TextBox)this.FindName("ColorB");
            diffusion = (TextBox)this.FindName("diffusion");
            reflection = (TextBox)this.FindName("reflection");
            transparency = (TextBox)this.FindName("transparency");

            RayTracer.AObject aObject = objects[shape.Name];
            r.Text = (aObject.color.r * 256).ToString();
            g.Text = (aObject.color.g * 256).ToString();
            b.Text = (aObject.color.b * 256).ToString();
            diffusion.Text = aObject.kd.ToString();
            reflection.Text = aObject.ks.ToString();
            transparency.Text = aObject.kt.ToString();
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

        private void PositionTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            bool changed = false;
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
                        changed = true;
                    }
                }
            }

            if (!changed && selected != null)
            {
                try
                {
                    double value = Double.Parse(currentTextBox.Text) * CANVAS_SCALE;
                    if (selected.GetType() == typeof(Ellipse))
                    {
                        value = value - (selected.Width / 2.0);
                    }

                    if (currentTextBox.Name == "positionX") {
                        if (currentView != View.RIGHT)
                        {
                            if (Canvas.GetLeft(selected) != value)
                            {
                                Canvas.SetLeft(selected, value);
                            }
                        }                    
                    }
                    if (currentTextBox.Name == "positionY") {
                        if (currentView != View.TOP)
                        {
                            if (Canvas.GetTop(selected) != value)
                            {
                                Canvas.SetTop(selected, value);
                            }
                        }
                    }
                    if (currentTextBox.Name == "positionZ") {
                        if (currentView == View.TOP)
                        {
                            if (Canvas.GetTop(selected) != value)
                            {
                                Canvas.SetTop(selected, value);
                            }
                        }
                        else if (currentView == View.RIGHT)
                        {
                            if (Canvas.GetLeft(selected) != value)
                            {
                                Canvas.SetLeft(selected, value);
                            }
                        }
                    }
                    if (currentTextBox.Name == "rectangleWidth") {
                        if (currentView != View.RIGHT)
                        {
                            selected.Width = value;
                        }
                    }
                    if (currentTextBox.Name == "rectangleHeight") {
                        if (currentView != View.TOP)
                        {
                            selected.Height = value;
                        }
                    }
                    if (currentTextBox.Name == "rectangleDepth")
                    {
                        if (currentView == View.TOP)
                        {
                            selected.Height = value;
                        }
                        else if (currentView == View.RIGHT)
                        {
                            selected.Width = value;
                        }
                    }
                    if (currentTextBox.Name == "diameter") {
                        selected.Width = selected.Height = Double.Parse(currentTextBox.Text) * CANVAS_SCALE;
                    }
                    
                    
                }
                catch (Exception) { }
            }


            this.previousTextBox = (TextBox)sender;
        }

        private void PropertiesTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (this.previousTextBox == currentTextBox)
            {
                try
                {
                    double parsed = Double.Parse(currentTextBox.Text, CultureInfo.InvariantCulture);
                    if (parsed < 0 || parsed > 1)
                    {
                        currentTextBox.Text = previousString;
                        e.Handled = true;
                    }
                    else
                    {
                        TextBox diffusion, reflection, transparency;
                        diffusion = (TextBox)this.FindName("diffusion");
                        reflection = (TextBox)this.FindName("reflection");
                        transparency = (TextBox)this.FindName("transparency");
                        float d = (float)Double.Parse(diffusion.Text, CultureInfo.InvariantCulture);
                        float r = (float)Double.Parse(reflection.Text, CultureInfo.InvariantCulture);
                        float t = (float)Double.Parse(transparency.Text, CultureInfo.InvariantCulture);

                        RayTracer.AObject aObject = objects[selected.Name];
                        aObject.kd = d;
                        aObject.ks = r;
                        aObject.kt = t;
                    }
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

        private void ColorTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (this.previousTextBox == currentTextBox)
            {
                try
                {
                    int parsed = Int32.Parse(currentTextBox.Text, CultureInfo.InvariantCulture);
                    if (parsed < 0 || parsed > 255)
                    {
                        currentTextBox.Text = previousString;
                        e.Handled = true;
                    } else
                    {
                        TextBox r, g, b;
                        r = (TextBox)this.FindName("ColorR");
                        g = (TextBox)this.FindName("ColorG");
                        b = (TextBox)this.FindName("ColorB");
                        int colorR = Int32.Parse(r.Text);
                        int colorG = Int32.Parse(g.Text);
                        int colorB = Int32.Parse(b.Text);
                        selected.Fill = new SolidColorBrush(Color.FromRgb((byte)colorR, (byte)colorG, (byte)colorB));
                        RayTracer.AObject aObject = objects[selected.Name];
                        aObject.color.r = (float)(colorR / 256.0);
                        aObject.color.g = (float)(colorG / 256.0);
                        aObject.color.b = (float)(colorB / 256.0);
                    }
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

        private void TextBoxGotFocusEventHandler(object sender, RoutedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            this.previousTextBox = (TextBox)sender;
            this.previousString = currentTextBox.Text;
        }

        private double GetX(Shape shape)
        {
            return Canvas.GetLeft(shape) / CANVAS_SCALE;
        }
        private double GetY(Shape shape)
        {
            return Canvas.GetTop(shape) / CANVAS_SCALE;
        }

        private void RepositionShapes()
        {
            foreach (Shape shape in list)
            {
                if (shape.GetType() == typeof(Rectangle))
                {
                    Rectangle rectangle = (Rectangle)shape;
                    RayTracer.Block block = (RayTracer.Block)objects[rectangle.Name];
                    if (currentView == View.FRONT)
                    {
                        Canvas.SetLeft(rectangle, block.a.X * CANVAS_SCALE);
                        Canvas.SetTop(rectangle, block.a.Y * CANVAS_SCALE);
                    }
                    else if (currentView == View.TOP)
                    {
                        Canvas.SetLeft(rectangle, block.a.X * CANVAS_SCALE);
                        Canvas.SetTop(rectangle, block.a.Z * CANVAS_SCALE);
                    }
                    else
                    {
                        Canvas.SetLeft(rectangle, block.a.Z * CANVAS_SCALE);
                        Canvas.SetTop(rectangle, block.a.Y * CANVAS_SCALE);
                    }
                }
                else if (shape.GetType() == typeof(Ellipse))
                {
                    Ellipse ellipse = (Ellipse)shape;
                    RayTracer.Sphere sphere = (RayTracer.Sphere)objects[ellipse.Name];
                    if (currentView == View.FRONT)
                    {
                        Canvas.SetLeft(ellipse, sphere.xPos);
                        Canvas.SetTop(ellipse, sphere.yPos);
                    }
                    else if (currentView == View.TOP)
                    {
                        Canvas.SetLeft(ellipse, sphere.xPos);
                        Canvas.SetTop(ellipse, sphere.zPos);
                    }
                    else
                    {
                        Canvas.SetLeft(ellipse, sphere.zPos);
                        Canvas.SetTop(ellipse, sphere.yPos);
                    }
                }

            }
        }



    }
}
