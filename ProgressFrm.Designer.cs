namespace CuSum_Analysis
{
    partial class ProgressFrm
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
            this.lblProgressPF = new System.Windows.Forms.Label();
            this.pBarPF = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblProgressPF
            // 
            this.lblProgressPF.AutoSize = true;
            this.lblProgressPF.Location = new System.Drawing.Point(12, 20);
            this.lblProgressPF.Name = "lblProgressPF";
            this.lblProgressPF.Size = new System.Drawing.Size(35, 13);
            this.lblProgressPF.TabIndex = 0;
            this.lblProgressPF.Text = "label1";
            // 
            // pBarPF
            // 
            this.pBarPF.Location = new System.Drawing.Point(15, 61);
            this.pBarPF.Name = "pBarPF";
            this.pBarPF.Size = new System.Drawing.Size(303, 23);
            this.pBarPF.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(228, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ProgressFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 110);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pBarPF);
            this.Controls.Add(this.lblProgressPF);
            this.Name = "ProgressFrm";
            this.Text = "Progress Status";
            this.Load += new System.EventHandler(this.ProgressFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProgressPF;
        private System.Windows.Forms.ProgressBar pBarPF;
        private System.Windows.Forms.Button btnClose;
    }
}