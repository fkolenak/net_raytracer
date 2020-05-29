using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UI
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private TextBox previousTextBox;
        private string previousString;
        private MainWindow mainWindow;
        public Settings()
        {
            InitializeComponent();
            
        }

        public Settings(MainWindow mainWindow): this()
        {
            if (mainWindow == null)
            {
                return;
            }
            this.mainWindow = mainWindow;
            CheckBox addFloor = (CheckBox)this.FindName("addFloor");

            TextBox lightPositionX = (TextBox)this.FindName("LightPositionX");
            TextBox lightPositionY = (TextBox)this.FindName("LightPositionY");
            TextBox lightPositionZ = (TextBox)this.FindName("LightPositionZ");
            TextBox CameraPositionX = (TextBox)this.FindName("CameraPositionX");
            TextBox CameraPositionY = (TextBox)this.FindName("CameraPositionY");
            TextBox CameraPositionZ = (TextBox)this.FindName("CameraPositionZ");
            TextBox CameraDirectionX = (TextBox)this.FindName("CameraDirectionX");
            TextBox CameraDirectionY = (TextBox)this.FindName("CameraDirectionY");
            TextBox CameraDirectionZ = (TextBox)this.FindName("CameraDirectionZ");
            TextBox CameraDirectionUpX = (TextBox)this.FindName("CameraDirectionUpX");
            TextBox CameraDirectionUpY = (TextBox)this.FindName("CameraDirectionUpY");
            TextBox CameraDirectionUpZ = (TextBox)this.FindName("CameraDirectionUpZ");
            TextBox fovy = (TextBox)this.FindName("FOVY");
            addFloor.IsChecked = mainWindow.addFloor;

            if (mainWindow.light != null)
            {
                RayTracer.Light light = mainWindow.light;
                lightPositionX.Text = light.xPos.ToString();
                lightPositionY.Text = light.yPos.ToString();
                lightPositionZ.Text = light.zPos.ToString();
            }
            if (mainWindow.camera != null)
            {
                RayTracer.Camera camera = mainWindow.camera;
                CameraPositionX.Text = camera.location.X.ToString();
                CameraPositionY.Text = camera.location.Y.ToString();
                CameraPositionZ.Text = camera.location.Z.ToString();
                CameraDirectionX.Text = camera.direction.x.ToString();
                CameraDirectionY.Text = camera.direction.y.ToString();
                CameraDirectionZ.Text = camera.direction.z.ToString();
                CameraDirectionUpX.Text = camera.up.z.ToString();
                CameraDirectionUpY.Text = camera.up.z.ToString();
                CameraDirectionUpZ.Text = camera.up.z.ToString();
                fovy.Text = camera.fovy.ToString();
            }
        }

        private void TextBoxGotFocusEventHandler(object sender, RoutedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            this.previousTextBox = (TextBox)sender;
            this.previousString = currentTextBox.Text;
        }

        private void DoubleNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {   // Only positive
            Regex regex = new Regex("[^0-9.-]+");
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
            Validate();
            this.previousTextBox = (TextBox)sender;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow = (MainWindow)this.Owner;
           
            if (Validate())
            {
                return;    
            } else
            {
                TextBox lightPositionX = (TextBox)this.FindName("LightPositionX");
                TextBox lightPositionY = (TextBox)this.FindName("LightPositionY");
                TextBox lightPositionZ = (TextBox)this.FindName("LightPositionZ");
                TextBox CameraPositionX = (TextBox)this.FindName("CameraPositionX");
                TextBox CameraPositionY = (TextBox)this.FindName("CameraPositionY");
                TextBox CameraPositionZ = (TextBox)this.FindName("CameraPositionZ");
                TextBox CameraDirectionX = (TextBox)this.FindName("CameraDirectionX");
                TextBox CameraDirectionY = (TextBox)this.FindName("CameraDirectionY");
                TextBox CameraDirectionZ = (TextBox)this.FindName("CameraDirectionZ");
                TextBox CameraDirectionUpX = (TextBox)this.FindName("CameraDirectionUpX");
                TextBox CameraDirectionUpY = (TextBox)this.FindName("CameraDirectionUpY");
                TextBox CameraDirectionUpZ = (TextBox)this.FindName("CameraDirectionUpZ");
                TextBox fovy = (TextBox)this.FindName("FOVY");
                try
                {
                    RayTracer.Light light = new RayTracer.Light(-1, new RayTracer.Point(
                        (float)double.Parse(lightPositionX.Text),
                        (float)double.Parse(lightPositionY.Text),
                        (float)double.Parse(lightPositionZ.Text)));
                    mainWindow.light = light;
                    RayTracer.Point point = new RayTracer.Point(
                        (float)double.Parse(CameraPositionX.Text),
                        (float)double.Parse(CameraPositionY.Text),
                        (float)double.Parse(CameraPositionZ.Text));

                    RayTracer.Vector direction = new RayTracer.Vector(
                        (float)double.Parse(CameraDirectionX.Text),
                        (float)double.Parse(CameraDirectionY.Text),
                        (float)double.Parse(CameraDirectionZ.Text));

                    RayTracer.Vector directionUp = new RayTracer.Vector(
                        (float)double.Parse(CameraDirectionUpX.Text),
                        (float)double.Parse(CameraDirectionUpY.Text),
                        (float)double.Parse(CameraDirectionUpZ.Text));

                    RayTracer.Camera camera = new RayTracer.Camera(-1, point, direction, directionUp, Double.Parse(fovy.Text));
                    mainWindow.camera = camera;

                    this.Close();
                }
                catch (Exception)
                {
                    Validate();
                }
            }
        }

        private bool Validate()
        {
            bool hasError = false;

            TextBox lightPositionX = (TextBox)this.FindName("LightPositionX");
            TextBox lightPositionY = (TextBox)this.FindName("LightPositionY");
            TextBox lightPositionZ = (TextBox)this.FindName("LightPositionZ");
            TextBox CameraPositionX = (TextBox)this.FindName("CameraPositionX");
            TextBox CameraPositionY = (TextBox)this.FindName("CameraPositionY");
            TextBox CameraPositionZ = (TextBox)this.FindName("CameraPositionZ");
            TextBox CameraDirectionX = (TextBox)this.FindName("CameraDirectionX");
            TextBox CameraDirectionY = (TextBox)this.FindName("CameraDirectionY");
            TextBox CameraDirectionZ = (TextBox)this.FindName("CameraDirectionZ");
            TextBox CameraDirectionUpX = (TextBox)this.FindName("CameraDirectionUpX");
            TextBox CameraDirectionUpY = (TextBox)this.FindName("CameraDirectionUpY");
            TextBox CameraDirectionUpZ = (TextBox)this.FindName("CameraDirectionUpZ");
            TextBox fovy = (TextBox)this.FindName("FOVY");

            if (lightPositionX.Text == "")
            {
                lightPositionX.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                lightPositionX.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            if (lightPositionY.Text == "")
            {
                lightPositionY.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                lightPositionY.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            if (lightPositionZ.Text == "")
            {
                lightPositionZ.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                lightPositionZ.BorderBrush = new SolidColorBrush(Colors.Black);
            }


            if (CameraPositionX.Text == "")
            {
                CameraPositionX.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraPositionX.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            if (CameraPositionY.Text == "")
            {
                CameraPositionY.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraPositionY.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            if (CameraPositionZ.Text == "")
            {
                CameraPositionZ.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraPositionZ.BorderBrush = new SolidColorBrush(Colors.Black);
            }


            if (CameraDirectionX.Text == "")
            {
                CameraDirectionX.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraDirectionX.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (CameraDirectionY.Text == "")
            {
                CameraDirectionY.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraDirectionY.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (CameraDirectionZ.Text == "")
            {
                CameraDirectionZ.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraDirectionZ.BorderBrush = new SolidColorBrush(Colors.Black);
            }


            if (CameraDirectionUpX.Text == "")
            {
                CameraDirectionUpX.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraDirectionUpX.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            if (CameraDirectionUpY.Text == "")
            {
                CameraDirectionUpY.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraDirectionUpY.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            if (CameraDirectionUpZ.Text == "")
            {
                CameraDirectionUpZ.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                CameraDirectionUpZ.BorderBrush = new SolidColorBrush(Colors.Black);
            }


            if (fovy.Text == "")
            {
                fovy.BorderBrush = new SolidColorBrush(Colors.Red);
                hasError = true;
            }
            else
            {
                fovy.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            return hasError;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            this.mainWindow.addFloor = (bool)checkBox.IsChecked;
        }
    }
}
