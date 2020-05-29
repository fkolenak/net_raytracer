using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Globalization;
using System.Linq;

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
        public RayTracer.Light light;
        public RayTracer.Camera camera;
        public bool addFloor = false;

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
            light = new RayTracer.Light(1, new RayTracer.Point(-15, 10, 20));
            RayTracer.Point point = new RayTracer.Point(5.3368f, 8.0531f, 9.8769f);

            RayTracer.Vector direction = new RayTracer.Vector(-0.38363f, -0.42482f, -0.82f);

            RayTracer.Vector directionUp = new RayTracer.Vector(-0.16485f, 0.90515f, -0.391826f);

            this.camera = new RayTracer.Camera(-1, point, direction, directionUp, 60f);


            Button front = (Button)this.FindName("viewButtonFront");
            front.Foreground = new SolidColorBrush(Colors.Red);

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
        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            Window settings = new Settings(this);
            settings.Owner = this;
            settings.ShowDialog();
        }

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
        private void renderButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if ((string)button.Content == "Render")
            {
                RayTracer.Scene scene = CreateScene();
                if (addFloor)
                {
                    RayTracer.Floor floor = new RayTracer.Floor(1, new RayTracer.Point(-10, 0, -10), new RayTracer.Point(10, 0, 10));
                    floor.SetProperties(0.6f, 0.4f, 0);
                    floor.SetColors(new RayTracer.Color(0,0,0), new RayTracer.Color(1,1,1));
                    scene.allObjects.Add(floor);
                }
                RayTracer.Renderer renderer = new RayTracer.Renderer(scene, scene.allObjects);
                Console.WriteLine("Raytracing started.");
                int time = Environment.TickCount;
                RayTracer.RenderWindow window = renderer.Render();
                time = -time + Environment.TickCount;
                Console.WriteLine("Raytracing finished.");
                window.ShowImage();
                Console.WriteLine("Intersection calculating time: \t" + scene.intersectionCalculationCount + "\nRender time: \t\t" + time + "ms");
                window.ShowDialog();

                button.Content = "Cancel";
            } else
            {
                button.Content = "Render";
            }
        }

        private RayTracer.Scene CreateScene()
        {
            RayTracer.Scene scene =  new RayTracer.Scene();

            scene.SetLight(this.light);
            scene.SetCamera(this.camera);
            scene.allObjects = this.objects.Values.ToList();
            return scene;
        }

        private void viewButtonFront_Click(object sender, RoutedEventArgs e)
        {
            if (currentView != View.FRONT)
            {
                ResetButtonsFont();
                Button button = (Button)sender;
                button.Foreground = new SolidColorBrush(Colors.Red);
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
                ResetButtonsFont();
                Button button = (Button)sender;
                button.Foreground = new SolidColorBrush(Colors.Red);
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
                ResetButtonsFont();
                Button button = (Button)sender;
                button.Foreground = new SolidColorBrush(Colors.Red);
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

        private void ResetButtonsFont()
        {
            Button front = (Button)this.FindName("viewButtonFront");
            Button top = (Button)this.FindName("viewButtonTop");
            Button right = (Button)this.FindName("viewButtonRight");

            front.Foreground = new SolidColorBrush(Colors.Black);
            top.Foreground = new SolidColorBrush(Colors.Black);
            right.Foreground = new SolidColorBrush(Colors.Black);
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
                
                positionX.Text = block.a.X.ToString().Replace(',', '.');
                positionY.Text = block.a.Y.ToString().Replace(',', '.');
                positionZ.Text = block.a.Z.ToString().Replace(',', '.');
                width.Text = block.getWidth().ToString().Replace(',', '.');
                height.Text = block.getHeight().ToString().Replace(',', '.');
                depth.Text = block.getDepth().ToString().Replace(',', '.');
            }
            else if (shape.GetType() == typeof(Ellipse))
            {
                Ellipse ellipse = (Ellipse)shape;
                TextBox diameter;

                diameter = (TextBox)this.FindName("diameter");

                UpdatePositionValues(shape);

                RayTracer.Sphere sphere = (RayTracer.Sphere)objects[ellipse.Name];                
                positionX.Text = sphere.xPos.ToString().Replace(',', '.');
                positionY.Text = sphere.yPos.ToString().Replace(',', '.');
                positionZ.Text = sphere.zPos.ToString().Replace(',', '.');
                diameter.Text = sphere.diameter.ToString().Replace(',', '.');
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
            r.Text = (aObject.color.r * 256).ToString().Replace(',', '.');
            g.Text = (aObject.color.g * 256).ToString().Replace(',', '.');
            b.Text = (aObject.color.b * 256).ToString().Replace(',', '.');
            diffusion.Text = aObject.kd.ToString().Replace(',', '.');
            reflection.Text = aObject.ks.ToString().Replace(',', '.');
            transparency.Text = aObject.kt.ToString().Replace(',', '.');
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

        private void PositionTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            bool changed = false;
            if (this.previousTextBox == currentTextBox)
            {
                try
                {
                    Double.Parse(currentTextBox.Text, CultureInfo.InvariantCulture);
                    if (currentTextBox.Text.Contains(','))
                    {
                        currentTextBox.Text = currentTextBox.Text.Replace(',', '.');
                    }
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
                    double value = Double.Parse(currentTextBox.Text, CultureInfo.InvariantCulture) * CANVAS_SCALE;
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
                        selected.Width = selected.Height = Double.Parse(currentTextBox.Text, CultureInfo.InvariantCulture) * CANVAS_SCALE;
                    }
                    UpdatePositionValues(selected);
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
                    if (currentTextBox.Text.Contains(','))
                    {
                        currentTextBox.Text = currentTextBox.Text.Replace(',', '.');
                    }
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
                        aObject.SetColor((float)(colorR / 256.0), (float)(colorG / 256.0), (float)(colorB / 256.0));
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
