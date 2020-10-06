using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FSharp.Core;

namespace MandelbrotSetForm
{
    public partial class Form1 : Form
    {
        private static int height = 400;
        private static int width = 400;
        private static int clientWidth = 500;

        private static PictureBox pictureBox;

        public Form1()
        {
            ClientSize = new Size(clientWidth, height);
            
            var currentImage = new CurrentImage(width, height);

            pictureBox = new PictureBox();
            pictureBox.Size = new Size(width, height);
            pictureBox.Image = currentImage.Image;

            Button plusButton = new Button();
            plusButton.Location = new Point(clientWidth - plusButton.Size.Width, 0);
            plusButton.Text = "+";
            plusButton.Click += (sender, e) => currentImage.ZoomIn();

            Button minusButton = new Button();
            minusButton.Location = new Point(clientWidth - minusButton.Size.Width, 
                                             plusButton.Location.Y + plusButton.Size.Height + 10);
            minusButton.Text = "-";
            minusButton.Click += (sender, e) => currentImage.ZoomOut();
            
            Button upButton = new Button();
            upButton.Location = new Point(clientWidth - minusButton.Size.Width, 
                                          minusButton.Location.Y + minusButton.Size.Height + 10);
            upButton.Text = "Up";
            upButton.Click += (sender, e) => currentImage.GoUp();

            Button downButton = new Button();
            downButton.Location = new Point(clientWidth - minusButton.Size.Width, 
                                            upButton.Location.Y + upButton.Size.Height + 10);
            downButton.Text = "Down";
            downButton.Click += (sender, e) => currentImage.GoDown();
            
            Button rightButton = new Button();
            rightButton.Location = new Point(clientWidth - minusButton.Size.Width, 
                                             downButton.Location.Y + downButton.Size.Height + 10);
            rightButton.Text = "Rigth";
            rightButton.Click += (sender, e) => currentImage.GoRight();

            Button leftButton = new Button();
            leftButton.Location = new Point(clientWidth - minusButton.Size.Width, 
                                            rightButton.Location.Y + rightButton.Size.Height + 10);
            leftButton.Text = "Left";
            leftButton.Click += (sender, e) => currentImage.GoLeft();
            
            Controls.Add(plusButton);
            Controls.Add(minusButton);
            Controls.Add(upButton);
            Controls.Add(downButton);
            Controls.Add(rightButton);
            Controls.Add(leftButton);
            Controls.Add(pictureBox);
        }

        class CurrentImage
        {
            public Bitmap Image;
            private Point center;
            private double scale;

            private static int movingStep = 5;
            private static double scalingStep = 2;

            public CurrentImage(int width, int height)
            {
                Image = new Bitmap(width, height);
                center = new Point(width / 2, height / 2);
                scale = 1;
                Update();
            }

            public void Update()
            {
                for (var i = 0; i < Image.Width; i++)
                for (var j = 0; j < Image.Height; j++)
                {
                    var x = center.X + (i - center.X) * scale;
                    var y = center.Y + (j - center.Y) * scale;
                    var a = (x - (double)Image.Width / 2) / ((double)Image.Width / 4);
                    var b = (y - (double)Image.Height / 2) / ((double)Image.Height / 4);
                    Image.SetPixel(i, j, MandelbrotSet.getColor(new MandelbrotSet.Z(a, b)));
                }
            }
            
            public void GoUp()
            {
                center.Y -= movingStep;
                Update();
                pictureBox.Image.Dispose();
            }
            
            public void GoDown()
            {
                center.Y += movingStep;
                Update();
                pictureBox.Image.Dispose();
            }
            
            public void GoRight()
            {
                center.X += movingStep;
                Update();
                pictureBox.Image.Dispose();
            }
            
            public void GoLeft()
            {
                center.X -= movingStep;
                Update();
                pictureBox.Image.Dispose();
            }

            public void ZoomIn()
            {
                scale *= scalingStep;
                Update();
                pictureBox.Image.Dispose();
            }
            
            public void ZoomOut()
            {
                if (scale > 1)
                {
                    scale /= scalingStep;
                    Update();
                    pictureBox.Image.Dispose();
                }
            }
        }
    }
}