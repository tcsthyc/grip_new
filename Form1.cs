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
    public partial class Form1 : Form
    {
        private bool connected;
        private bool test2connected;       
        private SerialPort sp;
        private String portName;
        public delegate void updateTextBox(char[] data);
        public delegate void ReceiveDataOfTest2(char[] data);
        public delegate void ReceiveDataOfTest3(char[] data);
        public static ArrayList  dataRecord;
       // private ArrayList DataOfTest2;
        //private ArrayList pointSet;
        public String tempString;
        private Bitmap bmp;
        private string userName;        
        private Graphics g;       
        private float maxForce;
        private int startTimeCount;
        private int maxForceTimeCount;
        private float clipX;
        private float clipY;
        private float averageMVC;       
        private ArrayList MVC_Record;
        private bool isReceiving; 
        private bool portAndNameSetCompleted;
        private float maxForceOfTest2;
        private float minForceOfTest2;
        private int maxForceOfTest2TimeCount;
        private float deltaPercentage;
        private ArrayList perRecord;
        private ArrayList Test3MVCRecord;
        private bool test3connected;
        private float test3_MVC;
        private float pointsPerSecond;
        public static float dataTimeSpan=6.249f/1000;
        private bool test1StartSoundPlayed;
        private bool test2StartSoundPlayed;
        private bool test3StartSoundPlayed;
        private System.Timers.Timer Test2EndTimer = new System.Timers.Timer();

        public static Int32 baudRate;       
        public static Parity parity ;
        public static Int32 dataBits ;
        public static StopBits stopBits;
        public static ArrayList DataOfTest2;
        public static ArrayList DataOfTest3;
        public static int ViewTestDataNum;

        private int drawTime;
        private int imgWidth;
        private int imgHeight;
        private int x_pixelsPerSecond;
        private int y_pixelsEveryTenKg;

        public Form1()
        {
            InitializeComponent();          
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            test3_MVC = 0f;
            userName = "";
            maxForceTimeCount = 0;
            startTimeCount = 0;
            averageMVC = 0.0f;

            dataRecord = new ArrayList();
            DataOfTest2 = new ArrayList();
            DataOfTest3 = new ArrayList();
            Test3MVCRecord = new ArrayList();
            MVC_Record = new ArrayList();
            perRecord = new ArrayList();
          //  pointSet = new ArrayList();
           
            isReceiving = false;  

            baudRate = 9600;
            portName = "";
            parity = Parity.None;
            dataBits = 8;
            stopBits = StopBits.One;

            clipX = 0.0f;
            clipY = 400.0f;
            maxForce = 0.0F;
            
            tempString = "";
            
            connected = false;
            test2connected = false;
            test3connected = false;
            comboBox_portName.Items.Clear();
            sp = new SerialPort();
            sp.ReadTimeout = 1000;

            tabControl_MVCTest.Enabled = false;
            portAndNameSetCompleted = false;


            drawTime = 60;
            x_pixelsPerSecond = 300;
            y_pixelsEveryTenKg = 80;
            pointsPerSecond = 1/dataTimeSpan;
            imgWidth = x_pixelsPerSecond * drawTime + 50;
            imgHeight = y_pixelsEveryTenKg * 10 + 50;
            bmp = new Bitmap(imgWidth, imgHeight);
            g = Graphics.FromImage(bmp);            
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;         
            g.TranslateTransform(25.0f, imgHeight-25f);
            drawAxis(g);           
            getBmpToShow(bmp, clipX, clipY);



            try
            {
                string[] str = SerialPort.GetPortNames();           
                if (str.Equals(null))
                {
                    MessageBox.Show("本机没有串口！", "Error");
                    return;
                }
                if (str == null)
                {
                    MessageBox.Show("本机没有串口！", "Error");
                    return;
                }
               /* Console.WriteLine("test");
                foreach (string s in str)
                {
                    Console.WriteLine(s);
                }*/
           
                comboBox_portName.Items.AddRange(str);
                if (comboBox_portName.Items.Count > 0)
                {
                    comboBox_portName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常！！！！！！！");
                Console.WriteLine(ex.ToString());
            }

        }

      

        private void setSerialPort()
        {
           /* if (comboBox_portName.Text == "")
            {
                MessageBox.Show("未选择串口！", "Error");
                return;
            }
            else
            {
                portName = comboBox_portName.Text;
            }*/

            sp.PortName = portName;
            sp.BaudRate = baudRate;
            sp.Parity = parity;
            sp.DataBits = dataBits;
            sp.StopBits = stopBits;
            sp.ReceivedBytesThreshold = 10;
            sp.RtsEnable = true;
            sp.DtrEnable = true;
            //sp.Handshake = Handshake.XOnXOff;
        }  

        private void getBmpToShow(Bitmap resourceBmp,float x,float y)
        {
            System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
            RectangleF cloneRect = new RectangleF(x,y,650f,450f);
            pictureBox_data.Image=resourceBmp.Clone(cloneRect,format);
        }       

       /* private void showCurve()
        {
            PointF[] allPoints=(PointF[])pointSet.ToArray(typeof(PointF));
            GraphicsPath gp = new GraphicsPath();
            Pen redPen = new Pen(Color.Red);
            try
            {
                gp.AddCurve(allPoints, 0.7f);
                g.DrawPath(redPen, gp);
                //g.Flush();
                getBmpToShow(bmp, clipX, clipY);
            }
            catch
            {
            }
        }*/

        private void  sp_DataReceived(object sender,SerialDataReceivedEventArgs e)
        {
            if (!test1StartSoundPlayed)
            {
                playSound();
                test1StartSoundPlayed = true;
            }
            isReceiving = true;
            char[] data = new char[sp.BytesToRead];
            sp.Read(data, 0, data.Length);   
            
            
            updateTextBox utb = new updateTextBox(addData);
            this.Invoke(utb,new object[]{data});
            isReceiving = false;
        }

        private void sp_DataOfTest2Received(object sender, SerialDataReceivedEventArgs e)
        {
            if (!test2StartSoundPlayed)
            {
                playSound();
                test2StartSoundPlayed = true;
            }
            isReceiving = true;
            char[] data = new char[sp.BytesToRead];
            sp.Read(data, 0, data.Length);


            ReceiveDataOfTest2 rdt = new ReceiveDataOfTest2(ReadDataOfTest2);
            this.Invoke(rdt, new object[] { data });
            isReceiving = false;
        }

        private void sp_DataOfTest3Received(object sender, SerialDataReceivedEventArgs e)
        {
            if (!test3StartSoundPlayed)
            {
                playSound();
                test3StartSoundPlayed = true;
            }
            isReceiving = true;
            char[] data = new char[sp.BytesToRead];
            sp.Read(data, 0, data.Length);


            ReceiveDataOfTest3 rdt3 = new ReceiveDataOfTest3(ReadDataOfTest3);
            this.Invoke(rdt3, new object[] { data });
            isReceiving = false;
        }
      
        public void addData(char[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
              //  textBox1.Text += data[i];
                if (data[i] != '\r' && data[i] != '\n')
                {
                    tempString += data[i];
                }
                else if (data[i] == '\n')
                {
                    float value;
                    try
                    {
                        value = float.Parse(tempString);
                    }
                    catch
                    {
                        tempString = "";
                        continue;
                    }
                    //dataRecord.Add(tempString);
                    dataRecord.Add(value);
                    label_StrengthData.Text = tempString;
                    drawPoint(g,value,dataRecord.Count);
                    tempString = "";
                    if (value > 1 && startTimeCount == 0)// 1为判定的最小值，可调为2
                    {
                        startTimeCount = dataRecord.Count-1;
                    }
                    if (value > maxForce)
                    {
                        maxForce = value;
                        label_MaxForce.Text = value.ToString(); //记录最大力的值
                        maxForceTimeCount = dataRecord.Count-1; //记录最大力出现的时间
                       
                    }
                }
            }
        }

        public void ReadDataOfTest2(char[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != '\r' && data[i] != '\n')
                {
                    tempString += data[i];
                }
                else if (data[i] == '\n')
                {
                    float value;
                    try
                    {
                        value = float.Parse(tempString);
                    }
                    catch
                    {
                        tempString = "";
                        continue;
                    }
                    //dataRecord.Add(tempString);
                    
                    DataOfTest2.Add(value);
                    drawPoint(g, value, DataOfTest2.Count);
                
                    tempString = "";
                    if (value > maxForceOfTest2)
                    {
                        maxForceOfTest2 = value;
                        label_Test2_Maxforce .Text= value.ToString();
                        maxForceOfTest2TimeCount = DataOfTest2.Count-1;
                        label_Test2_MaxForceTime.Text = ((maxForceOfTest2TimeCount-1) * dataTimeSpan).ToString("G5");
                        label_Test2_EndTime.Text = ((maxForceOfTest2TimeCount - 1) * dataTimeSpan+21).ToString("G5"); 
                        Test2EndTimer.Interval = 21000;
                    }
                    
                }
            }
        }

        public void ReadDataOfTest3(char[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != '\r' && data[i] != '\n')
                {
                    tempString += data[i];
                }
                else if (data[i] == '\n')
                {
                    float value;
                    try
                    {
                        value = float.Parse(tempString);
                    }
                    catch
                    {
                        tempString = "";
                        continue;
                    }                    

                    DataOfTest3.Add(value);
                    drawPoint(g, value, DataOfTest3.Count);
                    label_Test3_RealTimeForce.Text = tempString;

                    tempString = "";
                    if (value > test3_MVC)
                    {
                        test3_MVC = value;
                        label_Test3_MVC.Text = value.ToString();                        
                    }

                }
            }
        }       

        private void drawPoint(Graphics g,float value,int count)
        {
            if (count > pointsPerSecond*drawTime)
            {
                return;
            }
            float y=-value /10f* y_pixelsEveryTenKg;
            float x=(float)(count-1)*(x_pixelsPerSecond*1.0f/pointsPerSecond);
           // pointSet.Add(new PointF(x,y));

            Brush pointBrush = new SolidBrush(Color.Blue); 
            g.FillEllipse(pointBrush, new RectangleF(new PointF(x - 1, y - 1), new SizeF(2.0F, 2.0F)));
            g.Flush();

            if(x+25>clipX+650&&x+25<=imgWidth-650)
            {
                hScrollBar1.Value = (int)((x+25) / (imgWidth-650f) * (hScrollBar1.Maximum + 1 - hScrollBar1.LargeChange));
            }
            else if(x+25>imgWidth-650){
                hScrollBar1.Value = hScrollBar1.Maximum + 1 - hScrollBar1.LargeChange;
                getBmpToShow(bmp, clipX, clipY);
            }
            else
            {
                getBmpToShow(bmp,clipX,clipY);
            }
           
        }

        private void drawAxis(Graphics g)
        {
            g.Clear(Color.White);

            Font textFont = new Font("Arial", 8);
            SolidBrush textBrush=new SolidBrush(Color.Black);
            Pen whitePen = new Pen(Color.White);
            Pen blackPen = new Pen(Color.Black);
            //add axis
            g.DrawLine(blackPen, new Point(0, 0), new Point(imgWidth-35, 0));
            g.DrawLine(blackPen, new Point(0, 0), new Point(0, 35-imgHeight));
            //add text of axis
            g.DrawString("Time(s)", textFont, textBrush,new Point(imgWidth-70,-25));
            g.DrawString("Force(kg)",textFont,textBrush,new Point(10,30-imgHeight));
            g.DrawString("0",textFont,textBrush,new Point(-10,0));
            //add graduation
            //y axis
            for (int i = 1; i <= 10; i++)
            {
                g.DrawLine(blackPen, new Point(0, -i *y_pixelsEveryTenKg ), new Point(5, -i * y_pixelsEveryTenKg));
                g.DrawString((i * 10).ToString(), textFont,textBrush,new Point(-20,-i*y_pixelsEveryTenKg-5));
            }            
            //x axis
            for (int i = 1; i <= drawTime; i++)
            {
                g.DrawLine(blackPen, new Point(x_pixelsPerSecond * i, 0), new Point(x_pixelsPerSecond * i, -5));
                g.DrawString(i.ToString(), textFont, textBrush, new Point(x_pixelsPerSecond * i-5, 0));
            }

           
        }
       
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {   
            float percentage = hScrollBar1.Value * 1.0f / (hScrollBar1.Maximum+1-hScrollBar1.LargeChange);
            clipX = (imgWidth-650) * percentage;  
            getBmpToShow(bmp, clipX, clipY);
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            float percentage = vScrollBar1.Value * 1.0f / (vScrollBar1.Maximum + 1 - vScrollBar1.LargeChange);
            clipY = (imgHeight-450) * percentage;
            getBmpToShow(bmp, clipX, clipY);
        }

        private void 串口设置ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Settings settingForm = new Settings();
           // settingForm.Show();
            settingForm.ShowDialog();
        }

        private void Btn_restartTest1_Click(object sender, EventArgs e)
        {
            MVC_Record.Clear();
            //MVCForTest2 = averageMVC;
            drawAxis(g);
            getBmpToShow(bmp, clipX, clipY);
           // pointSet.Clear();
            dataRecord.Clear();
            maxForce = 0.00f;
            label_MaxForce.Text = "0.00";
            label_TestTimes1.Text = "0";
            label1_AverageMVC.Text = "0.0000";
            label_StrengthData.Text = "0.00";
            label_MaxForceChangingSpeed.Text = "0";
            label_TimeToMVC.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                drawAxis(g);
                getBmpToShow(bmp, clipX, clipY);
                hScrollBar1.Value = 0;
                vScrollBar1.Value = vScrollBar1.Maximum + 1 - vScrollBar1.LargeChange;
                //pointSet.Clear();
                dataRecord.Clear();
                maxForce = 0.00f;
                label_MaxForce.Text = "0.00";
                label_MaxForceChangingSpeed.Text = "0";
                label_TimeToMVC.Text = "0";
                startTimeCount = 0;
                test1StartSoundPlayed = false;

                if (sp.IsOpen)
                {
                    int i = Environment.TickCount;
                    while (Environment.TickCount - i < 2000 && isReceiving)
                    {

                        Application.DoEvents();
                    }
                    sp.Close();
                }
                try
                {
                    setSerialPort();
                    sp.DataReceived += sp_DataReceived;
                    sp.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "异常");
                    return;
                }
                connected = true;
                btn_test1_switch.Text = "停止(&S)";

                comboBox_portName.Enabled = false;
                Btn_restartTest1.Enabled = false;
                btn_test2.Enabled = false;
                btn_setting.Enabled = false;
                button_Test3_Switch.Enabled = false; ;

            }
            else
            {
                sp.DataReceived -= sp_DataReceived;
                int j = Environment.TickCount;
                while (Environment.TickCount - j < 2000 && isReceiving)
                {

                    Application.DoEvents();
                }
                sp.Close();
                connected = false;

                hScrollBar1.Value = 0;
                vScrollBar1.Value = vScrollBar1.Maximum + 1 - vScrollBar1.LargeChange;

                label_TimeToMVC.Text = ((maxForceTimeCount - startTimeCount) * 1000/pointsPerSecond).ToString();
                label_MaxForceChangingSpeed.Text = calMaxChangingSpeed().ToString();

               // showCurve();

                DialogResult result = MessageBox.Show("是否保存此次数据？", "提示", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    MVC_Record.Add(maxForce);
                    label_TestTimes1.Text = MVC_Record.Count.ToString();
                    averageMVC = 0.0f;
                    for (int i = 0; i < MVC_Record.Count; i++)
                    {
                        averageMVC += (float)MVC_Record[i];
                    }
                    if (MVC_Record.Count != 0)
                    {
                        averageMVC /= MVC_Record.Count;
                    }
                    label1_AverageMVC.Text = averageMVC.ToString("G5");
                    saveData();
                }
                else
                {
                    //do nothing
                }
                dataRecord.Clear();


                comboBox_portName.Enabled = true;
                btn_test1_switch.Text = "读取数据(&D)";
                Btn_restartTest1.Enabled = true;
                btn_test2.Enabled = true;
                btn_setting.Enabled = true;
                button_Test3_Switch.Enabled = true;

            }


        }

        private void btn_test2_Click(object sender, EventArgs e)
        {
            if (!test2connected)
            {
                drawAxis(g);
                getBmpToShow(bmp, clipX, clipY);
                hScrollBar1.Value = 0;
                vScrollBar1.Value = vScrollBar1.Maximum + 1 - vScrollBar1.LargeChange;
                DataOfTest2.Clear();
                maxForceOfTest2 = 0.0f;
                label_Test2_Maxforce.Text = "0.00";
                minForceOfTest2 = 0.0f;
                maxForceOfTest2TimeCount = 0;
                label_Test2_EndTime.Text = "0";
                label_Test2_MaxForceTime.Text = "0";
                test2StartSoundPlayed = false;

                Test2EndTimer = new System.Timers.Timer();
                Test2EndTimer.AutoReset = false;
                Test2EndTimer.Enabled = true;
                Test2EndTimer.Interval = 21000;
                Test2EndTimer.Elapsed+=Test2EndTimer_Elapsed;

                if (sp.IsOpen)
                {
                    int i = Environment.TickCount;
                    while (Environment.TickCount - i < 2000 && isReceiving)
                    {

                        Application.DoEvents();
                    }
                    sp.Close();
                }
                try
                {
                    setSerialPort();                   
                    sp.DataReceived += sp_DataOfTest2Received;
                    sp.Open();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                    return;
                }
                test2connected = true;
                Btn_restartTest1.Enabled = false;
                btn_test1_switch.Enabled = false;
                btn_setting.Enabled = false;
                button_Test3_Switch.Enabled = false;
                btn_test2.Text = "停止";
            }
            else
            {
                stopTest2();
                DataOfTest2.Clear();
            }       


        }

        private void button_Test3_Switch_Click(object sender, EventArgs e)
        {
            if (!test3connected)
            {
                drawAxis(g);
                getBmpToShow(bmp, clipX, clipY);
                hScrollBar1.Value = 0;
                vScrollBar1.Value = vScrollBar1.Maximum + 1 - vScrollBar1.LargeChange;
                DataOfTest3.Clear();
                label_Test3_MVC.Text = "0.00";
                label_Test3_RealTimeForce.Text = "0.00";
                test3_MVC = 0f;
                test3StartSoundPlayed = false;

                if (sp.IsOpen)
                {
                    int i = Environment.TickCount;
                    while (Environment.TickCount - i < 2000 && isReceiving)
                    {

                        Application.DoEvents();
                    }
                    sp.Close();
                }
                try
                {
                    setSerialPort();
                    sp.DataReceived += sp_DataOfTest3Received;
                    sp.Open();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                    return;
                }
                test3connected = true;
                Btn_restartTest1.Enabled = false;
                btn_test1_switch.Enabled = false;
                btn_setting.Enabled = false;
                btn_test2.Enabled = false;
                button_Test3_Switch.Text = "停止";
            }
            else
            {
                sp.DataReceived -= sp_DataOfTest3Received;
                int j = Environment.TickCount;
                while (Environment.TickCount - j < 2000 && isReceiving)
                {

                    Application.DoEvents();
                }
                sp.Close();
                test3connected = false;

                hScrollBar1.Value = 0;
                vScrollBar1.Value = vScrollBar1.Maximum + 1 - vScrollBar1.LargeChange;


                label_Test3_MVC.Text = test3_MVC.ToString();


                DialogResult result = MessageBox.Show("保存数据?", "测试完成", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    Test3MVCRecord.Add(test3_MVC);
                    label_Test3_TestTimes.Text = Test3MVCRecord.Count.ToString();
                    float ave=0f;
                    for (int i = 0; i < Test3MVCRecord.Count; i++)
                    {
                        ave += (float)Test3MVCRecord[i];
                    }
                    ave /= Test3MVCRecord.Count;
                    label_Test3_AverageMVC.Text = ave.ToString("G5");
                    saveTest3Data();
                }
                else
                {
                }
                DataOfTest3.Clear();
                button_Test3_Switch.Text = "开始";
                Btn_restartTest1.Enabled = true;
                btn_test1_switch.Enabled = true;
                btn_setting.Enabled = true;
                btn_test2.Enabled = true;
            }

        }

        private void stopTest2() 
        {
            
            sp.DataReceived -= sp_DataOfTest2Received;
            int j = Environment.TickCount;
            while (Environment.TickCount - j < 2000 && isReceiving)
            {

                Application.DoEvents();
            }
            sp.Close();
            test2connected = false;

            hScrollBar1.Value = 0;
            vScrollBar1.Value = vScrollBar1.Maximum + 1 - vScrollBar1.LargeChange;

            minForceOfTest2=endForceOfTest2(maxForceOfTest2TimeCount,20);
            label_Test2_minForce.Text = minForceOfTest2.ToString();
            deltaPercentage = (maxForceOfTest2 - minForceOfTest2) / maxForceOfTest2;
            label_Test2_per.Text = (deltaPercentage * 100).ToString("G5")+"%";
            

            DialogResult result= MessageBox.Show("保存数据?","测试完成",MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                perRecord.Add(deltaPercentage);
                label_TestTimes2.Text = perRecord.Count.ToString();
                saveTest2Data();
            }
            else
            {
            }           
            
            btn_test2.Text = "测试耐力(&T)";
            Btn_restartTest1.Enabled = true;
            btn_test1_switch.Enabled = true;
            btn_setting.Enabled = true;
            button_Test3_Switch.Enabled = true;
        }

        private void Test2EndTimer_Elapsed(object s, System.Timers.ElapsedEventArgs e)
        {
            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = "EndSound.wav";
            sp.Play();

        }

        private void saveData()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string dirPath = Path.Combine(currentDir, "TestData");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string dataPath = Path.Combine(dirPath, "OriginalData");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            string UserFolderName = Path.Combine(dataPath, userName);
            if (!Directory.Exists(UserFolderName))
            {
                Directory.CreateDirectory(UserFolderName);
            }

            string fileName = Path.Combine(UserFolderName, "爆发力_"  + label_TestTimes1.Text + ".txt");
            FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
           

            for (int i = 0; i < dataRecord.Count; i++)
            {
               // sw.WriteLine(i.ToString() + "\t" + Convert.ToString (i * dataTimeSpan,provider) + "\t" + ((float)dataRecord[i]).ToString());
                sw.Write(i.ToString()+'\t');
                sw.Write("{0:F6}", i * dataTimeSpan);
                sw.WriteLine('\t' + ((float)dataRecord[i]).ToString());
            }
            sw.Close();
            fs.Close(); //save all records of forces(dataTimeSpan per sec)

            string MVCRecordFileName = Path.Combine(dirPath, "爆发力_Record.txt");
            fs = File.Open(MVCRecordFileName, FileMode.Append);
            sw = new StreamWriter(fs);
            sw.WriteLine(userName + '\t' + maxForce.ToString() + '\t' + label_TimeToMVC.Text + '\t' + label_MaxForceChangingSpeed.Text);
            sw.Close();
            fs.Close();
        }

        private void saveTest2Data()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string dirPath = Path.Combine(currentDir, "TestData");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string dataPath = Path.Combine(dirPath, "OriginalData");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            string UserFolderName = Path.Combine(dataPath, userName);
            if (!Directory.Exists(UserFolderName))
            {
                Directory.CreateDirectory(UserFolderName);
            }                   //检查用户文件夹是否存在
            string fileName = Path.Combine(UserFolderName, "耐力_"  + label_TestTimes2.Text + ".txt");
            FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < DataOfTest2.Count; i++)
            {
               // sw.WriteLine(i.ToString() + '\t' + (i * dataTimeSpan).ToString() + '\t' + ((float)DataOfTest2[i]).ToString());
                sw.Write(i.ToString() + '\t');
                sw.Write("{0:F6}", i * dataTimeSpan);
                sw.WriteLine('\t' + ((float)DataOfTest2[i]).ToString());
            }
            sw.Close();
            fs.Close();

            string TimeRecordFileName = Path.Combine(dirPath, "耐力_Record.txt");           
            fs = File.Open(TimeRecordFileName, FileMode.Append);
            sw = new StreamWriter(fs);
            sw.WriteLine(userName+'\t'+deltaPercentage.ToString());
            sw.Close();
            fs.Close();


        }

        private void saveTest3Data()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string dirPath = Path.Combine(currentDir, "TestData");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }                    //根目录:TestData

            string dataPath = Path.Combine(dirPath, "OriginalData");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }                   //检查OriginalData目录是否存在

            string UserFolderName = Path.Combine(dataPath,userName);
            if (!Directory.Exists(UserFolderName))
            {
                Directory.CreateDirectory(UserFolderName);
            }                   //检查用户文件夹是否存在

            string fileName = Path.Combine(UserFolderName, "最大力_"  + label_Test3_TestTimes.Text + ".txt");
            FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < DataOfTest3.Count; i++)
            {
                //sw.WriteLine(i.ToString() + '\t' + (i * dataTimeSpan).ToString() + '\t' + ((float)DataOfTest3[i]).ToString());
                sw.Write(i.ToString() + '\t');
                sw.Write("{0:F6}", i * dataTimeSpan);
                sw.WriteLine('\t' + ((float)DataOfTest3[i]).ToString());
            }
            sw.Close();
            fs.Close();

            string TimeRecordFileName = Path.Combine(dirPath, "最大力_Record.txt");
            fs = File.Open(TimeRecordFileName, FileMode.Append);
            sw = new StreamWriter(fs);
            sw.WriteLine(userName + '\t' + test3_MVC.ToString());
            sw.Close();
            fs.Close();


        }       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sp.IsOpen)
            {
                int i = Environment.TickCount;
                while (Environment.TickCount - i < 2000 && isReceiving)
                {

                    Application.DoEvents();
                }
                sp.Close();
                sp.Dispose();
            }
        }

        private void btn_setting_Click(object sender, EventArgs e)
        {
            if (!portAndNameSetCompleted)
            {
                if (textBox_testName.Text != "" && comboBox_portName.Text != "")
                {
                    userName = textBox_testName.Text;
                    portName = comboBox_portName.Text;
                    comboBox_portName.Enabled = false;
                    textBox_testName.Enabled = false;
                    tabControl_MVCTest.Enabled = true;
                    btn_setting.Text = "×";
                    portAndNameSetCompleted = true;
                }
            }
            else
            {
                MVC_Record.Clear();
                //pointSet.Clear();
                dataRecord.Clear();
                DataOfTest2.Clear();
                perRecord.Clear();
                Test3MVCRecord.Clear();

                drawAxis(g);
                getBmpToShow(bmp, clipX, clipY);

                maxForce = 0.00f;
                label_MaxForce.Text = "0.00";
                label_TestTimes1.Text = "0";
                label_TestTimes2.Text = "0";
                label1_AverageMVC.Text = "0.0000";
                label_StrengthData.Text = "0.00";
                label_MaxForceChangingSpeed.Text = "0";
                label_TimeToMVC.Text = "0";
                label_Test2_Maxforce.Text = label_Test2_minForce.Text = "0";
                label_Test2_per.Text = "0%";

               textBox_testName.Text = "";
               comboBox_portName.Text = "";
               comboBox_portName.Enabled = true;
               textBox_testName.Enabled = true;
               tabControl_MVCTest.Enabled = false;
               btn_setting.Text = "√";
               portAndNameSetCompleted = false;
            }

        }

        private float calMaxChangingSpeed()
        {
            float k;
            float max_k = 0;
            for (int i = startTimeCount; i < maxForceTimeCount; i++)
            {
                k = (float)dataRecord[i + 1] - (float)dataRecord[i];
                if (k > max_k)
                {
                    max_k = k;
                }
            }
            return max_k / (1f/pointsPerSecond);
        }

        private float endForceOfTest2(int maxPosition,int timeCount)
        {
            if (DataOfTest2.Count-1 < timeCount * pointsPerSecond + maxPosition)
            {
                return 0f;
            }
            float minForce = maxForceOfTest2;
            //int i;
            for (int i = maxPosition; i < Math.Ceiling( timeCount * pointsPerSecond )+ maxPosition; i++)
            {
                if ((float)DataOfTest2[i] < minForce)
                {
                    minForce = (float)DataOfTest2[i];
                }
            }
            return minForce;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            System.Media.SoundPlayer sp = new SoundPlayer();    
            sp.SoundLocation = "Sound.wav";
            sp.Play();
        }

      /* private void btn_viewTest2Data_Click(object sender, EventArgs e)
        {
            ViewTestDataNum = 2;
            ViewDataForm wdf = new ViewDataForm();
            wdf.ShowDialog();
        }

        private void btn_viewTest1Data_Click(object sender, EventArgs e)
        {
            ViewTestDataNum = 1;
            ViewDataForm wdf = new ViewDataForm();
            wdf.ShowDialog();

        }*/

        private void playSound()
        {
            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = "Sound.wav";
            sp.Play();
        }

       
      


       
    }
}
