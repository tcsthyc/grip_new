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
using NCalc;


namespace GrippingTest
{
    class DataPointView
    {
        private static SolidBrush textBrushBlack = new SolidBrush(Color.Black);
        private static Pen whitePen = new Pen(Color.White);
        private static Pen blackPen = new Pen(Color.Black);
        private static Brush pointBrush = new SolidBrush(Color.Blue);
        private static Brush assistLineBrush = new SolidBrush(Color.LightGray);

        private int bufferSize { get; set; }
        private List<PointF> points { get; set; }

        private System.Windows.Forms.PictureBox pictureBox;

        private float timeSpan {get; set;}

        private System.Drawing.Bitmap bmp;

        private Graphics g;

        private DataPointViewConfig config;

        private float yPixelsPerTenKg;

        private float xPixelsPerSec;

        private float paddingRight = 35;
        private float paddingTop = 35;
        private float paddingBottom = 25;
        private float paddingLeft = 25;


        private int pointer { get; set; }

        private int screenCount;

        private List<List<PointF>> strategyCache;

        public DataPointView(System.Windows.Forms.Timer timer, System.Windows.Forms.PictureBox picBox, float timeSpan, DataPointViewConfig config)
        {
            this.pointer = 0;
            this.screenCount = 0;
            this.strategyCache = new List<List<PointF>>();
            
            this.pictureBox = picBox;
            this.timeSpan = timeSpan;
            this.config = config;
            this.bufferSize = (int)(this.config.rangeX / timeSpan);
            this.yPixelsPerTenKg = (this.pictureBox.Height - this.paddingTop - this.paddingBottom) / this.config.rangeY * 10;
            this.xPixelsPerSec = (this.pictureBox.Width - this.paddingLeft - this.paddingRight) / this.config.rangeX;

            this.initList();
            this.calAssistLineCache();          
            this.initBmp();
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
                this.calAssistLineCache();
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
            this.drawAssistLine();
            this.drawPoint();
            
            this.g.Flush();
            this.pictureBox.Image = this.bmp;
        }

        private void initBmp()
        {
            this.bmp = new System.Drawing.Bitmap(this.pictureBox.Width, this.pictureBox.Height);
            this.g = Graphics.FromImage(this.bmp);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TranslateTransform(this.paddingLeft, this.pictureBox.Height - this.paddingBottom);
        }

        private void initList()
        {
            this.points = new List<PointF>();
            for (int i = 0; i < bufferSize; i++)
            {
                this.points.Add(new PointF(0,0));
            }
        }

        private void drawAssistLine()
        {
            foreach (List<PointF> line in strategyCache)
            {
                foreach (PointF point in line)
                {
                    g.FillEllipse(assistLineBrush, new RectangleF(point, new SizeF(2.0F, 2.0F)));
                }
            }
        }

        private void calAssistLineCache()
        {
            if(config.alStratogy == null || config.alStratogy.sections.Count == 0) return;

            float tPerPixel = 1 / xPixelsPerSec;
            float startTime = config.rangeX * screenCount;

            strategyCache.Clear();
            for(int lineCount=0;lineCount<config.alStratogy.sections.Count;lineCount++){
                try{
                    List<PointF> assistLineCacheList = new List<PointF>();
                    AssistLineSecion assistLine = config.alStratogy.sections[lineCount];
                    TimePeriod tp = joinTime(new TimePeriod(assistLine.start,assistLine.end), new TimePeriod(config.rangeX * screenCount, config.rangeX *(screenCount + 1)));
                    if (tp == null) continue;
                    int startPixel = (int)((tp.start - config.rangeX * screenCount) * xPixelsPerSec);
                    int endPixel = (int)((tp.end - config.rangeX * screenCount) * xPixelsPerSec);
                    String expressionText = assistLine.expression;
                    
                    for (float i = startPixel; i < endPixel; i += assistLine.xPixelSpan)
                    {
                        float realTime = startTime + i/xPixelsPerSec;
                        Expression ex = new Expression(expressionText.Replace("t", realTime.ToString()));
                        assistLineCacheList.Add(new PointF(i,-yPixelsPerTenKg/10 * float.Parse(ex.Evaluate().ToString())));
                    }
                    strategyCache.Add(assistLineCacheList);
                }
                catch{

                }
            }
        }
        private void drawAxis()
        {
            g.Clear(Color.White);

            Font textFont = new Font("Arial", 8);
            
            //add axis
            g.DrawLine(blackPen, new PointF(0, 0), new PointF(this.pictureBox.Width - this.paddingRight, 0));
            g.DrawLine(blackPen, new PointF(0, 0), new PointF(0, this.paddingTop - this.pictureBox.Height));
            //add text of axis
            g.DrawString("Time(s)", textFont, textBrushBlack, new PointF(this.pictureBox.Width - 70, -this.paddingLeft));
            g.DrawString("Force(kg)", textFont, textBrushBlack, new Point(10, 30 - this.pictureBox.Height));
            g.DrawString("0", textFont, textBrushBlack, new Point(-10, 0));
            //add graduation
            //y axis
            for (int i = 1; i <= this.config.rangeY/10; i++)
            {
                g.DrawLine(blackPen, new PointF(0, -i * this.yPixelsPerTenKg), new PointF(5, -i * this.yPixelsPerTenKg));
                g.DrawString((i * 10).ToString(), textFont, textBrushBlack, new PointF(-20, -i * this.yPixelsPerTenKg - 5));
            }

            int realTime;
            //x axis
            for (int i = 1; i <= this.config.rangeX; i++)
            {
                g.DrawLine(blackPen, new PointF(this.xPixelsPerSec * i, 0), new PointF(this.xPixelsPerSec * i, -5));

                realTime = this.config.rangeX*this.screenCount + i;
                if (config.showXText)
                {
                    g.DrawString(realTime.ToString(), textFont, textBrushBlack, new PointF(this.xPixelsPerSec * i - 5 - ((int)(Math.Floor(Math.Log10(realTime)) + 1)) * 2.5f, 0));
                }
            }

        }

        private void drawPoint()
        {
            float x, y;
            for (int i = 0; i < this.points.Count; i+=10)
            {
                x = points[i].X*this.xPixelsPerSec;
                y = -points[i].Y/10f * this.yPixelsPerTenKg;
                g.FillEllipse(pointBrush, new RectangleF(new PointF(x - 1, y - 1), new SizeF(2.0F, 2.0F)));
            }          
        }

        public TimePeriod joinTime(TimePeriod tp1, TimePeriod tp2)
        {
            if (tp1.start >= tp1.end || tp2.start>=tp2.end) return null;

            float start = Math.Max(tp1.start, tp2.start);
            float end = Math.Min(tp1.end, tp2.end);

            if (start >= end) return null;
            return new TimePeriod(start, end);
        }
    }
}
