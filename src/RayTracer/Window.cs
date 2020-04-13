using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace RayTracer
{
    /// <summary>
    /// Represents window to show picture
    /// </summary>
    public partial class Window : Form
    {
        /// <summary>
        /// Pixels in picture.
        /// </summary>
        private byte[] pixels;

        /// <summary>
        /// Resolution.
        /// </summary>
        public Size Resolution
        {
            get;
            private set;
        }


        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="pictureWidth">Resolution - width.</param>
        /// <param name="pictureHeight">Resolution - height</param>
        public Window(int pictureWidth = 1280, int pictureHeight = 720)
        {
            InitializeComponent();
            Resolution = new System.Drawing.Size(pictureWidth, pictureHeight);
            pixels = new byte[4 * Resolution.Width * Resolution.Height];
            ShowImage();
        }



        /// <summary>
        /// Show image in the window.
        /// </summary>
        public void ShowImage()
        {
            this.pictureBox.Image = GetImage();
            this.pictureBox.Refresh();
        }


        /// <summary>
        /// Create bitmap of the image.
        /// </summary>
        /// <returns>Picture.</returns>
        private Bitmap GetImage()
        {
            Bitmap bmp = new Bitmap(Resolution.Width, Resolution.Height, PixelFormat.Format32bppArgb);

            BitmapData bmData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(pixels, 0, bmData.Scan0, bmData.Stride * bmData.Height);

            bmp.UnlockBits(bmData);

            return bmp;
        }


        /// <summary>
        /// Set pixel in the image.
        /// </summary>
        /// <param name="i">Index line.</param>
        /// <param name="j">Index column.</param>
        /// <param name="color">color.</param>
        /// <returns>if the pixel was set.</returns>
        public bool SetPixel(int i, int j, Color color)
        {
            if (j < 0 || j >= Resolution.Width || i < 0 || i >= Resolution.Height)
                return false;

            int index = 4 * (i * Resolution.Width + j);
            pixels[index] = (byte)(255 * color.b);
            pixels[index + 1] = (byte)(255 * color.g);
            pixels[index + 2] = (byte)(255 * color.r);
            pixels[index + 3] = (byte)(255 * color.a);

            return true;
        }


        /// <summary>
        /// Action for save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image (*.png)|*.png|All files|*.*";
            saveFileDialog.FileName = DateTime.Now.ToString().Replace('.', '-').Replace(':', '-');
            saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(saveFileDialog_FileOk);
            saveFileDialog.ShowDialog();
        }

        /// <summary>
        /// Action when select directory button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFileDialog saveFileDialog = (SaveFileDialog)sender;

            if (saveFileDialog.FileName.Length != 0)
            {
                Bitmap bmp = GetImage();
                bmp.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }

        }
    }
}
