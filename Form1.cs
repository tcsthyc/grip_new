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
using System.Windows.Forms;


namespace GrippingTest
{
    public partial class Form1 : Form
    {
        private bool connected;      
        private SerialPort sp;
        private String portName;
        public delegate void AddDataDelegate(char[] data);
        public List<GripData>  dataRecord;
        public StringBuilder tempStringBuilder;
        private string userName;           

        private bool isReceiving;
        private System.Windows.Forms.Timer refreshTimer;
        private ArrayList perRecord;
        private bool started;

        private float pointsPerSecond;
        public static float dataTimeSpan=6.249f/1000;
        private bool test1StartSoundPlayed;
        private System.Timers.Timer Test2EndTimer = new System.Timers.Timer();

        public static Int32 baudRate;       
        public static Parity parity ;
        public static Int32 dataBits ;
        public static StopBits stopBits;

        private int errorCount;


        public Form1()
        {
            InitializeComponent();          
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            userName = "";
            dataRecord = new List<GripData>();
            perRecord = new ArrayList();          
            isReceiving = false;  

            baudRate = 115200;
            portName = "";
            parity = Parity.None;
            dataBits = 8;
            stopBits = StopBits.One;

            tempStringBuilder = new StringBuilder();
            
            connected = false;
            comboBoxPorts.Items.Clear();
            sp = new SerialPort();
            sp.ReadTimeout = 1000;

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 50;

            pointsPerSecond = 1/dataTimeSpan;

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
           
                comboBoxPorts.Items.AddRange(str);
                if (comboBoxPorts.Items.Count > 0)
                {
                    comboBoxPorts.SelectedIndex = 0;
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
           /* if (comboBoxPorts.Text == "")
            {
                MessageBox.Show("未选择串口！", "Error");
                return;
            }
            else
            {
                portName = comboBoxPorts.Text;
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

        private void sp_DataReceived(object sender,SerialDataReceivedEventArgs e)
        {
            if (!test1StartSoundPlayed)
            {
                playSound();
                test1StartSoundPlayed = true;
            }
            isReceiving = true;
            char[] data = new char[sp.BytesToRead];
            sp.Read(data, 0, data.Length);   
                     
            AddDataDelegate utb = new AddDataDelegate(addData);
            this.Invoke(utb,new object[]{data});
            isReceiving = false;
        }
      
        public void addData(char[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
              //  textBox1.Text += data[i];
                if (data[i] != '\r' && data[i] != '\n')
                {
                    tempStringBuilder.Append(data[i]);
                }
                else if (data[i] == '\r')
                {
                    Console.WriteLine(tempStringBuilder.ToString());
                    if (!started)
                    {
                        if (tempStringBuilder.ToString().Contains("Start"))
                        {
                            started = true;
                            Console.WriteLine("started!!!!!!!!!!!!!!");
                        }
                        continue;
                    }

                    string[] values = tempStringBuilder.ToString().Split(new char[]{' '});
                    
                    GripData gd = new GripData();
                    try
                    {
                        gd.index = float.Parse(values[0]);
                        gd.middle = float.Parse(values[1]);
                        gd.ring = float.Parse(values[2]);
                        gd.little = float.Parse(values[3]);
                        dataRecord.Add(gd);
                    }
                    catch
                    {
                        int id = dataRecord.Count - 1;
                        if (id < 1)
                        {
                            gd.index = 0;
                            gd.middle = 0;
                            gd.ring = 0;
                            gd.little = 0;
                        }
                        else
                        {
                            gd.index = dataRecord[id].index;
                            gd.middle = dataRecord[id].middle;
                            gd.ring = dataRecord[id].ring;
                            gd.little = dataRecord[id].little;
                        }
                        dataRecord.Add(gd);
                        errorCount++;
                    }
               
                    tempStringBuilder.Clear();
                }
            }
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

            string fileName = Path.Combine(UserFolderName, DateTime.Now.ToString().Replace(' ','-').Replace('/','-').Replace(':','-') + ".txt");
            FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
           

            for (int i = 0; i < dataRecord.Count; i++)
            {
               // sw.WriteLine(i.ToString() + "\t" + Convert.ToString (i * dataTimeSpan,provider) + "\t" + ((float)dataRecord[i]).ToString());
                sw.Write(i.ToString()+'\t');
                sw.Write("{0:F6}", i * dataTimeSpan);
                sw.WriteLine('\t' + dataRecord[i].index.ToString() + '\t' + dataRecord[i].middle.ToString() + '\t' +dataRecord[i].ring.ToString() + '\t' + dataRecord[i].little.ToString());
            }
            sw.Close();
            fs.Close(); //save all records of forces(dataTimeSpan per sec)
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

        private void playSound()
        {
            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = "Sound.wav";
            sp.Play();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                dataRecord.Clear();
                errorCount = 0;
                started = false;
                if (comboBoxPorts.Text != "" && textBoxUserName.Text!="")
                {
                    userName = textBoxUserName.Text;
                    portName = comboBoxPorts.Text;
                    comboBoxPorts.Enabled = false;
                }

                new DataPointView(refreshTimer, 6000, pbIndex, dataTimeSpan);

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

                comboBoxPorts.Enabled = false;
                playSound();
                buttonStart.Text = "停止";
            }
            else
            {
                sp.DataReceived -= sp_DataReceived;
                int j = Environment.TickCount;
                while (Environment.TickCount - j < 10000 && isReceiving)
                {
                    Console.WriteLine("looping");
                    Application.DoEvents();
                }
                sp.Close();
                connected = false;
                Console.WriteLine(errorCount);
                DialogResult result = MessageBox.Show("是否保存此次数据？", "提示", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    saveData();
                }
                else
                {
                    //do nothing
                }
                dataRecord.Clear();
                comboBoxPorts.Enabled = true;
                buttonStart.Text = "开始";
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

        }

        private void buttonSerialSettings_Click(object sender, EventArgs e)
        {
            Settings settingForm = new Settings();
            settingForm.ShowDialog();
        }   
    }
}
