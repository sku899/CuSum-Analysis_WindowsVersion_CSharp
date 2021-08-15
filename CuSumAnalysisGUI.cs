using System;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data;

namespace CuSum_Analysis
{

    public partial class CuSumAnalysisGUI : Form
    {
        public NumericUpDown[] numUD;
        TrackBar[] tBar;
        ImageList iconsList = new ImageList();
        public bool isIPFormOpened = false;
        public bool isfChartOpened = false;
        bool isadd = false;
        IPForm frm2 ;
        FrmChart fChart;
        FrmAllChart fAChart;
        int ci = 0;
        CuSumParameters csp;
        bool isReady = false;
        int currentSelectdTabIndex = 0;
        Series[] chartSeries;
        string openedExcelFileName;
        public string openedExcelFileNameMultiple="";
        ProgressFrm PF;
        decimal[] currentVals=new decimal[4];
        bool newRowAdded = false;
        int lastTab = 0;
        Point originalPosition;
        Size originalSize;
        bool addNewRecord = false;
        int[] lastCellPos = new int[2] { -1, -1 };
        int[] currentCellPos = new int[2] { -1, -1 };
        bool isLeavingRow= false;
        public bool isFAChartOPened = false;
        string[] currentSelection = { "Trainee", "Surgeon", "Procedure", "Step" };
        Decimal[] currentParameterValue = { -1, -1, -1, -1 };
        bool isSelectionChanged = false;
        int numberOfZoom = 2;
        Point mousePos = new Point(0, 0);
        int circleRadius = 20;
        int maxViewPoint = 40;
        int currentView = 0;
        Point[] viewXPos;
        Point[] viewYPos;
        public CuSumAnalysisGUI()
        {
            InitializeComponent();
            tabGUI.ImageList = UIControl.addIcons();
            
            tabPage1.ImageIndex = 4;
            tabPage2.ImageIndex = 2;
            tabPage3.ImageIndex = 3;
            ((Control)tabGUI.TabPages[3]).Enabled = false;
            Console.WriteLine(iconsList.ImageSize);
            numUD = new NumericUpDown[] { numUpDownAlpha, numUpDownBeta, numUpDownP0, numUpDownP1 };
            tBar = new TrackBar[] { tBarAlpha, tBarBeta, tBarP0, tBarP1 };
            fChart = new FrmChart(this);
            UIControl.setButtonIcon(btnOpenExcelFilewSelection, "", @".\images\open-file.png",toolTip1,"Open Excel File");
            UIControl.setButtonIcon(btnSaveAsNew, "", @".\images\Save-as-icon.png",toolTip1, "Save As a New File");
            UIControl.setButtonIcon(btnSave, "", @".\images\Save-file-icon.png",toolTip1, "Save File");
            UIControl.setButtonIcon(btnStartNew, "", @".\images\start-new.png",toolTip1, "Start a New Table");
            UIControl.setButtonIcon(btnDeleteSelectedRecords, "", @".\images\delete-record.png",toolTip1, "Delete Selected Records");
            UIControl.setButtonIcon(btnUnselectedRecords, "", @".\images\deselect.png",toolTip1, "Deselected Records");

            UIControl.setButtonIcon(btnMoveToNextRow, ">", @".\images\next.png",toolTip1, "Move to Next Record");
            UIControl.setButtonIcon(btnMoveToLastRow, ">>", @".\images\last.png",toolTip1, "Move to Last Record");
            UIControl.setButtonIcon(btnBackToLastRow, "<", @".\images\previous.png",toolTip1, "Back to Previous Record");
            UIControl.setButtonIcon(btnBackToFirstRow, "<<", @".\images\first.png",toolTip1, "Back to First Record");

            UIControl.setButtonIcon(btnPlotExcelSheet, "", @".\images\plot.png",toolTip1, "Plot Chart");
            UIControl.setButtonIcon(btnChartClear, "", @".\images\clear.png",toolTip1, "Clear Chart");
            UIControl.setButtonIcon(btnChartSave, "", @".\images\save-chart.png",toolTip1, "Save Chart");
            UIControl.setButtonIcon(btnBackToOriginalZoom, "", @".\images\back-to-original-zoom.png", toolTip1, "Back to Origin Zoom");
            UIControl.setButtonIcon(btnLoadTraineeProfile, "", @".\images\trainee-profile-no-data.png", toolTip1, "No Trainee Profile");
            UIControl.setButtonIcon(button1, "", @".\images\select-folder.png", toolTip1, "Select a Different Working Directory");
            //toolStrip1.add(btnStartNew);
            tabGUI.Enabled = false;

        }
        

        private void CuSumAnalysisGUI_Load(object sender, EventArgs e)
        {
            UIControl.ComboFill("Trainee", cbxUserName, @".\Text Files\trainee.txt", 0);
            UIControl.ComboFill("Surgeon", cbxSurgeon, @".\Text Files\surgeon.txt", 0);
            UIControl.ComboFill("Operation", cbxOperation, @".\Text Files\operations\operations.txt", 0);
            UIControl.getDefaultWorkingDirectory(txtWorkingDirectory,chkUseDefaultWorkingDirectoy,true);
            UIControl.initialNumUDTBar(numUD, tBar);
            for (int i = 0; i < numUD.Length; i++)
            {
                currentVals[i] = numUD[i].Value;
            }
            csp = new CuSumParameters(numUpDownAlpha.Value, numUpDownBeta.Value, numUpDownP0.Value, numUpDownP1.Value);
            
            UIControl.writeSepecialCharToLabel(new Label[] { lblAlpha, lblBeta, lblP0, lblP1 });
            Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            int xpos = (workingRectangle.Width - this.Size.Width) / 2;
            int ypos = (workingRectangle.Height - this.Size.Height) / 2;
            this.Location = new Point(xpos, ypos);
            originalPosition = this.Location;
            originalSize = this.Size;
            decimal[] vals = new decimal[] { numUpDownAlpha.Value, numUpDownBeta.Value, numUpDownP0.Value, numUpDownP1.Value, };
            frm2 = new IPForm(vals, this,1);
            PF = new ProgressFrm();
            chkZoomXAxis.Checked = true;
            chkZoomYAxis.Checked = true;
            chart1.Series.Clear();
            UIControl.setFont(this);
            if (workingRectangle.Height > 0.9 * this.Height)
            {
                int portion = 8;
                xpos = (workingRectangle.Width - this.Size.Width) / 2;
                ypos = workingRectangle.Height/ portion;
                this.Location = new Point(xpos, ypos);
                this.Size = new Size(this.Width, workingRectangle.Height / portion * (portion-2));
            }
            checkBox1.Checked = false;
            chart2.MouseWheel += chart2_MouseWheel;
            chart2.Series.Clear();
            viewXPos = new Point[maxViewPoint];
            viewYPos = new Point[maxViewPoint];
            
            //node = treeViewHospital.Nodes.Add("Scotland");
            //node.Nodes.Add("Scotland H1");
            //node.Nodes.Add("Scotland H2");
            //node.Nodes.Add("Scotland H3");

            //node = treeViewHospital.Nodes.Add("England");
            //node.Nodes.Add("England H1");
            //node.Nodes.Add("England H2");
            //node.Nodes.Add("England H3");
            //
            string fileName = @".\Text Files\hospitals\" + "hospitals.txt";
            var reader = new StreamReader(File.OpenRead(fileName));
            TreeNode node =new TreeNode();
            bool newArea = false;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line.Trim().Length > 0)
                    if (newArea)
                    {
                        node = treeViewHospital.Nodes.Add(line);
                        newArea = false;
                    }
                    else
                    {
                        node.Nodes.Add(line);
                    }
                else
                { newArea = true; }
            }

            //


        }

//======================================================================

        private void button1_Click(object sender, EventArgs e)
        {
            if (chkUseDefaultWorkingDirectoy.Checked)
            {
                chkUseDefaultWorkingDirectoy.Checked = false;
            }
            else
            {
                UIControl.changeWorkingDirectory(txtWorkingDirectory, chkUseDefaultWorkingDirectoy);
            }

        }

        private void numUpDownAlpha_ValueChanged(object sender, EventArgs e)
        {
            //if ((numUpDownP1.Value - numUpDownP0.Value) <= 0.05m) { return; }

                UIControl.setIPValues(sender, numUD, tBar, isIPFormOpened, frm2);
                csp = new CuSumParameters(numUpDownAlpha.Value, numUpDownBeta.Value, numUpDownP0.Value, numUpDownP1.Value);

            CHT.plotChart(dgvProgressData, new CheckBox[] { chkCUSUMChart, chkIndividualScore, chkSuccessNum }, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart1);
            for (int i=0;i<numUD.Length;i++)
            {
                if (numUD[i].Value.ToString("0.00")!=currentParameterValue[i].ToString("0.00"))
                {
                    isSelectionChanged = true;
                }
                currentParameterValue[i] = numUD[i].Value;
            }

        }



        private void tabGUI_SelectedIndexChanged(object sender, EventArgs e)
        {
            // tabControl1.SelectedTab
            isReady = (cbxSurgeon.SelectedIndex > 0)  ;
            if (tabGUI.SelectedIndex == 3)
            {
                tabGUI.SelectedIndex = lastTab;
                return;
            }
            if ((!isReady) && (tabGUI.SelectedTab == tabPage3))
            {
                string str = "";
                if ((cbxSurgeon.SelectedIndex <= 0))
                {
                    //errorProvider1.SetError(cbxSurgeon, "Please Select One Surgeon");
                    //str = str+"Surgeon, ";
                }


                //MessageBox.Show("Inputs of "+ str.Substring(0,str.Length-2)+ " are required.", "Input Incomplete", MessageBoxButtons.OK);
                tabGUI.SelectedIndex=lastTab;
                return;
            }
            tabPage1.ImageIndex = 1;
            tabPage2.ImageIndex = 2;
            tabPage3.ImageIndex = 3;
            for (int i = 0; i < 3; i++)
            {
                if (tabGUI.SelectedTab == tabGUI.TabPages[i])//your specific tabname
                {
                    tabGUI.TabPages[i].ImageIndex = 4+i;
                }
            }
            lastTab = tabGUI.SelectedIndex;

            //create trainee profile
            //using (StreamWriter w = )
            if ((tabGUI.SelectedIndex == 2) && (isReady))
            {


                if (tabExcelSheet.TabPages.Count > 0)
                {
                    //MessageBox.Show(tabExcelSheet.SelectedIndex.ToString());
                    CHT.plotChart(DGV.dgvs[tabExcelSheet.SelectedIndex], new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber },
                        new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
                    //chart1.Update();
                }
            }
        }

//==========================================

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openedExcelFileName=DGV.openExcelToGetData(txtWorkingDirectory.Text, dgvProgressData,txtNavidationIndex,PF,this.Location,this.Size);
            //MessageBox.Show(openedExcelFileName);
        }

        private void btnMoveToNextRecord_Click(object sender, EventArgs e)
        {
            DataGridView dgv = dgvProgressData;
            TextBox txtb = txtNavidationIndex;
            if (((Button)sender).Tag.ToString() == "tagBtnMove")
            {
                //MessageBox.Show("get in tag");
                dgv = DGV.dgvs[tabExcelSheet.SelectedIndex];
                txtb = txtNavigation;
            }
            if (dgv.RowCount>0)
            DGV.changeSelectionOfRecord(sender, dgv, txtb);
        }

        private void dgvs_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            UIControl.changeSelect(e, DGV.dgvs[tabExcelSheet.SelectedIndex], txtNavigation);
        }
        private void dgvProgressData_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) || (e.Button == MouseButtons.Left))
            {
                //progressDataGridView.CurrentCell = progressDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgvProgressData.ClearSelection();
                if (e.RowIndex != -1)
                {
                    if (e.ColumnIndex == -1)
                    {
                        dgvProgressData.Rows[e.RowIndex].Cells[0].Selected = true;
                    }
                    else
                    {
                        dgvProgressData.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        //csd.selectedColumn = e.ColumnIndex;
                    }
                    if (e.RowIndex > dgvProgressData.Rows.Count - 2)
                    {
                        txtNavidationIndex.Text = "New Row";
                    }
                    else
                    {
                        txtNavidationIndex.Text = (e.RowIndex + 1).ToString() + " of " + (dgvProgressData.Rows.Count - 1).ToString();
                    }
                }
                //csd.selectedRow = e.RowIndex;
                //csd.displaySelectedRowToBufferGDV();
                //MessageBox.Show("selected Row" + e.RowIndex.ToString(), csd.dt.Rows.Count.ToString(), MessageBoxButtons.OK);
            }
        }

        private void btnDeleteSelectedRow_Click(object sender, EventArgs e)
        {
            if (tabExcelSheet.TabPages.Count > 0)
            {
                string name = ((Button)sender).Name;
                DataGridView dgv = new DataGridView();
                if (name == "btnDeleteSelectedRow")
                {
                    dgv = dgvProgressData;
                }
                else
                {
                    dgv = DGV.dgvs[tabExcelSheet.SelectedIndex];
                }
                DGV.deleteSelectedRecords(dgv,txtNavigation);
                //plot
                CheckBox[] plotOptions = new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber };
                if (tabExcelSheet.SelectedIndex >= 0)
                {
                    DataGridView selectedDGV = DGV.dgvs[tabExcelSheet.SelectedIndex];
                    CHT.plotChart(selectedDGV, plotOptions, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
                    CHT.plotHistogramToNewForm(plotOptions[2], fChart, this, selectedDGV);
                 }
                //plot


            }
            return;
        }

        private void chkDeselectedAllRows_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkDeselectedAllRows.Checked)&&(dgvProgressData.Rows.Count>0))
            {
                dgvProgressData.ClearSelection();
                dgvProgressData.Rows[dgvProgressData.Rows.Count-1].Selected = true;                
            }
            chkDeselectedAllRows.Checked = false;
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            if (c is CheckBox)
            {
                if (!(chkCUSUMChart.Checked || chkIndividualScore.Checked))
                {
                    chkCUSUMChart.Checked = true;
                }
            }
            try
            {
                if ((!isfChartOpened) && (chkSuccessNum.Checked) && (dgvProgressData.Rows.Count > 1))
                {
                    fChart = new FrmChart(this);
                    fChart.Show();
                    isfChartOpened = true;
                    //
                    Point pt = new Point();
                    pt.X = this.Size.Width;// + frm2.Size.Width;
                    pt.Y = this.Location.Y+50;
                    if ((rbCenter.Checked) || (rbUpperLeft.Checked) || (rbLowerLeft.Checked))
                    {
                        pt.X = this.Location.X - fChart.Size.Width;
                        pt.Y = this.Location.Y+this.Size.Height/2;
                    }
                    fChart.Location = pt;


                }
                CHT.plotChart(dgvProgressData, new CheckBox[] { chkCUSUMChart, chkIndividualScore, chkSuccessNum }, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart1);
                if (chkSuccessNum.Checked)
                {
                    CHT.plotChartToNewForm(chkSuccessNum.Checked, isfChartOpened, fChart, this, dgvProgressData);
                }
                   
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Plot Error", MessageBoxButtons.OK);
            }
            return;
        }

        private void dgvProgressData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!isadd)
            {
                DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
                if (((DataGridView)sender).Name == "dgvProgressData")
                    tb.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress);
                else
                    tb.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress2);
                //tb.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress);
                isadd = true;
                if (((DataGridView)sender).Name == "dgvProgressData")
                    e.Control.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress);
                else
                    e.Control.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress2);
            }
        }
        private void dataGridViewTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ci = DGV.checkInputValue(dgvProgressData, e, ci, sender);
        }

        //=====================================
        private void dataGridViewTextBox_KeyPress2(object sender, KeyPressEventArgs e)
        {
            ci = DGV.checkInputValue(DGV.dgvs[tabExcelSheet.SelectedIndex], e, ci,sender);
        }
        //**************************************

        private void dgvProgressData_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            ci = 0;
        }

        private void dgvProgressData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                ci = 0;
            }
            if (addNewRecord) { return; }
            try
            {
                string dgvName = ((DataGridView)sender).Name;
                Chart ch = chart1;
                CheckBox[] plotOptions = new CheckBox[] { chkCUSUMChart, chkIndividualScore, chkSuccessNum };
                if (dgvName.Contains("_"))
                {
                    ch = chart2;
                    plotOptions = new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber };
                }
                CHT.plotHistogramToNewForm(plotOptions[2],fChart, this, (DataGridView)sender);
                CHT.plotChart((DataGridView)sender, plotOptions, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, ch);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Plot Error - CellEndEdit", MessageBoxButtons.OK);
            }
        }

        private void btnDisplayParameters_Click(object sender, EventArgs e)
        {
            if (!isIPFormOpened)
            {
                decimal[] vals = new decimal[] { numUpDownAlpha.Value, numUpDownBeta.Value, numUpDownP0.Value, numUpDownP1.Value, };
                int pos = 1;
                if ((rbCenter.Checked) || (rbLowerLeft.Checked) || (rbUpperLeft.Checked)) { pos = 2; }
                frm2 = new IPForm(vals, this, pos);
                
                isIPFormOpened = true;
                frm2.Show();
                Point pt = new Point();
                pt.X = this.Size.Width;// + frm2.Size.Width;
                pt.Y = this.Location.Y;
                if ((rbCenter.Checked) || (rbUpperLeft.Checked) || (rbLowerLeft.Checked))
                {
                    pt.X = this.Location.X;// - frm2.Size.Width;
                    pt.Y = this.Location.Y+50;                        
                }
                frm2.Location = pt;
            }
        }
        public void valueFromIPform(int index, double val, int type)
        {
            if (type==1)//numericUPDown
            {
                numUD[index].Value = (decimal)val;
            }
            if (type==2)//traceBar
            {
                tBar[index].Value = (int)val;
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            MessageBox.Show("Location X: " + workingRectangle.Width.ToString() + " Y: " + workingRectangle.Height.ToString());
        }

        private void rbUpperLeft_CheckedChanged(object sender, EventArgs e)
        {
            UIControl.setGUIPosition(sender, this,frm2,fChart);
        }

        private void chkUseDefaultWorkingDirectoy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseDefaultWorkingDirectoy.Checked)
                UIControl.getDefaultWorkingDirectory(txtWorkingDirectory, chkUseDefaultWorkingDirectoy, true);
            else
                UIControl.changeWorkingDirectory(txtWorkingDirectory, chkUseDefaultWorkingDirectoy);
        }

        private void tabGUI_MouseClick(object sender, MouseEventArgs e)
        {
            //currentSelectdTabIndex = tabGUI.SelectedIndex;
            
        }

        private void cbxTrainee_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            string[] cbxNames = new string[] { "cbxTrainee", "cbxSurgeon", "cbxSurgery", "cbxSurgerySkill" };            
            string[] preFixString = new string[] { "Trainee: ", "Surgeon: ", "Surgery: ", "Skill: " };
            for (int i = 0; i < cbxNames.Length; i++)
            {
                if (cbx.Name == cbxNames[i])
                {
                    if (cbx.SelectedIndex > 0)
                    {
                        
                        if (cbx.Items[cbx.SelectedIndex].ToString()!=currentSelection[i])
                        {
                            currentSelection[i] = cbx.Items[cbx.SelectedIndex].ToString();
                            isSelectionChanged = true;
                        }
                        if ((i == 2))
                        {
                        csp = new CuSumParameters(numUpDownAlpha.Value, numUpDownBeta.Value, numUpDownP0.Value, numUpDownP1.Value);
                        }      
                    }
                    else
                    {
                        if (i==0)
                        {
                            btnLoadTraineeProfile.Enabled = false;
                            UIControl.setButtonIcon(btnLoadTraineeProfile, "", @".\images\trainee-profile-no-data.png", toolTip1, "No Trainee Profile");
                        }

                    }
                }
            }          
        }

        private void chkZoomXAxis_CheckedChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = chkZoomXAxis.Checked;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = chkZoomYAxis.Checked;
        }

        private void chkResetOriginalZoom_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if (chkBox.Checked)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
                chkBox.Checked = false;
            }
        }

        //private void btnSaveToFile_Click(object sender, EventArgs e)
        //{
            //UIControl.saveToExcelFile(rbSaveToOpenedFile.Checked, openedExcelFileName, dgvProgressData, txtWorkingDirectory.Text);
        //}

        private void btnClearChart_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            //chart1.Series.Add(utl.addSeries(progressDT, -1, "", Color.Black, MarkerStyle.None, 0, SeriesChartType.Line));
            chart1.Invalidate();
        }

        private void btnSaveChart_Click(object sender, EventArgs e)
        {
            if (chart1.Series.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|png Image|*.png";
                saveFileDialog1.Title = "Save an Image File";
                saveFileDialog1.InitialDirectory = txtWorkingDirectory.Text;
                saveFileDialog1.FilterIndex = 4;
                string message = "The chart is not saved";
                string caption = "Not Saved";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ChartImageFormat frmt = new ChartImageFormat();
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            frmt = ChartImageFormat.Jpeg;
                            break;

                        case 2:
                            frmt = ChartImageFormat.Bmp;
                            break;

                        case 3:
                            frmt = ChartImageFormat.Bmp;
                            break;
                        case 4:
                            frmt = ChartImageFormat.Png;
                            break;
                    }
                    Console.WriteLine(saveFileDialog1.FileName);
                    chart1.SaveImage(@saveFileDialog1.FileName, frmt);
                    message = "Image Saved to " + saveFileDialog1.FileName;
                    caption = "Image Saved";
                }
                MessageBox.Show(message, caption, buttons);
            }
            else
            {
                MessageBox.Show("There is no chart to be saved. ", "No Chart Plotted", MessageBoxButtons.OK);
            }//else
        }

        private void chkSuccessNum_CheckedChanged(object sender, EventArgs e)
        {
            if ((!isfChartOpened) &&  (dgvProgressData.Rows.Count > 1) && (chkSuccessNum.Checked))
            {
                fChart = new FrmChart(this);
                fChart.Show();
                //isfChartOpened = true;
                Point pt = new Point();
                pt.X = this.Size.Width;// + frm2.Size.Width;
                pt.Y = this.Location.Y;
                if ((rbCenter.Checked) || (rbUpperLeft.Checked) || (rbLowerLeft.Checked))
                {
                    pt.X = this.Location.X;// - fChart.Size.Width;
                    pt.Y = this.Location.Y+50;// + this.Size.Height / 2;
                }
                fChart.Location = pt;
            }
            //isfChartOpened = CHT.plotChartToNewForm(chkSuccessNum.Checked, isfChartOpened, fChart, this, dgvProgressData);

            CHT.plotHistogramToNewForm(chkSuccessNum,fChart, this, dgvProgressData);
            return;
       
        }

        private void btnOpenMultipleExcelFile_Click(object sender, EventArgs e)
        {
            openedExcelFileNameMultiple = DGV.openMutlipleSheetExcelFile(txtWorkingDirectory.Text, tabExcelSheet, this, PF, chkTestMode.Checked);

            if (openedExcelFileNameMultiple != "")
            {
                DGV.setNavigationText(DGV.dgvs[0], txtNavigation);
                addDGVEvent();
                //plot
                CheckBox[] plotOptions = new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber };
                if (tabExcelSheet.SelectedIndex >= 0)
                {
                    DataGridView selectedDGV = DGV.dgvs[tabExcelSheet.SelectedIndex];
                    CHT.plotChart(selectedDGV, plotOptions, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
                    CHT.plotHistogramToNewForm(plotOptions[2],fChart, this, selectedDGV);
                }
            }
        }

        private void tabExcelSheet_SelectedIndexChanged(object sender, EventArgs e)
        {           
            DataGridView selectedDGV = DGV.dgvs[tabExcelSheet.SelectedIndex];
            DGV.dgvs[tabExcelSheet.SelectedIndex].ClearSelection();
            CHT.plotChart(selectedDGV, new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber }, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
            CHT.plotHistogramToNewForm(chkPlotSuccessNumber,fChart, this, selectedDGV);
            if (selectedDGV.RowCount == 1)
            {
                txtNavigation.Text = "No Records";
            }
            else
            {
                txtNavigation.Text = (selectedDGV.CurrentCell.RowIndex + 1).ToString() + " of " + (selectedDGV.Rows.Count - 1).ToString();
            }
            //
            for (int i = 0; i < tabExcelSheet.TabCount; i++)
            {
                tabExcelSheet.TabPages[i].Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            }
            tabExcelSheet.TabPages[tabExcelSheet.SelectedIndex].Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Bold);
            //
        }

        private void btnPlotExcelSheet_Click(object sender, EventArgs e)
        {
            CheckBox[] plotOptions = new CheckBox[] { chkCUSUMChart, chkIndividualScore, chkSuccessNum };
            string name = ((Button)sender).Name;
            if (name == "btnPlotExcelSheet")
            {
                plotOptions = new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber };
            }
            if (tabExcelSheet.SelectedIndex >= 0)
            {
                DataGridView selectedDGV = DGV.dgvs[tabExcelSheet.SelectedIndex];
                CHT.plotChart(selectedDGV, plotOptions, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
                CHT.plotHistogramToNewForm(plotOptions[2], fChart, this, selectedDGV);
            }
        }

        private void dgvProgressData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbxTrainee_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            if (cbx.SelectedIndex <= 0)
            {
                errorProvider1.SetError(cbx, "Please Select One Option");
            }
            else
            {
                errorProvider1.SetError(cbx, "");
            }
        }

        private void btnSaveToExcel_Click(object sender, EventArgs e)
        {
            //if (((Control)sender).Name == "btnSave") { rbSaveToAExistingFile.Checked = true; rbSaveToANewFile.Checked = false; }
            //if (((Control)sender).Name == "btnSaveAsNew") { rbSaveToAExistingFile.Checked = false; rbSaveToANewFile.Checked = true; }
            //if (!(((rbSaveToAExistingFile.Checked)&&(openedExcelFileNameMultiple != "")) || (rbSaveToANewFile.Checked)))
            //{ return; }
            try
            {
                //saveToExcelFile(bool saveToOpen, string excelProgressFileName, DataGridView dgv, string workingDirectory, int sheetIndex);
                string[] sheetname = new string[tabExcelSheet.TabCount];
                for (int i =0; i < sheetname.Length; i++) { sheetname[i] = tabExcelSheet.TabPages[i].Text; }
                //DGV.saveToExcelFile(rbSaveToAExistingFile.Checked, openedExcelFileNameMultiple, txtWorkingDirectory.Text, sheetname,DGV.dgvs,this);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, err.Source, MessageBoxButtons.OK);
            }
        }

        private void btnOpenExcelFilewSelection_Click(object sender, EventArgs e)
        {
            //for (int i= tabExcelSheet.TabPages.Count-1; i>0 ; i--)
            //{
            //    MessageBox.Show(i.ToString()+", "+ tabExcelSheet.TabPages.Count.ToString());
            //    tabExcelSheet.TabPages.RemoveAt(0);                
            //}
            //int original = tabExcelSheet.TabPages.Count;
            openedExcelFileNameMultiple=DGV.SelectFileToOpen(txtWorkingDirectory.Text);
            string fileExtension = "";
            if (openedExcelFileNameMultiple == "") { return; }
            else
            {
                fileExtension = Path.GetExtension(openedExcelFileNameMultiple);
                //MessageBox.Show(fileExtension);
                //return;
            }

            openedExcelFileNameMultiple = DGV.openExcelFileWSelection(txtWorkingDirectory.Text,
                tabExcelSheet, this, PF, chkTestMode.Checked, openedExcelFileNameMultiple);
            if (openedExcelFileNameMultiple != "")
            {
                isadd = false;
                addDGVEvent();            
                tabExcelSheet.SelectedIndex = 0;
                CheckBox[] plotOptions = new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber };
                CHT.plotChart(DGV.dgvs[0], plotOptions, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
                //CHT.plotChartToNewForm(plotOptions[2].Checked, isfChartOpened, fChart, this, DGV.dgvs[0]);
                CHT.plotHistogramToNewForm(plotOptions[2],fChart, this, DGV.dgvs[0]);
                if (DGV.dgvs[0].RowCount == 1)
                {
                    txtNavigation.Text = "No Records";
                }
                else
                {
                    txtNavigation.Text = (DGV.dgvs[0].CurrentCell.RowIndex + 1).ToString() + " of " + (DGV.dgvs[0].Rows.Count - 1).ToString();
                }
                string fileNameExt = Path.GetFileName(openedExcelFileNameMultiple);
                lblOpenedFile.Text = "Current Opened File:" + fileNameExt;
                MessageBox.Show("Data in File - " + fileNameExt + " have been read and written to table", "Open File Successfully", MessageBoxButtons.OK);
            }
            else
            {
                if (lblOpenedFile.Text == "No Opened File")
                {
                    lblOpenedFile.Text = "No Opened File";
                }
            }
        }

        private void btnUnselectedRecords_Click(object sender, EventArgs e)
        {
            if (tabExcelSheet.TabPages.Count > 0)
            {
                DGV.dgvs[tabExcelSheet.SelectedIndex].ClearSelection();
                if (DGV.dgvs[tabExcelSheet.SelectedIndex].Rows.Count > 1)
                {
                    DGV.dgvs[tabExcelSheet.SelectedIndex].Rows[DGV.dgvs[tabExcelSheet.SelectedIndex].Rows.Count - 1].Selected = true;
                }
            }
        }

        private void chkPlotCusum_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] plotOptions = new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber };
            if (addNewRecord) { return; } 
            if (tabExcelSheet.SelectedIndex >= 0)
            {
                DataGridView selectedDGV = DGV.dgvs[tabExcelSheet.SelectedIndex];
                CHT.plotChart(selectedDGV, plotOptions, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
                CHT.plotHistogramToNewForm(plotOptions[2], fChart, this, selectedDGV);
            }
        }

        private void dgvProgressData_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //MessageBox.Show("Delete Row");
            if (tabExcelSheet.SelectedIndex >= 0)
            {
                DataGridView dgv = DGV.dgvs[tabExcelSheet.SelectedIndex];
                if (dgv.Rows.Count > 1)
                {
                    txtNavigation.Text = (dgv.CurrentCell.RowIndex + 1).ToString() + " of " + (dgv.Rows.Count - 1).ToString();
                }
                else
                {
                    txtNavigation.Text = " 0 record in Table";
                }
                                             
                CheckBox[] plotOptions = new CheckBox[] { chkPlotCusum, chkPlotScore, chkPlotSuccessNumber };
                DataGridView selectedDGV = DGV.dgvs[tabExcelSheet.SelectedIndex];
                CHT.plotChart(selectedDGV, plotOptions, new CheckBox[] { chkZoomXAxis, chkZoomYAxis }, csp, chart2);
                CHT.plotHistogramToNewForm(plotOptions[2],fChart, this, selectedDGV);
            }
        }


        private void btnChartClear_Click(object sender, EventArgs e)
        {
            chart2.Series.Clear();
            //chart1.Series.Add(utl.addSeries(progressDT, -1, "", Color.Black, MarkerStyle.None, 0, SeriesChartType.Line));
            chart2.Invalidate();
        }

        private void btnChartSave_Click(object sender, EventArgs e)
        {
            if (chart2.Series.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|png Image|*.png";
                saveFileDialog1.Title = "Save an Image File";
                saveFileDialog1.InitialDirectory = txtWorkingDirectory.Text;
                saveFileDialog1.FilterIndex = 4;
                string message = "The chart is not saved";
                string caption = "Not Saved";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ChartImageFormat frmt = new ChartImageFormat();
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            frmt = ChartImageFormat.Jpeg;
                            break;

                        case 2:
                            frmt = ChartImageFormat.Bmp;
                            break;

                        case 3:
                            frmt = ChartImageFormat.Bmp;
                            break;
                        case 4:
                            frmt = ChartImageFormat.Png;
                            break;
                    }
                    Console.WriteLine(saveFileDialog1.FileName);
                    chart2.SaveImage(@saveFileDialog1.FileName, frmt);
                    message = "Image Saved to " + saveFileDialog1.FileName;
                    caption = "Image Saved";
                }
                MessageBox.Show(message, caption, buttons);
            }
            else
            {
                MessageBox.Show("There is no chart to be saved. ", "No Chart Plotted", MessageBoxButtons.OK);
            }//else
        }

        private void dgvProgressData_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //MessageBox.Show("add Row");
            addNewRecord = true;
            isLeavingRow = false;
            //chkPlotSuccessNumber.Checked = false;
            //chkPlotCusum.Checked=false;
            //chkPlotScore.Checked = false;

        }

        private void btnStartNew_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult diaRes = DialogResult.No;
                if ((tabExcelSheet.TabPages.Count > 0))
                {
                    diaRes = MessageBox.Show("Start A New Table will Start from an Empty Table. Continue??", "Empty Table", MessageBoxButtons.YesNo);
                    tabExcelSheet.TabPages[0].Name = "ToBeDeleted";
                }
                if ((tabExcelSheet.TabPages.Count == 0) || (diaRes==DialogResult.Yes))
                {
                    int tabCount = tabExcelSheet.TabPages.Count;
                    for (int i = tabCount - 1; i > 0; i--)
                    {
                        tabExcelSheet.TabPages.RemoveAt(0);
                    }
                    TabPage tPages = new TabPage() ;
                    DataGridView dgv = new DataGridView();
                    DGV.dgvs = new DataGridView[1] { dgv };
                    tPages.Name = "Sheet_1";
                    tPages.Text = "Sheet1";
                    tabExcelSheet.TabPages.Add(tPages);
                    tabExcelSheet.TabPages["Sheet_1"].Controls.Add(DGV.dgvs[0]);
                    DGV.dgvs[0].Name = "dataGridView_" + ("1").ToString();
                    DGV.dgvs[0].Dock = DockStyle.Fill;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Training#");
                    dt.Columns.Add("Success");
                    dt.Columns.Add("Score");
                    DGV.dgvs[0].DataSource = dt;
                    isadd = false;
                    if (tabExcelSheet.TabPages.Count > 1) { tabExcelSheet.TabPages.RemoveAt(0); }
                }
                txtNavigation.Text = " No Record";
                addDGVEvent();


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Start New Table Error", MessageBoxButtons.OK);
            }
            return;
        }

        private void chkAdjustSizePosition_CheckedChanged(object sender, EventArgs e)
        {
            
            gBoxGUIPosition.Enabled = !chkAdjustSizePosition.Checked;
            if (!chkAdjustSizePosition.Checked)
            {
                this.Location = originalPosition;
                this.Size = originalSize;
            }
            else
            {
                Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
                Point pt = new Point();
                pt.X = (workingRectangle.Width - this.Width) / 2;
                int spacePortion = 6; ;
                pt.Y = (workingRectangle.Height) / spacePortion;
                this.Location = pt;
                this.Height = (workingRectangle.Height) / spacePortion * (spacePortion - 2);
            }
        }

        private void numUDFormPosY_ValueChanged(object sender, EventArgs e)
        {
            if (chkAdjustSizePosition.Checked)
            {
                int spacePortion = 6; ;
                Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
                Point pt = new Point();
                pt.X = this.Location.X;
                pt.Y = (workingRectangle.Height) / spacePortion + (6-(int)numUDFormPosY.Value) * 10;
                this.Location = pt;
                this.Height = (workingRectangle.Height) / spacePortion * (spacePortion - 2) + ((int)numUDFormPosY.Value - 6) * 20;
            }
        }

        private void dgvProgressData_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("leave Row");
            
            addNewRecord = false;
            isLeavingRow = true;
        }

        private void dgvProgressData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(lastCellPos[0].ToString() + ", " + lastCellPos[1].ToString()+", "+ currentCellPos[0].ToString()+", "+ currentCellPos[1].ToString());
            lastCellPos[0] = currentCellPos[0];
            lastCellPos[1] = currentCellPos[1];
            currentCellPos[0] = e.RowIndex;
            currentCellPos[1] = e.ColumnIndex;
            if (lastCellPos[0] == -1)
            {
                lastCellPos[0] = currentCellPos[0];
                lastCellPos[1] = currentCellPos[1];
            }
        }

        private void dgvProgressData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString()); // &&
            if (addNewRecord || e.RowIndex<=DGV.dgvs[tabExcelSheet.SelectedIndex].RowCount-2)
            {
                if ( e.FormattedValue.ToString() == string.Empty)
                {
                    MessageBox.Show("Cell Must Have a Value!!", "Empty");
                    e.Cancel = true;
                }
            }
            if (e.RowIndex == DGV.dgvs[tabExcelSheet.SelectedIndex].RowCount - 1)
            {
                if (!(e.FormattedValue.ToString() == string.Empty))
                {
                    chkPlotCusum.Checked = true;
                    chkPlotScore.Checked = true;
                    chkPlotSuccessNumber.Checked = true;
                }
            }
        }

        //=======================================================================================
        private void button2_Click_1(object sender, EventArgs e)
        {

            fAChart = new FrmAllChart(DGV.dgvs,this);
            if (!isFAChartOPened)
            {
                fAChart.Show();
                isFAChartOPened = true;
            }
            fAChart.mainChart.Series.Clear();
            int numOfChart = tabExcelSheet.TabCount;
            fAChart.xAxisTick = new string[numOfChart];
            //get series
            Series[] ser = { new Series(), new Series() };
            int maxValue = 0;
            for (int i=0; i < numOfChart; i ++)
            {
                int[] vals = new int[2] { 0, 0 };
                for (int j=0; j<DGV.dgvs[i].RowCount-1;j++)
                {
                    
                    int res = int.Parse(DGV.dgvs[i].Rows[j].Cells[1].Value.ToString());
                    vals[res] = vals[res] + 1;
                }
                fAChart.xAxisTick[i] = tabExcelSheet.TabPages[i].Text;
                ser[0].Points.AddXY(tabExcelSheet.TabPages[i].Text, vals[0]);
                ser[1].Points.AddXY(tabExcelSheet.TabPages[i].Text, vals[1]);
                maxValue = Math.Max(maxValue, vals[0]+vals[1]);
                vals = new int[2] { 0, 0 };
            }
            ;
            ser[0].Name = "Not Success";
            ser[1].Name = "Success";
            ser[0].Color = System.Drawing.Color.Orange;
            ser[1].Color = System.Drawing.Color.Blue;
            ser[0].ChartType =SeriesChartType.StackedColumn;
            ser[1].ChartType =SeriesChartType.StackedColumn;
            ser[0].IsVisibleInLegend = true;
            ser[1].IsVisibleInLegend = true;
            fAChart.mainChart.Series.Add(ser[1]);
            fAChart.mainChart.Series.Add(ser[0]);
            fAChart.mainChart.ChartAreas[0].RecalculateAxesScale();
            fAChart.mainChart.Legends["Legend1"].Docking = Docking.Bottom;
            fAChart.mainChart.ChartAreas[0].AxisY.Maximum = maxValue*1.25;
            fAChart.mainChart.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            fAChart.mainChart.ChartAreas[0].BorderColor = System.Drawing.Color.Black;
            fAChart.mainChart.ChartAreas[0].BorderWidth = 3;
            fAChart.mainChart.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Visible = checkBox1.Checked;
            chkAdjustSizePosition.Visible = checkBox1.Checked;
            numUDFormPosY.Visible = checkBox1.Checked;
        }

        //==================================================================================================
        private void addDGVEvent()
        {
            for (int i = 0; i < DGV.dgvs.Length; i++)
            {
                //DGV.dgvs[i].AutoSize=Datagridvi;
                DGV.dgvs[i].CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvs_CellMouseDown);
                DGV.dgvs[i].CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProgressData_CellContentClick);
                DGV.dgvs[i].CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProgressData_CellEndEdit);
                DGV.dgvs[i].CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProgressData_CellLeave);
                DGV.dgvs[i].EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvProgressData_EditingControlShowing);
                DGV.dgvs[i].UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvProgressData_UserDeletedRow);
                DGV.dgvs[i].UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvProgressData_UserAddedRow);
                DGV.dgvs[i].RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProgressData_RowLeave);
                DGV.dgvs[i].CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProgressData_CellEnter);
                DGV.dgvs[i].CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvProgressData_CellValidating);
            }


        }

        private void btnBackToOriginalZoom_Click(object sender, EventArgs e)
        {
            circleRadius = 0;
            chart2.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            circleRadius = 0;
            chart2.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);            
            //MessageBox.Show("call reset");
        }

        private void chart2_MouseHover(object sender, EventArgs e)
        {

        }

        private void chart2_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos.X = e.X;
            mousePos.Y = e.Y;
            if (chart2.Series.Count>0)
            chart2.Invalidate();
        }
        //***************************************************************************************************

        //==================================================================================================
        private void chart2_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                zoomInOut(sender, e);
            }
            catch { }
        }

        private void chart2_Paint(object sender, PaintEventArgs e)
        {
            //MessageBox.Show("Run Paint");
            if (((Chart)sender).Series.Count == 0){ return; }
            if (!((Control)sender is Button))
            {
                Pen pen = new Pen(Brushes.DarkGoldenrod);
                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(64, 255 * 3 / (maxViewPoint - currentView + 3), 0, 0));
                int r = circleRadius;
                e.Graphics.FillEllipse(semiTransBrush, mousePos.X - r / 2, mousePos.Y - r / 2, r, r);
                if (r == 0)
                {
                    circleRadius = 20;
                }
            }
        }

        private void chart2_MouseLeave(object sender, EventArgs e)
        {
            circleRadius = 0;
            chart2.Invalidate();
        }

        private void chart2_MouseClick(object sender, MouseEventArgs e)
        {
            zoomInOut(sender, e);
        }
        //***************************************************************************************************
        private void zoomInOut(object sender, MouseEventArgs e)
        {
            
            var chart = (Chart)sender;
            if (chart.Series.Count == 0){ return; }
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;
            var xMin = xAxis.ScaleView.ViewMinimum;
            var xMax = xAxis.ScaleView.ViewMaximum;
            var yMin = yAxis.ScaleView.ViewMinimum;
            var yMax = yAxis.ScaleView.ViewMaximum;
            var posXStart = 0.0d;
            var posXFinish = 0.0d;
            var posYStart = 0.0d;
            var posYFinish = 0.0d;
            double step = 2;
            if (e.Button == MouseButtons.Left || e.Delta > 0)
            {
                if (currentView == maxViewPoint)
                { return; }

                posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / step;
                posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / step;
                posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / (step);
                posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / (step);
                viewXPos[currentView] = new Point((int)posXStart, (int)posXFinish);
                viewYPos[currentView] = new Point((int)posYStart, (int)posYFinish);
                circleRadius += (int)(step * 2);
                currentView++;

            }
            if (e.Button == MouseButtons.Right || e.Delta < 0)
            {
                if (currentView == 0) //Last scrolled dowm
                {
                    yAxis.ScaleView.ZoomReset();
                    xAxis.ScaleView.ZoomReset();
                    return;
                }
                posXStart = viewXPos[currentView - 1].X;
                posXFinish = viewXPos[currentView - 1].Y;
                posYStart = viewYPos[currentView - 1].X;
                posYFinish = viewYPos[currentView - 1].Y;
                circleRadius -= (int)(step * 2); ;
                currentView--;
            }
            xAxis.ScaleView.Zoom(posXStart, posXFinish);
            yAxis.ScaleView.Zoom(posYStart, posYFinish);
        }

        private void rbSaveToANewFile_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lLabelSignIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (lLabelSignIn.Text == "Sign In")
            {
                if (cbxUserName.SelectedIndex > 0)
                {
                    if (txtPassWord.Text == "PassWord")
                    {
                        lLabelSignIn.Text = "Sign Out";
                        lblLoginUser.Text = cbxUserName.Items[cbxUserName.SelectedIndex].ToString();
                        cbxUserName.SelectedIndex = 0;
                        txtPassWord.Text = "";                        
                        lblLoginTime.Text = DateTime.Now.ToString("h:mm tt");
                        tabGUI.Enabled = true;
                        pnlLogin.Visible = false;
                        pnlLoginInf.Visible = true;
                    }
                }
            }
            else
            {
                lLabelSignIn.Text = "Sign In";
                lblUserName.Text = "User Name";
                lblPassWord.Text = "Password";
                CuSumAnalysisGUI NewForm = new CuSumAnalysisGUI();
                NewForm.Show();
                this.Dispose(false);
                pnlLoginInf.Visible = false;
                pnlLogin.Visible = true;
            }


        }

        private void treeViewHospital_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tn = treeViewHospital.SelectedNode;

            if (tn == null)
                Console.WriteLine("No tree node selected.");
            else
                MessageBox.Show("Selected tree node:"+ tn.Text);
        }
        //==================================================================================================

        //****
        //==================================================================================================
        private void treeViewHospital_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (!e.Node.Checked)
                {
                    foreach (TreeNode child in e.Node.Nodes)
                    {
                        child.Checked = false;
                    }
                    e.Node.Collapse(false);
                }
                else
                {
                    e.Node.ExpandAll();
                }
            }
            else
            {
                if (e.Node.Checked)
                {
                    
                    foreach (TreeNode cur_node in e.Node.Parent.Nodes)
                    {
                        if (cur_node != e.Node)
                        {
                            cur_node.Checked = false;
                        }
                    }
                    MessageBox.Show(e.Node.Text);
                }
            }


        }

        private void treeViewHospital_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Checked = true;
        }

        private void treeViewHospital_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.Checked = false;
        }

        private void rbCEPODElective_Click(object sender, EventArgs e)
        {
            ((RadioButton)sender).Checked = !((RadioButton)sender).Checked;
            foreach (Control ctl in groupBox12.Controls)
            {
                if (((RadioButton)ctl)!= ((RadioButton)sender))
                {
                    ((RadioButton)ctl).Checked = false;
                }
            }
        }

        private void cbxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxOperation.SelectedIndex > 0)
            {
                lbxOperations.Items.Clear();
                string selectedOp = cbxOperation.Items[cbxOperation.SelectedIndex].ToString();
                string fileName = @".\Text Files\operations\"+ selectedOp +".txt";
                var reader = new StreamReader(File.OpenRead(fileName));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Trim().Length > 0)
                        lbxOperations.Items.Add(line);
                }
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void txtPatientAgeYears_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                //MessageBox.Show("Get here", "check input Value");                
                e.Handled = true;
            }
            else
            {
                if (!char.IsControl(e.KeyChar))
                {
                    int numOfDigits = (sender as TextBox).Text.Length;
                    if ((((Control)sender).Name == "txtPatientAgeYears"))
                    {                        
                        if ((numOfDigits == 2) && (int.Parse((sender as TextBox).Text)>20) || (numOfDigits >= 3) || 
                            (numOfDigits == 1) && (e.KeyChar =='0') && (sender as TextBox).Text=="0")
                            e.Handled = true;
                    }
                    if (((Control)sender).Name == "txtPatientAgeMonths")
                    {
                        if ( ((numOfDigits == 1) && (int.Parse((sender as TextBox).Text) > 1)) || (numOfDigits >= 2) ||
                            ((numOfDigits == 1) && (int.Parse(e.KeyChar.ToString())>=2) && (int.Parse((sender as TextBox).Text) != 0)) 
                            )
                            e.Handled = true;
                    }
                    if (((Control)sender).Name == "txtPatientID")
                    {
                        if (numOfDigits >= 10)
                            e.Handled = true;
                    }

                }
            }
        }

        private void txtPatientAgeMonths_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        //***************************************************************************************************

    }//class
}
