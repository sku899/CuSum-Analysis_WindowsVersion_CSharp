using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Data;

namespace CuSum_Analysis
{
    class CHT
    {
        //=================================================================================================================================//
        public static Series addSeries(string seriesName, System.Drawing.Color c, MarkerStyle m, int lineWidth, SeriesChartType st)
        {
            var series1 = new Series
            {
                Name = seriesName,
                Color = c,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                MarkerStyle = m,
                BorderWidth = lineWidth,
                ChartType = st
            };
            //for (int i = 0; i < dt.Rows.Count; i++)
            for (int i = 0; i < 100; i++)
            {
                series1.Points.AddXY(i, 3 * i);
            }

            return series1;
        }
        //*********************************************************************************************************************************//

        //=================================================================================================================================//
        public static Series addCuSumSeries(string seriesName, System.Drawing.Color c, MarkerStyle m, int lineWidth, SeriesChartType st,CuSumParameters cs, DataGridView dgv1,bool isSum)
        {
            var series1 = new Series
            {
                Name = seriesName,
                Color = c,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                MarkerStyle = m,
                BorderWidth = lineWidth,                
                ChartType = st
                
            };
            decimal val = 0.0m;
            for (int i=0; i < dgv1.Rows.Count-1; i++)
            {
                decimal[] scores = { 1 - cs.s, -cs.s };
                int result = int.Parse(dgv1.Rows[i].Cells[1].Value.ToString());
                if (isSum)
                {
                    val = val + scores[result];
                }
                else
                {
                    val = scores[result];
                }

                series1.Points.AddXY(int.Parse(dgv1.Rows[i].Cells[0].Value.ToString()), val);
            }
            return series1;
        }

        //*********************************************************************************************************************************//
        public static void plotChart(DataGridView dgvProgressData, CheckBox[] chkbox, CheckBox[] chkzoom, CuSumParameters csp,Chart chart1)
        {
            if ((dgvProgressData.RowCount <= 1) || (!((chkbox[0].Checked) || (chkbox[1].Checked))))
            {
                //MessageBox.Show("No data to plot", "No Data", MessageBoxButtons.OK);
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();
                chart1.Invalidate();
                chart1.ChartAreas.Add(new ChartArea());
                return;
            }
           try
            {
                chart1.ChartAreas.Clear();
                chart1.ChartAreas.Add(new ChartArea());
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = chkzoom[0].Checked;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = chkzoom[1].Checked;
                chart1.Series.Clear();
                if ((chkbox[0].Checked) || (chkbox[1].Checked))
                {
                    Series ser = new Series();
                    if (chkbox[0].Checked)
                    {
                        ser = addCuSumSeries("CusUm", System.Drawing.Color.Black, MarkerStyle.Circle, 2, SeriesChartType.Line, csp, dgvProgressData, true);
                        chart1.Series.Add(ser);
                    }
                    if ((chkbox[1].Checked))
                    {
                        ser = CHT.addCuSumSeries("Individual", System.Drawing.Color.Blue, MarkerStyle.None, 2, SeriesChartType.Column, csp, dgvProgressData, false);
                        chart1.Series.Add(ser);
                    }
                    double maxYValue = 0;
                    double minYValue = 0;
                    //chart1.Series[0].YAxisType = AxisType.Primary;
                    //chart1.Series[1].YAxisType = AxisType.Secondary;
                    DataPoint maxDP = chart1.Series[0].Points.FindMaxByValue();
                    maxYValue = maxDP.YValues[0];
                    //MessageBox.Show(maxYValue.ToString());
                    chart1.ChartAreas[0].AxisY2.Maximum = 2 * Math.Ceiling(maxYValue);
                    DataPoint minDP = chart1.Series[0].Points.FindMinByValue();
                    minYValue = minDP.YValues[0];
                    //chart1.ChartAreas[0].AxisY2.Minimum = -Math.Ceiling(maxYValue);

                    //chart1.ChartAreas[0].AxisY2.Interval = Math.Round((double)(maxYValue / 2), 0);
                    int numOfInterval = 0;
                    while ((double)(numOfInterval * csp.h0) < Math.Max(Math.Abs(minYValue), maxYValue))
                    {
                        numOfInterval++;
                    }
                    //int numOfIntervalNa = 0;
                    //while ((double)(numOfIntervalNa * (-csp.h0)) > minYValue)
                    //{
                    //    numOfIntervalNa++;
                    //}
                    numOfInterval = Math.Max(2, numOfInterval);

                    chart1.ChartAreas[0].AxisX.Maximum = chart1.Series[0].Points.Count + 1;
                    chart1.ChartAreas[0].AxisX.Minimum = 0.5;

                    //MessageBox.Show("Y max: " + Math.Ceiling((double)(numOfInterval * csp.h0)).ToString() + ", Ymin: " + Math.Floor((double)(-numOfInterval * csp.h0)).ToString());
                    //if (chart1.Series[0].Points.Count <= 5)
                    //{
                    //    chart1.ChartAreas[0].AxisY.Maximum = (double)(4* csp.h0);
                    //    chart1.ChartAreas[0].AxisY.Minimum = (double)(-4* csp.h0);
                    //}
                    //else
                    //{
                    chart1.ChartAreas[0].AxisY.Maximum = Math.Ceiling((double)(numOfInterval * csp.h0));
                    chart1.ChartAreas[0].AxisY.Minimum = Math.Floor((double)(-numOfInterval * csp.h0));
                    //}
                    chart1.ChartAreas[0].AxisY.Interval = Math.Round((double)(csp.h0), 2);
                    chart1.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular);
                    //add tick label in Y
                    CustomLabel labelY;
                    int a = 1;
                    int increament = 1;
                    if (numOfInterval >= 8)
                    {
                        increament = 2;
                    }
                    if (numOfInterval >= 16)
                    {
                        increament = 4;
                    }
                    if (numOfInterval >= 24)
                    {
                        increament = 8;
                    }
                    if (numOfInterval >= 32)
                    {
                        increament = 12;
                    }
                    if ((numOfInterval % increament) != 0)
                        numOfInterval = numOfInterval - (numOfInterval % increament)+increament;



                    //for (int i = 0; i <= numOfInterval; i = i + increament)
                    int yStartPos = numOfInterval;
                    for (int i = yStartPos; i >=0; i = i - increament)
                    {

                        if (i == 0)
                        {
                            labelY = new CustomLabel();
                            labelY.FromPosition = 0;
                            labelY.ToPosition = 0.05;
                            labelY.Text = "0.0";
                            chart1.ChartAreas[0].AxisY.CustomLabels.Add(labelY);
                        }
                        else
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                labelY = new CustomLabel();
                                labelY.FromPosition = 0;
                                labelY.ToPosition = (double)(2 * a * (numOfInterval * csp.h0 - (i) * csp.h0));
                                labelY.Text = (a * (numOfInterval - i)).ToString() + "h\u2080";

                                a = a * (-1);
                                chart1.ChartAreas[0].AxisY.CustomLabels.Add(labelY);
                            }
                        }
                    }//making label
                    chart1.ChartAreas[0].AxisY.Title = "Scores";
                    chart1.ChartAreas[0].AxisX.Title = "Number of Attempts";

                    chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                    chart1.ChartAreas[0].BorderColor = System.Drawing.Color.DarkSlateBlue;
                    chart1.ChartAreas[0].BorderWidth = 3;
                }

                chart1.Invalidate();
                //MessageBox.Show("Plot");
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Plot CUSUM");
            }
        }
        //=================================================================================================================================//
        public static bool plotChartToNewForm(bool plotToNewForm, bool newFormOpened, FrmChart fChart, CuSumAnalysisGUI MainForm, DataGridView dgv)
        {
            bool isOpened = newFormOpened;
            if ((plotToNewForm) && (dgv.Rows.Count > 1))
            {
                try {
                    SeriesChartType sct = fChart.chart2.Series[0].ChartType;                    
                    //add bar series
                    int[] vals = new int[2] { 0, 0 };
                    for (int i = 0; i < dgv.RowCount - 1; i++)
                    {
                        int result = int.Parse((string)dgv.Rows[i].Cells[1].Value);
                        vals[result] = vals[result] + 1;
                    }
                    fChart.chart2.ChartAreas.Clear();
                    fChart.chart2.ChartAreas.Add(new System.Windows.Forms.DataVisualization.Charting.ChartArea());
                    fChart.chart2.Series.Clear();
                    Series ser = new Series();
                    fChart.chart2.Series.Add(ser);
                    ser.Points.DataBindXY(new string[] { "Not Success", "Sucess" }, vals);
                    ser.ChartType = fChart.sct;

                    foreach (DataPoint p in fChart.chart2.Series[0].Points)
                    {
                        p.Label = "#VALX\n(#VALY)\n#PERCENT";
                    }
                    fChart.chart2.Legends.Clear();
                    fChart.chart2.Series[0].Points[0].Color = System.Drawing.Color.Orange;
                    fChart.chart2.Invalidate();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Plt Pie", MessageBoxButtons.OK);
                }
            }
                
            return isOpened;
        }
        //*********************************************************************************************************************************//

        //=================================================================================================================================//
        public static void plotHistogramToNewForm(CheckBox chkPlot, FrmChart fChart, CuSumAnalysisGUI MainForm, DataGridView dgv)
        {
                     
            if ( (dgv.Rows.Count > 1) && (chkPlot.Checked))
            {
                try
                {
                    fChart.Show();
                    if (MainForm.Location.X - fChart.Width < 0)
                    {
                        //MessageBox.Show((MainForm.Location.X + MainForm.Width).ToString()+);
                        fChart.Location = new System.Drawing.Point(MainForm.Location.X - 30, MainForm.Location.Y);
                    }
                    else
                    {
                        fChart.Location = new System.Drawing.Point(MainForm.Location.X - fChart.Width, MainForm.Location.Y + 10);
                    }
                    fChart.Location = new System.Drawing.Point(10, MainForm.Location.Y + 10);

                    //add bar series
                    int numOfStages = fChart.stagePBarVal;
                    //int numPerSatge = (int)Math.Ceiling(((double)dgv.RowCount / (double)numOfStages));
                    int remainder = dgv.RowCount % numOfStages;
                    int numPerSatge = (int)(dgv.RowCount - remainder) / numOfStages;
                    int extraItem = 0;
                    if (remainder != 0)
                    {
                        extraItem = 1;
                    }
                    //
                    int currentStage = 1;
                    Series ser0 = new Series();
                    Series ser1 = new Series();
                    Series[] series = {new Series(),new Series() };
                    int[] val = new int[2] { 0, 0 };
                    double[] per = new double[2] { 0.0, 0.0 };
                    string[] seriesName = { "Not Success", "Success" };
                    System.Drawing.Color[] seriesColor= { System.Drawing.Color.Orange, System.Drawing.Color.Blue};
                    fChart.chart2.Series.Clear();

                    //fChart.chart2.Series.Add(ser);
                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        if ((i == (currentStage * numPerSatge + extraItem)) || (i == (dgv.RowCount - 1)))
                        {
                            string xVal = "Overall";
                            if (numOfStages > 1)
                            {
                                xVal = "Stage " + currentStage.ToString();
                            }
                            for (int j = 0; j < series.Length; j++)
                            {
                                series[j].Points.AddXY(xVal, val[j]);
                                per[j]= (double)val[j] / (val[0] + val[1]) * 100.0d;
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
                            val[result] ++;
                            Console.WriteLine("Adding");
                        }
                    }
                    for (int j=0;j<series.Length;j++)
                    {
                        series[j].Name = seriesName[j];
                        series[j].IsVisibleInLegend = true;
                        series[j].Color = seriesColor[j];
                        fChart.chart2.Series.Add(series[j]);
                        fChart.chart2.Series[j].ToolTip = "#VALX\n " + series[j].Name + ": #VALY\nPercentage: #PERCENT";
                        if (numOfStages == 1)
                        {
                            foreach (DataPoint p in fChart.chart2.Series[j].Points)
                            {
                                p.Label = series[j].Name + "\n(#VALY)\n(" + per[j].ToString("0.00") + "%)";
                            }
                        }
                    }
                    DataPoint[] maxDP = { fChart.chart2.Series[0].Points.FindMaxByValue(), fChart.chart2.Series[1].Points.FindMaxByValue() };
                    double maxYValue = Math.Max(maxDP[0].YValues[0], maxDP[1].YValues[0]);
                    // inteval
                    fChart.chart2.ChartAreas[0].AxisY.Maximum = Math.Max(5, maxYValue);
                    if (fChart.chart2.ChartAreas[0].AxisY.Maximum <= 6)
                    {
                        fChart.chart2.ChartAreas[0].AxisY.Interval = 1;
                    }
                    else
                    {
                        fChart.chart2.ChartAreas[0].RecalculateAxesScale();
                        fChart.chart2.ChartAreas[0].AxisY.Maximum = fChart.chart2.ChartAreas[0].AxisY.Maximum * 1.25;
                        double sp = 8;
                        int interval = (int)(Math.Ceiling((fChart.chart2.ChartAreas[0].AxisY.Maximum)) / sp);
                        int r = interval % 10;
                        if (r <= 5)
                        {
                            interval = interval - r;
                        }
                        if ((r > 2) && (r < 8))
                        { }//interval = interval - (interval % 5); 
                        if (r >= 6)
                        { interval = interval - r + 10; }
                        fChart.chart2.ChartAreas[0].AxisY.Interval = (interval);
                    }
                    //MessageBox.Show(maxYValue.ToString());
                    fChart.lblTiltle.Text = "Progress Data from " + System.IO.Path.GetFileName(MainForm.openedExcelFileNameMultiple);
                    fChart.dgvFChart = dgv;
                    fChart.chart2.Invalidate();
                    fChart.chart2.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                    fChart.chart2.ChartAreas[0].BorderColor = System.Drawing.Color.Black;
                    fChart.chart2.ChartAreas[0].BorderWidth = 3;
                    fChart.changeLabel(dgv.RowCount - 1);
                    fChart.setTBarValue(numOfStages);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Plt Pie", MessageBoxButtons.OK);
                }
            }
            else
            {
                fChart.chart2.Series.Clear();
                fChart.chart2.ChartAreas.Clear();
                fChart.chart2.Invalidate();
                fChart.chart2.ChartAreas.Add(new ChartArea());
                fChart.Hide();
            }

            //return isOpened;
        }
        //*********************************************************************************************************************************//
        //=================================================================================================================================//
        private static bool isChartFormOpened(string frmName)
        {
            FormCollection fc = Application.OpenForms;
            bool isOpen = false;
            foreach (Form frm in fc)
            {
                if (frm.Name == frmName)
                {
                    isOpen = true;
                    MessageBox.Show("Open");
                    break;
                }
            }
            return isOpen;
        }
        //*********************************************************************************************************************************//
        
        //=================================================================================================================================//

        //*********************************************************************************************************************************//
        
        //=================================================================================================================================//

        //*********************************************************************************************************************************//
        
        //=================================================================================================================================//

        //*********************************************************************************************************************************//
        
        //=================================================================================================================================//

        //*********************************************************************************************************************************//

    }
}
