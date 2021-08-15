namespace CuSum_Analysis
{
    partial class FrmChart
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
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.rbBarChart = new System.Windows.Forms.RadioButton();
            this.rbPieChart = new System.Windows.Forms.RadioButton();
            this.lblTiltle = new System.Windows.Forms.Label();
            this.tBarStage = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTBarValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarStage)).BeginInit();
            this.SuspendLayout();
            // 
            // chart2
            // 
            chartArea1.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chart2.Legends.Add(legend1);
            this.chart2.Location = new System.Drawing.Point(12, 60);
            this.chart2.Name = "chart2";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart2.Series.Add(series1);
            this.chart2.Size = new System.Drawing.Size(381, 318);
            this.chart2.TabIndex = 0;
            this.chart2.Text = "ChartOnFrm";
            this.chart2.Click += new System.EventHandler(this.chart2_Click);
            this.chart2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseClick);
            this.chart2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseMove);
            this.chart2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseUp);
            // 
            // rbBarChart
            // 
            this.rbBarChart.AutoSize = true;
            this.rbBarChart.Location = new System.Drawing.Point(-3, 398);
            this.rbBarChart.Name = "rbBarChart";
            this.rbBarChart.Size = new System.Drawing.Size(69, 17);
            this.rbBarChart.TabIndex = 1;
            this.rbBarChart.TabStop = true;
            this.rbBarChart.Text = "Bar Chart";
            this.rbBarChart.UseVisualStyleBackColor = true;
            this.rbBarChart.Visible = false;
            this.rbBarChart.CheckedChanged += new System.EventHandler(this.rbBarChart_CheckedChanged);
            // 
            // rbPieChart
            // 
            this.rbPieChart.AutoSize = true;
            this.rbPieChart.Location = new System.Drawing.Point(72, 398);
            this.rbPieChart.Name = "rbPieChart";
            this.rbPieChart.Size = new System.Drawing.Size(68, 17);
            this.rbPieChart.TabIndex = 2;
            this.rbPieChart.TabStop = true;
            this.rbPieChart.Text = "Pie Chart";
            this.rbPieChart.UseVisualStyleBackColor = true;
            this.rbPieChart.Visible = false;
            // 
            // lblTiltle
            // 
            this.lblTiltle.AutoSize = true;
            this.lblTiltle.Location = new System.Drawing.Point(38, 23);
            this.lblTiltle.Name = "lblTiltle";
            this.lblTiltle.Size = new System.Drawing.Size(35, 13);
            this.lblTiltle.TabIndex = 3;
            this.lblTiltle.Text = "label1";
            // 
            // tBarStage
            // 
            this.tBarStage.LargeChange = 1;
            this.tBarStage.Location = new System.Drawing.Point(264, 393);
            this.tBarStage.Maximum = 20;
            this.tBarStage.Minimum = 1;
            this.tBarStage.Name = "tBarStage";
            this.tBarStage.Size = new System.Drawing.Size(129, 45);
            this.tBarStage.TabIndex = 4;
            this.tBarStage.Value = 3;
            this.tBarStage.Scroll += new System.EventHandler(this.tBarStage_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(96, 398);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Change number of stages";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(213, 353);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(35, 13);
            this.lblTotal.TabIndex = 6;
            this.lblTotal.Text = "label1";
            // 
            // lblTBarValue
            // 
            this.lblTBarValue.AutoSize = true;
            this.lblTBarValue.Location = new System.Drawing.Point(273, 425);
            this.lblTBarValue.Name = "lblTBarValue";
            this.lblTBarValue.Size = new System.Drawing.Size(28, 13);
            this.lblTBarValue.TabIndex = 7;
            this.lblTBarValue.Text = "Val=";
            // 
            // FrmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 450);
            this.Controls.Add(this.lblTBarValue);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tBarStage);
            this.Controls.Add(this.lblTiltle);
            this.Controls.Add(this.rbPieChart);
            this.Controls.Add(this.rbBarChart);
            this.Controls.Add(this.chart2);
            this.Name = "FrmChart";
            this.Text = "FrmChart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmChart_FormClosing);
            this.Load += new System.EventHandler(this.FrmChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarStage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.RadioButton rbBarChart;
        private System.Windows.Forms.RadioButton rbPieChart;
        public System.Windows.Forms.Label lblTiltle;
        private System.Windows.Forms.TrackBar tBarStage;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTBarValue;
    }
}