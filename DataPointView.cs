using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Media;


namespace GrippingTest
{
    class DataPointView
    {
        private static SolidBrush textBrushBlack = new SolidBrush(Color.Black);
        private static Pen whitePen = new Pen(Color.White);
        private static Pen blackPen = new Pen(Color.Black);
        private static Brush pointBrush = new SolidBrush(Color.Blue);

        private int bufferSize { get; set; }
        private List<PointF> points { get; set; }

        private System.Windows.Forms.PictureBox pictureBox;

        private float timeSpan {get; set;}

        private System.Drawing.Bitmap bmp;

        private Graphics g;

        private float rangeY { get; set; }

        private float rangeX { get; set; }

        private float yPixelsPerTenKg;

        private float xPixelsPerSec;


        private int pointer { get; set; }

        private int screenCount;

        public DataPointView(System.Windows.Forms.Timer timer, int bufferSize, System.Windows.Forms.PictureBox picBox, float timeSpan)
        {
            this.bufferSize = bufferSize;
            this.pointer = 0;
            this.screenCount = 0;
            this.initList();
            this.initBmp();
            this.pictureBox = picBox;
            this.timeSpan = timeSpan;
            this.rangeX = 60;
            this.rangeY = 30;

            this.yPixelsPerTenKg = this.pictureBox.Height / this.rangeY * 10;
            this.xPixelsPerSec = this.pictureBox.Width / this.rangeX;
            
            timer.Tick += new EventHandler(refresh);
        }

        public void pushData(float data)
        {
            PointF point = new PointF(this.pointer * this.timeSpan,data);            
            this.points[this.pointer] = point;
            if (this.pointer == this.bufferSize - 1)
            {
                this.pointer = 0;
                this.screenCount++;
            }
            else
            {
                this.pointer++;
            }
        }

        private void refresh(Object myObject,
                                          EventArgs myEventArgs)
        {
            this.drawAxis();
            this.drawPoint();
            this.g.Flush();
            this.pictureBox.Image = this.bmp;
        }

        private void initBmp()
        {
            this.bmp = new System.Drawing.Bitmap(this.pictureBox.Width, this.pictureBox.Height);
            this.g = Graphics.FromImage(this.bmp);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TranslateTransform(25.0f, this.pictureBox.Height - 25f);
        }

        private void initList()
        {
            this.points = new List<PointF>();
            for (int i = 0; i < bufferSize; i++)
            {
                this.points.Add(new PointF(0,0));
            }
        }

        private void drawAxis()
        {
            g.Clear(Color.White);

            Font textFont = new Font("Arial", 8);
            
            //add axis
            g.DrawLine(blackPen, new Point(0, 0), new Point(this.pictureBox.Width - 35, 0));
            g.DrawLine(blackPen, new Point(0, 0), new Point(0, 35 - this.pictureBox.Height));
            //add text of axis
            g.DrawString("Time(s)", textFont, textBrushBlack, new Point(this.pictureBox.Width - 70, -25));
            g.DrawString("Force(kg)", textFont, textBrushBlack, new Point(10, 30 - this.pictureBox.Height));
            g.DrawString("0", textFont, textBrushBlack, new Point(-10, 0));
            //add graduation
            //y axis
            for (int i = 1; i <= 10; i++)
            {
                g.DrawLine(blackPen, new PointF(0, -i * this.yPixelsPerTenKg), new PointF(5, -i * this.yPixelsPerTenKg));
                g.DrawString((i * 10).ToString(), textFont, textBrushBlack, new PointF(-20, -i * this.yPixelsPerTenKg - 5));
            }
            //x axis
            for (int i = 1; i <= this.rangeX; i++)
            {
                g.DrawLine(blackPen, new PointF(this.xPixelsPerSec * i, 0), new PointF(this.xPixelsPerSec * i, -5));
                g.DrawString(this.rangeX*this.screenCount + i.ToString(), textFont, textBrushBlack, new PointF(this.xPixelsPerSec * i - 5, 0));
            }

        }

        private void drawPoint()
        {
            float x, y;
            for (int i = 0; i < this.points.Count; i++)
            {
                x = (points[i].X - this.screenCount * this.rangeX)*this.xPixelsPerSec;
                y = -points[i].Y/10f * this.yPixelsPerTenKg;
                g.FillEllipse(pointBrush, new RectangleF(new PointF(x - 1, y - 1), new SizeF(2.0F, 2.0F)));
            }          
        }
    }
}
