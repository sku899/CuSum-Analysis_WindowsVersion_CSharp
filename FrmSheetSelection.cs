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
    public partial class FrmSheetSelection : Form
    {
        public bool confirmSelection = false;
        public string[] msg;
        public FrmSheetSelection(string[] sheetName)
        {
            InitializeComponent();
            string sn="";
            for (int i = 0; i < sheetName.Length; i++)
                checkedListBox1.Items.Insert(i, sheetName[i]);
            lblSheetNames.Text = sn;
            msg= new string[0];

        }

        private void btnConfirmSelection_Click(object sender, EventArgs e)
        {
            //msg = new string[0];
            msg = new string[checkedListBox1.CheckedItems.Count];
            if (checkedListBox1.CheckedItems.Count > 0)
            {

                


                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {

                    msg[i] =checkedListBox1.CheckedItems[i].ToString();

                }
            }

            this.Close();
        }

        private void FrmSheetSelection_Load(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkSelectNone.Checked = false;

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
        }
        public void changeLabel(string str)
        {
            lblSheetNames.Text = str;
        }

        private void chkSelectNone_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectNone.Checked)
            {
                chkSelectAll.Checked = false;

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }
    }
}
