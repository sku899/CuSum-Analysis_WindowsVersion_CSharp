using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CuSum_Analysis
{
    public partial class FrmAllChart : Form
    {
        public DataGridView[] dgvs;
        DataGridView selectedDGV;
        public string[] xAxisTick;
        CuSumAnalysisGUI mainForm;
        public FrmAllChart(DataGridView[] dgv,CuSumAnalysisGUI mainForm)
        {
            InitializeComponent();
            dgvs= dgv;
            this.mainForm = mainForm;
        }

        private void FrmAllChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            //mainform.isfChartOpened = false;
            this.Hide();
            this.Parent = null;
            e.Cancel = true;
            mainForm.isFAChartOPened = false;
        }

        private void mainChart_MouseClick(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            var results = mainChart.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    //MessageBox.Show(result.PointIndex.ToString());
                    selectedDGV = dgvs[result.PointIndex];
                    extendingChart(dgvs[result.PointIndex]);
                    lblSelectedStep.Text = xAxisTick[result.PointIndex];
                }
            }
        }
        private void extendingChart(DataGridView dgv)
        {
            int numOfStages = tBarStage.Value;
            if (dgv.RowCount < 5)
            {
                numOfStages = 1;
                tBarStage.Value = 1;
            }
            //int numPerSatge = (int)(dgvFChart.RowCount / numOfStages);
            int remainder = dgv.RowCount % numOfStages;
            int numPerSatge = (int)(dgv.RowCount - remainder) / numOfStages;
            int extraItem = 0;
            if (remainder != 0)
            {
                extraItem = 1;
            }
            //if ((dgvFChart.RowCount % numOfStages) > 0) numOfStages++;
            //MessageBox.Show("Stage: " + numOfStages.ToString() + ", perStage: " + numPerSatge.ToString());
            int[] vals = new int[numOfStages];
            int[] valn = new int[numOfStages];
            for (int i = 0; i < numOfStages; i++)
            { vals[i] = 0; valn[i] = 0; }

            int currentStage = 1;
            Series ser0 = new Series();
            Series ser1 = new Series();
            //Series ser2 = new Series();
            //Series ser3 = new Series();
            int[] val = new int[2] { 0, 0 };
            double[] per = new double[2] { 0.00d, 0.00d };

            extendedChart.Series.Clear();
            //fChart.chart2.Series.Add(ser);
            for (int i = 0; i < dgv.RowCount; i++)
            {

                if ((i == (currentStage * numPerSatge + extraItem)) || (i == (dgv.RowCount - 1)))
                {
                    if (tBarStage.Value == 1)
                    //if (numOfStages == 1)
                    {
                        ser0.Points.AddXY("Overall", val[0]);
                        ser1.Points.AddXY("Overall", val[1]);
                        per[0] = ((double)val[0] / (val[0] + val[1]) * 100.0d);
                        per[1] = ((double)val[1] / (val[0] + val[1]) * 100.0d);
                    }
                    else
                    {
                        ser0.Points.AddXY("Stage" + currentStage.ToString(), val[0]);
                        ser1.Points.AddXY("Stage" + currentStage.ToString(), val[1]);

                    }
                    if (extraItem < remainder - 1)
                    {
                        extraItem++;
                    }
                    currentStage++;
                    val = new int[2] { 0, 0 };
                }
                if (i < (dgv.RowCount - 1))
                {
                    int result = int.Parse((string)dgv.Rows[i].Cells[1].Value);
                    val[result] = val[result] + 1;
                    Console.WriteLine("Adding");

                }
            }

            ser0.Name = "Not Success";
            ser1.Name = "Success";
            ser0.IsVisibleInLegend = true;
            ser1.IsVisibleInLegend = true;
            ser0.Color = System.Drawing.Color.Orange;
            ser1.Color = System.Drawing.Color.Blue;
            ser0.ChartType = SeriesChartType.Column;
            ser1.ChartType = SeriesChartType.Column;
            extendedChart.Series.Add(ser0);
            extendedChart.Series.Add(ser1);
            //tooltip
            foreach (Series s in extendedChart.Series)
            {
                s.ToolTip = "#VALX\n" + s.Name + ": #VALY\nPercentage: #PERCENT";
            }
            //
            if (tBarStage.Value == 1)
            //if (numOfStages == 1)
            {
                for (int si = 0; si < 2; si++)
                {

                    foreach (DataPoint p in extendedChart.Series[si].Points)
                    {
                        p.Label = extendedChart.Series[si].Name + "\n #VALY\n(" + per[si].ToString("0.00") + "%)"; //(#VALY)\n#PERCENT";
                    }
                }
            }

            DataPoint[] maxDP = { extendedChart.Series[0].Points.FindMaxByValue(), extendedChart.Series[1].Points.FindMaxByValue() };
            double maxYValue = Math.Max(maxDP[0].YValues[0], maxDP[1].YValues[0]);
            //MessageBox.Show(maxYValue.ToString());
            extendedChart.ChartAreas[0].AxisY.Maximum = Math.Max(5, maxYValue + 1);

            if (extendedChart.ChartAreas[0].AxisY.Maximum <= 6)
            {
                extendedChart.ChartAreas[0].AxisY.Interval = 1;
            }
            else
            {

                extendedChart.ChartAreas[0].RecalculateAxesScale();
                extendedChart.ChartAreas[0].AxisY.Maximum = extendedChart.ChartAreas[0].AxisY.Maximum * 1.25;
                double sp = 8;
                int interval = (int)(Math.Ceiling((extendedChart.ChartAreas[0].AxisY.Maximum)) / sp);
                int r = interval % 10;
                if (r <= 5)
                {
                    interval = interval - r;
                }
                if ((r > 2) && (r < 8))
                { }//interval = interval - (interval % 5); 
                if (r >= 6)
                { interval = interval - r + 10; }
                extendedChart.ChartAreas[0].AxisY.Interval = (interval);
            }

            extendedChart.Invalidate();
            extendedChart.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            extendedChart.ChartAreas[0].BorderColor = System.Drawing.Color.Black;
            extendedChart.ChartAreas[0].BorderWidth = 3;
            //lblTotal.Text = "Total Attempts: " + (dgvFChart.RowCount - 1).ToString();
            //setTBarValue(tBarStage.Value);


        }

        private void mainChart_Click(object sender, EventArgs e)
        {

        }

        private void tBart_Scroll(object sender, EventArgs e)
        {
            extendingChart(selectedDGV);
        }
    }//class
}
