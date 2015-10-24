using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace GrippingTest
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (comboBox_baudRate.Text == "")
            {
                MessageBox.Show("未设定波特率！", "error");
                return;
            }
            else
            {
                Form1.baudRate = Int32.Parse(comboBox_baudRate.Text);
            }

            if (comboBox_bits.Text == "")
            {
                MessageBox.Show("未设定波特率！", "error");
                return;
            }
            else
            {
                Form1.dataBits = Int32.Parse(comboBox_bits.Text);
            }

            switch (comboBox_parity.Text)
            {
                case "":
                    MessageBox.Show("未设定校验位！", "error");
                    return;
                case "None":
                    Form1.parity = Parity.None;
                    break;
                case "Space":
                    Form1.parity = Parity.Space;
                    break;
                case "Odd":
                    Form1.parity = Parity.Odd;
                    break;
                case "Even":
                    Form1.parity = Parity.Even;
                    break;
                case "Mark":
                    Form1.parity = Parity.Mark;
                    break;
                default:
                    Form1.parity = Parity.None;
                    break;
            }

            switch (comboBox_stopBits.Text)
            {
                case "":
                    MessageBox.Show("未设定停止位！", "error");
                    return;
                case "None":
                    Form1.stopBits = StopBits.None;
                    break;
                case "One":
                    Form1.stopBits = StopBits.One;
                    break;
                case "Two":
                    Form1.stopBits = StopBits.Two;
                    break;
                case "OnePointFive":
                    Form1.stopBits = StopBits.OnePointFive;
                    break;
                default:
                    Form1.stopBits = StopBits.None;
                    break;
            }
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
