using Microsoft.Office.Core;
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
    public partial class FrmChart : Form
    {
        CuSumAnalysisGUI mainform;
        public SeriesChartType sct ;
        public DataGridView dgvFChart;
        ToolTip tooltip = new ToolTip();
        public int stagePBarVal;
        CheckBox chkPlot = new CheckBox();
        public FrmChart(CuSumAnalysisGUI frm)
        {
            InitializeComponent();
            mainform = frm;
            stagePBarVal = 1;
            sct =SeriesChartType.Pie;
            dgvFChart = new DataGridView();
        }

        private void FrmChart_Load(object sender, EventArgs e)
        {
            lblTBarValue.Location = new Point(tBarStage.Location.X - 5, tBarStage.Location.Y + tBarStage.Height / 2);
            chkPlot.Checked = true;
        }

        private void FrmChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainform.isfChartOpened = false;
            this.Hide();
            this.Parent = null;
            e.Cancel = true;
        }

        private void rbBarChart_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPieChart.Checked)
            {
                chart2.Series[0].ChartType = SeriesChartType.Pie;
                chart2.Series[0].IsValueShownAsLabel = true; ;
                sct= SeriesChartType.Pie;
                foreach (DataPoint p in chart2.Series[0].Points)
                {
                    p.Label = "#VALX\n(#VALY)\n#PERCENT";
                }
            }
            else
            {
                chart2.Series[0].ChartType = SeriesChartType.Bar;
                chart2.Series[0].IsValueShownAsLabel = true; ;
                sct= SeriesChartType.Bar;
                foreach (DataPoint p in chart2.Series[0].Points)
                {
                    p.Label = "#VALX\n(#VALY)\n#PERCENT";
                }
            }
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void tBarStage_Scroll(object sender, EventArgs e)
        {
            stagePBarVal = tBarStage.Value;
            CHT.plotHistogramToNewForm(chkPlot,this, mainform, dgvFChart);
            return;
            int numOfStages = tBarStage.Value;
            if (dgvFChart.RowCount < 5)
            {
                numOfStages = 1;
                tBarStage.Value = 1;
            }
            //int numPerSatge = (int)(dgvFChart.RowCount / numOfStages);
            int remainder = dgvFChart.RowCount % numOfStages;
            int numPerSatge = (int)(dgvFChart.RowCount-remainder) / numOfStages;
            int extraItem = 0;
            if (remainder!=0)
            {
                extraItem = 1;
            }
            //if ((dgvFChart.RowCount % numOfStages) > 0) numOfStages++;
            //MessageBox.Show("Stage: " + numOfStages.ToString() + ", perStage: " + numPerSatge.ToString());
            int currentStage = 1;
            int[] val = new int[2] { 0, 0 };
            double[] per = new double[2] { 0.00d,0.00d };
            Series[] series = { new Series { }, new Series { } };

            chart2.Series.Clear();
            for (int i = 0; i < dgvFChart.RowCount; i++)
            {
                if ((i == (currentStage * numPerSatge + extraItem)) || (i == (dgvFChart.RowCount - 1)))
                {
                    for (int j = 0; j < series.Length; j++)
                    {
                        if (tBarStage.Value == 1)
                        {
                            series[j].Points.AddXY("Overall", val[j]);
                            per[j] = ((double)val[j] / (val[0] + val[1]) * 100.0d);
                        }
                        else
                        {
                            series[j].Points.AddXY("Stage" + currentStage.ToString(), val[j]);
                        }
                    }
                    //reset to next stage
                    if (extraItem < remainder - 1)
                    {
                        extraItem++;
                    }
                    currentStage++;
                    val = new int[2] { 0, 0 };
                }
                if (i < (dgvFChart.RowCount - 1))
                {
                    int result = int.Parse((string)dgvFChart.Rows[i].Cells[1].Value);
                    val[result]++;
                    Console.WriteLine("Adding");
                }
            }
            string[] seriesName = { "Not Success", "Success" };
            Color[] seriesColor = { Color.Orange, Color.Blue };
            SeriesChartType[] sCT = { SeriesChartType.Column , SeriesChartType.Column };    
            for (int si=0;si<series.Length;si++)
            {
                series[si].Name = seriesName[si];
                series[si].IsVisibleInLegend = true;
                series[si].Color = seriesColor[si];
                series[si].ChartType = sCT[si];
                chart2.Series.Add(series[si]);
                chart2.Series[si].ToolTip = "#VALX\n" + series[si].Name + ": #VALY\nPercentage: #PERCENT";
                if (tBarStage.Value == 1)
                {
                    foreach (DataPoint p in chart2.Series[si].Points)
                    {
                        p.Label = chart2.Series[si].Name + "\n #VALY\n(" + per[si].ToString("0.00") + "%)"; //(#VALY)\n#PERCENT";
                    }
                }
            }
            DataPoint[] maxDP = { chart2.Series[0].Points.FindMaxByValue(), chart2.Series[1].Points.FindMaxByValue() };
            double maxYValue = Math.Max(maxDP[0].YValues[0], maxDP[1].YValues[0]);
            //MessageBox.Show(maxYValue.ToString());
            double sp = 8;
            chart2.ChartAreas[0].AxisY.Maximum = Math.Max(8,maxYValue + 1);

            chart2.ChartAreas[0].AxisY.Interval = 1;
            
            if (chart2.ChartAreas[0].AxisY.Maximum > sp)
            {
                //chart2.ChartAreas[0].RecalculateAxesScale();
                chart2.ChartAreas[0].AxisY.Maximum = chart2.ChartAreas[0].AxisY.Maximum * 1.25;                
                int interval = (int)(Math.Ceiling((chart2.ChartAreas[0].AxisY.Maximum))/sp);
                int r = interval % 5;
                if (r <= 2)
                {
                    interval = interval - r;
                }
                //interval = interval - (interval % 5); 
                if (r >= 3)
                { interval = interval - r + 5; }
                chart2.ChartAreas[0].AxisY.Interval =(interval);
            }

            chart2.Invalidate();
            chart2.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            chart2.ChartAreas[0].BorderColor = System.Drawing.Color.Black;
            chart2.ChartAreas[0].BorderWidth = 3;
            lblTotal.Text = "Total Attempts: " + (dgvFChart.RowCount-1).ToString();
            setTBarValue(tBarStage.Value);
        }
        public void changeLabel(int total)
        {
            lblTotal.Text = "Total Attempts: " + (total).ToString();
        }

        private void chart2_MouseClick(object sender, MouseEventArgs e)
        {


        }

        private void chart2_MouseMove(object sender, MouseEventArgs e)
        {
            ////MessageBox.Show("mouse click");
            //var pos = e.Location;
            //if (prevPosition.HasValue && pos == prevPosition.Value)
            //    return;
            //tooltip.RemoveAll();
            //prevPosition = pos;
            //var results = chart2.HitTest(pos.X, pos.Y, false,
            //                                ChartElementType.DataPoint);
            //foreach (var result in results)
            //{
            //    if (result.ChartElementType == ChartElementType.DataPoint)
            //    {
            //        var prop = result.Object as DataPoint;

            //        if (prop != null)
            //        {
            //            var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
            //            var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);
            //            //MessageBox.Show("mouse click", "3");
            //            // check if the cursor is really close to the point (2 pixels around the point)
            //            //if (Math.Abs(pos.X - pointXPixel) < 20 &&
            //            //    Math.Abs(pos.Y - pointYPixel) < 20)
            //            //{
            //            tooltip.Show("X=" + prop.XValue.ToString() + ", Y=" + prop.YValues[0], this.chart2,
            //                            pos.X, pos.Y - 15);

            //            //}
            //        }
            //    }
            //}

        }

        private void chart2_MouseUp(object sender, MouseEventArgs e)
        {

        }
        public void setTBarValue(int val)
        {
            tBarStage.Value = val;
            lblTBarValue.Text = "Number of Stage: " + val;
            lblTBarValue.Location = new Point(tBarStage.Location.X - 5 + (tBarStage.Width-lblTBarValue.Width) * (val - tBarStage.Minimum) / (tBarStage.Maximum - tBarStage.Minimum),
                        tBarStage.Location.Y + tBarStage.Height / 2);
            
        }
    }
}
