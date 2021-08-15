using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuSum_Analysis
{
    public partial class IPForm : Form
    {

        CuSumAnalysisGUI mainform;
        bool isini = true;
        public IPForm(decimal[] vals, CuSumAnalysisGUI frm,int pos)
        {
            InitializeComponent();
            mainform = frm;
            numUPAlphaIPForm.Value = vals[0];
            numUPBetaIPForm.Value = vals[1];
            numUPAlP0IPForm.Value = vals[2];
            numUPP1IPForm.Value = vals[3];
            Point pt = new Point();
            if (pos == 1) //main form is on the right
            {
                pt.X = mainform.Size.Width + this.Size.Width;
                pt.Y = mainform.Location.Y;
            }
            else
            {
                pt.X = mainform.Location.X - this.Size.Width;
                pt.Y = mainform.Location.Y;
            }
            this.Location = new Point(pt.X,pt.Y);
            
        }

        private void IPForm_Load(object sender, EventArgs e)
        {
            string[] labelString = new string[] { "\u03B1", "\u03B2", "p" + "\u2080", "p" + "\u2081" };
            Label[] lbl = new Label[] {lblAlpha,lblBeta,lblP0,lblP1 };
            for (int i = 0; i < labelString.Length; i++)
            {
                lbl[i].Text = labelString[i];
            }

        }

        private void numUPAlphaIPForm_ValueChanged(object sender, EventArgs e)
        {
            string[] names=new string[4];
            string type = sender.GetType().Name;
            int index = -1;
            NumericUpDown[] numUDs = new NumericUpDown[] {numUPAlphaIPForm,numUPBetaIPForm,numUPAlP0IPForm,numUPP1IPForm };
            TrackBar[] tBars = new TrackBar[] { tBarAlphaIPForm, tBarBetaIPForm, tBarP0, tBarP1IPForm };
            if (type == "NumericUpDown")
            {
                names = new string[] { "numUPAlphaIPForm", "numUPBetaIPForm", "numUPAlP0IPForm", "numUPP1IPForm" };
                for (int i=0; i < names.Length; i++)
                {
                    if (((NumericUpDown)sender).Name==names[i])
                    {
                        index = i;
                        mainform.valueFromIPform(i, (double)((NumericUpDown)sender).Value, 1);
                        tBars[i].Value = (int)(((NumericUpDown)sender).Value * 100m);
                    }
                }
            }
            if (type == "TrackBar")
            {
                names = new string[] { "tBarAlphaIPForm", "tBarBetaIPForm", "tBarP0", "tBarP1IPForm" };
                for (int i=0; i < names.Length; i++)
                {
                    if (((TrackBar)sender).Name==names[i])
                    {
                        index = i;
                        mainform.valueFromIPform(i, (double)((TrackBar)sender).Value, 2);
                        numUDs[i].Value= (decimal)((double)(((TrackBar)sender).Value)/100.0d);
                    }
                }
            }
                
        }
        //===================================
        public void valueFromMainForm(int index, double val, int type)
        {
            NumericUpDown[] numUDs = new NumericUpDown[] { numUPAlphaIPForm, numUPBetaIPForm, numUPAlP0IPForm, numUPP1IPForm };
            TrackBar[] tBars = new TrackBar[] { tBarAlphaIPForm, tBarBetaIPForm, tBarP0, tBarP1IPForm };
            if (type == 1)//numericUPDown
            {
                numUDs[index].Value = (decimal)val;
            }
            if (type == 2)//traceBar
            {
                tBars[index].Value = (int)val;
            }

        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IPForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainform.isIPFormOpened = false;
        }

        //*************************************

    }


}
