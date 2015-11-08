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
        //食指
        private const int CH_INDEX_MIN = 100;
        private const int CH_INDEX_MAX = 999;
        
        //中指
        private const int CH_MIDDLE_MIN = 100;
        private const int CH_MIDDLE_MAX = 550;

        //无名指
        private const int CH_RING_MIN = 100;
        private const int CH_RING_MAX = 999;

        //小指
        private const int CH_LITTLE_MIN = 100;
        private const int CH_LITTLE_MAX = 999;

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
        public static float dataTimeSpan=1.448f/1000;
        private bool test1StartSoundPlayed;
        private System.Timers.Timer Test2EndTimer = new System.Timers.Timer();

        public static Int32 baudRate;       
        public static Parity parity ;
        public static Int32 dataBits ;
        public static StopBits stopBits;

        private int errorCount;
        private DataPointView dpvTotal;
        private DataPointView dpvIndex;
        private DataPointView dpvMiddle;
        private DataPointView dpvRing;
        private DataPointView dpvLittle;

        public AssistLineStratogy strategy { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            strategy = new AssistLineStratogy();
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
            refreshTimer.Interval = 40;

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
                    //Console.WriteLine("str:"+tempStringBuilder.ToString());
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
                        gd.index = mapForceValue(CH_INDEX_MIN, CH_INDEX_MAX, float.Parse(values[0]));
                        gd.middle = mapForceValue(CH_MIDDLE_MIN, CH_MIDDLE_MAX, float.Parse(values[1]));
                        gd.ring = mapForceValue(CH_RING_MIN, CH_RING_MAX, float.Parse(values[2]));
                        gd.little = mapForceValue(CH_LITTLE_MIN, CH_LITTLE_MAX, float.Parse(values[3]));
                        gd.total = gd.index + gd.middle + gd.ring + gd.little;
                        dataRecord.Add(gd);
                    }
                    catch
                    {
                        //出错则使用上一行数据作为本行数据，同时error计数+1
                        int id = dataRecord.Count - 1;
                        if (id < 1)
                        {
                            gd.index = 0;
                            gd.middle = 0;
                            gd.ring = 0;
                            gd.little = 0;
                            gd.total = 0;
                        }
                        else
                        {
                            gd.index = dataRecord[id].index;
                            gd.middle = dataRecord[id].middle;
                            gd.ring = dataRecord[id].ring;
                            gd.little = dataRecord[id].little;
                            gd.total = dataRecord[id].total;
                        }
                        dataRecord.Add(gd);
                        errorCount++;
                    }

                    dpvTotal.pushData(gd.total);
                    dpvIndex.pushData(gd.index);
                    dpvMiddle.pushData(gd.middle);
                    dpvRing.pushData(gd.ring);
                    dpvLittle.pushData(gd.little);
               
                    tempStringBuilder.Clear();
                }
            }
        }

        private float mapForceValue(float min, float max, float val)
        {
            return (val - min) / (max - min) * 20;
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
                groupBoxStrategy.Enabled = false;
                dataRecord.Clear();
                errorCount = 0;
                started = false;
                if (comboBoxPorts.Text != "" && textBoxUserName.Text!="")
                {
                    userName = textBoxUserName.Text;
                    portName = comboBoxPorts.Text;
                    comboBoxPorts.Enabled = false;
                }

                initDpv();

                refreshTimer.Start();

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
                    groupBoxStrategy.Enabled = true;
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
                groupBoxStrategy.Enabled = true;
            }
        }

        private void initDpv()
        {
            dpvTotal = new DataPointView(refreshTimer, pbTotal, dataTimeSpan, new DataPointViewConfig(60, 80, true, true, strategy));
            dpvIndex = new DataPointView(refreshTimer, pbIndex, dataTimeSpan, new DataPointViewConfig(30, 20, false, true, strategy));
            dpvMiddle = new DataPointView(refreshTimer, pbMiddle, dataTimeSpan, new DataPointViewConfig(30, 20, false, true, strategy));
            dpvRing = new DataPointView(refreshTimer, pbRing, dataTimeSpan, new DataPointViewConfig(30, 20, false, true, strategy));
            dpvLittle = new DataPointView(refreshTimer, pbLittle, dataTimeSpan, new DataPointViewConfig(30, 20, false, true, strategy));
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

        }

        private void buttonSerialSettings_Click(object sender, EventArgs e)
        {
            Settings settingForm = new Settings();
            settingForm.ShowDialog();
        }

        private void buttonAddAssistLine_Click(object sender, EventArgs e)
        {
            FunctionMaker fm = new FunctionMaker();
            fm.formInstance = this;
            fm.ShowDialog(this);
        }

        public void refreshStratogy()
        {
            textBoxStratogy.Text = strategy.toString();
        }

        private void buttonDelLastLine_Click(object sender, EventArgs e)
        {
            if (strategy.sections.Count == 0) return;
            strategy.sections.RemoveAt(strategy.sections.Count - 1);
            refreshStratogy();
        }
    }
}
