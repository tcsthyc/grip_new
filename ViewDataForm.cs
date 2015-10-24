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
            Form1 form1 = (Form1)sender;
           
            object[] row;
            for (int i = 0; i < form1.dataRecord.Count; i++)
            {
                row = new object[] { (dataTimeSpanByMiliSec * i).ToString(), (Form1.dataTimeSpan * i).ToString(), (form1.dataRecord[i].index).ToString(), (form1.dataRecord[i].middle).ToString(), (form1.dataRecord[i].ring).ToString(), (form1.dataRecord[i].little).ToString() };
                dataGridView1.Rows.Add(row);
            }
            
        }
    }
}
