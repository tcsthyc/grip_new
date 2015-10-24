using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrippingTest
{
    public partial class ViewDataForm : Form
    {
        public ViewDataForm()
        {
            InitializeComponent();
        }

        private void ViewDataForm_Load(object sender, EventArgs e)
        {
            float dataTimeSpanByMiliSec = Form1.dataTimeSpan * 1000;
            if (Form1.ViewTestDataNum == 2)
            {
                object[] row;
                for (int i = 0; i < Form1.DataOfTest2.Count; i++)
                {
                    row = new object[] { (dataTimeSpanByMiliSec * i).ToString(), (Form1.dataTimeSpan * i ).ToString(), ((float)Form1.DataOfTest2[i]).ToString() };
                    dataGridView1.Rows.Add(row);
                }
            }
            else if (Form1.ViewTestDataNum == 1)
            {
                object[] row;
                for (int i = 0; i < Form1.dataRecord.Count; i++)
                {
                    row = new object[] { (dataTimeSpanByMiliSec * i).ToString(), (Form1.dataTimeSpan * i ).ToString(), ((float)Form1.dataRecord[i]).ToString() };
                    dataGridView1.Rows.Add(row);
                }
            }
        }
    }
}
