namespace CuSum_Analysis
{
    partial class FrmAllChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.extendedChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tBarStage = new System.Windows.Forms.TrackBar();
            this.lblSelectedStep = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extendedChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarStage)).BeginInit();
            this.SuspendLayout();
            // 
            // mainChart
            // 
            chartArea1.Name = "ChartArea1";
            this.mainChart.ChartAreas.Add(chartArea1);
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend1";
            this.mainChart.Legends.Add(legend1);
            this.mainChart.Location = new System.Drawing.Point(12, 12);
            this.mainChart.Name = "mainChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.mainChart.Series.Add(series1);
            this.mainChart.Size = new System.Drawing.Size(371, 261);
            this.mainChart.TabIndex = 0;
            this.mainChart.Click += new System.EventHandler(this.mainChart_Click);
            this.mainChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mainChart_MouseClick);
            // 
            // extendedChart
            // 
            chartArea2.Name = "ChartArea1";
            this.extendedChart.ChartAreas.Add(chartArea2);
            this.extendedChart.Location = new System.Drawing.Point(12, 279);
            this.extendedChart.Name = "extendedChart";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.extendedChart.Series.Add(series2);
            this.extendedChart.Size = new System.Drawing.Size(371, 234);
            this.extendedChart.TabIndex = 1;
            this.extendedChart.Text = "chart2";
            // 
            // tBarStage
            // 
            this.tBarStage.LargeChange = 1;
            this.tBarStage.Location = new System.Drawing.Point(254, 519);
            this.tBarStage.Maximum = 20;
            this.tBarStage.Minimum = 1;
            this.tBarStage.Name = "tBarStage";
            this.tBarStage.Size = new System.Drawing.Size(104, 45);
            this.tBarStage.TabIndex = 2;
            this.tBarStage.Value = 1;
            this.tBarStage.Scroll += new System.EventHandler(this.tBart_Scroll);
            // 
            // lblSelectedStep
            // 
            this.lblSelectedStep.AutoSize = true;
            this.lblSelectedStep.Location = new System.Drawing.Point(62, 520);
            this.lblSelectedStep.Name = "lblSelectedStep";
            this.lblSelectedStep.Size = new System.Drawing.Size(35, 13);
            this.lblSelectedStep.TabIndex = 3;
            this.lblSelectedStep.Text = "label1";
            // 
            // FrmAllChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 566);
            this.Controls.Add(this.lblSelectedStep);
            this.Controls.Add(this.tBarStage);
            this.Controls.Add(this.extendedChart);
            this.Controls.Add(this.mainChart);
            this.Name = "FrmAllChart";
            this.Text = "FrmAllChart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAllChart_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extendedChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarStage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart extendedChart;
        private System.Windows.Forms.TrackBar tBarStage;
        private System.Windows.Forms.Label lblSelectedStep;
    }
}