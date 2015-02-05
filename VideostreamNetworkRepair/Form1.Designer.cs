namespace VideostreamNetworkRepair
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.PictureBox pictureBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnClose = new System.Windows.Forms.Button();
            this.prgRepair = new System.Windows.Forms.ProgressBar();
            this.btnReboot = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tmrProgress = new System.Windows.Forms.Timer(this.components);
            this.pnlRepairStatus = new System.Windows.Forms.Panel();
            this.lstAntivirus = new System.Windows.Forms.ListBox();
            this.lblAntivirus = new System.Windows.Forms.Label();
            this.imgStatus = new System.Windows.Forms.PictureBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlRepairStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBox1.Image = global::VideostreamNetworkRepair.Properties.Resources.Title;
            pictureBox1.InitialImage = global::VideostreamNetworkRepair.Properties.Resources.Title;
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
            pictureBox1.Size = new System.Drawing.Size(287, 93);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(421, 255);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 27);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.button3_Click);
            // 
            // prgRepair
            // 
            this.prgRepair.Location = new System.Drawing.Point(12, 66);
            this.prgRepair.Maximum = 1000;
            this.prgRepair.Name = "prgRepair";
            this.prgRepair.Size = new System.Drawing.Size(496, 27);
            this.prgRepair.Step = 1;
            this.prgRepair.TabIndex = 5;
            // 
            // btnReboot
            // 
            this.btnReboot.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReboot.Location = new System.Drawing.Point(226, 254);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(185, 27);
            this.btnReboot.TabIndex = 6;
            this.btnReboot.Text = "Reboot Now";
            this.btnReboot.UseVisualStyleBackColor = true;
            this.btnReboot.Visible = false;
            this.btnReboot.Click += new System.EventHandler(this.btnReboot_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(108)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(523, 93);
            this.panel1.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 38);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(105, 25);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Repairing...";
            // 
            // tmrProgress
            // 
            this.tmrProgress.Interval = 20;
            this.tmrProgress.Tick += new System.EventHandler(this.tmrProgress_Tick);
            // 
            // pnlRepairStatus
            // 
            this.pnlRepairStatus.Controls.Add(this.lstAntivirus);
            this.pnlRepairStatus.Controls.Add(this.lblAntivirus);
            this.pnlRepairStatus.Controls.Add(this.imgStatus);
            this.pnlRepairStatus.Controls.Add(this.prgRepair);
            this.pnlRepairStatus.Controls.Add(this.lblStatus);
            this.pnlRepairStatus.Location = new System.Drawing.Point(0, 99);
            this.pnlRepairStatus.Name = "pnlRepairStatus";
            this.pnlRepairStatus.Size = new System.Drawing.Size(520, 149);
            this.pnlRepairStatus.TabIndex = 9;
            // 
            // lstAntivirus
            // 
            this.lstAntivirus.FormattingEnabled = true;
            this.lstAntivirus.ItemHeight = 15;
            this.lstAntivirus.Location = new System.Drawing.Point(0, 80);
            this.lstAntivirus.Name = "lstAntivirus";
            this.lstAntivirus.Size = new System.Drawing.Size(520, 64);
            this.lstAntivirus.TabIndex = 10;
            this.lstAntivirus.Visible = false;
            // 
            // lblAntivirus
            // 
            this.lblAntivirus.AutoSize = true;
            this.lblAntivirus.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAntivirus.Location = new System.Drawing.Point(6, 0);
            this.lblAntivirus.Name = "lblAntivirus";
            this.lblAntivirus.Size = new System.Drawing.Size(536, 60);
            this.lblAntivirus.TabIndex = 11;
            this.lblAntivirus.Text = "Couldn\'t successfully auto repair. Manually disable these\r\n firewalls and reboot." +
    " Email us if you need any help!";
            this.lblAntivirus.Visible = false;
            // 
            // imgStatus
            // 
            this.imgStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imgStatus.Image = global::VideostreamNetworkRepair.Properties.Resources.Enjoy1;
            this.imgStatus.Location = new System.Drawing.Point(0, 0);
            this.imgStatus.Name = "imgStatus";
            this.imgStatus.Size = new System.Drawing.Size(520, 146);
            this.imgStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.imgStatus.TabIndex = 9;
            this.imgStatus.TabStop = false;
            this.imgStatus.Visible = false;
            this.imgStatus.Click += new System.EventHandler(this.imgStatus_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(523, 294);
            this.Controls.Add(this.pnlRepairStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnReboot);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Videostream Repair";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlRepairStatus.ResumeLayout(false);
            this.pnlRepairStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar prgRepair;
        private System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer tmrProgress;
        private System.Windows.Forms.Panel pnlRepairStatus;
        private System.Windows.Forms.PictureBox imgStatus;
        private System.Windows.Forms.ListBox lstAntivirus;
        private System.Windows.Forms.Label lblAntivirus;
    }
}

