using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MandelbrotSetForm
{
    public partial class Form1 : Form
    {
        private static int height = 400;
        private static int width = 400;
        
        public Form1()
        {
            ClientSize = new Size(width, height);
            var image = new Bitmap(width, height);
            
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                var a = (i - width / 2) / ((double)width / 4);
                var b = (j - height / 2) / ((double)height / 4);
                image.SetPixel(i, j, MandelbrotSet.getColor(new MandelbrotSet.Z(a, b)));
            }
            
            var pb = new PictureBox()
            {
                Size = new Size(width, height),
                Image = image
            };
            
            Controls.Add(pb);
        }
    }
}