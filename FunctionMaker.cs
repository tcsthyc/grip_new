using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NCalc;

namespace GrippingTest
{
    public partial class FunctionMaker : Form
    {
        public Form1 formInstance { get; set; }

        public FunctionMaker()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
          
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            float textValue;
            float start;
            float end;
            try
            {
                textValue = float.Parse(textBoxTestInput.Text);
                start = float.Parse(textBoxRangeStart.Text);
                end = float.Parse(textBoxRangeEnd.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("不正确的输入", "Error");
                return;
            }


            if (textValue < start || textValue > end) 
            {
                labelTestResult.Text = "not in range, did not cal";
                return;
            }


            String realExp = textBoxExp.Text.Replace("t", textBoxTestInput.Text);
            try
            {
                Expression ex = new Expression(realExp);
                labelTestResult.Text = ex.Evaluate().ToString();
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.ToString(), "Error");
            }            
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            AssistLineSecion als = new AssistLineSecion();
            als.expression = textBoxExp.Text;
            als.start = float.Parse(textBoxRangeStart.Text);
            als.end = float.Parse(textBoxRangeEnd.Text);
            try
            {
                als.xPixelSpan = float.Parse(textBoxXSpan.Text);
            }
            catch
            {
                als.xPixelSpan = 2;
            }
            formInstance.strategy.sections.Add(als);
            formInstance.refreshStratogy();
        }
    }
}
