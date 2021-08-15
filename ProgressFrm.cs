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
    public partial class ProgressFrm : Form
    {
        public ProgressFrm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            Parent = null;
           
        }
        public void updateLabel(string upDateStatus)
        {
            lblProgressPF.Text = upDateStatus;
        }
        public void updateProgressBar(int value)
        {
            //pBarPF.Value = value;
            if (value == pBarPF.Maximum)
            {
                // Special case as value can't be set greater than Maximum.
                pBarPF.Maximum = value + 1;     // Temporarily Increase Maximum
                pBarPF.Value = value + 1;       // Move past
                pBarPF.Maximum = value;         // Reset maximum
            }
            else
            {
                pBarPF.Value = value + 1;       // Move past
            }
            pBarPF.Value = value;
            pBarPF.Update();
        }

        private void ProgressFrm_Load(object sender, EventArgs e)
        {
            pBarPF.Value = 0;
        }
        public void resetPBar()
        {
            pBarPF.Value = 0;
        }
    }
}
